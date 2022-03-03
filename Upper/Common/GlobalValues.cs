using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xpeng.View;
using Xpeng.ViewModel;

namespace Xpeng.Common
{
    public class GlobalValues
    {
        public static MainView mainView = null;
        public static MainViewModel mainViewModel = null;
        public static Canvas RouteArea = null;

        public static double AXES_X_MIN = -24.6;
        public static double AXES_X_MAX = 24.6;
        public static double AXES_Y_MIN = -15.0;
        public static double AXES_Y_MAX = 15.0;

        public static double DISTANCE_MIN = 0.0;
        public static double DISTANCE_MAX = 1000.0;
    }
}
