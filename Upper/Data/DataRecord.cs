using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Xpeng.Data
{
    public class DataRecord
    {
        public static StreamWriter recordWriter = null;
        

        /// <summary>
        /// 初始化数据数据流
        /// </summary>
        public static void InitRecord()
        {
            StreamWriter sw = new StreamWriter($"Data/RecordData.txt", true);
            sw.WriteLine($"==================== CoordData {DateTime.Now.AddMilliseconds(1.1)} ====================");
            sw.WriteLine("X\t\t\tY\t\t\tZ\t\t\tD\t\t\tTimeStamp");
            recordWriter = sw;
        }


        /// <summary>
        /// 记录数据
        /// </summary>
        /// <param name="msg">待记录的数据</param>
        /// <param name="timeStamp">是否打上时间戳</param>
        public static void Record(string msg, bool timeStamp = true)
        {
            if(recordWriter != null)
            {
                if (timeStamp)
                {
                    recordWriter.WriteLine($"{msg}\t\t{DateTime.Now.AddMilliseconds(1.1)}");
                }
                else
                {
                    recordWriter.WriteLine($"{msg}\t\t*");
                }
            }
        }


        /// <summary>
        /// 释放数据流
        /// </summary>
        public static void DisposeRecord()
        {
            if(recordWriter != null)
            {
                recordWriter.Close();
            }
            recordWriter = null;
        }
    }
}
