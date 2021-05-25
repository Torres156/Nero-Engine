using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    public static class Extensions
    {
        /// <summary>
        /// Corrige o valor flutuante
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static float Floor(this float obj)
            => (float)Math.Floor(obj);

        /// <summary>
        /// Valor numérico ou não
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        /// <summary>
        /// String to Int
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int ToInt32(this string s)
            => int.Parse(s);

        /// <summary>
        /// Escreve um vetor 2D
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        public static void Write(this System.IO.BinaryWriter s, Vector2 value)
        {
            s.Write(value.x);
            s.Write(value.y);
        }

        /// <summary>
        /// Le um vetor 2D
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Vector2 ReadVector2(this System.IO.BinaryReader s)
            => new Vector2(s.ReadSingle(), s.ReadSingle());

        /// <summary>
        /// Escreve um retângulo
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        public static void Write(this System.IO.BinaryWriter s, Rectangle value)
        {
            s.Write(value.position);
            s.Write(value.size);
        }

        /// <summary>
        /// Le um retângulo
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Rectangle ReadRectangle(this System.IO.BinaryReader s)
            => new Rectangle(s.ReadVector2(), s.ReadVector2());

        /// <summary>
        /// Escreve uma cor
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        public static void Write(this System.IO.BinaryWriter s, Color value)
            => s.Write(value.ToInteger());

        /// <summary>
        /// Le uma cor
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Color ReadColor(this System.IO.BinaryReader s)
            => new Color(s.ReadUInt32());

        /// <summary>
        /// Escreve um array de string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="value"></param>
        public static void Write(this System.IO.BinaryWriter s, string[] value)
        {
            for (int i = 0; i < value.Length; i++)
                s.Write(value[i]);
        }

        /// <summary>
        /// Le um array de string
        /// </summary>
        /// <param name="s"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public static string[] ReadStringArray(this System.IO.BinaryReader s, int lenght)
        {
            string[] value = new string[lenght];
            for (int i = 0; i < lenght; i++)
                value[i] = s.ReadString();
            return value;
        }
    }
}
