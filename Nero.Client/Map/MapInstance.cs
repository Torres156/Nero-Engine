using Nero.Client.Player;
using Nero.Client.World;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nero.Client.Map
{
    class MapInstance
    {
        #region Static        
        public static MapInstance Current = null;

        /// <summary>
        /// Cria o mapa
        /// </summary>
        /// <returns></returns>
        public static MapInstance Create()
        {
            var m = new MapInstance();
            for (int i = 0; i < (int)Layers.count; i++)
                m.Layer[i].SetMap(m);

            return m;
        }

        /// <summary>
        /// Salva o mapa
        /// </summary>
        public static void Save()
        {
            var path = Environment.CurrentDirectory + "/data/map/";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var filePath = path + $"{Character.My.MapID}.map";
            var json = JsonConvert.SerializeObject(Current);
            File.WriteAllBytes(filePath, MemoryService.Compress(Encoding.UTF8.GetBytes(json)));
        }

        /// <summary>
        /// Carrega o mapa
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static MapInstance Load(int ID)
        {
            var path = Environment.CurrentDirectory + "/data/map/";
            var filePath = path + $"{Character.My.MapID}.map";

            if (!File.Exists(filePath))
                return Create();

            var data = MemoryService.Decompress( File.ReadAllBytes(filePath));
            var json = Encoding.UTF8.GetString(data);
            var m = JsonConvert.DeserializeObject<MapInstance>(json);
            for (int i = 0; i < (int)Layers.count; i++)
            {
                m.Layer[i].SetMap(m, false);
                for (int x = 0; x <= m.Size.x; x++)
                    for (int y = 0; y <= m.Size.y; y++)
                        m.Layer[i].chunks[x, y]?.SetLayer(m.Layer[i]);
            }

            return m;
        }

        #endregion

        public int Revision = 0;                // Revisão do mapa
        public string Name = "";                // Nome do mapa
        public Int2 Size = new Int2(59, 31);    // Tamanho do mapa
        public Layer[] Layer;                   // Camadas


        // Client Only
        public int offWater { get; private set; }   // Animação de frame para Água
        bool Animation = false;                     // Animação de camada
        long timerAnimation;

        /// <summary>
        /// Construtor
        /// </summary>
        private MapInstance()
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
