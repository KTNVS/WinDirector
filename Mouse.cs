using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

using WinDirector.WinApiHandler;

namespace WinDirector.Input
{
    public static class Mouse
    {
        private static bool _enabled = true;
        public static bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (!mouseHookIsInitialized)
                {
                    InitializeHook();
                }
                _enabled = value;
            }
        }
        private static void InitializeHook() => _ = MouseHook.Instance;


        private static Location _point;
        public static Location Point
        {
            get
            {
                if (!mouseHookIsInitialized)
                {
                    _point = GetCursorPosition();
                }
                return _point;
            }
            set
            {
                _point = value;
                SetCursorPosition(value);
            }
        }
        private static bool mouseHookIsInitialized = false;

        public static Location GetCursorPosition()
        {
            WinApi.GetCursorPos(out Location currentPoint);
            return currentPoint;
        }
        public static void SetCursorPosition(Location location)
        {
            WinApi.SetCursorPos(location.X, location.Y);
        }
        public static void SetCursorPosition(Location location, IntPtr handle)
        {
            SendMouse(location, (uint)WM.MOUSEMOVE, handle); 
        }
        public static void SendMouse(Location point, uint WM_Message)
        {
            IntPtr lparam = (IntPtr)CreateLParamFromPosition(point.X, point.Y);
            WinApi.PostMessage(IntPtr.Zero, WM_Message, IntPtr.Zero, lparam);
        }
        public static void SendMouse(Location point, uint WM_Message, IntPtr handle)
        {
            IntPtr lparam = (IntPtr)CreateLParamFromPosition(point.X, point.Y);
            WinApi.PostMessage(handle, WM_Message, IntPtr.Zero, lparam);
        }
        private static int CreateLParamFromPosition(int LoWord, int HiWord) => (HiWord << 16) | (LoWord & 0xffff);

        public delegate void MouseMoveHandler(MouseEventArgs e);
        public delegate void MouseDownHandler(MouseEventArgs e);
        public delegate void MouseUpHandler(MouseEventArgs e);

        public class MouseHook
        {
            public static MouseHook Instance
            {
                get => init.Value;
            }
            private static readonly Lazy<MouseHook> init = new Lazy<MouseHook>(() => new MouseHook());


            private readonly IntPtr HookID;
            private readonly HookProc hProc;

            public event MouseMoveHandler OnMouseMove;
            public event MouseDownHandler OnMouseDown;
            public event MouseUpHandler OnMouseUp;

            private MouseHook()
            {
                mouseHookIsInitialized = true;
                _point = GetCursorPosition();

                hProc = MouseHookProc;
                HookID = HookManager.SetHook(hProc, WH.MOUSE_LL);
            }
            ~MouseHook()
            {
                HookManager.UnHook(HookID);
                OnMouseMove = null; OnMouseDown = null; OnMouseUp = null;
            }
            private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
            {
                MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));

                MouseEventArgs mouseArgs = new MouseEventArgs(MouseButtons.None, _point);
                if (nCode >= 0)
                {
                    switch ((WM)wParam)
                    {
                        case WM.MOUSEMOVE:
                            WinApi.GetCursorPos(out Location location);
                            _point = location;
                            OnMouseMove?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.None, _point));
                            break;
                        case WM.LBUTTONDOWN:
                            OnMouseDown?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Left, _point));
                            break;
                        case WM.LBUTTONUP:
                            OnMouseUp?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Left, _point));
                            break;
                        case WM.RBUTTONDOWN:
                            OnMouseDown?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Right, _point));
                            break;
                        case WM.RBUTTONUP:
                            OnMouseUp?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Right, _point));
                            break;
                        case WM.MBUTTONDOWN:
                            OnMouseDown?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Middle, _point));
                            break;
                        case WM.MBUTTONUP:
                            OnMouseUp?.Invoke(mouseArgs = new MouseEventArgs(MouseButtons.Middle, _point));
                            break;
                    }
                }

                if (!Enabled || mouseArgs.Cancel) { return 1; };
                return WinApi.CallNextHookEx(HookID, nCode, wParam, lParam);
            }
        }

        
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
    
}
namespace WinDirector
{
    public partial class WinApi
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out Location lpPoint);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
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