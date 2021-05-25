using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.SFML.Graphics
{
    ////////////////////////////////////////////////////////////
    /// <summary>
    /// Types of primitives that a VertexArray can render.
    ///
    /// Points and lines have no area, therefore their thickness
    /// will always be 1 pixel, regardless the current transform
    /// and view.
    /// </summary>
    ////////////////////////////////////////////////////////////
    public enum PrimitiveType
    {
        /// List of individual points
        Points,

        /// List of individual lines
        Lines,

        /// List of connected lines, a point uses the previous point to form a line
        LineStrip,

        /// List of individual triangles
        Triangles,

        /// List of connected triangles, a point uses the two previous points to form a triangle
        TriangleStrip,

        /// List of connected triangles, a point uses the common center and the previous point to form a triangle
        TriangleFan,

        /// List of individual quads
        Quads,
    }
}
