using Nero.Server;
using Nero.Server.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Client.World
{
    class Spawn
    {
        public IInstance Instance { get; private set; }
        public int MapID { get; private set; }
        public SpawnItem[] Items;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="MapID"></param>
        public Spawn(int MapID, IInstance Instance)
        {
            this.MapID = MapID;
            this.Instance = Instance;

            var f = GetFactory();
            Items = new SpawnItem[f.Items.Count];
            for (int i = 0; i < Items.Length; i++)
                Items[i] = new SpawnItem(f.Items[i]);
        }

        /// <summary>
        /// Atualiza o mapa
        /// </summary>
        public void Update()
        {
            foreach (var i in Items)
                i.Update();
        }

        public SpawnFactory GetFactory()
            => SpawnFactory.Factories[MapID];
    }
}
