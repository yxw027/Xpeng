using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xpeng.Common;

namespace Xpeng.View
{
    /// <summary>
    /// DisposeServerFailed.xaml 的交互逻辑
    /// </summary>
    public partial class DisposeServerFailedView : Window
    {
        public DisposeServerFailedView()
        {
            InitializeComponent();
            Owner = GlobalValues.mainView;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
