using Cosmos.System.Graphics.Fonts;
using Neptune.NDE.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptune.NDE.Windows
{
    public class MsgBoxWindow : Window
    {
        private Button okbutton;
        public int MinimumSize = 125;
        public string message;
        public override void Draw()
        {
            okbutton.PositionX = PositionX + SizeX - 70;
            okbutton.PositionY = PositionY + SizeY - 35;
            okbutton.Draw();
            NDEManager.screen.DrawString(message,PCScreenFont.Default,Color.Black,PositionX+5, PositionY + ((SizeY / 2) - 20));
        }

        public override void Open()
        {
            SizeX = (message.Length * 8) + 10;
            if (SizeX < MinimumSize) { SizeX = MinimumSize; }
            SizeY = 75;
            okbutton = new Button();
            okbutton.Text = "OK";
            okbutton.PositionX = PositionX + SizeX - 70;
            okbutton.PositionY = PositionY + SizeY - 35;
            okbutton.SizeX = 65;
            okbutton.SizeY = 30;
        }

        public override void Run()
        {
            if (okbutton.Run())
            {
                Close();
            }
        }
    }
}
