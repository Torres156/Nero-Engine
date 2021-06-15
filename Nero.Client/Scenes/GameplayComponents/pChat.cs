using Nero.Client.Helpers;
using Nero.Client.World.Chat;
using Nero.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Client.Scenes.GameplayComponents
{
    using static Renderer;
    class pChat : Panel
    {
        TabControl tabChat;
        TextBox txtChat;
        public VScroll scrlNormal, scrlSystem;


        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public pChat(Bond bond) : base(bond)
        {
            Anchor = Anchors.BottomLeft;
            Position = new Vector2(4, 15);
            Size = new Vector2(300, 200);
            FillColor = new Color(0, 0, 0, 180);

            tabChat = new TabControl(this)
            {
                Size = new Vector2(Size.x - 8, Size.y - 26),
                Position = new Vector2(4, 2),
                FillColor = new Color(0, 0, 0, 150),
            };
            tabChat.Add("Normal", "Normal");
            tabChat.Add("Sistema", "System");
            tabChat.OnDrawTabPanel += TabChat_OnDrawTabPanel;
            tabChat.OnMouseScrolledTabPanel += TabChat_OnMouseScrolledTabPanel;

            txtChat = new TextBox(this)
            {
                Anchor = Anchors.BottomLeft,
                Size = new Vector2(Size.x, 20),
                FillColor = new Color(0, 0, 0, 150),
                MaxLength = 20,
            };
            txtChat.OnEnter += TxtChat_OnEnter;

            scrlNormal = new VScroll(tabChat.TabPanels[0])
            {
                Size = new Vector2(6, tabChat.TabPanels[0].Size.y),
                Maximum = 0,
                BlockScrolled = true,
            };

            scrlSystem = new VScroll(tabChat.TabPanels[1])
            {
                Size = new Vector2(6, tabChat.TabPanels[1].Size.y),
                Maximum = 0,
                BlockScrolled = true,
            };
        }

        private void TabChat_OnMouseScrolledTabPanel(Panel panel, int Index, SFML.Window.MouseWheelScrollEventArgs e)
        {
            if (Index == 0)
            {
                scrlNormal.MouseScrolled2(e);
            }
        }

        private void TxtChat_OnEnter(ControlBase sender)
        {
            var text = txtChat.Text.Trim();
            if (text.Length > 0)
                Network.Sender.ChatSpeak(text);

            // Clear Text
            txtChat.Text = "";
        }

        /// <summary>
        /// Desenha o chat
        /// </summary>
        /// <param name="target"></param>
        /// <param name="panel"></param>
        /// <param name="Index"></param>
        private void TabChat_OnDrawTabPanel(RenderTarget target, Panel panel, int Index)
        {
            var gp = panel.GlobalPosition();
            int max_view = 10;

            if (Index == 0)
            {                
                var lines = ChatHelper.Chat_Normal;
                var count = lines.Count;                
                if (count > 0)
                {
                    int last_line = Math.Min(scrlNormal.Value + max_view, count);
                    for (int i = scrlNormal.Value; i < last_line; i++)
                        DrawText(target, lines[i].Text, 12, gp + new Vector2(12, 14 * (i - scrlNormal.Value)), lines[i].Color);
                }
            }
            else if (Index == 1)
            {
                var lines = ChatHelper.Chat_System;
                var count = lines.Count;                
                if (count > 0)
                {
                    int last_line = Math.Min(scrlNormal.Value + max_view, count);
                    for (int i = scrlNormal.Value; i < last_line; i++)
                        DrawText(target, lines[i].Text, 12, gp + new Vector2(12, 14 * (i - scrlNormal.Value)), lines[i].Color);
                }
            }
        }

        /// <summary>
        /// Verifica o scroll maximo
        /// </summary>
        public void CheckScrollMaximum()
        {
            int max_view = 10;
            var lines = ChatHelper.Chat_System;
            var count = lines.Count;
            scrlSystem.Maximum = Math.Max(0, count - max_view);            

            lines = ChatHelper.Chat_Normal;
            count = lines.Count;
            scrlNormal.Maximum = Math.Max(0, count - max_view);            
        }

        /// <summary>
        /// Coloca o scroll no máximo para Sistema
        /// </summary>
        public void GoSystemMaximum()
        {
            scrlSystem.Value = scrlSystem.Maximum;
        }

        /// <summary>
        /// Coloca o scroll no máximo para Normal
        /// </summary>
        public void GoNormalMaximum()
        {
            scrlNormal.Value = scrlNormal.Maximum;
        }
    }
}
