using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using Xpeng.Common;
using Xpeng.Data;
using Xpeng.Model;
using Xpeng.View;

namespace Xpeng.ViewModel
{
    public class MainViewModel
    {
        public MainModel mainModel { get; set; }
        public CommandBase LaunchServerCommand { get; set; }
        public CommandBase ClearDrawArea { get; set; }

        public bool tcpServerLaunched = false;
        public Point lastPoint = default;

        public MainViewModel()
        {
            mainModel = new MainModel() 
            {
                X = "0.000",
                Y = "0.000",
                Z = "0.000",
                D = "0.000",
                LaunchButtonBack = "#FF0682FF",
                LaunchButtonContent = "启   动",
            };
            LaunchServerCommand = new CommandBase()
            {
                DoCanExecute = new Func<object, bool>((o)=>true),
                DoExecute = LaunchServer
            };
            ClearDrawArea = new CommandBase
            {
                DoCanExecute = new Func<object, bool>((o) => true),
                DoExecute = new Action<object>((o) =>
                {
                    ClearPoints();
                })
            };
        }


        /// <summary>
        /// 启动TCP服务器
        /// </summary>
        /// <param name="o"></param>
        private void LaunchServer(object o)
        {
            if (!tcpServerLaunched)
            {
                if (Communicate.InitServer(mainModel.config))
                {
                    tcpServerLaunched = true;
                    mainModel.LaunchButtonBack = "#FFC04048";
                    mainModel.LaunchButtonContent = "停   止";
                    //new InitServerSuccessView().Show();
                    new Thread(Communicate.ConnectClient).Start();
                }
                else
                {
                    new InitServerFailedView().Show();
                }
            }
            else
            {
                if (Communicate.DisposeServer())
                {
                    tcpServerLaunched = false;
                    mainModel.LaunchButtonBack = "#FF0682FF";
                    mainModel.LaunchButtonContent = "启   动";
                    //new DisposeServerSuccessView().Show();
                }
                else
                {
                    new DisposeServerFailedView().Show();
                }
            }
        }


        /// <summary>
        /// 向曲线中添加点
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        public void AddPoints(double x, double y)
        {
            Point point = CoordTrans(x, y);

            if (lastPoint != default)
            {
                mainModel.Lines.Add(new MyLine(new Line
                {
                    X1 = lastPoint.X,
                    Y1 = lastPoint.Y,
                    X2 = point.X,
                    Y2 = point.Y
                }));
            }
            lastPoint = point;
            RefreshCoord(x, y);
        }


        /// <summary>
        /// 更新侧边栏坐标信息
        /// </summary>
        /// <param name="x">横坐标</param>
        /// <param name="y">纵坐标</param>
        private void RefreshCoord(double x, double y)
        {
            mainModel.X = x.ToString("f3");
            mainModel.Y = y.ToString("f3");
            mainModel.D = Math.Sqrt(y * y + x * x).ToString("f3");

            if (mainModel.config.RecordData)
            {
                if (DataRecord.recordWriter == null)
                {
                    DataRecord.InitRecord();
                }
                DataRecord.Record($"{mainModel.X}\t\t{mainModel.Y}\t\t0.000\t\t{mainModel.D}", mainModel.config.TimeStamp);
            }
        }


        /// <summary>
        /// 清空绘图区域
        /// </summary>
        private void ClearPoints()
        {
            mainModel.Lines.Clear();
            lastPoint = default;
        }


        /// <summary>
        /// 将标签坐标映射至画布
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        private Point CoordTrans(double x, double y)
        {
            return new Point
            {
                X = GlobalValues.RouteArea.ActualWidth * (x - GlobalValues.AXES_X_MIN) / (GlobalValues.AXES_X_MAX - GlobalValues.AXES_X_MIN),
                Y = GlobalValues.RouteArea.ActualHeight * (y - GlobalValues.AXES_Y_MAX) / (GlobalValues.AXES_Y_MIN - GlobalValues. AXES_Y_MAX)
            };
        }
    }
}
