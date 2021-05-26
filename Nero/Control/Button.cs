using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Control
{
    using SFML.Window;
    //using SFML.Graphics;
    using static Renderer;
    using Color = Nero.Color;
    public class Button : ControlBase
    {
        #region Properties
        /// <summary>
        /// Texto no botão
        /// </summary>
        public string[] Text = new string[(int)Languages.count];

        /// <summary>
        /// Cor do Botão
        /// </summary>
        public Color FillColor = ThemeColors.Button_FillColor;

        /// <summary>
        /// Espessura da borda
        /// </summary>
        public float OutlineThickness = 0f;

        /// <summary>
        /// Cor da borda
        /// </summary>
        public Color OutlineColor = ThemeColors.Button_OutlineColor;

        /// <summary>
        /// Transparência da borda
        /// </summary>
        public byte Border_Opacity = 255;

        /// <summary>
        /// Escala da borda
        /// </summary>
        public float Border_Scale = 0.1f;

        /// <summary>
        /// Cor do texto
        /// </summary>
        public Color TextColor = new Color(10, 10, 10, 230);


        /// <summary>
        /// Tamanho do texto
        /// </summary>
        public int TextSize = 12;

        /// <summary>
        /// Textura
        /// </summary>
        public Texture Texture = null;

        /// <summary>
        /// Check automatico
        /// </summary>
        public bool CanAutoCheck = true;

        /// <summary>
        /// Usa o cursor de mão quando está sobre o botão
        /// </summary>
        public bool UseCursorHand = true;

        /// <summary>
        /// Usa multiplas línguas
        /// </summary>
        public bool UseMultipleLanguage = true;

        /// <summary>
        /// Marcador
        /// </summary>
        public bool Checked
        {
            get => _check;
            set
            {
                _check = value;
                OnCheckedChanged?.Invoke(this);
            }
        }
        bool _check = false;

        /// <summary>
        /// Modo marcação
        /// </summary>
        public bool isChecked = false;

        /// <summary>
        /// Arredondamento
        /// </summary>
        public int Border_Rounded = 4;

        /// <summary>
        /// Cor da borda
        /// </summary>
        public Color OutlineColorChecked = new Color(60, 165, 253);

        /// <summary>
        /// Espessura da borda
        /// </summary>
        public float OutlineThicknessChecked = 2f;

        bool _debugHover = false;
        bool _debugPress = false;
        #endregion

        public event HandleCommon OnCheckedChanged;

        #region Methods
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public Button(Bond bond) : base(bond)
        {
            for (int i = 0; i < (int)Languages.count; i++)
                Text[i] = "";
        }

        /// <summary>
        /// Desenha o botão
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            var gp = GlobalPosition();

            if (_debugHover && !Hover()) _debugHover = false;

            // Fundo 
            if (Border_Rounded > 0)
            {
                if (isChecked)
                    DrawRoundedRectangle(target, gp, Size, FillColor, Border_Rounded, 16,
                        Checked ? OutlineThicknessChecked : OutlineThickness, Checked ? OutlineColorChecked : OutlineColor);
                else
                    DrawRoundedRectangle(target, gp, Size, FillColor, Border_Rounded, 16, OutlineThickness, OutlineColor);
                if (_debugHover)
                    if (_debugPress)
                        DrawRoundedRectangle(target, gp, Size, new Color(0, 0, 0, 60), Border_Rounded, 4);
                    else
                        DrawRoundedRectangle(target, gp, Size, new Color(255, 255, 255, 60), Border_Rounded, 4);
            }
            else
            {
                if (isChecked)
                    DrawRectangle(target, gp, Size, FillColor,
                        Checked ? OutlineThicknessChecked : OutlineThickness, Checked ? OutlineColorChecked : OutlineColor);
                else
                    DrawRectangle(target, gp, Size, FillColor, OutlineThickness, OutlineColor);
                if (_debugHover)
                    if (_debugPress)
                        DrawRectangle(target, gp, Size, new Color(0, 0, 0, 60));
                    else
                        DrawRectangle(target, gp, Size, new Color(255, 255, 255, 60));

            }

            byte opacity = 225;
            if (_debugHover)
                opacity = (byte)(_debugPress ? 200 : 255);

            if (Texture != null)
                DrawTexture(target, Texture, new Rectangle(gp + new Vector2(1), Size - new Vector2(2)), new Color(255, 255, 255, opacity));

            // Texto do botão
            string currentText = UseMultipleLanguage ? Text[(int)Game.CurrentLanguage] : Text[0];
            DrawText(target, currentText, TextSize, gp + new Vector2((Size.x - GetTextWidth(currentText, (uint)TextSize)) / 2, (Size.y / 2) - (TextSize * .7f)), TextColor);



            base.Draw(target, states);
        }

        /// <summary>
        /// Movimento do mouse
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseMoved(Vector2 e)
        {
            var result = base.MouseMoved(e);
            _debugHover = false;
                        
            if (Hover())
            {
                if (UseCursorHand)
                    Game.SetCursor(Cursor.CursorType.Hand);

                _debugHover = true;
            }                

            return result;
        }

        /// <summary>
        /// Mouse pressionado
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MousePressed(MouseButtonEvent e)
        {
            var result = base.MousePressed(e);
            if (_debugHover)
                _debugPress = true;
            return result;
        }

        /// <summary>
        /// Solta o clique
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseReleased(MouseButtonEvent e)
        {
            var result = base.MouseReleased(e);
            if (_debugPress && isChecked && CanAutoCheck)
                Checked = !Checked;

            _debugPress = false;
            return result;
        }

        public override void Destroy()
        {
            base.Destroy();

            if (Texture != null)
                Texture.Destroy();
        }

        public void SetText(Languages lang, string text)
            => Text[(int)lang] = text;
        #endregion 
    }
}
