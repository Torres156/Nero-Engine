using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Map
{
    enum ZoneTypes
    {
        /// <summary>
        /// Normal : Permite PVP e PK
        /// </summary>
        Normal,

        /// <summary>
        /// Paz : Não permite PVP e PK
        /// </summary>
        Peace,

        /// <summary>
        /// Caótico : Zona de batalha, causar PK não irá conceder status de PK/Karma
        /// </summary>
        Chaotic,
    }
}
