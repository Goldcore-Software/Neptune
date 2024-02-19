using GrapeGL.Graphics.Fonts;
using Neptune.NDE.Controls;

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
            NDEManager.screen.DrawString(PositionX+5, PositionY + ((SizeY / 2) - 20),message, Font.Fallback, GrapeGL.Graphics.Color.Black);
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
