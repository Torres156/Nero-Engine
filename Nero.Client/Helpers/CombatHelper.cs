using Nero.Client.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Client.Helpers
{
    static class CombatHelper
    {
        /// <summary>
        /// Requesita um ataque
        /// </summary>
        public static void Request()
        {
            if (CanAttack())
            {
                var player = Character.My;
                player.timerAttack = Environment.TickCount64 + 1000;
                Network.Sender.RequestAttack();
            }
        }

        static bool CanAttack()
        {
            var player = Character.My;

            if (player.timerAttack > 0)
                return false;

            return true;
        }
    }
}
