using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    using SFML.Graphics;
    public struct Rectangle : IEquatable<Rectangle>
    {
        public float x;
        public float y;
        public float width;
        public float height;

        /// <summary>
        /// Posição
        /// </summary>
        public Vector2 position
        {
            get => new Vector2(x, y);
            set
            {
                x = value.x;
                y = value.y;
            }
        }

        /// <summary>
        /// Tamanho
        /// </summary>
        public Vector2 size
        {
            get => new Vector2(width, height);
            set
            {
                width = value.x;
                height = value.y;
            }
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public Rectangle(Vector2 position, Vector2 size) : this(position.x, position.y, size.x, size.y)
        { }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Check if a point is inside the rectangle's area
        /// </summary>
        /// <param name="x">X coordinate of the point to test</param>
        /// <param name="y">Y coordinate of the point to test</param>
        /// <returns>True if the point is inside</returns>
        ////////////////////////////////////////////////////////////
        public bool Contains(float x, float y)
            => x >= this.x && x <= this.x + width && y >= this.y && y <= this.y + height;

        public bool Contains(Vector2 value)
            => Contains(value.x, value.y);

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Check intersection between two rectangles
        /// </summary>
        /// <param name="rect"> Rectangle to test</param>
        /// <returns>True if rectangles overlap</returns>
        ////////////////////////////////////////////////////////////
        public bool Intersects(Rectangle rect)
        {
            Rectangle overlap;
            return Intersects(rect, out overlap);
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Check intersection between two rectangles
        /// </summary>
        /// <param name="rect"> Rectangle to test</param>
        /// <param name="overlap">Rectangle to be filled with overlapping rect</param>
        /// <returns>True if rectangles overlap</returns>
        ////////////////////////////////////////////////////////////
        public bool Intersects(Rectangle rect, out Rectangle overlap)
        {
            // Rectangles with negative dimensions are allowed, so we must handle them correctly

            // Compute the min and max of the first rectangle on both axes
            float r1MinX = Math.Min(x, x + width);
            float r1MaxX = Math.Max(x, x + width);
            float r1MinY = Math.Min(y, y + height);
            float r1MaxY = Math.Max(y, y + height);

            // Compute the min and max of the second rectangle on both axes
            float r2MinX = Math.Min(rect.x, rect.x + rect.width);
            float r2MaxX = Math.Max(rect.x, rect.x + rect.width);
            float r2MinY = Math.Min(rect.y, rect.y + rect.height);
            float r2MaxY = Math.Max(rect.y, rect.y + rect.height);

            // Compute the intersection boundaries
            float interLeft = Math.Max(r1MinX, r2MinX);
            float interTop = Math.Max(r1MinY, r2MinY);
            float interRight = Math.Min(r1MaxX, r2MaxX);
            float interBottom = Math.Min(r1MaxY, r2MaxY);

            // If the intersection is valid (positive non zero area), then there is an intersection
            if ((interLeft < interRight) && (interTop < interBottom))
            {
                overlap.x = interLeft;
                overlap.y = interTop;
                overlap.width = interRight - interLeft;
                overlap.height = interBottom - interTop;
                return true;
            }
            else
            {
                overlap.x = 0;
                overlap.y = 0;
                overlap.width = 0;
                overlap.height = 0;
                return false;
            }
        }

        #region Operators
        public static Rectangle operator +(Rectangle v1, Rectangle v2)
            => new Rectangle(v1.position + v2.position, v1.size + v2.size);

        public static Rectangle operator -(Rectangle v1, Rectangle v2)
            => new Rectangle(v1.position - v2.position, v1.size - v2.size);

        public static bool operator ==(Rectangle v1, Rectangle v2)
            => v1.Equals(v2);

        public static bool operator !=(Rectangle v1, Rectangle v2)
            => !v1.Equals(v2);

        public static implicit operator IntRect(Rectangle v)
            => new IntRect(v.position, v.size);

        public static explicit operator Rectangle(IntRect v)
            => new Rectangle(v.Left, v.Top, v.Width, v.Height);

        public static implicit operator FloatRect(Rectangle v)
            => new FloatRect(v.position, v.size);

        public static explicit operator Rectangle(FloatRect v)
            => new Rectangle(v.Left, v.Top, v.Width, v.Height);

        #endregion

        public bool Equals(Rectangle other)
            => position.Equals(other.position) && size.Equals(other.size);

        public override bool Equals(object obj)
            => obj is Rectangle && Equals((Rectangle)obj);

        public override int GetHashCode()
            => position.GetHashCode() + size.GetHashCode();        
    }
}
