using Nero.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Scenes
{
    using static Renderer;
    class SelectCharacterScene : SceneBase
    {
        // Texturas
        Texture background; // Fundo


        // Controles
        Form frmSelect;
        Button btnUseCreate;


        // Privates
        byte currentSlot = 0;
        int hoverSlot = -1;
        LanguageWords words = new LanguageWords();


        /// <summary>
        /// Carrega os recursos
        /// </summary>
        public override void LoadContent()
        {
            background = new Texture("res/ui/background-selectchar.jpg", true);

            LoadJson("data/ui/select_character/form_select.json", out frmSelect);
            frmSelect.OnDraw += FrmSelect_OnDraw;
            frmSelect.OnMouseMove += FrmSelect_OnMouseMove;
            frmSelect.OnMouseReleased += FrmSelect_OnMouseReleased;
            frmSelect.OnVisibleChanged += (sender) => { if (!frmSelect.Visible) { Network.Socket.Device.FirstPeer.Disconnect(); Game.SetScene<MenuScene>(); } };
            
            LoadJson("data/ui/select_character/button_usecreate.json", out btnUseCreate);
            btnUseCreate.OnMouseReleased += BtnUseCreate_OnMouseReleased;

            JsonHelper.Load("data/ui/select_character/words.json", out words);

            Sound.PlayMusic("res/music/select.ogg");
        }

        /// <summary>
        /// Cria ou usa um personagem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUseCreate_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            Game.SetScene<CreateCharacterScene>(currentSlot);
        }

        /// <summary>
        /// Clique do mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSelect_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            if (hoverSlot > -1)
                currentSlot = (byte)hoverSlot;
        }

        /// <summary>
        /// Movimento do mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSelect_OnMouseMove(ControlBase sender, Vector2 e)
        {
            var gp = sender.GlobalPosition();
            hoverSlot = -1;
            for (int i = 0; i < Constants.MAX_CHARACTERS; i++)
            {
                var pos = gp + new Vector2(sender.Size.x - 200, 40 + 40 * i);
                if (new Rectangle(pos, new Vector2(180, 40)).Contains(e))
                {
                    hoverSlot = i;
                    Game.SetCursor(SFML.Window.Cursor.CursorType.Hand);
                    return;
                }
            }
        }

        /// <summary>
        /// Desenha no form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        private void FrmSelect_OnDraw(ControlBase sender, RenderTarget target)
        {
            var gp = sender.GlobalPosition();
            string text = "";

            // Slots
            for (int i = 0; i < Constants.MAX_CHARACTERS; i++)
            {
                var pos = gp + new Vector2(sender.Size.x - 200, 40 + 40 * i);

                if (i > 0)
                    DrawLine(target, pos, pos + new Vector2(180,0), new Color(60, 60, 60));

                // Hover
                if (i != currentSlot && i == hoverSlot)
                {
                    var c1 = new Color(255, 255, 255, 0);
                    var c2 = new Color(255, 255, 255, 20);

                    DrawGradient(target, pos + new Vector2(0, 39), c1, pos + new Vector2(0,1), c1, pos + new Vector2(90, 1), c2,
                        pos + new Vector2(90, 39), c2);
                    DrawGradient(target, pos + new Vector2(90, 39), c2, pos + new Vector2(90,1), c2, pos + new Vector2(180, 1), c1,
                        pos + new Vector2(180, 39), c1);
                }

                // Select
                if (i == currentSlot)
                {
                    var c1 = new Color(93, 162, 251, 0);
                    var c2 = new Color(93, 162, 251, 80);

                    DrawGradient(target, pos + new Vector2(0, 39), c1, pos + new Vector2(0,1), c1, pos + new Vector2(90, 1), c2,
                        pos + new Vector2(90, 39), c2);
                    DrawGradient(target, pos + new Vector2(90, 39), c2, pos + new Vector2(90, 1), c2, pos + new Vector2(180, 1), c1,
                        pos + new Vector2(180, 39), c1);
                }

                text = words.GetText(2);
                DrawText(target, text, 16, pos + new Vector2((180 - GetTextWidth(text, 16)) / 2, (20 - 8)), Color.White);
            }

            // Current
            DrawRoundedRectangle(target, gp + new Vector2(4, 40), new Vector2(sender.Size.x - 210, sender.Size.y - 80), new Color(255, 255, 255, 15), 4, 8);
            btnUseCreate.Text[0] =  words.GetText(0); 
        }

        /// <summary>
        /// Descarrega os recursos
        /// </summary>
        public override void UnloadContent()
        {
            Sound.StopMusic();
            background.Destroy();
            base.UnloadContent();
        }

        /// <summary>
        /// Desenha a cena
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public override void Draw(RenderTarget target, RenderStates states)
        {
            DrawTexture(target, background, new Rectangle(Vector2.Zero, Game.Size));

            base.Draw(target, states);
        }
    }
}
