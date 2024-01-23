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
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("[" + messager + "] ", LogTerminal);
            switch (type)
            {
                case LogType.Info:
                    Console.Write("[INFO] ", LogTerminal);
                    break;
                case LogType.Fatal:
                    Console.Write("[", LogTerminal);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("FATAL", LogTerminal);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("] ", LogTerminal);
                    break;
                case LogType.Error:
                    Console.Write("[", LogTerminal);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("ERROR", LogTerminal);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("] ", LogTerminal);
                    break;
                case LogType.Warning:
                    Console.Write("[", LogTerminal);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("WARN", LogTerminal);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("] ", LogTerminal);
                    break;
                case LogType.Ok:
                    Console.Write("[", LogTerminal);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("OK", LogTerminal);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("] ", LogTerminal);
                    break;
                default:
                    break;
            }
            Console.WriteLine(message, LogTerminal);
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
