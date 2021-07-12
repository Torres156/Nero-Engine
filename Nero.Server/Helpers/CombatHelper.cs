using Nero.Server.Player;
using Nero.Server.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Server.Helpers
{
    static class CombatHelper
    {
        /// <summary>
        /// Requer um ataque
        /// </summary>
        /// <param name="player"></param>
        public static void RequestAttack(Character player)
        {
            int realDamage = 0;

            // Envia a animação
            Network.Sender.AttackAnimation(player.GetInstance(), player);

            // Dano de entrada
            int damage = player.GetStatCombat(StatCombats.Damage_Physic);

            // Antes do ataque básico
            player.BeforeAttackBasic(ref damage);

            IEnumerable<SpawnItem> npcs = null;
            switch (player.Direction)
            {
                case Directions.Up:
                    npcs = player.GetInstance().Spawn.Items.Where(i => CanAttackable(i) &&
            i.Position.x >= player.Position.x - 1 && i.Position.x <= player.Position.x + 1 &&
            i.Position.y >= player.Position.y - 2 && i.Position.y <= player.Position.y - 1);
                    break;

                case Directions.Down:
                    npcs = player.GetInstance().Spawn.Items.Where(i => CanAttackable(i) &&
            i.Position.x >= player.Position.x - 1 && i.Position.x <= player.Position.x + 1 &&
            i.Position.y >= player.Position.y + 1 && i.Position.y <= player.Position.y + 2);
                    break;

                case Directions.Left:
                    npcs = player.GetInstance().Spawn.Items.Where(i => CanAttackable(i) &&
           i.Position.x >= player.Position.x - 2 && i.Position.x <= player.Position.x - 1 &&
           i.Position.y >= player.Position.y - 1 && i.Position.y <= player.Position.y + 1);
                    break;

                case Directions.Right:
                    npcs = player.GetInstance().Spawn.Items.Where(i => CanAttackable(i) &&
           i.Position.x >= player.Position.x + 1 && i.Position.x <= player.Position.x + 2 &&
           i.Position.y >= player.Position.y - 1 && i.Position.y <= player.Position.y + 1);
                    break;
            }

            foreach (var i in npcs)
            {
                realDamage = damage;
                i.CalculateDamageReceive(ref realDamage, DamageTypes.Physic);
                if (realDamage > 0)
                    i.ReceiveDamage(player, realDamage, DamageTypes.Physic);
                return; // Only 1
            }
        }

        /// <summary>
        /// Npc requer um ataque
        /// </summary>
        /// <param name="spawn"></param>
        /// <param name="player"></param>
        public static void NpcRequestAttack(SpawnItem spawn, Character player)
        {
            // Dano de entrada
            int damage = Math.Max( spawn.GetNpc().Damage,1);

            // Calcular dano
            player.CalculateDamageReceive(ref damage, DamageTypes.Physic);

            // Antes do ataque básico
            player.BeforeAttackedByNpc(ref damage);

            // Causa o dano
            player.ReceiveDamage(spawn, damage, DamageTypes.Physic);
        }

        /// <summary>
        /// Verifica se o npc pode tomar dano
        /// </summary>
        /// <param name="spawn"></param>
        /// <returns></returns>
        public static bool CanAttackable(SpawnItem spawn)
        {
            if (spawn.State == SpawnStates.Dead)
                return false;
            return true;
        }


    }
}
