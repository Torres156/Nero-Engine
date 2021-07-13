using LiteNetLib;
using Nero.Server.Helpers;
using Nero.Server.Map;
using Nero.Server.World;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nero.Server.Player
{
    class Character : Entity
    {
        #region 
        public static List<Character> Items = new List<Character>();
        public static readonly string Path = Environment.CurrentDirectory + "/data/character/";


        /// <summary>
        /// Verifica a existência do diretório
        /// </summary>
        public static void CheckDirectory()
        {
            Console.WriteLine("Checando diretório de personagens...");
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        /// <summary>
        /// Verifica se existe o personagem
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Exist(string name)
            => File.Exists(Path + $"{name.ToLower()}.json");

        /// <summary>
        /// Salva o personagem
        /// </summary>
        /// <param name="controller"></param>
        public static void Save(Character controller)
        {
            var filePath = Path + $"{controller.Name.ToLower()}.json";
            JsonHelper.Save(filePath, controller);
        }

        /// <summary>
        /// Carrega o personagem
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Character Load(string name)
        {
            var filePath = Path + $"{name.ToLower()}.json";

            if (!File.Exists(filePath))
                return null;

            Character c;
            JsonHelper.Load(filePath, out c);
            return c;
        }

        /// <summary>
        /// Procura o personagem com o nome desejado
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Character Find(string name)
            => Items.Find(i => i.Name.ToLower().Equals(name.ToLower()));

        /// <summary>
        /// Procura o personagem com a entrada de conexão
        /// </summary>
        /// <param name="peer"></param>
        /// <returns></returns>
        public static Character Find(NetPeer peer)
            => Items.Find(i => i.peer == peer);

        /// <summary>
        /// Procura o personagem com a conta
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static Character Find(Account account)
            => Items.Find(i => i.account == account);

        #endregion 

        // Publics
        public string Name = "";                                        // Nome
        public int ClassID = 0;                                         // Id da classe
        public int SpriteID = 0;                                        // Aparência
        public int Level = 1;                                           // Nível do personagem
        public long Experience = 0;                                     // Experiência
        public int[] StatPrimary = new int[(int)StatPrimaries.count];   // Atributos primários
        public Directions Direction = Directions.Down;                  // Direção do personagem
        public int Points = 0;                                          // Pontos de atributos        
        public AccessLevels AccessLevel = AccessLevels.Player;          // Acesso de administrador


        // Server Only
        [JsonIgnore]
        public NetPeer peer = null;     // Entrada de conexão
        [JsonIgnore]
        public Account account = null;  // Conta vinculada

        // Privates
        long timerRegen;        


        /// <summary>
        /// Construtor
        /// </summary>
        public Character()
        {
        }

        /// <summary>
        /// Instancia no qual o jogador está
        /// </summary>
        /// <returns></returns>
        public absInstance GetInstance()
            => MapInstance.Items[MapID];

        /// <summary>
        /// Atributos vitals máximo
        /// </summary>
        /// <param name="vital"></param>
        /// <returns></returns>
        public override int VitalMaximum(Vitals vital)
        {
            if (vital == Vitals.HP)
            {
                int value = 150 + 50 * Level;
                value = (int)(value * (1f + GetStatPrimary(StatPrimaries.Constitution) * .015f)); // 1,5% por CON
                return value;
            }
            else if (vital == Vitals.MP)
            {
                int value = 50 + 20 * Level;
                value = (int)(value * (1f + GetStatPrimary(StatPrimaries.Mental) * .025f + GetStatPrimary(StatPrimaries.Intelligency) * .01f)); // 2,5% por MEN + 1% por INT
                return value;
            }

            return 0;
        }

        /// <summary>
        /// Sai da instância
        /// </summary>
        public void ExitInstance()
        {
            GetInstance().PlayerCount--;
            Network.Sender.RemoveCharacter(this);
        }

        /// <summary>
        /// Antes do ataque basico
        /// </summary>
        public void BeforeAttackBasic(ref int damage)
        {

        }

        /// <summary>
        /// Modifica o dano em npcs
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="spawn"></param>
        public void ModifyAttackBasic(ref int damage, SpawnItem spawn)
        {

        }

        /// <summary>
        /// Antes do ataque do npc
        /// </summary>
        /// <param name="damage"></param>
        public void BeforeAttackedByNpc(ref int damage)
        {

        }

        /// <summary>
        /// Calcula o dano recebido
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageType"></param>
        /// <param name="ignoreResist"></param>
        public void CalculateDamageReceive(ref int value, DamageTypes damageType, int ignoreResist = 0)
        {
            var resist = damageType == DamageTypes.Physic ? GetStatCombat(StatCombats.Resist_Physic) : GetStatCombat(StatCombats.Resist_Magic);
            resist = Math.Max(resist - ignoreResist, 1);
            value = (int)(MathF.Pow(value, 2) / (resist * 3));
        }

        /// <summary>
        /// Leva um dano
        /// </summary>
        /// <param name="spawn"></param>
        /// <param name="damage"></param>
        /// <param name="damageType"></param>
        public void ReceiveDamage(SpawnItem spawn, int damage, DamageTypes damageType)
        {
            var die = ReceiveDamage(damage, damageType);
            if (die)
            {
                var spawnDevice = spawn.SpawnDevice;
                foreach (var i in spawnDevice.Items)
                    if (i.target == this)
                        i.target = null;
            }
        }

        /// <summary>
        /// Leva um dano
        /// </summary>
        /// <param name="spawn"></param>
        /// <param name="damage"></param>
        /// <param name="damageType"></param>
        public void ReceiveDamage(Character player, int damage, DamageTypes damageType)
        {
            ReceiveDamage(damage, damageType);
            Network.Sender.FloatMessage(player, $"-{damage}", Color.Red, Position * 8 + new Vector2(20, -20));
        }

        /// <summary>
        /// Leva um dano
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageType"></param>
        bool ReceiveDamage(int damage, DamageTypes damageType)
        {
            if (damage < 0) return false;

            if (damage > Vital[(int)Vitals.HP])
            {
                Death();
                Network.Sender.FloatMessage(this, $"-{damage}", Color.Red, Position * 8 + new Vector2(20,-20));
                return true;
            }
            else
            {
                Vital[(int)Vitals.HP] -= damage;
                Network.Sender.FloatMessage(this, $"-{damage}", Color.Red, Position * 8 + new Vector2(20,-20));
                // Send Update Vital
            }
            return false;
        }

        /// <summary>
        /// Morre o personagem
        /// </summary>
        public void Death()
        {
            Vital[(int)Vitals.HP] = VitalMaximum(Vitals.HP);
            Vital[(int)Vitals.MP] = VitalMaximum(Vitals.MP);

            var c = CharacterClass.Items[ClassID];
            PlayerHelper.GoMap(this, c.MapID, c.StartPosition);
        }

        /// <summary>
        /// Pega atributo de combate
        /// </summary>
        /// <param name="stat"></param>
        public int GetStatCombat(StatCombats stat)
        {
            int value = 0;

            switch (stat)
            {
                case StatCombats.Damage_Physic:
                    value = 10;
                    value = (int)(value * (1f + GetStatPrimary(StatPrimaries.Strenght) * .025f)); // 2,5% por STR
                    break;

                case StatCombats.Damage_Magic:
                    value = 10;
                    value = (int)(value * (1f + GetStatPrimary(StatPrimaries.Intelligency) * .035f)); // 3,5% por INT
                    break;

                case StatCombats.Resist_Physic:
                    value = 5;
                    value = (int)(value * (1f + GetStatPrimary(StatPrimaries.Constitution) * .01)); // 1% por CON
                    break;

                case StatCombats.Resist_Magic:
                    value = 5;
                    value = (int)(value * (1f + GetStatPrimary(StatPrimaries.Mental) * .015)); // 1,5% por MEN
                    break;

                case StatCombats.Regeneration_HP:
                    value = 1 + (int)(VitalMaximum(Vitals.HP) * .015f); // 1,5%
                    break;

                case StatCombats.Regeneration_MP:
                    value = 1 + (int)(VitalMaximum(Vitals.MP) * .02f); // 2%
                    break;
            }
            return value;
        }

        /// <summary>
        /// Atributos primarios
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int GetStatPrimary(StatPrimaries stat)
        {
            var baseValue = StatPrimary[(int)stat];

            return baseValue;
        }

        /// <summary>
        /// Atualiza o jogador
        /// </summary>
        public void Update()
        {
            // Regeneração
            if (Environment.TickCount64 > timerRegen)
            {
                AddVital(Vitals.HP, GetStatCombat(StatCombats.Regeneration_HP));
                AddVital(Vitals.MP, GetStatCombat(StatCombats.Regeneration_MP));
                timerRegen = Environment.TickCount64 + Constants.TIMER_REGEN;
            }
        }

        /// <summary>
        /// Adiciona um vital
        /// </summary>
        /// <param name="vitalType"></param>
        /// <param name="value"></param>
        public void AddVital(Vitals vitalType, int value)
        {
            if (Vital[(int)vitalType] < VitalMaximum(vitalType))
            {
                Vital[(int)vitalType] += value;
                if (Vital[(int)vitalType] > VitalMaximum(vitalType))
                    Vital[(int)vitalType] = VitalMaximum(vitalType);

                Network.Sender.FloatMessage(GetInstance(), $"+{value}", vitalType == Vitals.HP ? Color.Green : Color.CornflowerBlue,
                    Position * 8 + (vitalType == Vitals.HP ? new Vector2(5, -12 + Utils.Rand(1, 5)) : new Vector2(-10, -10 + Utils.Rand(1, 5))));

                // SEND UPDATE VITALS
            }
        }
    }
}
