using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neptune.NDE.Controls;

namespace Neptune.NDE.Windows
{
    public class TaskManagerWindow : Window
    {
        private Label label;
        private List<string> windows = new List<string>();
        public override void Draw()
        {
            label.PositionX = PositionX + 2;
            label.PositionY = PositionY + 2;
            windows.Clear();
            foreach (var wind in NDEManager.Windows)
            {
                windows.Add(wind.Title);
            }
            label.Text = "";
            foreach (var title in windows)
            {
                label.Text += title+"\n";
            }
            label.Draw();
        }

        public override void Open()
        {
            Title = "Task List";
            label = new Label();
        }

        public override void Run()
        {
            
        }
    }
}
