using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xpeng.Common;

namespace Xpeng.Assets.Config
{
    public class Config : NotifyBase
    {
        private string _ip;
        public string IP
        {
            get { return _ip; }
            set
            {
                _ip = value;
                DoNotify();
            }
        }

        private string _port;
        public string Port
        {
            get { return _port; }
            set
            {
                _port = value;
                DoNotify();
            }
        }

        private bool _ipDetect;
        public bool IPDetect
        {
            get { return _ipDetect; }
            set
            {
                _ipDetect = value;
                DoNotify();
            }
        }

        private string _directionA;
        public string DirectionA
        {
            get { return _directionA; }
            set
            {
                _directionA = value;
                DoNotify();
            }
        }

        private string _directionB;
        public string DirectionB
        {
            get { return _directionB; }
            set
            {
                _directionB = value;
                DoNotify();
            }
        }

        private string _directionC;
        public string DirectionC
        {
            get { return _directionC; }
            set
            {
                _directionC = value;
                DoNotify();
            }
        }

        private string _directionD;
        public string DirectionD
        {
            get { return _directionD; }
            set
            {
                _directionD = value;
                DoNotify();
            }
        }

        public Config() { }
        public Config(Config config)
        {
            _ip = config.IP;
            _port = config.Port;
            _ipDetect = config.IPDetect;
        }


        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static bool SaveConfig(Config config)
        {
            try
            {
                File.WriteAllText("Assets/Config/config.json", JsonConvert.SerializeObject(config));

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取设置
        /// </summary>
        /// <returns></returns>
        public static Config GetConfig()
        {
            try
            {
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText("Assets/Config/config.json"));
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// 获取本机IPv4地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalAddress()
        {
            string name = Dns.GetHostName();
            IPAddress[] ipadrlist = Dns.GetHostAddresses(name);
            foreach (IPAddress ipa in ipadrlist)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipa.ToString();
                }
            }
            return null;
        }
    }
}
