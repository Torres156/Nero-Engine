using Nero.Client.Player;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Client
{
    class PlayerSettings
    {
        public static PlayerSettingsData Instance;

        /// <summary>
        /// Carrega as configurações
        /// </summary>
        public static void Load()
        {
            var filePath = $"data/playersettings/{Character.My.Name.ToLower()}.json";
            if (!File.Exists(filePath))
            {
                Instance = new PlayerSettingsData();
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
            var filePath = $"data/playersettings/{Character.My.Name.ToLower()}.json";
            JsonHelper.Save(filePath, Instance);
        }
    }

    class PlayerSettingsData
    {
        // Movimentação
        public Keyboard.Key MoveUp = Keyboard.Key.W;
        public Keyboard.Key MoveDown = Keyboard.Key.S;
        public Keyboard.Key MoveLeft = Keyboard.Key.A;
        public Keyboard.Key MoveRight = Keyboard.Key.D;
        public Keyboard.Key MoveUp2 = Keyboard.Key.Up;
        public Keyboard.Key MoveDown2 = Keyboard.Key.Down;
        public Keyboard.Key MoveLeft2 = Keyboard.Key.Left;
        public Keyboard.Key MoveRight2 = Keyboard.Key.Right;

        // Combates
        public Keyboard.Key NormalAttack = Keyboard.Key.LControl;
        public Keyboard.Key NormalAttack2 = Keyboard.Key.RControl;

        // Outros
        public Keyboard.Key GetDrop = Keyboard.Key.Space;
    }
}
