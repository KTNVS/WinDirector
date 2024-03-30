using System;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace WinDirector.Processes
{
    public class ProcessWatcher
    {
        private const string SCOPE = @"\\.\root\CIMV2";

        public event Action OnProcessStart, OnProcessEnd;

        private readonly ManagementEventWatcher StartWatcher, EndWatcher;
        public ProcessWatcher(string processName)
        {
            string ProcessCreationQuery = $"SELECT TargetInstance FROM __InstanceCreationEvent WITHIN 10 WHERE TargetInstance ISA 'Win32_Process' AND TargetInstance.Name = '{processName}'";
            string ProcessDeletionQuery = $"SELECT TargetInstance FROM __InstanceDeletionEvent WITHIN 10 WHERE TargetInstance ISA 'Win32_Process' AND TargetInstance.Name = '{processName}'";

            StartWatcher = new ManagementEventWatcher(SCOPE, ProcessCreationQuery);
            EndWatcher = new ManagementEventWatcher(SCOPE, ProcessDeletionQuery);

            StartWatcher.EventArrived += StartWatcher_EventArrived;
            EndWatcher.EventArrived += EndWatcher_EventArrived;

            StartWatcher.Start();
            EndWatcher.Start();

            Process currentProcess = Process.GetCurrentProcess();
        }
        ~ProcessWatcher()
        {
            StartWatcher?.Stop();
            EndWatcher?.Stop();
        }

        private void StartWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            OnProcessStart?.Invoke();
        }
        private void EndWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            OnProcessEnd?.Invoke();
        }
    }
    public static class ProcessInfo
    {
        public static bool IsWorking(string name) => Process.GetProcessesByName(name).Any();
        public static Process GetProcessByName(string name) => Process.GetProcessesByName(name).First();
    }
}