using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nero.Client
{
    static class WindowSettings
    {
        public static WindowSettingsData Instance;

        /// <summary>
        /// Carrega as configurações
        /// </summary>
        public static void Load()
        {
            var filePath = "data/windowsettings.json";
            if (!File.Exists(filePath))
            {
                Instance = new WindowSettingsData();
                Save();
                return;
            }

            JsonHelper.Load(filePath, out Instance);
        }

        /// <summary>
        /// Salva as configurações
        /// </summary>
        public static void Save()
        {
            var filePath = "data/windowsettings.json";
            JsonHelper.Save(filePath, Instance);
        }
    }

    class WindowSettingsData
    {
        public Vector2 WindowSize = new Vector2(800, 600);  // Tamanho da janela
        public bool VSync = false;                          // Vertical syncronized
        public string IP = "localhost";                     // IP do servidor
        public int PORT = 4000;                             // Porta do servidor
        public Languages Language = Languages.PT_BR;        // Linguagem
        public bool WindowMaximize = true;                  // Maximizar janela
        public int Volume_Music = 100;                      // Volume da música
        public int Volume_Sound = 100;                      // Volume dos soms
        public string Account_Save = "";                    // Conta salva
    }
}
