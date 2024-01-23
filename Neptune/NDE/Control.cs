using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptune.NDE
{
    public abstract class Control
    {
        public int SizeX = 30;
        public int SizeY = 30;
        public int PositionX = 0;
        public int PositionY = 30;
        public abstract void Draw();
    }
}
