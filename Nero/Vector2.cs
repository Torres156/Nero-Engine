using System;

namespace Nero
{
    using SFML.System;
    public struct Vector2 : IEquatable<Vector2>
    {
        public float x;
        public float y;

        public static readonly Vector2 Zero = new Vector2(0);
        public static readonly Vector2 One = new Vector2(1f);
        

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="value"></param>
        public Vector2(float value) : this(value,value)
        { }

        /// <summary>
        /// Verifica se o vetor é igual
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Vector2 other)
            => other.x == x && other.y == y;

        /// <summary>
        /// Verifica se o vetor é igual
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Int2 other)
            => other.x == (int)x && other.y == (int)y;

        /// <summary>
        /// Distância entre 2 pontos
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public float Distance(Vector2 other)
            => (float)Math.Sqrt(Math.Pow(x - other.x, 2) + Math.Pow(y - other.y, 2));

        public Int2 ToInt2()
            => new Int2((int)x, (int)y);

        public override int GetHashCode()        
            => x.GetHashCode() + y.GetHashCode();
        
        public override bool Equals(object obj)
            => (obj is Vector2 || obj is Int2) && Equals((Vector2)obj);

        public Vector2 Floor()
            => new Vector2((float)Math.Floor(x), (float)Math.Floor(y));

        public Vector2 Round()
            => new Vector2((float)Math.Round(x,2), (float)Math.Round(y,2));

        public Vector2 Ceiling()
            => new Vector2(MathF.Ceiling(x), MathF.Ceiling(y));

        public Vector2 ToInt()
            => new Vector2((int)x, (int)y);

        public static Vector2 Max(Vector2 v1, Vector2 v2)
            => new Vector2(MathF.Max(v1.x, v2.x), MathF.Max(v1.y, v2.y));

        public static Vector2 Min(Vector2 v1, Vector2 v2)
            => new Vector2(MathF.Min(v1.x, v2.x), MathF.Min(v1.y, v2.y));

        #region Operators
        public static Vector2 operator -(Vector2 value, Vector2 other)
            => new Vector2(value.x - other.x, value.y - other.y);

        public static Vector2 operator -(Vector2 value, float other)
            => new Vector2(value.x - other, value.y - other);

        public static Vector2 operator +(Vector2 value, Vector2 other)
            => new Vector2(value.x + other.x, value.y + other.y);

        public static Vector2 operator +(Vector2 value, float other)
            => new Vector2(value.x + other, value.y + other);

        public static Vector2 operator *(Vector2 value, Vector2 other)
            => new Vector2(value.x * other.x, value.y * other.y);

        public static Vector2 operator *(Vector2 value, float other)
            => new Vector2(value.x * other, value.y * other);

        public static Vector2 operator /(Vector2 value, Vector2 other)
            => new Vector2(value.x / other.x, value.y / other.y);

        public static Vector2 operator /(Vector2 value, float other)
            => new Vector2(value.x / other, value.y / other);

        public static bool operator ==(Vector2 value, Vector2 other)
            => value.x == other.x && value.y == other.y;

        public static bool operator !=(Vector2 value, Vector2 other)
            => value.x != other.x || value.y != other.y;

        public static implicit operator Int2(Vector2 v)
           => new Int2((int)v.x, (int)v.y);

        public static explicit operator Vector2(Int2 v)
            => new Vector2(v.x, v.y);

        public static implicit operator Vector2f(Vector2 v) 
            => new Vector2f(v.x,v.y);

        public static explicit operator Vector2(Vector2f v)
            => new Vector2(v.X,v.Y);

        public static implicit operator Vector2i(Vector2 v)
            => new Vector2i((int)v.x, (int)v.y);

        public static explicit operator Vector2(Vector2i v)
            => new Vector2(v.X, v.Y);

        public static implicit operator Vector2u(Vector2 v)
            => new Vector2u((uint)v.x, (uint)v.y);

        public static explicit operator Vector2(Vector2u v)
            => new Vector2(v.X, v.Y);
        #endregion

    }
}
