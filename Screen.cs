using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WinDirector.Devices
{
    public static class Display
    {
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
                int LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
                int PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

                float ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

                return ScreenScalingFactor;
            }
            public enum DeviceCap
            {
                VERTRES = 10,
                DESKTOPVERTRES = 117,
            }


            [DllImport("gdi32.dll", CharSet = CharSet.Auto)]
            private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        }
    }
}
