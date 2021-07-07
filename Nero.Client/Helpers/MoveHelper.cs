using Nero.Client.Map;
using Nero.Client.Network;
using Nero.Client.Player;
using Nero.Client.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nero.Client.Helpers
{
    static class MoveHelper
    {
        /// <summary>
        /// Requer um movimento
        /// </summary>
        /// <param name="direction"></param>
        public static void Request(Directions direction)
        {
            var c = Character.My;
            if (!c.Moving && c.Direction != direction)
            {
                c.Direction = direction;
                // SEND UPDATE DIR
            }
            if (CanMove(direction))
            {                
                c.Moving = true;
                Sender.MoveCharacter(direction);
                switch (direction)
                {
                    case Directions.Up:
                        c.Position.y--;
                        c.OffSet.y = 8;
                        break;
                    case Directions.Down:
                        c.Position.y++;
                        c.OffSet.y = -8;
                        break;
                    case Directions.Left:
                        c.Position.x--;
                        c.OffSet.x = 8;
                        break;
                    case Directions.Right:
                        c.Position.x++;
                        c.OffSet.x = -8;
                        break;
                }                
            }
        }

        /// <summary>
        /// Pode se mover?
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        static bool CanMove(Directions direction)
        {
            var c = Character.My;

            // Já está em movimento?
            if (c.Moving)
                return false;

            var nextPos = c.Position;
            switch(direction)
            {
                case Directions.Up:
                    nextPos.y--;
                    break;
                case Directions.Down:
                    nextPos.y++;
                    break;
                case Directions.Left:
                    nextPos.x--;
                    break;
                case Directions.Right:
                    nextPos.x++;
                    break;
            }

            var m = Map.MapInstance.Current;
            if (nextPos.x < 0 || nextPos.y < 0) return false;
            if ((int)nextPos.x / 4 > m.Size.x || (int)nextPos.y / 4 > m.Size.y) return false;

            var attr = m.Attributes[(int)nextPos.x / 4, (int)nextPos.y / 4];
            if (attr.Any(i => i.Type == AttributeTypes.Block)) // Bloqueios
                return false;

            if (Character.Items.Any(i => i.Position.Equals(nextPos)))
                return false;

            if (Spawn.Items.Any(i => (i.Position.ToInt2() / 2).Equals(nextPos.ToInt2() / 2)))
                return false;

            return true;
        }
    }
}
