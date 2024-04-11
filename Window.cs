using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WinDirector.Processes.UI
{
    public static class Window
    {
        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Location Point);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr hWnd, string text);





        public static void HideWindow(IntPtr hWnd) => ShowWindow(hWnd, (int)SW.HIDE); // todo: add silent option. if not silent, focus on the next window in Z order
        public static void UnHideWindow(IntPtr hWnd, bool silent = false)
        {
            SW selectedOption = silent ? SW.SHOW_NO_ACTIVATE : SW.SHOW;
            ShowWindow(hWnd, (int)selectedOption);
        }
        public static void SetWindowVisibility(IntPtr hWnd, bool visible, bool silent = false)
        {
            SW selectedOption = visible ? SW.SHOW : SW.HIDE;
        }
        public static void MinimizeWindow(IntPtr hWnd, bool force = false, bool silent = false)
        {
            SW selectedOption = force ? SW.FORCE_MINIMIZE : silent ? SW.SHOW_MINIMIZED_NO_ACTIVATE : SW.MINIMIZE;
            ShowWindow(hWnd, (int)selectedOption);
        }
        public static void Maximize(IntPtr hWnd) => ShowWindow(hWnd, (int)SW.SHOW_MAXIMIZED);
        public static void Normalize(IntPtr hWnd, bool silent = false)
        {
            SW selectedOption = silent ? SW.NORMAL_NO_ACTIVATE : SW.NORMAL;
            ShowWindow(hWnd, (int)selectedOption);
        }

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private enum SW : int
        {
            HIDE = 0,
            NORMAL = 1,
            SHOW_MINIMIZED = 2,
            SHOW_MAXIMIZED = 3,
            NORMAL_NO_ACTIVATE = 4,
            SHOW = 5,
            MINIMIZE = 6,
            SHOW_MINIMIZED_NO_ACTIVATE = 7,
            SHOW_NO_ACTIVATE = 8,
            RESTORE = 9,
            SHOW_DEFAULT = 10,
            FORCE_MINIMIZE = 11
        }
    }
}
