using Nero.Server.Map;
using Nero.Server.Network;
using Nero.Server.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Server.Helpers
{
    static class PlayerHelper
    {

        /// <summary>
        /// Entra no jogo
        /// </summary>
        /// <param name="player"></param>
        public static void Join(Character player)
        {
            // Database
            Sender.UpdateMyCharacter(player.peer);  // Envia meus dados 
            Sender.UpdateNpcAll(player.peer);

            // Muda para cena de Gameplay
            Sender.ChangeToGameplay(player.peer);

            // Entra no mapa
            GoMap(player, player.MapID, player.Position, true); 
        }

        /// <summary>
        /// Entra no mapa
        /// </summary>
        /// <param name="mapID"></param>
        /// <param name="position"></param>
        public static void GoMap(Character player, int mapID, Vector2 position, bool startingGame = false)
        {
            // Remove o personagem do mapa anterior
            if (!startingGame)
            {
                // Apenas altera a posição
                if (mapID == player.MapID)
                {
                    player.Position = Vector2.Min(position, MapInstance.Items[mapID].Size.ToVector2() * 8);
                    Sender.UpdateCharacterPosition(player);
                    return;
                }
                player.ExitInstance();
            }
            // Posição
            player.MapID = mapID;
            player.Position = Vector2.Min(position, MapInstance.Items[mapID].Size.ToVector2() * 8);
            Sender.UpdateCharacterPosition(player);

            var inst = player.GetInstance();
            inst.PlayerCount++;

            // Verifica a revisão do mapa
            Sender.CheckMapRevision(player);
        }
    }
}
