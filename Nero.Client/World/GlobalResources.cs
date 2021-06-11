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
        public static List<Texture> Fog;
        public static List<Texture> Panorama;


        /// <summary>
        /// Carrega todos os recursos
        /// </summary>
        public static void LoadAllResources()
        {
            LoadCharacters();
            LoadTileset();
            LoadFog();
            LoadPanorama();
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

        /// <summary>
        /// Carrega os fogs
        /// </summary>
        public static void LoadFog()
        {
            if (Fog != null)
                return;

            var path = Environment.CurrentDirectory + "/res/fog/";
            int i = 1;
            Fog = new List<Texture>();
            Fog.Add(null);
            while (File.Exists(path + $"{i}.{Ext}"))
            {
                Fog.Add(new Texture(path + $"{i}.{Ext}"));
                i++;
            }
        }

        /// <summary>
        /// Carrega os panoramas
        /// </summary>
        public static void LoadPanorama()
        {
            if (Panorama != null)
                return;

            var path = Environment.CurrentDirectory + "/res/panorama/";
            int i = 1;
            Panorama = new List<Texture>();
            Panorama.Add(null);
            while (File.Exists(path + $"{i}.{Ext}"))
            {
                Panorama.Add(new Texture(path + $"{i}.{Ext}", true) { Smooth = true }); 
                i++;
            }
        }
    }
}
