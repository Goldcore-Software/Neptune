using Cosmos.Core.Memory;
using Cosmos.System.Graphics.Fonts;
using Microsoft.Win32;
using Neptune.NDE;
using Neptune.Terminal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Sys = Cosmos.System;

namespace Neptune
{
    public class Kernel : Sys.Kernel
    {
        private static bool running = true;
        public static bool graphicsmode = false;
        public readonly static int majorversion = 1;
        public readonly static int minorversion = 0;
        public readonly static int commit = 1; // only update on commits that change the code! if you are just changing the readme or some assets then don't increment this
        public readonly static string branch = "Development";
        public readonly static string OSName = "Neptune";
        public readonly static string VersionString = OSName + " " + branch + " " + majorversion.ToString() + "." + minorversion.ToString() + "-" + commit;
        public static int SystemDrive { get; set; } = -1;
        protected override void BeforeRun()
        {
            try
            {
                Console.WriteLine("Starting Neptune...");
                NDEManager.Initialize();
                Sys.FileSystem.CosmosVFS fs = new Sys.FileSystem.CosmosVFS();
                Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
                TerminalLogger.LogTerminal = 0;
                TerminalLogger.Log("NKernel", "File system initialized!", LogType.Ok);
                TerminalLogger.Log("NKernel", "Searching for system drive...", LogType.Info);
                for (int i = 0; i < 9; i++)
                {
                    if (File.Exists(i + @":\Neptune\sys.conf"))
                    {
                        SystemDrive = i;
                        TerminalLogger.Log("NKernel", "Found system drive at \"" + i + ":\\\".", LogType.Ok);
                        break;
                    }
                }
                if (SystemDrive == -1)
                {
                    TerminalLogger.Log("NKernel", "Cannot find system drive. You must set up a drive yourself by typing \"setup\" in the drive that you want to be your system drive.", LogType.Warning);
                }
                else
                {
                    try
                    {
                        Config.DefaultPath = SystemDrive + @":\Neptune\conf.reg";
                        Config.LoadReg();
                        TerminalLogger.Log("NKernel", "Loaded system registry.", LogType.Ok);
                    }
                    catch (Exception e)
                    {
                        TerminalLogger.Log("NKernel", "Failed to load system registry! The system can still boot but many features will be broken." + e.ToString(), LogType.Error);
                    }
                }
            }
            catch (Exception e)
            {
                Crash(e);
            }
            
        }

        protected override void Run()
        {
            if (running)
            {
                if (graphicsmode)
                {
                    try
                    {
                        NDEManager.Draw();
                    }
                    catch (Exception e)
                    {
                        NDEManager.Windows.Clear();
                        Heap.Collect();
                        NDEManager.screen.Clear(Color.Red);
                        NDEManager.screen.DrawString(e.ToString(), PCScreenFont.Default, Color.White, 2, 2);
                        NDEManager.screen.Display();
                        running = false;
                    }
                }
                else
                {
                    try
                    {
                        Console.Write(nsh.GetFullPath()+"> ");
                        string cmd = Console.ReadLine();
                        nsh.Command(cmd);
                    }
                    catch (Exception e)
                    {
                        Crash(e);
                    }
                }
            }
        }
        private static void Crash(Exception e)
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            Console.WriteLine("Neptune has crashed.", -1);
            Console.WriteLine("The exception was: " + e.ToString(), -1);
            Console.WriteLine("Please contact the developer, eli310#9755!", -1);
            Console.WriteLine("You need to manually restart", -1);
            while (true)
            {

            }
        }
    }
}
