using Cosmos.System;
using GrapeGL.Graphics;
using GrapeGL.Graphics.Fonts;

namespace Neptune.NDE.Controls
{
    public class Button : Control
    {
        public string Text = "Button";
        public override void Draw()
        {
            NDEManager.screen.DrawRectangle(PositionX,PositionY, (ushort)SizeX, (ushort)SizeY, 0, Color.Black);
            NDEManager.screen.DrawString(PositionX+5,(PositionY+(SizeY/2))-8, Text, Font.Fallback, Color.Black);
        }

        public bool Run()
        {
            if (MouseManager.MouseState == MouseState.Left)
            {
                if ((MouseManager.X >= PositionX && MouseManager.X <= PositionX+SizeX) && (MouseManager.Y >= PositionY && MouseManager.Y <= PositionY + SizeY))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
