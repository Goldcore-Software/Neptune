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
        private List<Label> labels = new List<Label>();
        private List<string> windows = new List<string>();
        public override void Draw()
        {
            
            windows.Clear();
            foreach (var wind in NDEManager.Windows)
            {
                windows.Add(wind.Title);
            }
            int i = 0;
            
            foreach (var title in windows)
            {
                if (SizeY > 2 + (i * 18))
                {
                    // start
                    /*if (labels.Count < i)
                    {
                        labels[i] = new Label();
                        labels[i].PositionX = PositionX + 2;
                        labels[i].PositionY = PositionY + 2 + (i * 18);
                    }
                    labels[i].Text = title;*/
                    // end
                }
                i++;
            }
            i = 0;
            foreach (var label in labels)
            {
                if (NDEManager.Windows.Count-1 <= i)
                {
                    label.Text = "";
                }
            
                label.Draw();
                i++;
            }
        }

        public override void Open()
        {
            Title = "Task List";
        }

        public override void Run()
        {
            
        }
    }
}
