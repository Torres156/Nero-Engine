using Nero.Client.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nero.Client.Map
{
    class Map
    {
        #region Static
        public static Map Current = null;

        public static Map Create()
        {
            var m = new Map();
            for (int i = 0; i < (int)Layers.count; i++)
                m.Layer[i].SetMap(m);

            return m;
        }
        #endregion


        public string Name = "";                // Nome do mapa
        public Int2 Size = new Int2(60, 33);    // Tamanho do mapa
        public Layer[] Layer;                   // Camadas


        // Client Only
        public int offWater { get; private set; }   // Animação de frame para Água
        bool Animation = false;                     // Animação de camada
        long timerAnimation;

        /// <summary>
        /// Construtor
        /// </summary>
        private Map()
        {
            Layer = new Layer[(int)Layers.count];
            for (int i = 0; i < Layer.Length; i++)            
                Layer[i] = new Layer();
        }



        /// <summary>
        /// Desenha o chão
        /// </summary>
        /// <param name="target"></param>
        public void DrawGround(RenderTarget target)
        {
            for (Layers i = Layers.Ground; i <= Layers.Mask2Anim; i++)
                if (i == Layers.MaskAnim || i == Layers.Mask2Anim)
                {
                    if (Animation)
                        Layer[(int)i].Draw(target);
                }
                else
                    Layer[(int)i].Draw(target);
        }

        /// <summary>
        /// Desenha os sobrepostos
        /// </summary>
        /// <param name="target"></param>
        public void DrawFringe(RenderTarget target)
        {
            for (Layers i = Layers.Fringe; i <= Layers.Fringe2Anim; i++)
                if (i == Layers.FringeAnim || i == Layers.Fringe2Anim)
                {
                    if (Animation)
                        Layer[(int)i].Draw(target);
                }
                else
                    Layer[(int)i].Draw(target);
        }

        /// <summary>
        /// Atualiza o mapa
        /// </summary>
        public void Update()
        {
            if (Environment.TickCount64 > timerAnimation)
            {
                Animation = !Animation;
                offWater++;
                if (offWater > 2)
                    offWater = 0;
                timerAnimation = Environment.TickCount64 + 250;
            }
        }

        /// <summary>
        /// Adiciona um novo chunk
        /// </summary>
        /// <param name="currentlayer"></param>
        /// <param name="tileid"></param>
        /// <param name="rect"></param>
        /// <param name="Position"></param>
        public void AddChunk(int currentlayer, ChunkTypes type, int tileid, Vector2 Source, Vector2 Position)
        {
            if (currentlayer < 0 || currentlayer >= Layer.Length)
                return;

            if (tileid < 0 || tileid >= GlobalResources.Tileset.Count)
                return;

            if (Position.x < 0 || Position.x > Size.x) return;
            if (Position.y < 0 || Position.y > Size.y) return;

            var l = Layer[currentlayer];
            l.chunks[(int)Position.x, (int)Position.y] = new Chunk();
            l.chunks[(int)Position.x, (int)Position.y].SetLayer(l);
            l.chunks[(int)Position.x, (int)Position.y].type = type;
            l.chunks[(int)Position.x, (int)Position.y].Position = Position;
            l.chunks[(int)Position.x, (int)Position.y].TileID = tileid;
            l.chunks[(int)Position.x, (int)Position.y].Source = Source;
            l.chunks[(int)Position.x, (int)Position.y].VerifyAutotile();

            Vector2[] chk_pos = { Position + new Vector2(-1,-1), Position + new Vector2(0, -1), Position + new Vector2(1, -1),
            Position + new Vector2(-1,0), Position + new Vector2(1,0),
            Position + new Vector2(-1,1), Position + new Vector2(0,1), Position + new Vector2(1,1)};

            foreach(var i in chk_pos)            
                if (i.x >= 0 && i.x <= Size.x && i.y >= 0 && i.y <= Size.y)                    
                    l.chunks[(int)i.x, (int)i.y]?.VerifyAutotile();
             
        }

        /// <summary>
        /// Remove um chunk
        /// </summary>
        /// <param name="currentLayer"></param>
        /// <param name="Position"></param>
        public void RemoveChunk(int currentlayer, Vector2 Position)
        {
            if (currentlayer < 0 || currentlayer >= Layer.Length)
                return;

            if (Position.x < 0 || Position.x > Size.x) return;
            if (Position.y < 0 || Position.y > Size.y) return;
            var l = Layer[currentlayer];
                        
            l.chunks[(int)Position.x, (int)Position.y] = null;   

            Vector2[] chk_pos = { Position + new Vector2(-1,-1), Position + new Vector2(0, -1), Position + new Vector2(1, -1),
            Position + new Vector2(-1,0), Position + new Vector2(1,0),
            Position + new Vector2(-1,1), Position + new Vector2(0,1), Position + new Vector2(1,1)};

            foreach (var i in chk_pos)
                if (i.x >= 0 && i.x <= Size.x && i.y >= 0 && i.y <= Size.y)
                    l.chunks[(int)i.x, (int)i.y]?.VerifyAutotile();
        }
    }
}
