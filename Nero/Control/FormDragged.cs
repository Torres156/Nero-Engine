using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Control
{
    public struct FormDragged
    {
        /// <summary>
        /// Formulario que será movido
        /// </summary>
        public Form form;

        /// <summary>
        /// Posição registrada do mouse
        /// </summary>
        public Vector2 Position;
        public Vector2 MousePosition;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="form"></param>
        /// <param name="Position"></param>
        public FormDragged(Form form, Vector2 Position)
        {
            this.form = form;
            MousePosition = Position;
            this.Position = form.Position;
        }
    }
}
