using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace AreaCalc.Tools
{
    public class IniTool
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int WritePrivateProfileStringW(string section, string key, string val, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileStringW(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static string Read(string section, string key, string def, string filePath)
        {
            StringBuilder stringBuilder = new StringBuilder(1024);
            IniTool.GetPrivateProfileStringW(section, key, def, stringBuilder, 1024, filePath);
            return stringBuilder.ToString();
        }

        public static int Write(string section, string key, string value, string filePath)
        {
            if (File.Exists(filePath))
            {
                return IniTool.WritePrivateProfileStringW(section, key, value, filePath);
            }
            return 0;
        }

        public static int DeleteSection(string section, string filePath)
        {
            return IniTool.Write(section, null, null, filePath);
        }

        public static int DeleteKey(string section, string key, string filePath)
        {
            return IniTool.Write(section, key, null, filePath);
        }
    }
}