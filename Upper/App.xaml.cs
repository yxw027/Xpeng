using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xpeng.Common;
using Xpeng.View;

namespace Xpeng
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            GlobalValues.mainView = new MainView();
            GlobalValues.mainView.Show();
            GlobalValues.RouteArea = UIHelper.FindChild<Canvas>(Application.Current.MainWindow, "RouteArea");
        }
    }
}
