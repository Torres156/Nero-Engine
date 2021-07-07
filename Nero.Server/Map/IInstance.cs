using Nero.Client.World;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Server
{
    abstract class absInstance
    {
        // Server Only
        [JsonIgnore]
        public Spawn Spawn { get; protected set; }    // Dispositivo de spawn

        public abstract void Update();

        /// <summary>
        /// Cria o dispositivo de spawn
        /// </summary>
        protected void CreateSpawnDevice(int mapID)
        {
            Spawn = new Spawn(mapID, this);
            Spawn.CreateItems();
        }
    }
}
