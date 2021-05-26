using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Control
{
    using static Renderer;
    using SFML.Graphics;
    using SFML.Window;
    using Color = Nero.Color;
    public class TextBox : ControlBase
    {
        #region Static
        /// <summary>
        /// Campo de texto com foco
        /// </summary>
        public static TextBox Focus
        {
            get
                => s_focus;
            set
            {
                s_focus?.Validate();
                s_focus = value;
            }
        }
        static TextBox s_focus = null;
        public static bool s_animation = false;
        #endregion

        #region Properties
        /// <summary>
        /// Texto
        /// </summary>
        public string Text
        {
            get
            {
                string copyValue = "";
                if (!Multiple_Lines)
                    copyValue = _text;
                else
                    copyValue = Lines[Line_SelectIndex];

                return copyValue;
            }
            set
            {
                if (!Multiple_Lines)
                    _text = value;
                else
                    Lines[Line_SelectIndex] = value;

                if (Character_CurrentIndex > value.Length + 1) Character_CurrentIndex = value.Length;
                OnTextChanged?.Invoke(this);
            }
        }
        string _text = "";

        /// <summary>
        /// Pré texto
        /// </summary>
        public string pre_Text = "";

        /// <summary>
        /// Máximo de caracteres
        /// </summary>
        public int MaxLength = 0;

        /// <summary>
        /// Cor de fundo
        /// </summary>
        public Color FillColor = ThemeColors.TextBox_FillColor;

        /// <summary>
        /// Cor de fundo
        /// </summary>
        public Color TextColor = new Color(210, 210, 210);

        /// <summary>
        /// Espessura da borda
        /// </summary>
        public float OutlineThickness = 0;

        /// <summary>
        /// Cor da borda
        /// </summary>
        public Color OutlineColor = ThemeColors.TextBox_OutlineColor;

        /// <summary>
        /// Modo senha
        /// </summary>
        public bool Password = false;

        /// <summary>
        /// Modo linhas multiplas
        /// </summary>
        public bool Multiple_Lines = false;

        /// <summary>
        /// Proximo campo de texto
        /// </summary>
        public TextBox Next = null;

        /// <summary>
        /// Alinhamento do texto
        /// </summary>
        public TextAligns Align = TextAligns.Left;


        /// <summary>
        /// Escala da borda
        /// </summary>
        public float Border_Radius = 4f;
        

        /// <summary>
        /// Bloqueia o campo de digitação
        /// </summary>
        public bool Blocked = false;

        /// <summary>
        /// Modo numérico
        /// </summary>
        public bool isNumeric = false;

        /// <summary>
        /// Valor mínimo
        /// </summary>
        public int Minimum = int.MinValue;

        /// <summary>
        /// Valor máximo
        /// </summary>
        public int Maximum = int.MaxValue;

        /// <summary>
        /// Linhas
        /// </summary>
        public List<string> Lines;

        /// <summary>
        /// Seleção de linha
        /// </summary>
        public int Line_SelectIndex = 0;

        /// <summary>
        /// Atual caractere
        /// </summary>
        public int Character_CurrentIndex = 0;

        /// <summary>
        /// Sugestões
        /// </summary>
        public Dictionary<string, string[]> Suggestion;

        /// <summary>
        /// Modo arredondado
        /// </summary>
        public bool Rounded = true;

        /// <summary>
        /// Valor
        /// </summary>
        public int Value
        {
            get => ToInt32();
            set
            {
                if (isNumeric) Text = value.ToString();
            }
        }

        RenderTexture render;
        #endregion

        #region Events
        public event HandleCommon OnValidate, OnEnter, OnTextChanged;
        #endregion

        #region Methods
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public TextBox(Bond bond) : base(bond)
        {
            Lines = new List<string>();
            Lines.Add("");
            Line_SelectIndex = 0;
            Suggestion = new Dictionary<string, string[]>();
        }

        /// <summary>
        /// Desenha o campo de texto
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (!Multiple_Lines)
                Draw_Normal(target);
            else
                Draw_Multiple(target);

            base.Draw(target, states);
        }

        /// <summary>
        /// Desenho multiplo
        /// </summary>
        /// <param name="target"></param>
        void Draw_Multiple(RenderTarget target)
        {
            var gp = GlobalPosition();

            // Fundo
            if (Rounded)
                DrawRoundedRectangle(target, gp, Size, FillColor, 4, 4, OutlineThickness, OutlineColor);

            else
                DrawRectangle(target, gp, Size, FillColor, OutlineThickness, OutlineColor);

            // Cria um render
            if (render == null)
            {
                render = new RenderTexture((uint)Size.x - 2, (uint)Size.y - 2);
                render.Smooth = true;
                render.SetActive(false);
            }

            render.Clear(Color.Transparent);

            int count = Lines.Count;
            for (int i = 0; i < count; i++)
            {
                string displaytext = Lines[i];
                if (HasFocus() && Line_SelectIndex == i && s_animation)
                {
                    if (Character_CurrentIndex > displaytext.Length) Character_CurrentIndex = displaytext.Length;
                    DrawText(render, "|", 12, new Vector2(GetTextWidth(displaytext.Substring(0, Character_CurrentIndex)) + 1, 2 + 14 * i), TextColor);
                }

                DrawText(render, displaytext, 12, new Vector2(2, 2 + 14 * i), TextColor);
            }

            render.Display();

            var spr = new Sprite(render.Texture);
            spr.Position = (gp + new Vector2(1, 1)).Floor();
            target.Draw(spr);

            if (HasFocus() && Text.Length > 0 && Suggestion.Count > 0) Draw_Suggestion(target);
        }

        /// <summary>
        /// Desenha normalmente
        /// </summary>
        /// <param name="target"></param>
        void Draw_Normal(RenderTarget target)
        {
            var gp = GlobalPosition();
            float X = 0;

            // Fundo
            if (Rounded)
                DrawRoundedRectangle(target, gp, Size, FillColor, Border_Radius, 8, OutlineThickness, OutlineColor);

            else
                DrawRectangle(target, gp, Size, FillColor, OutlineThickness, OutlineColor);

            // Pré texto
            if (!HasFocus() && Text.Trim().Length == 0)
            {
                switch (Align)
                {
                    case TextAligns.Left:
                        X = 4;
                        break;

                    case TextAligns.Center:
                        X = (Size.x - GetTextWidth(pre_Text)) / 2;
                        break;

                    case TextAligns.Right:
                        X = Size.x - 4 - GetTextWidth(pre_Text);
                        break;
                }

                DrawText(target, pre_Text, 12, gp + new Vector2(X, (Size.y / 2) - 7), TextColor - new Color(60, 60, 60,0));
                return;
            }

            string display = Text;
            if (Password)
            {
                display = "";
                foreach (var i in Text)
                    display += "*";
            }

            // Calcula o texto aparecer
            if (GetTextWidth(display) > Size.x - 8)
            {
                string display2 = "";
                for (int i = Text.Length - 1; i > 0; i--)
                    if (GetTextWidth(display.Substring(i)) < Size.x - 8)
                        display2 = display.Substring(i);
                    else
                        break;

                display = display2;
            }

            // Desenha o texto
            switch (Align)
            {
                case TextAligns.Left:
                    X = 4;
                    break;

                case TextAligns.Center:
                    X = (Size.x - GetTextWidth(display)) / 2;
                    break;

                case TextAligns.Right:
                    X = Size.x - 4 - GetTextWidth(display);
                    break;
            }

            if (HasFocus() && s_animation)
            {
                if (Character_CurrentIndex > Text.Length) Character_CurrentIndex = Text.Length;
                int diff = Text.Length - display.Length;
                DrawText(target, "|", 12, gp + new Vector2(X + (Character_CurrentIndex == 0 ? 0 : GetTextWidth(display.Substring(0, Character_CurrentIndex - diff))), (Size.y / 2) - 7), TextColor);
            }
            DrawText(target, display, 12, gp + new Vector2(X, (Size.y / 2) - 7), TextColor);
            if (HasFocus() && Text.Length > 0 && Suggestion.Count > 0) Draw_Suggestion(target);
        }

        public void AddSuggestion(string Name, string valueType, string desc)
            => Suggestion.Add(Name, new string[] { valueType, desc });

        /// <summary>
        /// Desenha as sugestões
        /// </summary>
        /// <param name="target"></param>
        void Draw_Suggestion(RenderTarget target)
        {
            var gp = GlobalPosition();

            var findKeys = Suggestion.ToList().FindAll(i => i.Key.Contains(Text) || (Text.Length >= i.Key.Length && i.Key.Contains(Text.Substring(0, i.Key.Length - 1))));
            int count = findKeys.Count;
            if (count > 0)
            {
                int h = 4;
                int w = 150;
                foreach (var i in findKeys)
                {
                    if (GetTextWidth(i.Key) + (i.Value[0].Length > 0 ? GetTextWidth($"({i.Value[0]})") + 8 : 4) > w) w = (int)GetTextWidth(i.Key) + (int)(i.Value[0].Length > 0 ? GetTextWidth($"({i.Value[0]})") + 8 : 4);
                    h += 14 + GetWordWrap(i.Value[1], w - 4).Length * 14 + 4;
                }

                var pos = gp + new Vector2(2, (Multiple_Lines ? 2 + Line_SelectIndex * 14 + 20 : 22));
                DrawRectangle(target, pos, new Vector2(w, h), new Color(10, 10, 10, 200), 1, new Color(100, 100, 100, 200));

                int off = 0;
                for (int i = 0; i < count; i++)
                {
                    DrawText(target, findKeys[i].Key, 12, pos + new Vector2(2, 2 + off), Color.White);
                    if (findKeys[i].Value[0].Length > 0)
                        DrawText(target, $"({findKeys[i].Value[0]})", 12, pos + new Vector2(2 + GetTextWidth(findKeys[i].Key) + 4, 2 + off), new Color(59, 104, 156));

                    var words = GetWordWrap(findKeys[i].Value[1], w - 4);
                    for (var x = 0; x < words.Length; x++)
                        DrawText(target, words[x], 12, pos + new Vector2(2, 2 + off + 14 + 14 * x), new Color(200, 200, 200));

                    off += 14 + 14 * words.Length + 4;
                }
            }
        }

        /// <summary>
        /// Verifica se está com foco
        /// </summary>
        /// <returns></returns>
        public bool HasFocus()
            => Focus == this;

        /// <summary>
        /// Quando o Tab é pressionado
        /// </summary>
        public void TabPress()
        {
            Next?.SetFocus();
        }

        /// <summary>
        /// Quando o Enter é pressionado
        /// </summary>
        public void EnterPress()
        {
            OnEnter?.Invoke(this);
            if (Multiple_Lines)
            {
                string newLine = "";
                if (Character_CurrentIndex < Text.Length)
                {
                    newLine = Text.Substring(Character_CurrentIndex);
                    Text = Text.Remove(Character_CurrentIndex);
                }
                Lines.Insert(Line_SelectIndex + 1, newLine);
                Line_SelectIndex++;
                Character_CurrentIndex = 0;
            }
        }

        /// <summary>
        /// Coloca o foco
        /// </summary>
        public void SetFocus()
        {
            if (!Blocked)
            {
                if (Focus != this)
                {
                    Focus = this;
                    Character_CurrentIndex = Text.Length;
                }
            }
        }

        /// <summary>
        /// Valida o campo de texto
        /// </summary>
        public void Validate()
        {
            if (isNumeric)
            {
                if (!Text.IsNumeric())
                    Text = "0";

                int value = int.Parse(Text);
                if (value < Minimum) Text = Minimum.ToString();
                if (value > Maximum) Text = Maximum.ToString();
            }

            OnValidate?.Invoke(this);
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
                if (!HasFocus())
                {
                    SetFocus();
                }
                else
                {
                    if (Multiple_Lines)
                    {
                        var gp = GlobalPosition();
                        int count = Lines.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var pos = gp + new Vector2(2, 2 + 14 * i);
                            if (e.X >= pos.x && e.X <= pos.x + Size.x - 4)
                                if (e.Y >= pos.y && e.Y <= pos.y + 14)
                                {
                                    Line_SelectIndex = i;
                                    Character_CurrentIndex = Text.Length;
                                    for (int x = 0; x < Text.Length; x++)
                                        if (e.X >= pos.x + GetTextWidth(Text.Substring(0, x)) && e.X <= pos.x + GetTextWidth(Text.Substring(0, x) + GetTextWidth(Text.Substring(x, 1))))
                                        {
                                            Character_CurrentIndex = x;
                                            break;
                                        }
                                    break;
                                }
                        }
                    }
                }
            }
            return result;
        }

        public int ToInt32()
        {
            int value = _text.IsNumeric() ? _text.ToInt32() : Minimum;
            if (value < Minimum) value = Minimum;
            if (value > Maximum) value = Maximum;
            return value;
        }

        public void ClearLines()
        {
            Lines.Clear();
            Lines.Add("");
            Line_SelectIndex = 0;
        }
        #endregion
    }
}
