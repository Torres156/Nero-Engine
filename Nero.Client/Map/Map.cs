using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Map
{
    class Map
    {
        #region Static
        public static Map Current = null;
        #endregion

        public string Name = "";
        public Int2 Size = new Int2(60, 33);
    }
}
