using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Control
{
    using SFML.Window;
    using SFML.Graphics;
    using static Renderer;
    using Color = Nero.Color;
    public class ListBox : Bond
    {
        /// <summary>
        /// Cor de fundo
        /// </summary>
        public Color FillColor = ThemeColors.ListBox_FillColor;

        /// <summary>
        /// Espessura da borda
        /// </summary>
        public float OutlineThickness = 0;

        /// <summary>
        /// Cor da borda
        /// </summary>
        public Color OutlineColor = ThemeColors.ListBox_OutlineColor;

        /// <summary>
        /// Coleção de items
        /// </summary>
        public List<string> Item;

        /// <summary>
        /// Marcação
        /// </summary>
        public List<bool> Item_Checked;

        /// <summary>
        /// Modo marcação
        /// </summary>
        public bool ListChecked = false;

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

        int _selectindex = -1;

        /// <summary>
        /// Quantia de item
        /// </summary>
        public int Length
            => Item.Count;

        public bool Rounded = true;

        VScroll scroll;
        RenderTexture render;
        bool iscreategraphics = false;
        SceneBase scene = null;
        #region Events
        public event HandleCommon OnSelectIndex;
        #endregion

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public ListBox(Bond bond) : base(bond)
        {
            Item = new List<string>();
            Item_Checked = new List<bool>();
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public ListBox(Bond bond, SceneBase scene) : this(bond)
        {
            this.scene = scene;
        }

        /// <summary>
        /// Cria o gráfico
        /// </summary>
        public void CreateGraphic()
        {
            render = new RenderTexture((uint)Size.x + 1 - 16, (uint)Size.y - 2);
            render.Smooth = true;
            render.SetActive(false);

            scroll = new VScroll(this)
            {
                Size = new Vector2(8, Size.y - 4),
                Anchor = Anchors.TopRight,
                Position = new Vector2(2, 2),
                Maximum = 0,
                Value = 0,
            };
            iscreategraphics = true;
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (!iscreategraphics) CreateGraphic();

            if (Item.Count == 0) SelectIndex = -1;

            var gp = GlobalPosition();
            if (Rounded)
                DrawRoundedRectangle(target, gp, Size, FillColor, 4, 4, OutlineThickness, OutlineColor);
            else
                DrawRectangle(target, gp, Size, FillColor, OutlineThickness, OutlineColor);

            if (scroll != null)
            {
                int max = Length * 14 - (int)Size.y + 4;
                scroll.Maximum = max < 0 ? 0 : max;
            }

            if (render != null && Length > 0 && scroll != null)
            {
                render.Clear(Color.Transparent);
                if (SelectIndex > -1)
                    if (Rounded)
                        DrawRoundedRectangle(render, new Vector2((ListChecked ? 18 : 0), 14 * SelectIndex - scroll.Value), new Vector2(render.Size.X - (ListChecked ? 18 : 0), 14), new Color(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)(OutlineColor.A - 40)), 4f, 4);
                    else
                        DrawRectangle(render, new Vector2((ListChecked ? 18 : 0), 14 * SelectIndex - scroll.Value), new Vector2(render.Size.X - (ListChecked ? 18 : 0), 14), new Color(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)(OutlineColor.A - 40)));

                for (int i = 0; i < Length; i++)
                {
                    string displaytext = "";
                    if (GetTextWidth(Item[i]) > Size.x - 4 - 16 - (ListChecked ? 18 : 0))
                    {
                        foreach (var s in Item[i])
                        {
                            if (GetTextWidth(displaytext + s) < Size.x - 4 - 16)
                                displaytext += s;
                            else
                                break;
                        }
                    }
                    else
                        displaytext = Item[i];

                    if (ListChecked)
                    {
                        if (Rounded)
                            DrawRoundedRectangle(render, new Vector2(2, 14 * i - scroll.Value + 1), new Vector2(11, 11), FillColor, 4, 4, 1, OutlineColor);
                        else
                            DrawRectangle(render, new Vector2(2, 14 * i - scroll.Value + 1), new Vector2(11, 11), FillColor, 1, OutlineColor);

                        if (Item_Checked[i])
                            if (Rounded)
                                DrawRoundedRectangle(render, new Vector2(2 + 1, 14 * i - scroll.Value + 2), new Vector2(9, 9), OutlineColor,4,4);
                            else
                                DrawRectangle(render, new Vector2(2 + 1, 14 * i - scroll.Value + 2), new Vector2(9, 9), OutlineColor);
                    }

                    DrawText(render, displaytext, 12, new Vector2(2 + (ListChecked ? 18 : 0), 14 * i - scroll.Value - 1), Color.White);
                }
                render.Display();
                var spr = new Sprite(render.Texture);
                spr.Position = (gp + new Vector2(1, 1)).Floor();
                target.Draw(spr);
            }

            base.Draw(target, states);
        }

        public override bool MouseReleased(MouseButtonEvent e)
        {
            var result = base.MouseReleased(e);

            if (Hover() && Length > 0 && scroll != null)
            {
                var gp = GlobalPosition();
                if (e.X >= gp.x + 1 && e.X <= gp.x + Size.x - 16)
                    if (e.Y >= gp.y + 1 && e.Y <= gp.y + Size.y - 2)
                        for (int i = 0; i < Length; i++)
                        {
                            var pos = gp + new Vector2(1, 1 + 14 * i - scroll.Value);
                            if (e.Y >= pos.y && e.Y <= pos.y + 14)
                            {
                                if (ListChecked && e.X >= pos.x + 2 && e.X <= pos.x + 14)
                                {
                                    Item_Checked[i] = !Item_Checked[i];
                                    break;
                                }
                                if (e.X >= pos.x + (ListChecked ? 18 : 0))
                                {
                                    SelectIndex = i;
                                    break;
                                }
                            }


                        }
            }

            return result;
        }

        public override bool MouseScrolled(MouseWheelScrollEventArgs e)
        {
            if (Hover() && scroll != null)
                return scroll.MouseScrolled2(e);
            return false;
        }

        public void Add(string txt)
        {
            Item.Add(txt);
            Item_Checked.Add(false);
        }

        public void Add(string txt, bool check)
        {
            Item.Add(txt);
            Item_Checked.Add(check);
        }

        /// <summary>
        /// Remove um item
        /// </summary>
        /// <param name="txt"></param>
        public void RemoveAt(int at)
        {
            if (at < Item.Count)
            {
                Item.RemoveAt(at);
                Item_Checked.RemoveAt(at);

                if (_selectindex >= Item.Count && Item.Count > 0) _selectindex--;
            }
        }

        public void Clear()
        {
            Item.Clear();
            Item_Checked.Clear();
        }
    }
}
