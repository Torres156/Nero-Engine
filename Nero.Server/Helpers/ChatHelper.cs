using Nero.Server.Network;
using Nero.Server.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Server.Helpers
{
    static class ChatHelper
    {
        /// <summary>
        /// Processa a fala
        /// </summary>
        /// <param name="player"></param>
        /// <param name="text"></param>
        public static void ProcessSpeak(Character player, string text)
        {
            // Chat Global
            if (text.Substring(0,1) == "!")
            {
                Sender.ChatTextToAll($"{player.Name}: {text.Substring(1)}", new Color(249, 119, 23));
                return;
            }

            // Verificar comandos
            var command = text.Split();


            // Chat Mapa
            Sender.ChatTextToInstance(player, $"{player.Name}: {text}", Color.White);
        }
    }
}
