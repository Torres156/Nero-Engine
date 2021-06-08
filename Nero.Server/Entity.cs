using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Server
{
    abstract class Entity
    {
        public int MapID { get; set; }          // Mapa
        public Vector2 Position { get; set; }   // Posição da entidade
        public int[] Vital { get; set; }        // Atributos vitais

        /// <summary>
        /// Construtor
        /// </summary>
        public Entity()
        {
            Vital = new int[(int)Vitals.count];
        }

        /// <summary>
        /// Atributos vital máximo
        /// </summary>
        /// <param name="vital"></param>
        /// <returns></returns>
        public abstract int VitalMaximum(Vitals vital);
    }
}
