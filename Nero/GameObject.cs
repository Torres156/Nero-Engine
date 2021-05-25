using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    public interface GameObject : Drawable
    {
        Vector2 Position { get; set; }
        Vector2 Size { get; set; }

        /// <summary>
        /// Desenha
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        new void Draw(RenderTarget target, RenderStates states);
    }
}
