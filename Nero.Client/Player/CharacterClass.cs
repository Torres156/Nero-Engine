using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Player
{
    class CharacterClass
    {
        public static List<CharacterClass> Items;


        public string[] Name = { "", "" };                              // Nome da classe
        public int[] StatPrimary = new int[(int)StatPrimaries.count];   // Atributos primarios
        public string[] Description = { "", "" };                       // Descrição da classe
        public int[] MaleSprite, FemaleSprite;                          // Sprites masculinas e femininas
    }
}
