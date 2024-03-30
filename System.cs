using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace WinDirector.System
{
    public static class SystemInfo
    {
        // https://stackoverflow.com/questions/9482253/is-there-really-any-way-to-uniquely-identify-any-computer-at-all
        private static string GetManagementInfo(string StrKey_String, string strIndex)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + StrKey_String);

            string strHwInfo = null;
            foreach (ManagementObject share in searcher.Get())
            {
                strHwInfo += share[strIndex];
            }
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
