using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nero.Server.Player
{
    class CharacterClass
    {
        #region Static
        public static List<CharacterClass> Items = new List<CharacterClass>();


        /// <summary>
        /// Inicializa as classes
        /// </summary>
        public static void Initialize()
        {
            Console.WriteLine("Carregando as classes...");

            int i = 0;
            var dirpath = Environment.CurrentDirectory + "/data/classes/";
            while (File.Exists(dirpath + $"{i}.json"))
            {
                CharacterClass c;
                JsonHelper.Load(dirpath + $"{i}.json", out c);                
                Items.Add(c);
                Console.WriteLine($"-> {c.Name[0]} carregado!");
                i++;
            }
        }

        /// <summary>
        /// Verifica os diretórios
        /// </summary>
        public static void CheckDirectory()
        {
            Console.WriteLine("Checando diretório de classes...");
            var dirpath = Environment.CurrentDirectory + "/data/classes/";
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
        }
        #endregion


        public string[] Name = { "", "" };                              // Nome da classe
        public int[] StatPrimary = new int[(int)StatPrimaries.count];   // Atributos primarios
        public string[] Description = { "", "" };                       // Descrição da classe
        public int[] MaleSprite, FemaleSprite;                          // Sprites masculinas e femininas
        public int MapID = 0;                                           // ID do mapa inicial
        public Vector2 StartPosition;                                   // Posição inicial
    }
}
