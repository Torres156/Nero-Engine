using Nero.SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Control
{
    using static Renderer;
    public class TabControl : Bond
    {
        public const int TAB_HEIGHT = 25;

        List<string[]> TabNames = new List<string[]>();
        public List<Panel> TabPanels { get; private set; } = new List<Panel>();

        public Color FillColor = new Color(0, 0, 0, 220);
        public Color OutlineColor = new Color(60,60,60);
        public int OutlineThickness = 0;
        public int SelectIndex { get; set; } = 0;
        public int TextOutlineThickness = 0;
        public Color TextOutlineColor = new Color(0, 0, 0, 220);

        int hoverTab = -1;

        public event HandleDrawTabPanel OnDrawTabPanel;
        public event HandleMouseMoveTabPanel OnMouseMoveTabPanel;
        public event HandleMouseButtonTabPanel OnMousePressedTabPanel, OnMouseReleasedTabPanel;
        public event HandleMouseScrolledTabPanel OnMouseScrolledTabPanel;
        public delegate void HandleDrawTabPanel(RenderTarget target, Panel panel, int Index);
        public delegate void HandleMouseMoveTabPanel(Panel panel, int Index, Vector2 e);
        public delegate void HandleMouseButtonTabPanel(Panel panel, int Index, MouseButtonEvent e);
        public delegate void HandleMouseScrolledTabPanel(Panel panel, int Index, MouseWheelScrollEventArgs e);

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public TabControl(Bond bond) : base(bond)
        {
            OnMouseMove += TabControl_OnMouseMove;
            OnMouseReleased += TabControl_OnMouseReleased;
        }

        /// <summary>
        /// Clique do mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            if (hoverTab > -1)
            {
                TabPanels[SelectIndex].Hide();
                SelectIndex = hoverTab;
                TabPanels[SelectIndex].Show();
            }
        }

        /// <summary>
        /// Movimento do mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabControl_OnMouseMove(ControlBase sender, Vector2 e)
        {
            var gp = GlobalPosition();
            hoverTab = -1;

            // Header
            var countTab = TabNames.Count;
            if (countTab == 0) return;
            float w = 0;
            for (int i = 0; i < countTab; i++)
            {
                var name = TabNames[i][(int)Game.CurrentLanguage];
                var pos = gp + new Vector2(w, 0);

                if (new Rectangle(pos, new Vector2(GetTextWidth(name) + 20, TAB_HEIGHT)).Contains(e))
                {
                    hoverTab = i;
                    Game.SetCursor(SFML.Window.Cursor.CursorType.Hand);
                    return;
                }

                w += 20 + GetTextWidth(name);
            }
        }

        /// <summary>
        /// Desenha o controle
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            if (TabNames.Count == 0)
                return;

            var gp = GlobalPosition();

            // Header
            var countTab = TabNames.Count;
            float w = 0;
            for(int i = 0; i < countTab;i++)
            {
                var name = TabNames[i][(int)Game.CurrentLanguage];
                if (hoverTab == i)
                    DrawRoundedRectangle(target, gp + new Vector2(w, 0), new Vector2(GetTextWidth(name) + 20, TAB_HEIGHT - 4),
                        new Color(60,60,60), 8, 8);

                DrawText(target, name, 12, gp + new Vector2(w + 8, 1), Color.White,TextOutlineThickness, TextOutlineColor);

                var cLine = new Color(90, 90, 90);
                if (SelectIndex == i) cLine = new Color(93, 162, 251);
                DrawRectangle(target, gp + new Vector2(w, TAB_HEIGHT - 2), new Vector2(GetTextWidth(name) + 20, 2), cLine);
                
                w += 20 + GetTextWidth(name);
            }
            
            base.Draw(target, states);
        }

        /// <summary>
        /// Adiciona um novo tab
        /// </summary>
        /// <param name="tabName"></param>
        public void Add(params string[] tabName)
        {
            TabNames.Add(tabName);

            var p = new Panel(this)
            {
                Size = new Vector2(Size.x, Size.y - TAB_HEIGHT - 2 - OutlineThickness),
                Position = new Vector2(0, TAB_HEIGHT + 2 + OutlineThickness),
                Visible = false,
                FillColor = FillColor,
                OutlineColor = OutlineColor,
                OutlineThickness = OutlineThickness,
            };
            p.OnDraw += P_OnDraw;
            p.OnMouseMove += P_OnMouseMove;
            p.OnMouseScrolled += P_OnMouseScrolled;
            p.OnMousePressed += P_OnMousePressed;
            p.OnMouseReleased += P_OnMouseReleased;
            TabPanels.Add(p);

            if (TabPanels.Count == 1)
                p.Show();
        }

        /// <summary>
        /// Clique do mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P_OnMouseReleased(ControlBase sender, MouseButtonEvent e)
        {
            OnMouseReleasedTabPanel?.Invoke((Panel)sender, TabPanels.IndexOf((Panel)sender), e);
        }

        /// <summary>
        /// Mouse pressionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P_OnMousePressed(ControlBase sender, MouseButtonEvent e)
        {
            OnMousePressedTabPanel?.Invoke((Panel)sender, TabPanels.IndexOf((Panel)sender), e);
        }

        /// <summary>
        /// Mouse scroll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P_OnMouseScrolled(ControlBase sender, MouseWheelScrollEventArgs e)
        {
            OnMouseScrolledTabPanel?.Invoke((Panel)sender, TabPanels.IndexOf((Panel)sender), e);
        }

        /// <summary>
        /// Movimento do mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void P_OnMouseMove(ControlBase sender, Vector2 e)
        {
            OnMouseMoveTabPanel?.Invoke((Panel)sender, TabPanels.IndexOf((Panel)sender), e);
        }

        /// <summary>
        /// Desenho
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        private void P_OnDraw(ControlBase sender, RenderTarget target)
        {
            OnDrawTabPanel?.Invoke(target, (Panel)sender, TabPanels.IndexOf((Panel)sender));
        }
    }
}
