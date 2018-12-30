using System;
using System.Collections.Generic;
using System.Globalization;

namespace KeepNvidia
{
    class I18N
    {
        private static readonly Dictionary<string, string> Chinese = new Dictionary<string, string>()
        {
            {"Setup", "安装 或 卸载 KeepNvidia\n是：安装\t否：卸载\t取消：取消"},
        };

        private static readonly Dictionary<string, string> English = new Dictionary<string, string>()
        {
            {"Setup", "Install or uninstall KeepNvidia\nYes: Install\tNo: Uninstall\tCancel: Cancel"},
        };

        public static string GetString(string value)
        {
            try
            {
                switch (CultureInfo.InstalledUICulture.Name)
                {
                    case "zh-CN":
                        return Chinese[value];
                    default:
                        return English[value];
                }
            }
            catch (Exception)
            {
                return value;
            }
        }
    }
}
