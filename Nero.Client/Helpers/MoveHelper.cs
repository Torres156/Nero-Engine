using Nero.Client.Player;
using Nero.Client.World;
using System;
using System.Collections.Generic;
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
            if (c.Direction != direction)
            {
                c.Direction = direction;
                // SEND UPDATE DIR
            }
            if (CanMove(direction))
            {                
                c.Moving = true;
                switch (direction)
                {
                    case Directions.Up:
                        c.Position.y--;
                        c.OffSet.y = 32;
                        break;
                    case Directions.Down:
                        c.Position.y++;
                        c.OffSet.y = -32;
                        break;
                    case Directions.Left:
                        c.Position.x--;
                        c.OffSet.x = 32;
                        break;
                    case Directions.Right:
                        c.Position.x++;
                        c.OffSet.x = -32;
                        break;
                }
                // SEND MOVEMENT
                
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

            var m = Map.Map.Current;
            if (nextPos.x < 0 || nextPos.y < 0) return false;
            if (nextPos.x > m.Size.x || nextPos.y > m.Size.y) return false;

            return true;
        }
    }
}
