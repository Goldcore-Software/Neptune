using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Cosmos.Core.Memory;
using Cosmos.System;
using IL2CPU.API.Attribs;
using GrapeGL.Hardware.GPU;
using GrapeGL.Graphics;
using GrapeGL.Graphics.Fonts;
using Neptune.NDE.Windows;
using GrapeGL.Hardware.GPU.VESA;
using Color = GrapeGL.Graphics.Color;
using GrapeGL.Hardware.GPU.VMWare;

namespace Neptune.NDE
{
    internal class NDEManager
    {
        public static Display screen;
        public static Color desktopBackground = Color.ClassicBlue;
        public static Color taskbarColor = Color.UbuntuPurple;
        public static bool graphicsMode = false;
        public static Canvas cursor;
        public static List<Window> Windows = new List<Window>();
        public static int ActiveWindow = 0;
        public static int FPS = 0;
        public static uint DisplayColumns = 0;
        public static uint DisplayRows = 0;
        public static int TaskbarHeight = 30;
        public static bool startmenuopen = false;
        public static DesktopWindow desktop = new DesktopWindow();

        private static int framecount = 0;
        private static bool draggingwindow = false;
        private static uint draggingx = 0;
        private static uint draggingy = 0;
        private static int draggingindex = 0;
        private static uint draggingoldx = 0;
        private static uint draggingoldy = 0;
        private static int LastS = -1;
        private static int Ticken = 0;

        public static void UpdateFPS()
        {
            if (LastS == -1)
            {
                LastS = DateTime.UtcNow.Second;
            }
            if (DateTime.UtcNow.Second - LastS != 0)
            {
                if (DateTime.UtcNow.Second > LastS)
                {
                    FPS = Ticken / (DateTime.UtcNow.Second - LastS);
                }
                LastS = DateTime.UtcNow.Second;
                Ticken = 0;
            }
            Ticken++;
        }
        public static void Initialize()
        {
            Resources.cursor = Image.FromBitmap(Resources.cursorbytes);
        }
        /// <summary>
        /// Enables graphics mode.
        /// </summary>
        /// <param name="columns">The amount of columns in the display.</param>
        /// <param name="rows">The amount of rows in the display.</param>
        public static void ChangeToGraphicsMode(uint columns, uint rows)
        {
            screen = SVGAIICanvas.GetDisplay((ushort)columns, (ushort)rows);
            DisplayColumns = columns;
            DisplayRows = rows;
            MouseManager.ScreenWidth = columns;
            MouseManager.ScreenHeight = rows;
            desktop.Open();
            screen.IsEnabled = true;
            screen.Clear(desktopBackground);
            screen.Update();
            graphicsMode = true;
            NDEMessageBox.ShowMsgBox("Message box test","Message Box!",200,100,100);
            NDEMessageBox.ShowMsgBox("Second box","This is a second box",200,130,190);
        }
        /// <summary>
        /// Disables graphics mode.
        /// </summary>
        public static void ExitGraphicsMode()
        {
            Windows.Clear();
            screen.IsEnabled = false;
            graphicsMode = false;
        }
        /// <summary>
        /// Draws the desktop and all windows, and calls Run() in the active window.
        /// </summary>
        public static void Draw()
        {
            // TODO: seriously why does GrapeGL make my OS slower I now need to optimize this
            UpdateFPS();
            desktop.Draw();
            int i = 0;
            MouseState mouseState = MouseManager.MouseState;
            bool draw = true;
            // check if there is a full screen window that is active
            if (Windows.Count != 0 && Windows.Count >= ActiveWindow + 1)
            {
                if (Windows[ActiveWindow].Fullscreen)
                {
                    draw = false;
                }
            }
            // if there is, don't bother drawing any other window as the user won't see it anyway
            foreach (var wind in Windows)
            {
                if (wind.Closed)
                {
                    // delete the Window that has closed
                    Windows.Remove(wind);
                }
                else if (draw)
                {
                    if (i != ActiveWindow)
                    {
                        try
                        {
                            wind.Active = false;
                            // draw the title bar and window background
                            wind.DrawTitleBar();
                            // draw all controls of the window
                            wind.Draw();
                        }
                        catch (Exception e)
                        {
                            NDEMessageBox.ShowMsgBox("Error in " + wind.Title, e.ToString(), 200, 30, 50);
                            Windows.RemoveAt(i);
                        }
                    }
                    if (MouseManager.MouseState == MouseState.Left)
                    {
                        if ((MouseManager.X >= wind.PositionX && MouseManager.X <= wind.PositionX + wind.SizeX - 30) && (MouseManager.Y >= wind.PositionY - 30 && MouseManager.Y <= wind.PositionY))
                        {
                            if (wind.HasTitleBar)
                            {
                                if (!draggingwindow) { draggingwindow = true; draggingx = MouseManager.X; draggingy = MouseManager.Y; draggingindex = i; draggingoldx = (uint)wind.PositionX; draggingoldy = (uint)wind.PositionY; wind.Dragging = true; }
                            }
                        }
                        if ((MouseManager.X >= wind.PositionX && MouseManager.X <= wind.PositionX + wind.SizeX) && (MouseManager.Y >= wind.PositionY - 30 && MouseManager.Y <= wind.PositionY + wind.SizeY))
                        {
                            ActiveWindow = i;
                        }
                    }
                    else
                    {
                        draggingwindow = false;
                        wind.Dragging = false;
                    }
                    if (draggingwindow && draggingindex == i)
                    {
                        wind.PositionX = (int)(draggingoldx + (MouseManager.X - draggingx));
                        wind.PositionY = (int)(draggingoldy + (MouseManager.Y - draggingy));
                    }
                }
                i++;
            }
            if (Windows.Count != 0 && Windows.Count >= ActiveWindow+1)
            {
                try
                {
                    // let the window know that it is active
                    Windows[ActiveWindow].Active = true;
                    // draw the titlebar and window background
                    Windows[ActiveWindow].DrawTitleBar();
                    // draw all controls of the window
                    Windows[ActiveWindow].Draw();
                    // run the code of the window
                    Windows[ActiveWindow].Run();
                }
                catch (Exception e)
                {
                    NDEMessageBox.ShowMsgBox("Error in " + Windows[ActiveWindow].Title, e.ToString(), 200, 30, 50);
                    Windows.RemoveAt(ActiveWindow);
                }
            }
            desktop.Run();
            DrawCursor();
            screen.Update();
            framecount++;
            
            if (framecount >= 20)
            {
                Heap.Collect();
                framecount = 0;
            }
        }
        public static void DrawCursor()
        {
            screen.DrawImage((int)MouseManager.X, (int)MouseManager.Y, Resources.cursor, true);
        }
        public static void OpenWindow(Window window)
        {
            window.Open();
            Windows.Add(window);
            ActiveWindow = Windows.Count - 1;
        }
    }
}
