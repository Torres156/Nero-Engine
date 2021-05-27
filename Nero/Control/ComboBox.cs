using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Control
{
    using SFML.Window;
    using static Renderer;
    public class ComboBox : Bond
    {
        #region Properties
        /// <summary>
        /// Items
        /// </summary>
        public List<string> Item;

        /// <summary>
        /// ID do item selecionado
        /// </summary>
        public int SelectIndex
        {
            get => _selectIndex;
            set
            {
                _selectIndex = value;
                OnSelectIndex?.Invoke(this);
            }
        }
        int _selectIndex = -1;

        /// <summary>
        /// Item selecionado
        /// </summary>
        public string SelectItem
        {
            get => Item.Count > 0 && _selectIndex > -1 ? Item[_selectIndex] : "";
            set
            {
                if (Item.Count > 0 && _selectIndex > -1)
                    Item[_selectIndex] = value;
            }
        }

        /// <summary>
        /// Quantia de item
        /// </summary>
        public int Length
            => Item.Count;

        /// <summary>
        /// Tamanho da caixa de item
        /// </summary>
        public new Vector2 Size
        {
            get => base.Size;
            set
               => base.Size = new Vector2(value.x, _openBox ? 18 + 14 * Length : 18);
        }

        /// <summary>
        /// Cor de fundo
        /// </summary>
        public Color FillColor = ThemeColors.ComboBox_FillColor;

        /// <summary>
        /// Espessura da borda
        /// </summary>
        public float OutlineThickness = 1;

        /// <summary>
        /// Cor da borda
        /// </summary>
        public Color OutlineColor = ThemeColors.ComboBox_OutlineColor;

        /// <summary>
        /// Transparência da borda
        /// </summary>
        public byte Border_Opacity = 255;

        /// <summary>
        /// Escala da borda
        /// </summary>
        public float Border_Scale = 0.2f;

        bool _openBox = false;
        int _hover = -1;
        Button b_open;

        #endregion

        #region Events
        public event HandleCommon OnSelectIndex;
        #endregion

        #region Methods
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public ComboBox(Bond bond) : base(bond)
        {
            Item = new List<string>();

            b_open = new Button(this)
            {
                Size = new Vector2(14, 14),
                Anchor = Anchors.TopRight,
                Position = new Vector2(2, 2),
                Border_Scale = 0,
                Border_Opacity = 0,
            };
            b_open.OnDraw += B_open_OnDraw;
            b_open.OnMouseReleased += B_open_OnMouseReleased;
        }

        /// <summary>
        /// Abre a caixa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void B_open_OnMouseReleased(ControlBase sender, MouseButtonEvent e)
        {
            _openBox = !_openBox;
            if (_openBox)
            {
                if (Game.GetScene() != null)
                    Game.GetScene()?.SetControlPriority(this);
                else
                    Bond?.SetControlPriority(this);
            }
            else
            {
                if (Game.GetScene() != null)
                    Game.GetScene()?.SetControlPriority(null);
                else
                    Bond?.SetControlPriority(null);
            }
            Size = new Vector2(Size.x, 0); // Atualiza
        }

        /// <summary>
        /// Desenha o botão de abrir a caixa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        private void B_open_OnDraw(ControlBase sender, RenderTarget target)
        {
            var gp = sender.GlobalPosition();
            DrawText(target, "˅", 12, gp + new Vector2(4, 1), new Color(10,10,10));
        }

        /// <summary>
        /// Desenha o ComboBox
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            var gp = GlobalPosition();

            // Fundo
            if (_openBox) DrawShadowRectangle(target, gp + new Vector2(0, 19) - new Vector2(OutlineThickness, OutlineThickness),
                new Vector2(Size.x + OutlineThickness * 2, Size.y - 18 + OutlineThickness * 2), Border_Opacity, Border_Scale);
            DrawRectangle(target, gp, new Vector2(Size.x, 18), FillColor, OutlineThickness, OutlineColor);
            DrawShadowRectangle(target, gp - new Vector2(OutlineThickness, OutlineThickness),
                new Vector2(Size.x + OutlineThickness * 2, 18 + OutlineThickness * 2), Border_Opacity, Border_Scale);

            // Item selecionado
            string displaySelect = "";
            if (GetTextWidth(SelectItem) > Size.x - 22)
            {
                foreach (var s in SelectItem)
                {
                    if (GetTextWidth(displaySelect + s) < Size.x - 22)
                        displaySelect += s;
                    else
                        break;
                }
            }
            else
                displaySelect = SelectItem;
            DrawText(target, displaySelect, 12, gp + new Vector2(4, 1), new Color(200, 200, 200));

            // Open?
            if (_openBox)
            {
                DrawRectangle(target, gp + new Vector2(0, 19), new Vector2(Size.x, Size.y - 18), FillColor, OutlineThickness, OutlineColor);

                if (_hover > -1)
                    DrawRectangle(target, gp + new Vector2(0, 19 + 14 * _hover), new Vector2(Size.x, 14), new Color(255, 255, 255, 30));

                for (int i = 0; i < Item.Count; i++)
                {
                    string displayItem = "";
                    if (GetTextWidth(Item[i]) > Size.x - 8)
                    {
                        foreach (var s in Item[i])
                        {
                            if (GetTextWidth(displayItem + s) < Size.x - 8)
                                displayItem += s;
                            else
                                break;
                        }
                    }
                    else
                        displayItem = Item[i];
                    DrawText(target, displayItem, 12, gp + new Vector2(4, 19 + 14 * i), new Color(180, 180, 180));
                }

            }


            base.Draw(target, states);
        }

        public override bool MouseMoved(Vector2 e)
        {
            var result = base.MouseMoved(e);
            var gp = GlobalPosition();
            _hover = -1;

            if (Hover() && _openBox && Item.Count > 0)
                for (int i = 0; i < Item.Count; i++)
                {
                    var pos = gp + new Vector2(0, 19 + 14 * i);
                    if (e.x >= pos.x && e.x <= pos.x + Size.x && e.y >= pos.y && e.y <= pos.y + 14)
                    {
                        _hover = i;
                        break;
                    }
                }

            return result;
        }

        public override bool MouseReleased(MouseButtonEvent e)
        {
            var result = base.MouseReleased(e);

            if (Hover() && _openBox)
            {
                if (_hover > -1)
                {
                    SelectIndex = _hover;
                    _openBox = false;
                    Size = new Vector2(Size.x, 0);
                    if (Game.GetScene() != null)
                        Game.GetScene()?.SetControlPriority(null);
                    else
                        Bond?.SetControlPriority(null);
                }
            }

            return result;
        }

        #endregion

    }
}
