using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WinDirector.Processes.UI
{
    public static class Window
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr WindowFromPoint(Location Point);
    }
}
