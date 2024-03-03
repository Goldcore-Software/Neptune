using Cosmos.System;
using GrapeGL.Graphics;
using GrapeGL.Graphics.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptune.NDE.Windows
{
    public class DesktopWindow : Window
    {
        public override void Draw()
        {
            NDEManager.screen.Clear(NDEManager.desktopBackground);
            NDEManager.screen.DrawFilledRectangle(0, 0, (ushort)(NDEManager.DisplayColumns - NDEManager.TaskbarHeight), (ushort)NDEManager.TaskbarHeight, 0, NDEManager.taskbarColor);
            NDEManager.screen.DrawFilledRectangle((int)(NDEManager.DisplayColumns - NDEManager.TaskbarHeight), 0, (ushort)NDEManager.TaskbarHeight, (ushort)NDEManager.TaskbarHeight, 0, Color.GoogleBlue);
            NDEManager.screen.DrawString(5, 8, "Neptune | FPS: " + NDEManager.FPS.ToString() + " | Active window: " + NDEManager.ActiveWindow.ToString(), Font.Fallback, Color.White);
        }

        public override void Open()
        {
            HasTitleBar = false;
            HasBody = false;
        }

        public override void Run()
        {
            if (!NDEManager.startmenuopen)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if ((MouseManager.X >= NDEManager.DisplayColumns - NDEManager.TaskbarHeight) && MouseManager.Y <= NDEManager.TaskbarHeight)
                    {
                        NDEManager.startmenuopen = true;
                        NDEManager.OpenWindow(new StartMenuWindow());
                    }
                }
            }
        }
    }
}
