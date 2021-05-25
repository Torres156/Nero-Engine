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
        public float OutlineThickness = 1;

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
        public float Border_Scale = 0.3f;
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
            DrawShadowRectangle(target, gp - new Vector2(OutlineThickness, OutlineThickness), Size + new Vector2(OutlineThickness, OutlineThickness) * 2, Border_Opacity, Border_Scale);
            DrawRectangle(target, gp, Size, FillColor, OutlineThickness, OutlineColor);

            base.Draw(target, states);
        }
        #endregion
    }
}
