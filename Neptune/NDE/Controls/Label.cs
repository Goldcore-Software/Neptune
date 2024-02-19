using GrapeGL.Graphics.Fonts;
using GrapeGL.Graphics;

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
                NDEManager.screen.DrawString(PositionX, PositionY+(i*18), splitstring[i], Font.Fallback, Color.Black);
                i++;
            }
        }
    }
}
