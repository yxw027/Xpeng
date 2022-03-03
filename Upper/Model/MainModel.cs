using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using Xpeng.Assets.Config;
using Xpeng.Common;

namespace Xpeng.Model
{
    public class MainModel : NotifyBase
    {
        private Config _config;
        public Config config
        {
            get { return _config; }
            set
            {
                _config = value;
                DoNotify();
            }
        }
        public ObservableCollection<MyLine> Lines { get; set; }
        private string _x;
        public string X
        {
            get { return _x; }
            set
            {
                _x = value;
                DoNotify();
            }
        }

        private string _y;
        public string Y
        {
            get { return _y; }
            set
            {
                _y = value;
                DoNotify();
            }
        }

        private string _z;
        public string Z
        {
            get { return _z; }
            set
            {
                _z = value;
                DoNotify();
            }
        }

        private string _d;
        public string D
        {
            get { return _d; }
            set
            {
                _d = value;
                DoNotify();
            }
        }

        private bool _zeroCheck;
        public bool ZeroCheck
        {
            get { return _zeroCheck; }
            set
            {
                _zeroCheck = value;
                DoNotify();
            }
        }

        public double ZeroError { get; set; }

        private string _launchButtonBack;
        public string LaunchButtonBack
        {
            get { return _launchButtonBack; }
            set
            {
                _launchButtonBack = value;
                DoNotify();
            }
        }

        private string _launchButtonContent;
        public string LaunchButtonContent
        {
            get { return _launchButtonContent; }
            set
            {
                _launchButtonContent = value;
                DoNotify();
            }
        }

        public MainModel()
        {
            // 加载设置数据
            config = Config.GetConfig();
            if (config.IPDetect)
            {
                config.IP = Config.GetLocalAddress();
            }
            Lines = new ObservableCollection<MyLine>();
        }
    }


    public class MyLine : NotifyBase
    {
        public Line line;
        public MyLine(Line line)
        {
            this.line = line;
        }

        public double X1
        {
            get { return line.X1; }
            set 
            { 
                line.X1 = value;
                DoNotify();
            }
        }

        public double X2
        {
            get { return line.X2; }
            set 
            { 
                line.X2 = value;
                DoNotify();
            }
        }

        public double Y1
        {
            get { return line.Y1; }
            set 
            { 
                line.Y1 = value;
                DoNotify();
            }
        }

        public double Y2
        {
            get { return line.Y2; }
            set 
            { 
                line.Y2 = value;
                DoNotify();
            }
        }
    }
}
