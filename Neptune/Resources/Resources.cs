﻿using GrapeGL.Graphics;
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
        public static Canvas cursor;
        [ManifestResourceStream(ResourceName = "Neptune.Resources.vga.acf")]
        public static byte[] vga12;
        [ManifestResourceStream(ResourceName = "Neptune.Resources.vga18.acf")]
        public static byte[] vga18;
    }
}
