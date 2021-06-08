using Nero.Server.Map;
using Nero.Server.Network;
using Nero.Server.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nero.Server.Helpers
{
    class MoveHelper
    {
        /// <summary>
        /// Movimento do jogador
        /// </summary>
        /// <param name="player"></param>
        /// <param name="direction"></param>
        public static void Move(Character player, Directions direction)
        {
            var map = MapInstance.Items[player.MapID];

            player.Direction = direction;

            var newPos = player.Position;
            switch(direction)
            {
                case Directions.Up:
                    newPos.y--;
                    break;
                case Directions.Down:
                    newPos.y++;
                    break;
                case Directions.Left:
                    newPos.x--;
                    break;
                case Directions.Right:
                    newPos.x++;
                    break;
            }
            
            // Atributos no mapa
            var attr = map.Attributes[(int)newPos.x, (int)newPos.y];
            foreach(var i in attr)
            {
                switch(i.Type)
                {
                    case AttributeTypes.Block:
                        Sender.UpdateCharacterPosition(player);
                        return;

                    case AttributeTypes.Warp:
                        var mapID = i.args[0].ToInt32();
                        var x = i.args[1].ToInt32();
                        var y = i.args[2].ToInt32();

                        PlayerHelper.GoMap(player, mapID, new Vector2(x, y));
                        return;
                }
            }

            player.Position = newPos;
            Sender.MoveCharacter(player);
        }
    }
}
