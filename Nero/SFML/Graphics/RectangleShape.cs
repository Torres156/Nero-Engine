using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.SFML.Graphics
{
    using System;
    ////////////////////////////////////////////////////////////
    /// <summary>
    /// Specialized shape representing a rectangle
    /// </summary>
    ////////////////////////////////////////////////////////////
    public class RectangleShape : Shape
    {
        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Default constructor
        /// </summary>
        ////////////////////////////////////////////////////////////
        public RectangleShape() :
            this(new Vector2(0, 0))
        {
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct the shape with an initial size
        /// </summary>
        /// <param name="size">Size of the shape</param>
        ////////////////////////////////////////////////////////////
        public RectangleShape(Vector2 size)
        {
            Size = size;
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Construct the shape from another shape
        /// </summary>
        /// <param name="copy">Shape to copy</param>
        ////////////////////////////////////////////////////////////
        public RectangleShape(RectangleShape copy) :
            base(copy)
        {
            Size = copy.Size;
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// The size of the rectangle
        /// </summary>
        ////////////////////////////////////////////////////////////
        public Vector2 Size
        {
            get { return mySize; }
            set { mySize = value; Update(); }
        }

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the total number of points of the rectangle.
        /// </summary>
        /// <returns>The total point count. For rectangle shapes,
        /// this number is always 4.</returns>
        ////////////////////////////////////////////////////////////
        public override uint GetPointCount() => 4;

        ////////////////////////////////////////////////////////////
        /// <summary>
        /// Get the position of a point
        ///
        /// The returned point is in local coordinates, that is,
        /// the shape's transforms (position, rotation, scale) are
        /// not taken into account.
        /// The result is undefined if index is out of the valid range.
        /// </summary>
        /// <param name="index">Index of the point to get, in range [0 .. 3]</param>
        /// <returns>index-th point of the shape</returns>
        ////////////////////////////////////////////////////////////
        public override Vector2 GetPoint(uint index)
        {
            switch (index)
            {
                default:
                case 0:
                    return new Vector2(0, 0);
                case 1:
                    return new Vector2(mySize.x, 0);
                case 2:
                    return new Vector2(mySize.x, mySize.y);
                case 3:
                    return new Vector2(0, mySize.y);
            }
        }

        private Vector2 mySize;
    }
}
