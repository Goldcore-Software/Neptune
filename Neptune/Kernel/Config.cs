using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Neptune
{
    internal class Config
    {
        public static Dictionary<string, string> conf = new Dictionary<string, string>();
        public static StringBuilder sb = new StringBuilder();
        public static string DefaultPath;
        public static void SaveReg(string path)
        {
            sb.Clear();
            foreach (var regkey in conf)
            {
                sb.AppendLine(regkey.Key+";"+regkey.Value);
            }
            File.WriteAllText(path, sb.ToString());
            sb.Clear();
        }
        public static void LoadReg(string path)
        {
            string regserialized = File.ReadAllText(path);
            string[] keyvalue;
            conf.Clear();
            foreach (var item in regserialized.Split("\n"))
            {
                if (!(item == string.Empty))
                {
                    keyvalue = item.Split(";");
                    conf.Add(keyvalue[0], keyvalue[1]);
                }
            }
        }
        public static void SaveReg()
        {
            SaveReg(DefaultPath);
        }
        public static void LoadReg()
        {
            LoadReg(DefaultPath);
        }
    }
}
