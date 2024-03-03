using Cosmos.Core.Memory;
//using Cosmos.System.Graphics.Fonts;
using Microsoft.Win32;
using Neptune.NDE;
using Neptune.Terminal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sys = Cosmos.System;
using GrapeGL.Hardware.GPU;
using GrapeGL.Graphics;
using GrapeGL.Graphics.Fonts;
using SVGAIITerminal;
using SVGAIITerminal.TextKit;

namespace Neptune
{
    public class Kernel : Sys.Kernel
    {
        private static bool running = true;
        public static bool graphicsmode = false;
        public readonly static int majorversion = 1;
        public readonly static int minorversion = 0;
        public readonly static int commit = 7; // only update on commits that change the code! if you are just changing the readme or some assets then don't increment this
        public readonly static string branch = "Development";
        public readonly static string OSName = "Neptune";
        public readonly static string VersionString = OSName + " " + branch + " " + majorversion.ToString() + "." + minorversion.ToString() + "-" + commit;
        public static SVGAIITerminal.SVGAIITerminal tty;
        public static int SystemDrive { get; set; } = -1;
        protected override void BeforeRun()
        {
            try
            {
                tty = new SVGAIITerminal.SVGAIITerminal(800, 600, new AcfFontFace(new MemoryStream(Resources.cascadia)));
                tty.WriteLine("Starting Neptune...");
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
                        NDEManager.screen.DrawString(2, 2, e.ToString(), Font.Fallback, Color.White);
                        NDEManager.screen.Update();
                        running = false;
                    }
                }
                else
                {
                    try
                    {
                        tty.Write(nsh.GetFullPath()+"> ");
                        string cmd = tty.ReadLine();
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
            tty.BackgroundColor = Color.Red;
            tty.ForegroundColor = Color.White;
            tty.Clear();
            tty.WriteLine("Neptune has crashed.");
            tty.WriteLine("The exception was: " + e.ToString());
            tty.WriteLine("Please contact the developer, eli310#9755!");
            tty.WriteLine("You need to manually restart");
            while (true)
            {

            }
        }
    }
}
