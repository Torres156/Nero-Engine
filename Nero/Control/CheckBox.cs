using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Control
{
    using SFML.Window;    
    using static Renderer;
    public class CheckBox : Control
    {
        #region Properties
        /// <summary>
        /// Alinhamento da caixa
        /// </summary>
        public CheckBoxAligns Align = CheckBoxAligns.Left;

        /// <summary>
        /// Texto
        /// </summary>
        public string[] Text = new string[(int)Languages.count];

        /// <summary>
        /// Valor
        /// </summary>
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                OnChecked?.Invoke(this);
            }
        }
        bool _checked = false;

        public bool CheckedOnly = false;

        /// <summary>
        /// Transparência da borda
        /// </summary>
        public byte Border_Opacity = 255;

        /// <summary>
        /// Escala da borda
        /// </summary>
        public float Border_Scale = 0.15f;

        /// <summary>
        /// Usa multiplas línguas
        /// </summary>
        public bool UseMultipleLanguage = true;
        #endregion

        #region Events
        public event HandleCommon OnChecked;
        #endregion

        #region Methods
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public CheckBox(Bond bond) : base(bond)
        {
            for (int i = 0; i < (int)Languages.count; i++)
                Text[i] = "";
        }

        /// <summary>
        /// Desenha o checkbox
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            var gp = GlobalPosition();
            var currentText = UseMultipleLanguage ? Text[(int)Game.CurrentLanguage] : Text[0];
            Size = new Vector2(18 + GetTextWidth(currentText, 11), 14);

            if (Align == CheckBoxAligns.Left)
            {
                // Box
                DrawRoundedRectangle(target, gp, new Vector2(14, 14), new Color(10, 10, 10),4,4, 1, new Color(200, 200, 200));
               // DrawShadowRectangle(target, gp - new Vector2(1, 1), new Vector2(16, 16), Border_Opacity, Border_Scale);

                // Valor
                if (_checked)
                    DrawRoundedRectangle(target, gp + new Vector2(1, 1), new Vector2(12, 12), new Color(200, 200, 200),4,4);

                DrawText(target, currentText, 11, gp + new Vector2(18, 0), new Color(200, 200, 200));
            }
            else if (Align == CheckBoxAligns.Right)
            {
                // Box
                DrawRectangle(target, gp + new Vector2(Size.x - 14, 0), new Vector2(14, 14), new Color(10, 10, 10), 1, new Color(200, 200, 200));
                DrawShadowRectangle(target, gp + new Vector2(Size.x - 15, 1), new Vector2(16, 16), Border_Opacity, Border_Scale);

                // Valor
                if (_checked)
                    DrawRectangle(target, gp + new Vector2(Size.x - 14, 0) + new Vector2(1, 1), new Vector2(12, 12), new Color(200, 200, 200));

                DrawText(target, currentText, 11, gp, new Color(200, 200, 200));
            }

            base.Draw(target, states);
        }

        public void SetText(Languages lang, string text)
            => Text[(int)lang] = text;

        /// <summary>
        /// Solta o clique
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseReleased(MouseButtonEvent e)
        {
            var result = base.MouseReleased(e);

            if (Hover())
            {
                if (!CheckedOnly)
                    Checked = !Checked;
                else
                {
                    if (!Checked)
                        Checked = true;
                }
            }
            return result;
        }
        #endregion
    }
}
