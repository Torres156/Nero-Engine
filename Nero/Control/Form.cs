using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Control
{
    using SFML.System;
    using SFML.Window;
    using static Renderer;
    public class Form : Bond
    {
        public const int BAR_HEIGHT = 35;

        #region Properties
        /// <summary>
        /// Titulo do formulário
        /// </summary>
        public string[] Title = new string[(int)Languages.count];
                
        /// <summary>
        /// Cor de fundo
        /// </summary>
        public Color FillColor = new Color(0, 0, 0, 240);

        /// <summary>
        /// Botão de fechar
        /// </summary>
        public bool Button_Exit = true;
        bool hover_exit = false;

        /// <summary>
        /// Transparência da borda
        /// </summary>
        public byte Border_Opacity = 255;

        /// <summary>
        /// Arredondamento
        /// </summary>
        public int Border_Rounded = 4;

        /// <summary>
        /// Pode ser arrastado
        /// </summary>
        public bool canDragged = true;

        /// <summary>
        /// Usar multiplas linguas
        /// </summary>
        public bool UseMultipleLanguage = true;
                
        private bool isModal = false;
        #endregion

        #region Methods
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public Form(Bond bond) : base(bond)
        {
            for (int i = 0; i < (int)Languages.count; i++)
                Title[i] = "";
        }

        /// <summary>
        /// Desenha o formulario
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            var gp = GlobalPosition();

            // Fundo
            if (Border_Rounded > 0)
                DrawRoundedRectangle(target, gp, Size, FillColor, Border_Rounded, 16);
            else
                DrawRectangle(target, gp, Size, FillColor);

            var currentTitle = UseMultipleLanguage ? Title[(int)Game.CurrentLanguage] : Title[0];
            DrawText(target, currentTitle, 20, gp + new Vector2((Size.x - GetTextWidth(currentTitle, 20)) / 2, 5), Color.White);

            //var cline = new Color(80, 80, 80,80);
            //DrawLine(target, gp + new Vector2(20, BAR_HEIGHT), gp + new Vector2(Size.x - 20, BAR_HEIGHT), cline);

            // Botão de fechar
            if (Button_Exit)
            {
                if (hover_exit)
                    DrawRoundedRectangle(target, gp + new Vector2(Size.x - 24, 4), new Vector2(20), new Color(255, 255, 255, 20), 4, 4);
                DrawText(target, "X", 16, gp + new Vector2(Size.x - 24 + (20 - GetTextWidth("X", 16)) / 2, 4), Color.White);
            }
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
            var gp = GlobalPosition();

            hover_exit = false;

            if (Hover())
                if (Button_Exit && e.x >= gp.x + (Size.x - 24) && e.x <= gp.x + Size.x - 4
                    && e.y >= gp.y + 4 && e.y <= gp.y + 28)
                {
                    hover_exit = true;
                    Game.SetCursor(Cursor.CursorType.Hand);
                }


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

            if (Hover())
            {
                if (Button_Exit && hover_exit)
                {
                    Hide();
                    Bond?.RemoveFocusForm(this);
                }
            }

            return result;
        }

        /// <summary>
        /// Clique pressionado
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MousePressed(MouseButtonEvent e)
        {
            var result = base.MousePressed(e);
            if (Hover())
            {
                Bond?.SetFocusForm(this);

                var gp = GlobalPosition();
                if (canDragged)
                    if (e.X >= gp.x && e.X <= gp.x + Size.x - 24)
                        if (e.Y >= gp.y && e.Y <= gp.y + BAR_HEIGHT)
                        {
                            var mousep = new Vector2(e.X, e.Y) - gp;
                            Bond?.SetDragForm(this, mousep);
                        }
            }
            return result;
        }

        /// <summary>
        /// Deixa o formulário visivel
        /// </summary>
        public new void Show()
        {
            base.Show();
            Bond?.SetFocusForm(this);
        }

        public void ShowDialog()
        {
            isModal = true;
            Game.GetScene().form_Priority = this;
            Show();
        }

        /// <summary>
        /// Esconde o formulário
        /// </summary>
        public new void Hide()
        {
            base.Hide();
            Bond?.RemoveFocusForm(this);

            if (isModal)
            {
                Game.GetScene().RemovePriority();
            }
        }

        /// <summary>
        /// Altera a visibilidade do formulário
        /// </summary>
        public new void Toggle()
        {
            if (Visible)
                Hide();
            else
                Show();
        }

        public virtual void Form_Dragged()
        { }

        public void SetTitle(Languages lang, string text)
            => Title[(int)lang] = text;
        #endregion

    }
}
