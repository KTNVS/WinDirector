using System;
using System.Runtime.InteropServices;


namespace WinDirector.Processes.UI
{
    public static class Window
    {
        public static Rectangle GetWindowRectangle(IntPtr hwnd)
        {
            WinApi.GetWindowRect(hwnd, out Rectangle rect);
            return rect;
        }
        public static void SetWindowOrder(IntPtr hWnd, HWND order) => WinApi.SetWindowPos(hWnd, (IntPtr)order, 0, 0, 0, 0, (uint)(SWP.NO_MOVE | SWP.NO_SIZE | SWP.NO_REDRAW));
        public static void SetWindowRect(IntPtr hWnd, Rectangle rect) => WinApi.SetWindowPos(hWnd, IntPtr.Zero, rect.X, rect.Y, rect.Width, rect.Height, (uint)(SWP.NO_Z_ORDER | SWP.NO_OWNER_Z_ORDER | SWP.NO_REDRAW));
        public static void SetWindowLocation(IntPtr hWnd, Location loc) => WinApi.SetWindowPos(hWnd, IntPtr.Zero, loc.X, loc.Y, 0, 0, (uint)(SWP.NO_Z_ORDER | SWP.NO_OWNER_Z_ORDER | SWP.NO_REDRAW | SWP.NO_SIZE));

        public static void HideWindow(IntPtr hWnd) => WinApi.ShowWindow(hWnd, (int)SW.HIDE); // todo: add silent option. if not silent, focus on the next window in Z order
        public static void UnHideWindow(IntPtr hWnd, bool silent = false)
        {
            SW selectedOption = silent ? SW.SHOW_NO_ACTIVATE : SW.SHOW;
            WinApi.ShowWindow(hWnd, (int)selectedOption);
        }
        public static void SetWindowVisibility(IntPtr hWnd, bool visible)
        {
            SW selectedOption = visible ? SW.SHOW : SW.HIDE;
            WinApi.ShowWindow(hWnd, (int)selectedOption);
        }
        public static void MinimizeWindow(IntPtr hWnd, bool force = false, bool silent = false)
        {
            SW selectedOption = force ? SW.FORCE_MINIMIZE : silent ? SW.SHOW_MINIMIZED_NO_ACTIVATE : SW.MINIMIZE;
            WinApi.ShowWindow(hWnd, (int)selectedOption);
        }
        public static void Maximize(IntPtr hWnd) => WinApi.ShowWindow(hWnd, (int)SW.SHOW_MAXIMIZED);
        public static void Normalize(IntPtr hWnd, bool silent = false)
        {
            SW selectedOption = silent ? SW.NORMAL_NO_ACTIVATE : SW.NORMAL;
            WinApi.ShowWindow(hWnd, (int)selectedOption);
        }
        public static void CloseWindow(IntPtr hWnd) => WinApi.SendMessage(hWnd, (uint)WM.SYSCOMMAND, (IntPtr)SC.CLOSE, IntPtr.Zero);
        public static IntPtr GetForeGroundWindow() => WinApi.GetForegroundWindow();
        public static IntPtr GetActiveWindow() => WinApi.GetActiveWindow();
        public static IntPtr WindowFromPoint(Location Point) => WinApi.WindowFromPoint(Point);


        public class WindowController // not functional, SetWindowLongPtr returns 0
        {
            public readonly IntPtr WindowHandle;

            private readonly Procedure.WindowProc ControlledWindowProc;
            private readonly IntPtr OriginalProc;

            private readonly bool ResetAtExit;

            public WindowController(IntPtr hWnd, bool resetAtExit = true)
            {
                WindowHandle = hWnd;
                ControlledWindowProc = ControlledWndProc;

                OriginalProc = WinApi.GetWindowLongPtr(hWnd, (int)GWL.WND_PROC);

                if(Procedure.SetProc(ControlledWindowProc, hWnd) != IntPtr.Zero)
                    Console.WriteLine("Hook set up");
                else
                    Console.WriteLine("Hook is not set up");

                ResetAtExit = resetAtExit;
            }

            ~WindowController()
            {
                if (ResetAtExit && OriginalProc != IntPtr.Zero)
                {
                    Procedure.SetProc(OriginalProc, WindowHandle);
                }
            }

            private IntPtr ControlledWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
            {
                Console.WriteLine(msg);
                if (msg == (uint)WM.SYSCOMMAND && (wParam.ToInt32() & 0xFFF0) == (int)SC.MOVE)
                    return IntPtr.Zero;

                // Call the original window procedure (if not overridden)
                return WinApi.CallWindowProc(OriginalProc, hWnd, msg, wParam, lParam);
            }
        }


        public static class Procedure
        {
            public delegate IntPtr WindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

            public static IntPtr SetProc(WindowProc hProc, IntPtr hWnd) => // returns original
                WinApi.SetWindowLongPtr(hWnd, (int)GWL.WND_PROC, Marshal.GetFunctionPointerForDelegate(hProc));
            public static IntPtr SetProc(IntPtr hProc, IntPtr hWnd) =>
                WinApi.SetWindowLongPtr(hWnd, (int)GWL.WND_PROC, hProc);
        }
    }
}
namespace WinDirector
{
    public partial class WinApi
    {

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Location Point);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out Rectangle lpRect);

        [DllImport("user32.dll")]
        public static extern int SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);


    }
    public enum GWL
    {
        EX_STYLE = -20,
        H_INSTANCE = -6,
        ID = -12,
        STYLE = -16,
        USER_DATA = -21,
        WND_PROC = -4
    }
    public enum HWND : int
    {
        BOTTOM = 1,
        NOT_TOP_MOST = -2,
        TOP = 0,
        TOP_MOST = -1,
        BROADCAST = 0xFFFF
    }
    public enum SW : int
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
    public enum SWP : uint // SetWindowPos flags
    {
        NO_SIZE = 0x0001,
        NO_MOVE = 0x0002,
        NO_Z_ORDER = 0x0004,
        NO_REDRAW = 0x0008,
        NO_ACTIVATE = 0x0010,
        FRAME_CHANGED = 0x0020,
        SHOW_WINDOW = 0x0040,
        HIDE_WINDOW = 0x0080,
        NO_OWNER_Z_ORDER = 0x0200,
        ASYNC_WINDOW_POS = 0x4000
    }
    public enum WS : long
    {
        BORDER = 0x00800000L,
        CAPTION = 0x00C00000L,
        CHILD = 0x40000000L,
        CHILD_WINDOW = 0x40000000L,
        CLIP_CHILDREN = 0x02000000L,
        CLIP_SIBLINGS = 0x04000000L,
        DISABLED = 0x08000000L,
        DLG_FRAME = 0x00400000L,
        GROUP = 0x00020000L,
        HSCROLL = 0x00100000L,
        ICONIC = 0x20000000L,
        MAXIMIZE = 0x01000000L,
        MAXIMIZE_BOX = 0x00010000L,
        MINIMIZE = 0x20000000L,
        MINIMIZE_BOX = 0x00020000L,
        OVERLAPPED = 0x00000000L,
        POP_UP = 0x80000000L,
        SIZE_BOX = 0x00040000L,
        SYS_MENU = 0x00080000L,
        TAB_STOP = 0x00010000L,
        THICK_FRAME = 0x00040000L,
        TILED = 0x00000000L,
        VISIBLE = 0x10000000L,
        VSCROLL = 0x00200000L
    }

}
