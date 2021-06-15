using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Control
{
    using SFML.Window;
    using SFML.Graphics;
    using static Renderer;
    using Color = Nero.Color;
    public class HScroll : ControlBase
    {
        /// <summary>
        /// Máximo de valor
        /// </summary>
        public int Maximum = 100;

        /// <summary>
        /// Valor atual
        /// </summary>
        public int Value = 0;

        /// <summary>
        /// Cor de fundo
        /// </summary>
        public Color FillColor = new Color(40, 40, 40, 200);

        /// <summary>
        /// Espessura da borda
        /// </summary>
        public float OutlineThickness = 1;

        /// <summary>
        /// Cor da borda
        /// </summary>
        public Color OutlineColor = ThemeColors.V_Scroll_OutlineColor;

        /// <summary>
        /// Modo arredondado
        /// </summary>
        public bool Rounded = true;

        /// <summary>
        /// Bloquei o scroll
        /// </summary>
        public bool BlockScrolled = false;


        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public HScroll(Bond bond) : base(bond)
        {
        }

        /// <summary>
        /// Desenha a scroll
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (Value < 0) Value = 0;
            if (Value > Maximum) Value = Maximum;
            var gp = GlobalPosition();

            if (Rounded)
                DrawRoundedRectangle(target, gp, Size, FillColor, 4, 4);
            else
                DrawRectangle(target, gp, Size, FillColor);

            float speedS = Game.DeltaTime * 10f;
            if (smooth_y > 0)
            {
                smooth_y -= speedS;
                if (smooth_y < 0) smooth_y = 0;
            }
            else
            {
                smooth_y += speedS;
                if (smooth_y > 0) smooth_y = 0;
            }


            base.Draw(target, states);

            float percent = (1f) / (Maximum + 1);
            float height = (Size.x - 2) * percent;
            float height_real = height < 8 ? 8 : height;
            percent = Value / (float)(Maximum + 1);
            float posY = (Size.x - 2) * percent;
            posY = posY < 0 ? 0 : posY;
            if (posY + height_real > Size.x - 2) posY = Size.x - 2 - height_real;
            if (Rounded)
                DrawRoundedRectangle(target, gp + new Vector2(posY, 1),
                new Vector2(height_real, Size.y - 2), OutlineColor, 2, 4);
            else
                DrawRectangle(target, gp + new Vector2(posY < 1 ? 1 : (posY + height_real > Size.y - 1 ? Size.y - 1 - height_real : posY),1),
                new Vector2(height_real, Size.y - 2), OutlineColor);
        }

        /// <summary>
        /// Scroll do Mouse
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MouseScrolled(MouseWheelScrollEventArgs e)
        {
            var result = base.MouseScrolled(e);
            if (Hover() && !BlockScrolled)
            {
                if (e.Wheel == Mouse.Wheel.VerticalWheel)
                {
                    var v = Value;
                    Value += (int)(e.Delta * 1);
                    if (Value < 0) Value = 0;
                    if (Value > Maximum) Value = Maximum;
                    smooth_y += v - Value;
                    return true;
                }
            }

            return result;
        }

        /// <summary>
        /// Scroll do Mouse
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool MouseScrolled2(MouseWheelScrollEventArgs e)
        {
            var result = base.MouseScrolled(e);

            if (e.Wheel == Mouse.Wheel.VerticalWheel)
            {
                var v = Value;
                Value += (int)(e.Delta * 1);
                if (Value < 0) Value = 0;
                if (Value > Maximum) Value = Maximum;
                smooth_y += v - Value;
                return true;
            }

            return result;
        }

        bool _press = false;
        int _y = 0;
        float smooth_y = 0;
        public override bool MousePressed(MouseButtonEvent e)
        {
            var result = base.MousePressed(e);
            if (Hover())
            {
                var gp = GlobalPosition();
                float percent = (1f) / (Maximum + 1);
                float height = (Size.x - 2) * percent;
                float height_real = height < 8 ? 8 : height;
                percent = Value / (float)(Maximum + 1);
                float posY = (Size.x - 2) * percent;
                posY = posY < 0 ? 0 : posY;
                if (posY + height_real > Size.x - 2) posY = Size.x - 2 - height_real;
                var pos = gp + new Vector2(posY, 1);
                if (e.Button == Mouse.Button.Left && Maximum > 1)
                    if (e.X >= pos.x && e.X <= pos.x + height_real)
                        if (e.Y >= pos.y && e.Y <= pos.y + Size.y )
                        {
                            _press = true;
                            _y = e.Y;
                            Game.GetScene()?.SetControlPriority(this);
                        }
            }
            return result;
        }

        public override bool MouseReleased(MouseButtonEvent e)
        {
            _press = false;
            Game.GetScene()?.SetControlPriority(null);
            return base.MouseReleased(e);
        }

        public override bool MouseMoved(Vector2 e)
        {
            var result = base.MouseMoved(e);

            if (Maximum > 1 && Mouse.IsButtonPressed(Mouse.Button.Left) && _press)
            {
                var calcy = e.x - (GlobalPosition().x + 1);
                if (calcy < 0) calcy = 0;

                float percent = calcy / (Size.x - 2);
                if (percent > 1f) percent = 1;

                var v = Value;
                Value = (int)(percent * Maximum);
                if (Value < 0) Value = 0;
                if (Value > Maximum) Value = Maximum;
                smooth_y = v - Value;
            }
            return result;
        }
    }
}
