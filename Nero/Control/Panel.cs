using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Control
{
    using SFML.Window;
    using static Renderer;
    public class Panel : Bond
    {
        #region Properties
        /// <summary>
        /// Cor de fundo
        /// </summary>
        public Color FillColor = ThemeColors.Panel_FillColor;

        /// <summary>
        /// Espessura da borda
        /// </summary>
        public float OutlineThickness = 0;

        /// <summary>
        /// Cor da borda
        /// </summary>
        public Color OutlineColor = ThemeColors.Panel_OutlineColor;

        /// <summary>
        /// Transparência da borda
        /// </summary>
        public byte Border_Opacity = 255;

        /// <summary>
        /// Escala da borda
        /// </summary>
        public int Border_Rounded = 4;
        #endregion

        #region Methods
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public Panel(Bond bond) : base(bond)
        { }

        /// <summary>
        /// Desenha o painel
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            var gp = GlobalPosition();

            if (Border_Rounded > 0)
                DrawRoundedRectangle(target, gp, Size, FillColor, Border_Rounded, 16, OutlineThickness, OutlineColor);
            else
                DrawRectangle(target, gp, Size, FillColor, OutlineThickness, OutlineColor);

            base.Draw(target, states);
        }
        #endregion
    }
}
