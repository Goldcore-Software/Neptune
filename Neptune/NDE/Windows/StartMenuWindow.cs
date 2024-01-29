using Neptune.NDE.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptune.NDE.Windows
{
    public class StartMenuWindow : Window
    {
        private Button msgboxbutton;
        private Button taskmanagerbutton;
        private Button aboutbutton;
        private Label startlabel;
        public override void Draw()
        {
            startlabel.Draw();
            msgboxbutton.Draw();
            taskmanagerbutton.Draw();
            aboutbutton.Draw();
        }

        public override void Open()
        {
            Title = "Start Menu";
            PositionX = (int)(NDEManager.DisplayColumns - 150);
            PositionY = NDEManager.TaskbarHeight;
            SizeX = 150;
            SizeY = 150;
            startlabel = new Label
            {
                Text = "Start",
                PositionX = PositionX + 2,
                PositionY = PositionY + 2
            };
            msgboxbutton = new Button
            {
                PositionX = PositionX + 2,
                PositionY = PositionY + 20,
                SizeY = 30,
                SizeX = SizeX - 2,
                Text = "MsgBox test"
            };
            taskmanagerbutton = new Button
            {
                PositionX = PositionX + 2,
                PositionY = PositionY + 52,
                SizeY = 30,
                SizeX = SizeX - 2,
                Text = "Task List"
            };
            aboutbutton = new Button
            {
                PositionX = PositionX + 2,
                PositionY = PositionY + 84,
                SizeY = 30,
                SizeX = SizeX - 2,
                Text = "About Neptune"
            };
            HasTitleBar = false;
        }

        public override void Run()
        {
            if (msgboxbutton.Run())
            {
                NDEMessageBox.ShowMsgBox("Menu box!","This is a start menu message box",200,300,400);
                Close();
            }
            if (taskmanagerbutton.Run())
            {
                TaskManagerWindow taskmanager = new TaskManagerWindow
                {
                    PositionX = 200,
                    PositionY = 200,
                    SizeX = 200,
                    SizeY = 300
                };
                NDEManager.OpenWindow(taskmanager);
                //NDEMessageBox.ShowMsgBox("Note","This feature is not complete.",150,300,400);
                Close();
            }
            if (aboutbutton.Run())
            {
                NDEMessageBox.ShowMsgBox("About Neptune","Version: " + Kernel.VersionString,150,300,400);
                Close();
            }
        }
        public override void Close()
        {
            NDEManager.startmenuopen = false;
            base.Close();
        }
    }
}
