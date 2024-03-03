using GrapeGL.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptune.Terminal
{
    internal class TerminalLogger
    {
        public static int LogTerminal = 1;
        public static void Log(string messager, string message, LogType type)
        {
            Kernel.tty.ForegroundColor = Color.White;
            Kernel.tty.Write("[" + messager + "] ");
            switch (type)
            {
                case LogType.Info:
                    Kernel.tty.Write("[INFO] ");
                    break;
                case LogType.Fatal:
                    Kernel.tty.Write("[");
                    Kernel.tty.ForegroundColor = Color.Red;
                    Kernel.tty.Write("FATAL");
                    Kernel.tty.ForegroundColor = Color.White;
                    Kernel.tty.Write("] ");
                    break;
                case LogType.Error:
                    Kernel.tty.Write("[");
                    Kernel.tty.ForegroundColor = Color.Red;
                    Kernel.tty.Write("ERROR");
                    Kernel.tty.ForegroundColor = Color.White;
                    Kernel.tty.Write("] ");
                    break;
                case LogType.Warning:
                    Kernel.tty.Write("[");
                    Kernel.tty.ForegroundColor = Color.Yellow;
                    Kernel.tty.Write("WARN");
                    Kernel.tty.ForegroundColor = Color.White;
                    Kernel.tty.Write("] ");
                    break;
                case LogType.Ok:
                    Kernel.tty.Write("[");
                    Kernel.tty.ForegroundColor = Color.Green;
                    Kernel.tty.Write("OK");
                    Kernel.tty.ForegroundColor = Color.White;
                    Kernel.tty.Write("] ");
                    break;
                default:
                    break;
            }
            Kernel.tty.WriteLine(message);
        }
    }
    enum LogType
    {
        Info,
        Fatal,
        Error,
        Warning,
        Ok
    }
}
