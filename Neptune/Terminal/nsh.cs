using Cosmos.System.FileSystem;
using IL2CPU.API.Attribs;
using Neptune.NDE;
using SVGAIITerminal.TextKit;
using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace Neptune.Terminal
{
    internal class nsh
    {
        public static string CurrentDir { get; private set; }
        public static string CurrentVol { get; private set; } = "0";
        public static void Command(string cmd)
        {
            string[] cmdsplit = cmd.Split(' ');
            try
            {
                switch (cmdsplit[0])
                {
                    case "cd":
                        if (cmdsplit[1] == "..")
                        {
                            if (CurrentDir == "")
                            {
                                break;
                            }
                            char currletter = CurrentDir[CurrentDir.Length - 1];
                            while (!(currletter == "\\".ToCharArray()[0]))
                            {
                                CurrentDir = CurrentDir.Remove(CurrentDir.Length - 1);
                                if (CurrentDir.Length == 0) { break; }
                                currletter = CurrentDir[CurrentDir.Length - 1];
                            }
                            if (CurrentDir.Length == 0) { break; }
                            CurrentDir = CurrentDir.Remove(CurrentDir.Length - 1);
                            break;
                        }
                        string bdir = CurrentDir;
                        if (CurrentDir == "")
                        {
                            CurrentDir += cmdsplit[1];
                        }
                        else
                        {
                            CurrentDir += "\\" + cmdsplit[1];
                        }
                        if (!Directory.Exists(CurrentVol + ":\\" + CurrentDir))
                        {
                            CurrentDir = bdir;
                            Kernel.tty.WriteLine("Directory not found!");
                        }
                        break;
                    case "cls":
                        Kernel.tty.Clear();
                        break;
                    case "logtest":
                        TerminalLogger.Log("Log Test!", "This is a test of the logger 1", LogType.Info);
                        TerminalLogger.Log("Log Test!", "This is a test of the logger 2", LogType.Fatal);
                        TerminalLogger.Log("Log Test!", "This is a test of the logger 3", LogType.Error);
                        TerminalLogger.Log("Log Test!", "This is a test of the logger 4", LogType.Warning);
                        TerminalLogger.Log("Log Test!", "This is a test of the logger 5", LogType.Ok);
                        break;
                    case "crashtest":
                        throw new SystemException("you broke it.");
                    case "md":
                    case "mkdir":
                        Directory.CreateDirectory(GetFullPath(cmdsplit[1]));
                        break;
                    case "rd":
                    case "rmdir":
                        Directory.Delete(GetFullPath(cmdsplit[1]), true);
                        break;
                    case "del":
                        File.Delete(GetFullPath(cmdsplit[1]));
                        break;
                    case "dir":
                    case "ls":
                        string[] filePaths = Directory.GetFiles(GetFullPath());
                        var drive = new DriveInfo(CurrentVol);
                        Kernel.tty.WriteLine("Volume in drive 0 is " + $"{drive.VolumeLabel}");
                        Kernel.tty.WriteLine("Directory of " + GetFullPath());
                        Kernel.tty.WriteLine("\n");
                        if (filePaths.Length == 0 && Directory.GetDirectories(GetFullPath()).Length == 0)
                        {
                            Kernel.tty.WriteLine("File Not Found");
                        }
                        else
                        {
                            for (int i = 0; i < filePaths.Length; ++i)
                            {
                                string path = filePaths[i];
                                Kernel.tty.WriteLine(Path.GetFileName(path));
                            }
                            foreach (var d in Directory.GetDirectories(GetFullPath()))
                            {
                                var dir = new DirectoryInfo(d);
                                var dirName = dir.Name;

                                Kernel.tty.WriteLine(dirName + " <DIR>");
                            }
                        }
                        Kernel.tty.WriteLine("\n");
                        Kernel.tty.WriteLine("        " + $"{drive.TotalSize}" + " bytes");
                        Kernel.tty.WriteLine("        " + $"{drive.AvailableFreeSpace}" + " bytes free");
                        break;
                    case "writefile":
                        string contents = cmd.Substring(cmdsplit[0].Length + cmdsplit[1].Length + 2);
                        File.WriteAllText(GetFullPath(cmdsplit[1]), contents);
                        break;
                    case "cat":
                        if (File.Exists(GetFullPath(cmdsplit[1])))
                        {
                            Kernel.tty.WriteLine("File exists! (File.Exists())");
                        }
                        else
                        {
                            Kernel.tty.WriteLine("File does not exist! (File.Exists())");
                        }
                        Kernel.tty.WriteLine(File.ReadAllText(GetFullPath(cmdsplit[1])));
                        break;
                    case "debug":
                        switch (cmdsplit[1])
                        {
                            case "currentdir":
                                Kernel.tty.WriteLine(CurrentDir);
                                break;
                            case "currentvol":
                                Kernel.tty.WriteLine(CurrentVol);
                                break;
                            default:
                                break;
                        }
                        break;
                    case "setup":
                        Kernel.tty.WriteLine("Setting up system drive...");
                        Directory.CreateDirectory(CurrentVol + @":\Neptune");
                        Kernel.SystemDrive = int.Parse(CurrentVol);
                        Config.DefaultPath = CurrentVol + @":\Neptune\sys.conf";
                        Config.SaveReg();
                        Kernel.tty.WriteLine("You must reboot the system to finish setting up the system drive.");
                        break;
                    case "reboot":
                        Kernel.tty.WriteLine("Restarting...", 0);
                        if (!(Kernel.SystemDrive == -1))
                        {
                            Config.SaveReg();
                        }
                        Cosmos.System.Power.Reboot();
                        break;
                    case "startnde":
                        Kernel.graphicsmode = true;
                        NDEManager.ChangeToGraphicsMode(800, 600);
                        break;
                    case "ver":
                        Kernel.tty.WriteLine(Kernel.VersionString);
                        break;
                    case "font":
                        switch (cmdsplit[1])
                        {
                            case "12":
                                Kernel.tty = new SVGAIITerminal.SVGAIITerminal(800, 600, new AcfFontFace(new MemoryStream(Resources.vga12)));
                                Kernel.tty.Clear();
                                break;
                            case "18":
                                Kernel.tty = new SVGAIITerminal.SVGAIITerminal(800, 600, new AcfFontFace(new MemoryStream(Resources.vga18)));
                                Kernel.tty.Clear();
                                break;
                            default:
                                Kernel.tty.WriteLine("Font does not exist.");
                                break;
                        }
                        break;
                    case "format":
                        try
                        {
                            var dindex = int.Parse(cmdsplit[1]);
                            var pindex = int.Parse(cmdsplit[2]);
                        }
                        catch (Exception)
                        {
                            Kernel.tty.WriteLine("Usage: format <disk index> <partition index>");
                        }
                        Kernel.fs.Disks[0].FormatPartition(0, "FAT32", true);
                        break;
                    case "diskinfo":
                        var disk = 0;
                        foreach (var item in Kernel.fs.Disks)
                        {
                            Kernel.tty.WriteLine("== DISK "+disk.ToString()+" ==");
                            foreach (var part in item.Partitions)
                            {
                                Kernel.tty.WriteLine(part.RootPath+" - Formatted: "+part.HasFileSystem.ToString());
                            }
                            disk++;
                            Kernel.tty.WriteLine();
                        }
                        break;
                    case "":
                        break;
                    default:
                        if (cmdsplit[0].EndsWith(":") && cmdsplit[0].Length == 2)
                        {
                            try
                            {
                                Directory.GetFiles(cmdsplit[0] + "\\");
                                CurrentVol = cmdsplit[0][0].ToString();
                            }
                            catch (Exception)
                            {
                                Kernel.tty.WriteLine("Could not change drive!");
                            }
                            break;
                        }
                        else
                        {
                            Kernel.tty.WriteLine("Command not found!");
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                if (e.GetType().Name == "SystemException")
                {
                    throw;
                }
                Kernel.tty.WriteLine(cmdsplit[0] + ": " + e.Message);
            }
            
        }
        /// <summary>
        /// Get the full path of a file.
        /// </summary>
        /// <param name="name">The filename.</param>
        /// <returns>The full path.</returns>
        public static string GetFullPath(string name)
        {
            if (CurrentDir == "")
            {
                return CurrentVol + @":\" + name;
            }
            else
            {
                return CurrentVol + @":\" + CurrentDir + "\\" + name;
            }
        }
        public static string GetFullPath()
        {
            if (CurrentDir == "")
            {
                return CurrentVol + @":\";
            }
            else
            {
                return CurrentVol + @":\" + CurrentDir;
            }
        }
        private static byte[] AssemblyCallback(string dll)
        {
            switch (dll)
            {
                case "System.Private.CoreLib":
                    return File.ReadAllBytes(Kernel.SystemDrive + @":\Neptune\dotnet\mscorlib.dll");
                default:
                    return null;
            }
        }
    }
}
