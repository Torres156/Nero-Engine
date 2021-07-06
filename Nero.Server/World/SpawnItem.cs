using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Server.World
{
    class SpawnItem
    {
        public int NpcID { get; set; }
        public Vector2 Position { get; set; }
        public long HP { get; set; }
        

        // Server Only
        public SpawnFactoryItem FactoryItem { get; private set; }
        SpawnStates State = SpawnStates.Normal;


        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="factoryItem"></param>
        public SpawnItem(SpawnFactoryItem factoryItem)
        {
            this.FactoryItem = factoryItem;
            State = SpawnStates.Normal;

        }

        /// <summary>
        /// Atualiza o spawn
        /// </summary>
        public void Update()
        {

        }
    }
}
