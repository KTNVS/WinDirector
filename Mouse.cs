using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using WinDirector.WinApiHandler;

namespace WinDirector.Input
{
    public static class Mouse
    {
        public static bool Enabled = true;
        private static readonly MouseHook Hook = new MouseHook(); // MouseHook works as a singleton

        public static void SetCursorPosition(Location location)
        {
            SetCursorPos(location.X, location.Y);
        }
        public static void SetCursorPosition(Location location, IntPtr handle)
        {
            SendMouse(location, (uint)WM.MOUSEMOVE, handle); 
        }
        public static void SendMouse(uint WM_Message)
        {
            Location point = Point;
            IntPtr lparam = (IntPtr)CreateLParamFromPosition(point.X, point.Y);
            Messages.PostMessage(IntPtr.Zero, WM_Message, IntPtr.Zero, lparam);
        }
        public static void SendMouse(Location point, uint WM_Message)
        {
            IntPtr lparam = (IntPtr)CreateLParamFromPosition(point.X, point.Y);
            Messages.PostMessage(IntPtr.Zero, WM_Message, IntPtr.Zero, lparam);
        }
        public static void SendMouse(Location point, uint WM_Message, IntPtr handle)
        {
            IntPtr lparam = (IntPtr)CreateLParamFromPosition(point.X, point.Y);
            Messages.PostMessage(handle, WM_Message, IntPtr.Zero, lparam);
        }
        private static int CreateLParamFromPosition(int LoWord, int HiWord) => (HiWord << 16) | (LoWord & 0xffff);


        public static Location Point { get; private set; }

        public delegate void MouseMoveHandler(MouseEventArgs e);
        public static event MouseMoveHandler OnMouseMove;

        public delegate void MouseDownHandler(MouseEventArgs e);
        public static event MouseDownHandler OnMouseDown;

        public delegate void MouseUpHandler(MouseEventArgs e);
        public static event MouseUpHandler OnMouseUp;
        private class MouseHook
        {
            private readonly IntPtr HookID;
            private readonly HookProc hProc;
            public MouseHook()
            {
                hProc = MouseHookProc;
                HookID = HookManager.SetHook(hProc, WH.MOUSE_LL);
            }
            ~MouseHook()
            {
                HookManager.UnHook(HookID);
                OnMouseMove = null;
                OnMouseDown = null;
                OnMouseUp = null;
            }
            private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
            {
                MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));

                MouseEventArgs mouseArgs = new MouseEventArgs(MouseButtons.None, Point);
                if (nCode >= 0)
                {
                    switch ((WM)wParam)
                    {
                        case WM.MOUSEMOVE:
                            GetCursorPos(out Location location);
                            Point = location;
                            OnMouseMove?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.None, Point));
                            break;
                        case WM.LBUTTONDOWN:
                            OnMouseDown?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Left, Point));
                            break;
                        case WM.LBUTTONUP:
                            OnMouseUp?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Left, Point));
                            break;
                        case WM.RBUTTONDOWN:
                            OnMouseDown?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Right, Point));
                            break;
                        case WM.RBUTTONUP:
                            OnMouseUp?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Right, Point));
                            break;
                        case WM.MBUTTONDOWN:
                            OnMouseDown?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Middle, Point));
                            break;
                        case WM.MBUTTONUP:
                            OnMouseUp?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Middle, Point));
                            break;
                    }
                }

                if (!Enabled || mouseArgs.Cancel) { return 1; };
                return HookManager.CallNextHookEx(HookID, nCode, wParam, lParam);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetCursorPos(out Location lpPoint);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
    }
    public class MouseEventArgs : CancelEventArgs
    {
        public MouseEventArgs(MouseButtons button, Location location)
        {
            Button = button;
            Location = location;
        }

        public MouseButtons Button { get; }
        public Location Location { get; }
    }
    public enum MouseButtons
    {
        None = 0,
        Left = 1048576,
        Right = 2097152,
        Middle = 4194304,
        XButton1 = 8388608,
        XButton2 = 16777216
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseHookStruct
    {
        public Location pt;
        public IntPtr hwnd;
        public uint wHitTestCode;
        public IntPtr dwExtraInfo;
    }
}
