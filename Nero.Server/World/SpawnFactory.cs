using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Server.World
{
    class SpawnFactory
    {
        #region Static
        public static SpawnFactory[] Factories;
        public static readonly string Path = Environment.CurrentDirectory + "/data/spawn/";

        /// <summary>
        /// Verifica se existe o diretório
        /// </summary>
        public static void CheckDirectory()
        {
            Console.WriteLine("Checando diretório de spawns...");
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        /// <summary>
        /// Inicializa os spawns
        /// </summary>
        public static void Initialize()
        {
            Factories = new SpawnFactory[Constants.MAX_MAPS];
            for (int i = 0; i < Constants.MAX_MAPS; i++)
            {
                Factories[i] = Load(i);
                Console.Write("\rCarregando os spawns...{0}", (int)(((i + 1) / (float)Constants.MAX_MAPS) * 100) + "%");
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// Salva o spawn
        /// </summary>
        public static void Save(int ID)
        {
            var filePath = Path + $"{ID}.json";
            var json = JsonConvert.SerializeObject(Factories[ID]);
            File.WriteAllBytes(filePath, MemoryService.Compress(Encoding.UTF8.GetBytes(json)));
        }

        /// <summary>
        /// Carrega o spawn
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static SpawnFactory Load(int ID)
        {
            var path = Environment.CurrentDirectory + "/data/spawn/";
            var filePath = path + $"{ID}.json";

            if (!File.Exists(filePath))
            {
                Factories[ID] = new SpawnFactory();
                Save(ID);
            }

            var data = MemoryService.Decompress(File.ReadAllBytes(filePath));
            var json = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<SpawnFactory>(json);
        }
        #endregion


        public List<SpawnFactoryItem> Items = new List<SpawnFactoryItem>();
    }
}
