using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Server
{
    enum StatPrimaries
    {
        Strenght,

        Intelligency,

        Constitution,

        Mental,

        /// <summary>
        /// Quantia de stats
        /// </summary>
        count,
    }

    enum Directions
    {
        Up,

        Down,

        Left,

        Right,
    }

    enum Vitals
    {
        HP,

        MP,

        count,
    }

    enum StatCombats
    {
        Damage_Physic,

        Damage_Magic,

        Resist_Physic,

        Resist_Magic,

        Regeneration_HP,

        Regeneration_MP,
    }

    enum DamageTypes
    {
        Physic,

        Magic,
    }
}
