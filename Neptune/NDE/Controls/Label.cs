using Cosmos.System.Graphics.Fonts;
using System.Drawing;

namespace Neptune.NDE.Controls
{
    public class Label : Control
    {
        public string Text = "Label";
        public override void Draw()
        {
            NDEManager.screen.DrawString(Text, PCScreenFont.Default,Color.Black,PositionX,PositionY);
        }
    }
}
