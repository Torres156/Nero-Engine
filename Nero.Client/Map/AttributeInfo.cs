using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Map
{
    class AttributeInfo
    {
        public AttributeTypes Type; // Tipo de atributo
        public string[] args = { }; // Variaveis
        
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="args"></param>
        public AttributeInfo(AttributeTypes Type, params string[] args)
        {
            this.Type = Type;
            this.args = args;
        }
    }
}
