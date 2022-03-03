using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xpeng.Assets.Config;
using Xpeng.Common;
using Xpeng.Data;
using Xpeng.ViewModel;

namespace Xpeng.View
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public Point lastPoint;
        public MainView()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.PrimaryScreenHeight;

            GlobalValues.mainViewModel = new MainViewModel();
            DataContext = GlobalValues.mainViewModel;
        }


        /// <summary>
        /// 拖动上方窗体跟随移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (WindowState == WindowState.Maximized)
                {
                    double src_width = GlobalValues.RouteArea.ActualWidth;
                    double src_height = GlobalValues.RouteArea.ActualHeight;

                    WindowState = WindowState.Normal;

                    double dst_width = GlobalValues.RouteArea.ActualWidth;
                    double dst_height = GlobalValues.RouteArea.ActualHeight;
                    CanvasScale(dst_width / src_width, dst_height / src_height, dst_width / 2, dst_height / 2);
                }
                DragMove();
            }
        }


        /// <summary>
        /// 最小化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMinSize(object sender, RoutedEventArgs e)
        {
            double src_width = GlobalValues.RouteArea.ActualWidth;
            double src_height = GlobalValues.RouteArea.ActualHeight;

            WindowState = WindowState.Minimized;

            double dst_width = GlobalValues.RouteArea.ActualWidth;
            double dst_height = GlobalValues.RouteArea.ActualHeight;
            CanvasScale(dst_width/src_width, dst_height/src_height, dst_width/2, dst_height/2);
        }


        /// <summary>
        /// 最大化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMaxSize(object sender, RoutedEventArgs e)
        {
            if(WindowState == WindowState.Maximized)
            {
                double src_width = GlobalValues.RouteArea.ActualWidth;
                double src_height = GlobalValues.RouteArea.ActualHeight;

                WindowState = WindowState.Normal;

                double dst_width = GlobalValues.RouteArea.ActualWidth;
                double dst_height = GlobalValues.RouteArea.ActualHeight;
                CanvasScale(dst_width / src_width, dst_height / src_height, dst_width / 2, dst_height / 2);
            }
            else
            {
                double src_width = GlobalValues.RouteArea.ActualWidth;
                double src_height = GlobalValues.RouteArea.ActualHeight;

                WindowState = WindowState.Maximized;

                double dst_width = GlobalValues.RouteArea.ActualWidth;
                double dst_height = GlobalValues.RouteArea.ActualHeight;
                CanvasScale(dst_width / src_width, dst_height / src_height, 0, 0);
            }
        }


        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClose(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// 关闭窗口，同时关闭应用程序
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            // 保存设置
            Config.SaveConfig(GlobalValues.mainViewModel.mainModel.config);

            // 保存日志
            DataRecord.DisposeRecord();

            // 释放TCP连接
            Communicate.DisposeServer();

            base.OnClosed(e);
            App.Current.Shutdown();
        }


        /// <summary>
        /// 放大Canvas
        /// </summary>
        private void CanvasScale(double scaleX, double scaleY, double centerX, double centerY)
        {
            ScaleTransform totalScale = new ScaleTransform
            {
                ScaleX = scaleX,
                ScaleY = scaleY,
                CenterX = centerX,
                CenterY = centerY
            };
            TransformGroup tfGroup = new TransformGroup();
            tfGroup.Children.Add(totalScale);
            GlobalValues.RouteArea.RenderTransform = tfGroup;
        }
    }
}
