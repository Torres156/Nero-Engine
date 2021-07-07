using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Server.World
{
    class SpawnFactoryItem
    {
        public int NpcID { get; set; } = 0;                             // ID do npc
        public bool BlockMove { get; set; }                             // Bloqueia o movimento
        public Directions Direction { get; set; } = Directions.Down;    // Direção do npc
        public bool UsePositionSpawn { get; set; }                      // Utiliza modo de posição de spawn
        public Vector2 Position { get; set; }                           // Posição
    }
}
