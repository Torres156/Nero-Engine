using System;
using System.Collections.Generic;
using System.Text;

namespace Nero
{
    public sealed class LanguageWords
    {
        public struct Words
        {
            public string[] word;

            /// <summary>
            /// Construtor
            /// </summary>
            /// <param name="value"></param>
            public Words(params string[] value)
            {
                word = new string[(int)Languages.count];
                for (int i = 0; i < word.Length; i++)
                    word[i] = "";

                var count = Math.Min(value.Length, word.Length);
                for (int i = 0; i < count; i++)
                    word[i] = value[i];
            }                
        }

        public List<Words> item = new List<Words>();

        /// <summary>
        /// Adiciona um texto de acordo com a linguagem
        /// </summary>
        /// <param name="words"></param>
        public void AddText(params string[] words)
            => item.Add(new Words(words));

        /// <summary>
        /// Pega um texto de acordo com a linguagem
        /// </summary>
        /// <param name="at"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public string GetText(int at)
            => item[at].word[(int)Game.CurrentLanguage];
    }
}
