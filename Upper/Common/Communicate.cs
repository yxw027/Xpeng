using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Xpeng.Assets.Config;
using Xpeng.Data;
using Xpeng.View;

namespace Xpeng.Common
{
    public class Communicate
    {
        public static Socket server = null;
        public static int BUFFER_SIZE = 256;


        /// <summary>
        /// 初始化服务器
        /// </summary>
        public static bool InitServer(Config config)
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 加载服务器ip和端口号
            string[] strsIP = config.IP?.Split('.');
            IPAddress iPAddress = new IPAddress(new byte[]
            {
                Convert.ToByte(strsIP[0]),
                Convert.ToByte(strsIP[1]),
                Convert.ToByte(strsIP[2]),
                Convert.ToByte(strsIP[3])
            });
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, int.Parse(config.Port));

            // 绑定服务器并开始监听
            try
            {
                server.Bind(iPEndPoint);
                server.Listen(10);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 释放TCP服务器资源
        /// </summary>
        /// <returns></returns>
        public static bool DisposeServer()
        {
            try
            {
                if (server != null)
                {
                    server.Close();
                    server = null;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 向客户端发送信息
        /// </summary>
        /// <param name="socket">连接的端口号，此参数可省略，但为了重用代码而保留，
        /// 使用时填入Communicate.client</param>
        /// <param name="message">待发送的信息</param>
        public static void SendMessage(Socket socket, string message)
        {
            socket.Send(Encoding.UTF8.GetBytes(message));
        }


        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="socket">连接的端口号，此参数可省略，但为了重用代码而保留，
        /// 使用时填入Communicate.client</param>
        /// <returns>接收到的信息</returns>
        public static string RecvMessage(Socket socket)
        {
            byte[] packet = new byte[BUFFER_SIZE];

            int len = socket.Receive(packet);
            return Encoding.UTF8.GetString(packet, 0, len);
        }


        /// <summary>
        /// 为每个客户端创建线程
        /// </summary>
        public static void ConnectClient()
        {
            while (server != null)
            {
                try
                {
                    Socket client = server.Accept();
                    new Thread(ResponseClient)
                    {
                        IsBackground = true
                    }.Start(client);
                    Thread.Sleep(100);
                }
                catch (Exception)
                {

                }
            }
        }


        /// <summary>
        /// 响应客户端
        /// </summary>
        /// <param name="o">与客户端相连的套接字，使用时需要强制转换</param>
        public static void ResponseClient(object o)
        {
            Socket client = o as Socket;
            Regex regex = new Regex(@"^#\d+,\d+,\d+,\d+,\d+,\d+#$");
            try
            {
                while (server != null)
                {
                    // 减少CPU占用
                    Thread.Sleep(400);

                    // 解析坐标信息
                    string msg = RecvMessage(client);
                    if (regex.IsMatch(msg))
                    {
                        var timeStamps = new Regex(@"\d+").Matches(msg);

                        List<long> timeStampList = new List<long>();
                        foreach (var timeStamp in timeStamps)
                        {
                            long timeStampNow = long.Parse(timeStamp.ToString());
                            if((timeStampList.Count > 0) && (timeStampNow < timeStampList.Last()))
                            {
                                // 当前时间戳小于前时间戳, 计数器计满清零
                                long timeStampLast = long.Parse(timeStampList.Last().ToString());
                                timeStampList.Add(timeStampNow + (long)Math.Pow(2, 40) - timeStampLast);
                            }
                            timeStampList.Add(long.Parse(timeStamp.ToString()));
                        }
                        
                        long T_round1 = timeStampList[3] - timeStampList[0];
                        long T_round2 = timeStampList[5] - timeStampList[2];
                        long T_reply1 = timeStampList[2] - timeStampList[1];
                        long T_reply2 = timeStampList[4] - timeStampList[3];
                        double T_prop = (T_round1 * T_round2 - T_reply1 * T_reply2) * 15.625 * Math.Pow(10, -12) / (T_round1 + T_round2 + T_reply1 + T_reply2);
                        double Distance = T_prop * 3 * Math.Pow(10, 8);

                        // 绘制图形
                        App.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            GlobalValues.mainViewModel.mainModel.D = Distance.ToString();
                        }));
                    }
                    
                    //foreach(string sub in msgSub)
                    //{
                    //    if (regex.IsMatch(sub))
                    //    {
                    //        string[] strs = sub.Split(',');
                    //        double x = double.Parse(strs[0]);
                    //        double y = double.Parse(strs[1]);
                    //        App.Current.Dispatcher.Invoke(new Action(() =>
                    //        {
                    //            GlobalValues.mainViewModel.AddPoints(x, y);
                    //        }));
                    //    }
                    //}
                }
                client.Close();
                GlobalValues.mainViewModel.mainModel.LaunchButtonBack = "#FF0682FF";
            }
            catch (Exception)
            {
                client.Close();
                GlobalValues.mainViewModel.mainModel.LaunchButtonBack = "#FF0682FF";
                Config.SaveConfig(GlobalValues.mainViewModel.mainModel.config);
                DataRecord.DisposeRecord();
            }
        }
    }
}
