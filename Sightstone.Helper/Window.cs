using System.Windows;

namespace Sightstone.Helper
{
    public static class Window
    {
        public static void FocusWindow(System.Windows.Window win)
        {
            if (win.WindowState == WindowState.Minimized)
                win.WindowState = WindowState.Normal;
            
            win.Activate();
            win.Topmost = true; // important
            win.Topmost = false; // important
            win.Focus(); // important
        }
    }
}
