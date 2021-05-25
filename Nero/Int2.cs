using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Nero
{
    using SFML.System;
    [StructLayout(LayoutKind.Sequential)]
    public struct Int2 : IEquatable<Int2>
    {
        public int x;
        public int y;

        public static readonly Int2 Zero = new Int2(0);
        public static readonly Int2 One = new Int2(1);


        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="value"></param>
        public Int2(int value) : this(value, value)
        { }

        /// <summary>
        /// Verifica se o vetor é igual
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Int2 other)
            => other.x == x && other.y == y;

        /// <summary>
        /// Verifica se o vetor é igual
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Vector2 other)
            => (int)other.x == x && (int)other.y == y;

        /// <summary>
        /// Distância entre 2 pontos
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int Distance(Int2 other)
            => (int)Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2));

        public override int GetHashCode()
            => x.GetHashCode() + y.GetHashCode();

        public override bool Equals(object obj)
            => (obj is Int2 || obj is Vector2) && Equals((Int2)obj);

        public Vector2 ToVector2()
            => new Vector2(x, y);

        public static Int2 Max(Int2 v1, Int2 v2)
            => new Int2(Math.Max(v1.x, v2.x), Math.Max(v1.y, v2.y));

        public static Int2 Min(Int2 v1, Int2 v2)
            => new Int2(Math.Min(v1.x, v2.x), Math.Min(v1.y, v2.y));

        #region Operators
        public static Int2 operator -(Int2 value, Int2 other)
            => new Int2(value.x - other.x, value.y - other.y);

        public static Int2 operator -(Int2 value, int other)
            => new Int2(value.x - other, value.y - other);

        public static Int2 operator +(Int2 value, Int2 other)
            => new Int2(value.x + other.x, value.y + other.y);

        public static Int2 operator +(Int2 value, int other)
            => new Int2(value.x + other, value.y + other);

        public static Int2 operator *(Int2 value, Int2 other)
            => new Int2(value.x * other.x, value.y * other.y);

        public static Int2 operator *(Int2 value, int other)
            => new Int2(value.x * other, value.y * other);

        public static Int2 operator /(Int2 value, Int2 other)
            => new Int2(value.x / other.x, value.y / other.y);

        public static Int2 operator /(Int2 value, int other)
            => new Int2(value.x / other, value.y / other);

        public static bool operator ==(Int2 value, Int2 other)
            => value.x == other.x && value.y == other.y;

        public static bool operator !=(Int2 value, Int2 other)
            => value.x != other.x || value.y != other.y;

        public static implicit operator Vector2f(Int2 v)
            => new Int2(v.x, v.y);

        public static explicit operator Int2(Vector2f v)
            => new Int2((int)v.X, (int)v.Y);

        public static implicit operator Vector2i(Int2 v)
            => new Vector2i(v.x, v.y);

        public static explicit operator Int2(Vector2i v)
            => new Int2(v.X, v.Y);

        public static implicit operator Vector2u(Int2 v)
            => new Vector2u((uint)v.x, (uint)v.y);

        public static explicit operator Int2(Vector2u v)
            => new Int2((int)v.X, (int)v.Y);

        public static explicit operator Int2(Vector2 v)
            => new Int2((int)v.x, (int)v.y);
        #endregion



    }
}
