using System;
using System.Runtime.InteropServices;
using WinDirector.WinApiHandler;

namespace WinDirector.WinApiHandler
{
    public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    
    public static class HookManager
    {
        public static IntPtr SetHook(HookProc hProc, WH hookType) => // Returns hook ID
            WinApi.SetWindowsHookEx((int) hookType, hProc, IntPtr.Zero, 0);
        public static void UnHook(IntPtr hookID) =>
            WinApi.UnhookWindowsHookEx(hookID);
    }
}
namespace WinDirector
{
    public partial class WinApi
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);
    }
    public enum WH : int
    {
        JOURNALRECORD = 0,
        JOURNALPLAYBACK = 1,
        KEYBOARD = 2,
        GETMESSAGE = 3,
        CALLWNDPROC = 4,
        CBT = 5,
        SYSMSGFILTER = 6,
        MOUSE = 7,
        HARDWARE = 8,
        DEBUG = 9,
        SHELL = 10,
        FOREGROUNDIDLE = 11,
        CALLWNDPROCRET = 12,
        KEYBOARD_LL = 13,
        MOUSE_LL = 14
    }
}