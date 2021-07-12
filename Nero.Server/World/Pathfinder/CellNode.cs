using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Server.World.Pathfinder
{
    class CellNode
    {
        public CellNode Parent;
        public bool Closed;
        public Int2 Position;
        public int F, H, G;

        public CellNode(Int2 Position)
            => this.Position = Position;
    }
}
