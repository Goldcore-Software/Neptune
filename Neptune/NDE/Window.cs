using Cosmos.System;
using Cosmos.System.Graphics.Fonts;
using System;
using System.Drawing;

namespace Neptune.NDE
{
    public abstract class Window
    {
        public string Title = "Window";
        private string titlesh = "Window";
        public int SizeX = 640;
        public int SizeY = 420;
        public int PositionX = 0;
        public int PositionY = 60;
        public bool Closed = false;
        private Color color;
        public bool Active = true;
        public bool Dragging = false;
        public bool HasTitleBar = true;
        public abstract void Draw();
        public abstract void Open();
        public abstract void Run();
        public void DrawTitleBar()
        {
            if (Active) { color = Color.DarkViolet; } else { color = Color.Gray; }
            if (HasTitleBar)
            {
                NDEManager.screen.DrawFilledRectangle(color, PositionX-1, PositionY - 30, SizeX+1, 30);
                NDEManager.screen.DrawFilledRectangle(Color.Red, PositionX + SizeX - 29, PositionY - 30, 30, 30);
            }
            NDEManager.screen.DrawRectangle(color,PositionX-1,PositionY-1,SizeX+1,SizeY+1);
            NDEManager.screen.DrawFilledRectangle(Color.White,PositionX,PositionY,SizeX,SizeY);
            if (HasTitleBar)
            {
                titlesh = Title;
                if ((Title.Length * 8) >= SizeX - 30)
                {
                    int maxletter = (int)Math.Truncate((decimal)(SizeX) / 8);
                    if (maxletter - 3 < 0)
                    {
                        titlesh = titlesh.Substring(0, maxletter);
                    }
                    else
                    {
                        titlesh = titlesh.Substring(0, maxletter - 3);
                        titlesh = titlesh + "...";
                    }
                }
                NDEManager.screen.DrawString(titlesh, PCScreenFont.Default, Color.White, PositionX + 3, PositionY - 22);
                if (MouseManager.MouseState == MouseState.Left && !Dragging)
                {
                    if ((MouseManager.X >= PositionX + SizeX - 30 && MouseManager.X <= PositionX + SizeX) && (MouseManager.Y >= PositionY - 30 && MouseManager.Y <= PositionY))
                    {
                        Close();
                    }
                }
            }
        }
        public virtual void Close()
        {
            Closed = true;
        }
    }
}
