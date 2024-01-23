using Cosmos.System.Graphics;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptune.NDE
{
    public class Resources
    {
        [ManifestResourceStream(ResourceName = "Neptune.Resources.cursor.bmp")]
        public static byte[] cursorbytes;
        public static Image cursor;
    }
}
