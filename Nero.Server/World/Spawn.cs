using Nero.Server;
using Nero.Server.Core;
using Nero.Server.Map;
using Nero.Server.World;
using Nero.Server.World.Pathfinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Server.World
{
    class Spawn
    {
        public absInstance Instance { get; private set; }
        public int MapID { get; private set; }
        public SpawnItem[] Items;
        public Greedy Greedy { get; private set; }


        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="MapID"></param>
        public Spawn(int MapID, absInstance Instance)
        {
            this.MapID = MapID;
            this.Instance = Instance;
        }

        /// <summary>
        /// Cria os items
        /// </summary>
        public void CreateItems()
        {
            var f = GetFactory();
            Items = new SpawnItem[f.Items.Count];
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = new SpawnItem(this, f.Items[i]);
                Items[i].Respawn();
            }

            Greedy = new Greedy(this);
        }

        /// <summary>
        /// Atualiza o mapa
        /// </summary>
        public void Update()
        {
            if (Instance.PlayerCount > 0)
                foreach (var i in Items)
                i.Update();
        }

        public SpawnFactory GetFactory()
            => SpawnFactory.Factories[MapID];

        public MapInstance GetMap()
            => MapInstance.Items[MapID];
    }
}
