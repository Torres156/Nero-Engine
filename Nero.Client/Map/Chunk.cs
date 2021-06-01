using Nero.Client.World;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nero.Client.Map
{
    using static Renderer;
    class Chunk
    {
        public int TileID = 0;                      // ID do gráfico
        public Vector2 Source;                      // Configuração do gráfico
        public ChunkTypes type = ChunkTypes.Normal; // Tipo do chunk
        public Vector2 Position;                    // Posição do chunk
        public byte[] Autotile = new byte[4];       // Configuração do Autotile

        [JsonIgnore]
        public Layer layer { get; private set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public Chunk()
        {
        }

        /// <summary>
        /// Seta a camada
        /// </summary>
        /// <param name="layer"></param>
        public void SetLayer(Layer layer)
        {
            this.layer = layer;
        }

        /// <summary>
        /// Desenha o tileset
        /// </summary>
        /// <param name="target"></param>
        public void Draw(RenderTarget target)
        {
            if (TileID == 0)
                return;

            switch(type)
            {
                case ChunkTypes.Normal:
                    DrawNormal(target);
                    break;

                case ChunkTypes.Autotile:
                    DrawAutotile(target);
                    break;

                case ChunkTypes.Water:
                    DrawWater(target);
                    break;
            }
        }

        /// <summary>
        /// Desenha o tileset normal
        /// </summary>
        /// <param name="target"></param>
        void DrawNormal(RenderTarget target)
        {
            var tex = GlobalResources.Tileset[TileID];
            DrawTexture(target, tex, new Rectangle(Position * 32, new Vector2(32)), new Rectangle(Source * 32, new Vector2(32)), Color.White);
        }

        /// <summary>
        /// Desenha o Autotile
        /// </summary>
        /// <param name="target"></param>
        void DrawAutotile(RenderTarget target)
        {
            var tex = GlobalResources.Tileset[TileID];

            // Normal
            if (Autotile.All(i => i == 0))
            {
                Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor(), Vector2.One * 32),
                    new Rectangle(Source * 32, Vector2.One * 32));
            }
            else
            {
                //Centro
                if (Autotile.All(i => i == 1))
                {
                    Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor(), Vector2.One * 32),
                    new Rectangle(Source * 32 + new Vector2(16, 48), Vector2.One * 32));
                    return;
                }

                //Centro
                if (Autotile.All(i => i == 5))
                {
                    Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor(), Vector2.One * 32),
                    new Rectangle(Source * 32 + new Vector2(32, 0), Vector2.One * 32));
                    return;
                }

                Rectangle rect = new Rectangle();

                //TOP LEFT
                switch (Autotile[0])
                {
                    case 0:
                        rect = new Rectangle(Source * 32 + new Vector2(0, 32), new Vector2(16, 16));
                        break;

                    case 1:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 0), new Vector2(16, 16));
                        break;

                    case 2:
                        rect = new Rectangle(Source * 32 + new Vector2(0, 64), new Vector2(16, 16));
                        break;

                    case 3:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 32), new Vector2(16, 16));
                        break;

                    case 4:
                        rect = new Rectangle(Source * 32 + new Vector2(16, 48), new Vector2(16, 16));
                        break;
                }
                Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor(), Vector2.One * 16), rect);

                //TOP RIGHT
                switch (Autotile[1])
                {
                    case 0:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 32), new Vector2(16, 16));
                        break;

                    case 1:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 0), new Vector2(16, 16));
                        break;

                    case 2:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 64), new Vector2(16, 16));
                        break;

                    case 3:
                        rect = new Rectangle(Source * 32 + new Vector2(16, 32), new Vector2(16, 16));
                        break;

                    case 4:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 48), new Vector2(16, 16));
                        break;
                }
                Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor() + new Vector2(16, 0), Vector2.One * 16), rect);

                //BOTTOM LEFT
                switch (Autotile[2])
                {
                    case 0:
                        rect = new Rectangle(Source * 32 + new Vector2(0, 80 - 1), new Vector2(16, 16 - 1));
                        break;

                    case 1:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 16 - 1), new Vector2(16, 16 - 1));
                        break;

                    case 2:
                        rect = new Rectangle(Source * 32 + new Vector2(0, 48 - 1), new Vector2(16, 16 - 1));
                        break;

                    case 3:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 80 - 1), new Vector2(16, 16 - 1));
                        break;

                    case 4:
                        rect = new Rectangle(Source * 32 + new Vector2(16, 64 - 1), new Vector2(16, 16 - 1));
                        break;
                }
                Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor() + new Vector2(0, 16), Vector2.One * 16), rect);

                //BOTTOM RIGHT
                switch (Autotile[3])
                {
                    case 0:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 80), new Vector2(16, 16 - 1));
                        break;

                    case 1:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 16), new Vector2(16, 16 - 1));
                        break;

                    case 2:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 48), new Vector2(16, 16 - 1));
                        break;

                    case 3:
                        rect = new Rectangle(Source * 32 + new Vector2(16, 80), new Vector2(16, 16 - 1));
                        break;

                    case 4:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 48), new Vector2(16, 16 - 1));
                        break;
                }
                Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor() + new Vector2(16), Vector2.One * 16), rect);
            }

        }

        /// <summary>
        /// Desenha a água
        /// </summary>
        /// <param name="target"></param>
        void DrawWater(RenderTarget target)
        {
            var tex = GlobalResources.Tileset[TileID];

            // Normal
            if (Autotile.All(i => i == 0))
            {
                Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor(), Vector2.One * 32),
                    new Rectangle(Source * 32 + new Vector2(64 * layer.Map.offWater, 0), Vector2.One * 32));
            }
            else
            {
                //Centro
                if (Autotile.All(i => i == 1))
                {
                    Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor(), Vector2.One * 32),
                    new Rectangle(Source * 32 + new Vector2(16, 48) + new Vector2(64 * layer.Map.offWater, 0), Vector2.One * 32));
                    return;
                }

                //Centro
                if (Autotile.All(i => i == 5))
                {
                    Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor(), Vector2.One * 32),
                    new Rectangle(Source * 32 + new Vector2(32, 0) + new Vector2(64 * layer.Map.offWater, 0), Vector2.One * 32));
                    return;
                }

                Rectangle rect = new Rectangle();

                //TOP LEFT
                switch (Autotile[0])
                {
                    case 0:
                        rect = new Rectangle(Source * 32 + new Vector2(0, 32) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16));
                        break;

                    case 1:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 0) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16));
                        break;

                    case 2:
                        rect = new Rectangle(Source * 32 + new Vector2(0, 64) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16));
                        break;

                    case 3:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 32) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16));
                        break;

                    case 4:
                        rect = new Rectangle(Source * 32 + new Vector2(16, 48) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16));
                        break;
                }
                Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor(), Vector2.One * 16), rect);

                //TOP RIGHT
                switch (Autotile[1])
                {
                    case 0:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 32) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16));
                        break;

                    case 1:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 0) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16));
                        break;

                    case 2:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 64) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16));
                        break;

                    case 3:
                        rect = new Rectangle(Source * 32 + new Vector2(16, 32) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16));
                        break;

                    case 4:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 48) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16));
                        break;
                }
                Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor() + new Vector2(16, 0), Vector2.One * 16), rect);

                //BOTTOM LEFT
                switch (Autotile[2])
                {
                    case 0:
                        rect = new Rectangle(Source * 32 + new Vector2(0, 80) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16 - 1));
                        break;

                    case 1:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 16) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16 - 1));
                        break;

                    case 2:
                        rect = new Rectangle(Source * 32 + new Vector2(0, 48) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16 - 1));
                        break;

                    case 3:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 80) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16 - 1));
                        break;

                    case 4:
                        rect = new Rectangle(Source * 32 + new Vector2(16, 64) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16 - 1));
                        break;
                }
                Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor() + new Vector2(0, 16), Vector2.One * 16), rect);

                //BOTTOM RIGHT
                switch (Autotile[3])
                {
                    case 0:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 80) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16 - 1));
                        break;

                    case 1:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 16) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16 - 1));
                        break;

                    case 2:
                        rect = new Rectangle(Source * 32 + new Vector2(48, 48) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16 - 1));
                        break;

                    case 3:
                        rect = new Rectangle(Source * 32 + new Vector2(16, 80) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16 - 1));
                        break;

                    case 4:
                        rect = new Rectangle(Source * 32 + new Vector2(32, 48) + new Vector2(64 * layer.Map.offWater, 0), new Vector2(16, 16 - 1));
                        break;
                }
                Renderer.DrawTexture(target, tex, new Rectangle((Position * 32).Floor() + new Vector2(16), Vector2.One * 16), rect);
            }

        }

        /// <summary>
        /// Verifica as Autotiles
        /// </summary>
        public void VerifyAutotile()
        {
            if (type == ChunkTypes.Normal)
                return;

            if (type == ChunkTypes.Autotile)
            {
                bool[] contains = {layer.HasAutotile(this, Position + new Vector2(0,-1)), // TOP 0
                layer.HasAutotile(this, Position + new Vector2(0,1)), // BOTTOM 1
                layer.HasAutotile(this, Position + new Vector2(-1,0)), // LEFT 2
                layer.HasAutotile(this, Position + new Vector2(1,0)), // RIGHT 3
                layer.HasAutotile(this, Position + new Vector2(-1,-1)), // TOP LEFT 4
                layer.HasAutotile(this, Position + new Vector2(1,-1)), // TOP RIGHT 5
                layer.HasAutotile(this, Position + new Vector2(-1,1)), // BOTTOM LEFT 6
                layer.HasAutotile(this, Position + new Vector2(1,1))}; // BOTTOM RIGHT 7

                // Normal PADRAO
                for (int i = 0; i < 4; i++)
                    Autotile[i] = 0;

                // Centro
                if (contains.All(i => i))
                {
                    for (int i = 0; i < 4; i++)
                        Autotile[i] = 1;
                    return;
                }

                if (contains[0] && contains[1] && contains[2] && contains[3] &&
                    !contains[4] && !contains[5] && !contains[6] && !contains[7])
                {
                    for (int i = 0; i < 4; i++)
                        Autotile[i] = 5;
                    return;
                }

                // TOP LEFT
                if (contains[0] && contains[2]) Autotile[0] = 1;
                if (contains[0] && !contains[2]) Autotile[0] = 2;
                if (!contains[0] && contains[2]) Autotile[0] = 3;
                if (contains[0] && contains[2] && contains[4]) Autotile[0] = 4;

                // TOP RIGHT
                if (contains[0] && contains[3]) Autotile[1] = 1;
                if (contains[0] && !contains[3]) Autotile[1] = 2;
                if (!contains[0] && contains[3]) Autotile[1] = 3;
                if (contains[0] && contains[3] && contains[5]) Autotile[1] = 4;

                // BOTTOM LEFT
                if (contains[1] && contains[2]) Autotile[2] = 1;
                if (contains[1] && !contains[2]) Autotile[2] = 2;
                if (!contains[1] && contains[2]) Autotile[2] = 3;
                if (contains[1] && contains[2] && contains[6]) Autotile[2] = 4;

                // BOTTOM RIGHT
                if (contains[1] && contains[3]) Autotile[3] = 1;
                if (contains[1] && !contains[3]) Autotile[3] = 2;
                if (!contains[1] && contains[3]) Autotile[3] = 3;
                if (contains[1] && contains[3] && contains[7]) Autotile[3] = 4;

            } // End Autotile


            // Water
            if (type == ChunkTypes.Water)
            {
                bool[] contains = {layer.HasWater(this, Position + new Vector2(0,-1)), // TOP 0
                layer.HasWater(this, Position + new Vector2(0,1)), // BOTTOM 1
                layer.HasWater(this, Position + new Vector2(-1,0)), // LEFT 2
                layer.HasWater(this, Position + new Vector2(1,0)), // RIGHT 3
                layer.HasWater(this, Position + new Vector2(-1,-1)), // TOP LEFT 4
                layer.HasWater(this, Position + new Vector2(1,-1)), // TOP RIGHT 5
                layer.HasWater(this, Position + new Vector2(-1,1)), // BOTTOM LEFT 6
                layer.HasWater(this, Position + new Vector2(1,1))}; // BOTTOM RIGHT 7

                // Normal PADRAO
                for (int i = 0; i < 4; i++)
                    Autotile[i] = 0;

                // Centro
                if (contains.All(i => i))
                {
                    for (int i = 0; i < 4; i++)
                        Autotile[i] = 1;
                    return;
                }

                if (contains[0] && contains[1] && contains[2] && contains[3] &&
                    !contains[4] && !contains[5] && !contains[6] && !contains[7])
                {
                    for (int i = 0; i < 4; i++)
                        Autotile[i] = 5;
                    return;
                }

                // TOP LEFT
                if (contains[0] && contains[2]) Autotile[0] = 1;
                if (contains[0] && !contains[2]) Autotile[0] = 2;
                if (!contains[0] && contains[2]) Autotile[0] = 3;
                if (contains[0] && contains[2] && contains[4]) Autotile[0] = 4;

                // TOP RIGHT
                if (contains[0] && contains[3]) Autotile[1] = 1;
                if (contains[0] && !contains[3]) Autotile[1] = 2;
                if (!contains[0] && contains[3]) Autotile[1] = 3;
                if (contains[0] && contains[3] && contains[5]) Autotile[1] = 4;

                // BOTTOM LEFT
                if (contains[1] && contains[2]) Autotile[2] = 1;
                if (contains[1] && !contains[2]) Autotile[2] = 2;
                if (!contains[1] && contains[2]) Autotile[2] = 3;
                if (contains[1] && contains[2] && contains[6]) Autotile[2] = 4;

                // BOTTOM RIGHT
                if (contains[1] && contains[3]) Autotile[3] = 1;
                if (contains[1] && !contains[3]) Autotile[3] = 2;
                if (!contains[1] && contains[3]) Autotile[3] = 3;
                if (contains[1] && contains[3] && contains[7]) Autotile[3] = 4;

            } // End Autotile
        }
    }
}
