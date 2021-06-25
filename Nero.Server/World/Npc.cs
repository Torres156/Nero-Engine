using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Server.World
{
    class Npc
    {
        #region Static
        public static List<Npc> Items = new List<Npc>();
        public static readonly string Path = "data/npc/";

        /// <summary>
        /// Inicializa as npcs
        /// </summary>
        public static void Initialize()
        {
            //Console.WriteLine("Carregando os npcs...");

            var files = Directory.GetFiles(Path);
            for (int i = 0; i < files.Length; i++)
            {
                Items.Add(Load(files[i]));
                Console.Write("\rCarregando os npcs...{0}", (int)(((i + 1) / (float)files.Length) * 100) + "%");
            } 
            Console.WriteLine("");
        }

        /// <summary>
        /// Verifica os diretórios
        /// </summary>
        public static void CheckDirectory()
        {
            Console.WriteLine("Checando diretório de npc...");
            var dirpath = Environment.CurrentDirectory + "/data/npc/";
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
        }

        /// <summary>
        /// Carrega o item
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Npc Load(string name)
        {
            var filePath = Path + name.Trim().ToLower() + ".json";

            Npc n;
            JsonHelper.Load<Npc>(filePath, out n);
            return n;
        }

        /// <summary>
        /// Salva o npc
        /// </summary>
        /// <param name="n"></param>
        public static void Save(Npc n)
        {
            if (n == null) return;
            var filePath = Path + n.Name.Trim().ToLower() + ".json";
            JsonHelper.Save(filePath, n);
        }
        #endregion

        public string Name { get; set; } = "";                              // Nome
        public NpcBehavior Behavior { get; set; } = NpcBehavior.Passive;    // Comportamento
        public int Level { get; set; } = 1;                                 // Level
        public long Exp { get; set; }                                       // Experiência
        public int SpriteID { get; set; }                                   // Aparência
        public int Scale { get; set; } = 100;                               // Escala
        public long HP { get; set; } = 1;                                   // Vida Máxima
        public int Regen { get; set; }                                      // Regeneração
        public int Damage { get; set; }                                     // Dano
        public int ResistPhysic { get; set; }                               // Resistência Física
        public int ResistMagic { get; set; }                                // Resistência Mágica
        public int Range { get; set; } = 0;                                 // Alcance
        public int AttackSpeed { get; set; } = 1000;                        // Velocidade de ataque
        public int MoveSpeed { get; set; } = 70;                            // Velocidade de Movimento

        /// <summary>
        /// Construtor
        /// </summary>
        public Npc()
        {

        }

    }
}
