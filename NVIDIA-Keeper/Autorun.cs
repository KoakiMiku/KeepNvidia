using Microsoft.Win32;
using System;
using System.Diagnostics;

namespace NVIDIAKeeper
{
    class Autorun
    {
        private static readonly string name = "NVIDIA-Keeper";

        private static readonly string path = Process.GetCurrentProcess().MainModule.FileName;
        private static readonly string autorunPath = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static void Add()
        {
            try
            {
                RegistryKey autorun = Registry.CurrentUser.OpenSubKey(autorunPath, true);
                autorun.SetValue(name, $"{path} -start");
                autorun.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(null, ex.InnerException ?? ex);
            }
        }

        public static void Remove()
        {
            try
            {
                RegistryKey autoRun = Registry.CurrentUser.OpenSubKey(autorunPath, true);
                autoRun.DeleteValue(name, false);
                autoRun.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(null, ex.InnerException ?? ex);
            }
        }
    }
}
