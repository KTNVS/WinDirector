using System;
using System.Runtime.InteropServices;



namespace WinDirector
{
    public partial class WinApi
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, Input.Input[] pInputs, int cbSize);

    }
    public enum InputType : uint
    {
        Mouse = 0,
        Keyboard = 1,
        Hardware = 2
    }
}
namespace WinDirector.Input 
{
    internal static class InputBuilder
    {
        public static Input[] BuildKeyInput(KeyCode key, KeyAction keyAction)
        {
            Input[] input = new Input[1];
            input[0].type = (uint)InputType.Keyboard;
            input[0].u.ki.wVk = (ushort)key;
            input[0].u.ki.wScan = (ushort)WinApi.MapVirtualKey((uint)key, 0);
            input[0].u.ki.time = 0;
            input[0].u.ki.dwExtraInfo = IntPtr.Zero;

            KEYEVENTF flags = keyAction == KeyAction.KeyDown ? KEYEVENTF.KEYDOWN : KEYEVENTF.KEYUP;

            if (Key.IsExtendedKey(key))
                flags |= KEYEVENTF.EXTENDEDKEY;

            input[0].u.ki.dwFlags = (uint)flags;

            return input;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct Input
    {
        public uint type;
        public InputUnion u;
    }


    [StructLayout(LayoutKind.Explicit)]
    public struct InputUnion
    {
        [FieldOffset(0)]
        public MOUSEINPUT mi;
        [FieldOffset(0)]
        public KEYBDINPUT ki;
        [FieldOffset(0)]
        public HARDWAREINPUT hi;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public uint mouseData;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct HARDWAREINPUT
    {
        public uint uMsg;
        public ushort wParamL;
        public ushort wParamH;
    }
}
