using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Client.World
{
    class Npc
    {
        #region Static
        public static Npc[] Items = new Npc[Constants.MAX_NPCS];
        #endregion                
        
        public string Name { get; set; } = "";                              // Nome
        public NpcBehavior Behavior { get; set; } = NpcBehavior.Passive;    // Comportamento
        public int Level { get; set; } = 1;                                 // Level
        public long Exp { get; set; }                                       // Experiência
        public int SpriteID { get; set; }                                   // Aparência
        public int Scale { get; set; } = 100;                               // Escala
        public long HP { get; set; } = 1;                                   // Vida Máxima
        public int Regen { get; set; }                                      // Regeneração
        public int Damage { get; set; }                                     // Dano
        public int ResistPhysic { get; set; }                               // Resistência Física
        public int ResistMagic { get; set; }                                // Resistência Mágica
        public int Range { get; set; } = 0;                                 // Alcance
        public int AttackSpeed { get; set; } = 1000;                        // Velocidade de ataque
        public int MoveSpeed { get; set; } = 70;                            // Velocidade de Movimento

        /// <summary>
        /// Construtor
        /// </summary>
        public Npc()
        {
            
        }
        
    }
}
