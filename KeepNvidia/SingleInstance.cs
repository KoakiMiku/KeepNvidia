using System;
using System.Diagnostics;
using System.Linq;

namespace KeepNvidia
{
    class SingleInstance
    {
        private static readonly string name = "KeepNvidia";

        public static bool IsSingle()
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(name);
                if (processes.Count() == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(null, ex.InnerException ?? ex);
            }
        }
    }
}
