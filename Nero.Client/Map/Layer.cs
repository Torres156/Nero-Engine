using Nero.Client.World;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Map
{
    class Layer
    {        
        public Chunk[,] chunks;

        [JsonIgnore]
        public MapInstance Map { get; private set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public Layer()
        {            
        }

        /// <summary>
        /// Seta o mapa
        /// </summary>
        /// <param name="map"></param>
        public void SetMap(MapInstance map, bool updateChunk = true)
        {
            this.Map = map;
            if (chunks == null)
                chunks = new Chunk[map.Size.x + 1, map.Size.y + 1];
        }

        /// <summary>
        /// Desenha a camada
        /// </summary>
        /// <param name="target"></param>
        public void Draw(RenderTarget target)
        {
            var start = Camera.Start();
            var end = Camera.End(Map);

            for (int x = start.x; x <= end.x; x++)
                for (int y = start.y; y <= end.y; y++)
                    chunks[x, y]?.Draw(target);
        }

        /// <summary>
        /// Verifica se tem autotile
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool HasAutotile(Chunk chunk, Vector2 position)
        {            
            if (position.x < 0 || position.x > Map.Size.x) return true;
            if (position.y < 0 || position.y > Map.Size.y) return true;

            var c = chunks[(int)position.x, (int)position.y];
            return c != null && c.type == ChunkTypes.Autotile && c.TileID == chunk.TileID && c.Source.Equals(chunk.Source);
        }

        /// <summary>
        /// Verifica se tem water
        /// </summary>
        /// <param name="chunk"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool HasWater(Chunk chunk, Vector2 position)
        {
            if (position.x < 0 || position.x > Map.Size.x) return true;
            if (position.y < 0 || position.y > Map.Size.y) return true;

            var c = chunks[(int)position.x,(int)position.y];
            return c != null && c.type == ChunkTypes.Water && c.TileID == chunk.TileID && c.Source.Equals(chunk.Source);
        }

        /// <summary>
        /// Altera a quantidade de chunks
        /// </summary>
        /// <param name="valueX"></param>
        /// <param name="valueY"></param>
        public void SetSize(int valueX, int valueY)
        {
            var copyChunks = new Chunk[valueX + 1, valueY + 1];
            int copyX = Math.Min(valueX, Map.Size.x);
            int copyY = Math.Min(valueY, Map.Size.y);
            for (int x = 0; x <= copyX; x++)
                for (int y = 0; y <= copyY; y++)
                    copyChunks[x, y] = chunks[x, y];

            chunks = copyChunks;
        }
    }
}
