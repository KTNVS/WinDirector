using System;
using System.Runtime.InteropServices;

namespace WinDirector.WinApiHandler
{
    public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
    public enum WH : int
    {
        KEYBOARD_LL = 13,
        MOUSE_LL = 14
    }

    public static class HookManager
    {
        public static IntPtr SetHook(HookProc hProc, WH hookType) // Returns hook ID
        {
            return SetWindowsHookEx((int)hookType, hProc, IntPtr.Zero, 0);
        }
        public static void UnHook(IntPtr hookID)
        {
            UnhookWindowsHookEx(hookID);
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern bool UnhookWindowsHookEx(IntPtr idHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);
    }
}
