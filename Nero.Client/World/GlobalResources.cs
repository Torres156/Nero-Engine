using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nero.Client.World
{
    static class GlobalResources
    {
        const string Ext = "png";
        public static List<Texture> Character;
        public static List<Texture> Tileset;


        /// <summary>
        /// Carrega todos os recursos
        /// </summary>
        public static void LoadAllResources()
        {
            LoadCharacters();
            LoadTileset();
        }

        /// <summary>
        /// Carrega as sprites dos personagens
        /// </summary>
        public static void LoadCharacters()
        {
            if (Character != null)
                return;

            var path = Environment.CurrentDirectory + "/res/character/";
            int i = 1;
            Character = new List<Texture>();
            Character.Add(null);
            while(File.Exists(path + $"{i}.{Ext}"))
            {
                Character.Add(new Texture(path + $"{i}.{Ext}"));
                i++;
            }
        }

        /// <summary>
        /// Carrega os tileset
        /// </summary>
        public static void LoadTileset()
        {
            if (Tileset != null)
                return;

            var path = Environment.CurrentDirectory + "/res/tileset/";
            int i = 1;
            Tileset = new List<Texture>();
            Tileset.Add(null);
            while (File.Exists(path + $"{i}.{Ext}"))
            {
                Tileset.Add(new Texture(path + $"{i}.{Ext}"));
                i++;
            }
        }


    }
}
