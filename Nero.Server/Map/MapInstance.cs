using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nero.Server.Map
{
    class MapInstance : IInstance
    {
        #region Static        
        public static MapInstance[] Items = new MapInstance[Constants.MAX_MAPS];
        public static readonly string Path = Environment.CurrentDirectory + "/data/map/";

        /// <summary>
        /// Verifica se existe o diretório
        /// </summary>
        public static void CheckDirectory()
        {
            Console.WriteLine("Checando diretório de mapas...");
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        /// <summary>
        /// Inicializa as classes
        /// </summary>
        public static void Initialize()
        {                                    
            var dirpath = Environment.CurrentDirectory + "/data/classes/";
            for (int i = 0; i < Constants.MAX_MAPS; i++)
            {
                MapInstance.Items[i] = Load(i);
                Console.Write("\rCarregando os mapas...{0}", (int)(((i + 1) / (float)Constants.MAX_MAPS) * 100) + "%");
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// Salva o mapa
        /// </summary>
        public static void Save(int ID)
        {
            var filePath = Path + $"{ID}.map";
            var json = JsonConvert.SerializeObject(Items[ID]);
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
            var filePath = path + $"{ID}.map";

            if (!File.Exists(filePath))
            {
                Items[ID] = new MapInstance();
                Save(ID);
            }

            var data = MemoryService.Decompress(File.ReadAllBytes(filePath));
            var json = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<MapInstance>(json);
        }

        #endregion


        public int Revision = 0;                    // Revisão do mapa
        public string Name = "";                    // Nome do mapa
        public Int2 Size = new Int2(59, 31);        // Tamanho do mapa
        public Layer[] Layer;                       // Camadas
        public List<AttributeInfo>[,] Attributes;   // Atributos


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

            Attributes = new List<AttributeInfo>[Size.x + 1, Size.y + 1];
            for (int x = 0; x <= Size.x; x++)
                for (int y = 0; y <= Size.y; y++)
                    Attributes[x, y] = new List<AttributeInfo>();
        }

        /// <summary>
        /// Atualiza o mapa
        /// </summary>
        public void Update()
        {            
        }
    }
}