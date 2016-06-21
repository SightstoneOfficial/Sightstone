using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using MahApps.Metro.Controls;

namespace Sightstone.Core
{
    public static class WindowData
    {
        public static MetroWindow MainWindow;
        public static IWindowManager WindowManager;

        public static bool FocusWindow()
        {
            if (MainWindow == null)
                return false;
            if (MainWindow.WindowState == WindowState.Minimized)
                MainWindow.WindowState = WindowState.Normal;

            MainWindow.Activate();
            MainWindow.Topmost = true; // important
            MainWindow.Topmost = false; // important
            MainWindow.Focus(); // important
            return true;
        }

        public static void RunOnUIThread(System.Action function)
        {
            MainWindow.Dispatcher.BeginInvoke(DispatcherPriority.Input, function);
        }
    }
}
