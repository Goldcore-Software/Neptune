using Neptune.NDE.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptune.NDE
{
    public class NDEMessageBox
    {
        public static void ShowMsgBox(string title, string message, int minimumx, int x, int y)
        {
            MsgBoxWindow boxWindow = new MsgBoxWindow();
            boxWindow.Title = title;
            boxWindow.message = message;
            boxWindow.MinimumSize = minimumx;
            boxWindow.PositionX = x;
            boxWindow.PositionY = y;
            NDEManager.OpenWindow(boxWindow);
        }
    }
}
