using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WinDirector.Devices
{
    public static class Display
    {
        public static void TurnOffMonitor() => WinApi.PostMessage((IntPtr)HWND.BROADCAST, (uint)WM.SYSCOMMAND, (IntPtr)SC.MONITORPOWER, new IntPtr(2));
        public static void TurnOnMonitor() => WinApi.PostMessage((IntPtr)HWND.BROADCAST, (uint)WM.SYSCOMMAND, (IntPtr)SC.MONITORPOWER, new IntPtr(-1));

        public static Rectangle GetScreenWorkingRectangle()
        {
            WinApi.SystemParametersInfo((uint)SPI.GETWORKAREA, 0, out Rectangle rect, 0);
            return rect;
        }

        public static class Settings
        {
            // subscribe to SystemEvents.DisplaySettingsChanged event with singleton to change ScalingFactor after initialization
            private static float _scalingFactor;
            public static float ScalingFactor
            {
                get
                {
                    if (_scalingFactor == 0)
                    {
                        _scalingFactor = GetScalingFactor();
                    }
                    return _scalingFactor;
                }
            }

            public static float GetScalingFactor()
            {
                Graphics g = Graphics.FromHwnd(IntPtr.Zero);
                IntPtr desktop = g.GetHdc();
                int LogicalScreenHeight = WinApi.GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
                int PhysicalScreenHeight = WinApi.GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

                float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

                return ScreenScalingFactor;
            }
            
        }
    }
}
namespace WinDirector
{
    public partial class WinApi
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
    }
    enum DeviceCap
    {
        VERTRES = 10,
        DESKTOPVERTRES = 117,
    }
}