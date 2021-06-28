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
        public static Npc[] Items = new Npc[Constants.MAX_NPCS];
        public static readonly string Path = "data/npc/";

        /// <summary>
        /// Inicializa as npcs
        /// </summary>
        public static void Initialize()
        {            
            for (int i = 0; i < Constants.MAX_NPCS; i++)
            {
                if (!File.Exists(Path + $"{i}.json"))
                {
                    Items[i] = new Npc();
                    Save(i);
                } 
                Items[i] = Load(i);
                Console.Write("\rCarregando os npc...{0}", (int)(((i + 1) / (float)Constants.MAX_NPCS) * 100) + "%");
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
        /// Salva os npcs
        /// </summary>
        /// <param name="n"></param>
        public static void Save(int ID)
        {
            var filePath = Path + $"{ID}.json";
            JsonHelper.Save(filePath, Items[ID]);
        }

        /// <summary>
        /// Carrega o npc
        /// </summary>
        /// <param name="ID"></param>
        public static Npc Load(int ID)
        {
            var filePath = Path + ID + ".json";
            return JsonHelper.Load<Npc>(filePath);
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
