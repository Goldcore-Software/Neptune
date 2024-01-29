using Cosmos.System.Graphics.Fonts;
using System.Drawing;

namespace Neptune.NDE.Controls
{
    public class Label : Control
    {
        public string Text = "Label";
        public override void Draw()
        {
            string[] splitstring = Text.Split('\n');
            int i = 0;
            foreach (string line in splitstring)
            {
                NDEManager.screen.DrawString(splitstring[i], PCScreenFont.Default, Color.Black, PositionX, PositionY+(i*18));
                i++;
            }
        }
    }
}
