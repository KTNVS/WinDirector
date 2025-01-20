using System;
using System.Linq;
using System.Management;
using System.Text;

namespace WinDirector.Hardware
{
    public static class SystemInfo
    {
        // https://stackoverflow.com/questions/9482253/is-there-really-any-way-to-uniquely-identify-any-computer-at-all
        private static string GetManagementInfo(string StrKey_String, string strIndex)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + StrKey_String);

            string strHwInfo = null;
            foreach (ManagementObject share in searcher.Get().Cast<ManagementObject>())
                strHwInfo += share[strIndex];

            return strHwInfo;
        }

        public static string MachineName => Environment.MachineName;
        public static string MacAddress => GetManagementInfo("Win32_NetworkAdapterConfiguration", "MacAddress");
        public static string ProcessorID => GetManagementInfo("Win32_Processor", "ProcessorId");
        public static string MotherboardID => GetManagementInfo("Win32_BaseBoard", "SerialNumber");

        public static int GetUniqueSystemIdentifier()
        {
            StringBuilder uniqueID = new StringBuilder();
            uniqueID.Append(ProcessorID);
            uniqueID.Append(MotherboardID);

            return uniqueID.ToString().GetHashCode();
        }
    }
}
namespace WinDirector
{
    public enum SC
    {
        SIZE = 0xF000,
        MOVE = 0xF010,
        MINIMIZE = 0xF020,
        MAXIMIZE = 0xF030,
        NEXTWINDOW = 0xF040,
        PREVWINDOW = 0xF050,
        CLOSE = 0xF060,
        VSCROLL = 0xF070,
        HSCROLL = 0xF080,
        MOUSEMENU = 0xF090,
        KEYMENU = 0xF100,
        ARRANGE = 0xF110,
        RESTORE = 0xF120,
        TASKLIST = 0xF130,
        SCREENSAVE = 0xF140,
        HOTKEY = 0xF150,
        DEFAULT = 0xF160,
        MONITORPOWER = 0xF170,
        CONTEXTHELP = 0xF180,
        SEPARATOR = 0xF00F
    }
}