using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Cosmos.Core.Memory;
using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using IL2CPU.API.Attribs;
using Neptune.NDE.Windows;

namespace Neptune.NDE
{
    internal class NDEManager
    {
        public static VBECanvas screen;
        public static Color desktopBackground = Color.DodgerBlue;
        public static Color taskbarColor = Color.DarkViolet;
        public static bool graphicsMode = false;
        public static Image cursor;
        public static List<Window> Windows = new List<Window>();
        public static int ActiveWindow = 0;
        public static int FPS = 0;
        public static uint DisplayColumns = 0;
        public static uint DisplayRows = 0;
        public static int TaskbarHeight = 30;
        public static bool startmenuopen = false;

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
            Resources.cursor = new Bitmap(Resources.cursorbytes);
        }
        /// <summary>
        /// Enables graphics mode.
        /// </summary>
        /// <param name="columns">The amount of columns in the display.</param>
        /// <param name="rows">The amount of rows in the display.</param>
        public static void ChangeToGraphicsMode(uint columns, uint rows)
        {
            screen = (VBECanvas)FullScreenCanvas.GetFullScreenCanvas(new Mode(columns, rows, ColorDepth.ColorDepth32));
            DisplayColumns = columns;
            DisplayRows = rows;
            MouseManager.ScreenWidth = columns;
            MouseManager.ScreenHeight = rows;
            screen.Clear(desktopBackground);
            screen.Display();
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
            screen.Disable();
            graphicsMode = false;
        }
        /// <summary>
        /// Draws the desktop and all windows, and calls Run() in the active window.
        /// </summary>
        public static void Draw()
        {
            UpdateFPS();
            DrawDesktop();

            int i = 0;
            MouseState mouseState = MouseManager.MouseState;
            foreach (var wind in Windows)
            {
                if (wind.Closed)
                {
                    // delete the Window that has closed
                    Windows.Remove(wind);
                }
                else
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
                            NDEMessageBox.ShowMsgBox("Error in " + wind.Title,e.ToString(),200,30,50);
                            Windows.RemoveAt(i);
                        }
                    }
                    if (MouseManager.MouseState == MouseState.Left)
                    {
                        if ((MouseManager.X >= wind.PositionX && MouseManager.X <= wind.PositionX + wind.SizeX-30) && (MouseManager.Y >= wind.PositionY - 30 && MouseManager.Y <= wind.PositionY))
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
            if (!startmenuopen)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if ((MouseManager.X >= DisplayColumns - TaskbarHeight) && MouseManager.Y <= TaskbarHeight)
                    {
                        startmenuopen = true;
                        OpenWindow(new StartMenuWindow());
                    }
                }
            }
            DrawCursor();
            screen.Display();
            framecount++;
            if (framecount >= 20)
            {
                Heap.Collect();
                framecount = 0;
            }
        }
        /// <summary>
        /// Draws the desktop.
        /// </summary>
        public static void DrawDesktop()
        {
            screen.Clear(desktopBackground);
            screen.DrawFilledRectangle(taskbarColor, 0, 0, (int)DisplayColumns-TaskbarHeight, TaskbarHeight);
            screen.DrawFilledRectangle(Color.Aqua, (int)(DisplayColumns - TaskbarHeight), 0,TaskbarHeight,TaskbarHeight);
            screen.DrawString("Neptune | FPS: " + FPS.ToString() + " | Active window: " + ActiveWindow.ToString(), PCScreenFont.Default,Color.White,5,8);
        }
        public static void DrawCursor()
        {
            screen.DrawImageAlpha(Resources.cursor, (int)MouseManager.X, (int)MouseManager.Y);
        }
        public static void OpenWindow(Window window)
        {
            window.Open();
            Windows.Add(window);
            ActiveWindow = Windows.Count - 1;
        }
    }
}
