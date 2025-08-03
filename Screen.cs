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

            public static float GetScalingFactor() => GetScalingFactor(IntPtr.Zero);
            public static float GetScalingFactor(IntPtr hwnd)
            {
                IntPtr hdc = WinApi.GetDC(hwnd);

                int logicalScreenHeight = WinApi.GetDeviceCaps(hdc, Convert.ToInt32(DeviceCap.VERTRES));
                int physicalScreenHeight = WinApi.GetDeviceCaps(hdc, Convert.ToInt32(DeviceCap.DESKTOPVERTRES));

                float scalingFactor = (float)physicalScreenHeight / (float)logicalScreenHeight;

                WinApi.ReleaseDC(IntPtr.Zero, hdc);

                return scalingFactor;
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

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }
    enum DeviceCap
    {
        VERTRES = 10,
        DESKTOPVERTRES = 117,
    }
}