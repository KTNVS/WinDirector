using Microsoft.Win32;

namespace WinDirector.System.Admin
{
    public static class TaskManager
    {
        private const string TaskMgrRegKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Policies\System";
        public static bool Enabled
        {
            get
            {
                RegistryKey taskMgrRegistryKey = Registry.CurrentUser.CreateSubKey(TaskMgrRegKeyPath);
                bool isEnabled = taskMgrRegistryKey.GetValue("DisableTaskMgr") == null;
                taskMgrRegistryKey.Close();
                return isEnabled;
            }
            set
            {
                RegistryKey taskMgrRegistryKey = Registry.CurrentUser.CreateSubKey(TaskMgrRegKeyPath);
                if (value && taskMgrRegistryKey.GetValue("DisableTaskMgr") != null)
                {
                    taskMgrRegistryKey.DeleteValue("DisableTaskMgr");
                }
                else if (!value)
                {
                    taskMgrRegistryKey.SetValue("DisableTaskMgr", "1");
                }
                taskMgrRegistryKey.Close();
            }
        }
    }
}
