using Nero.Client.Player;
using Nero.Client.World;
using Nero.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Scenes
{
    using static Renderer;
    class CreateCharacterScene : SceneBase
    {
        Texture background;
        int currentSlot = 0;
        int currentClass = 0;
        int currentSprite = 0;
        byte currentGender = 0;

        Form frmCreate;
        TextBox txtName, txtClass;
        Button btnLeft, btnRight;
        Button btnSLeft, btnSRight;
        Button btnCreate;

        LanguageWords words = new LanguageWords();

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="currentSlot"></param>
        public CreateCharacterScene(int currentSlot)
        {
            this.currentSlot = currentSlot;            
        }

        /// <summary>
        /// Carrega os recursos
        /// </summary>
        public override void LoadContent()
        {
            background = new Texture("res/ui/background-createchar.png", true) { Smooth = true };
            GlobalResources.LoadCharacters();

            LoadJson("data/ui/create_character/form_create.json", out frmCreate);
            frmCreate.OnVisibleChanged += (sender) => { if (!frmCreate.Visible) Game.SetScene<SelectCharacterScene>(); };
            frmCreate.OnDraw += FrmCreate_OnDraw;

            LoadJson("data/ui/create_character/textbox_name.json", out txtName);
            LoadJson("data/ui/create_character/button_left.json", out btnLeft);
            LoadJson("data/ui/create_character/button_right.json", out btnRight);
            LoadJson("data/ui/create_character/textbox_class.json", out txtClass);            
            LoadJson("data/ui/create_character/button_sleft.json", out btnSLeft);
            LoadJson("data/ui/create_character/button_sright.json", out btnSRight);
            LoadJson("data/ui/create_character/button_create.json", out btnCreate);

            txtClass.Text = CharacterClass.Items[currentClass].Name[(int)Game.CurrentLanguage];

            // Events
            btnLeft.OnMouseReleased += BtnLeft_OnMouseReleased;
            btnRight.OnMouseReleased += BtnRight_OnMouseReleased;
            btnSLeft.OnMouseReleased += BtnSLeft_OnMouseReleased;
            btnSRight.OnMouseReleased += BtnSRight_OnMouseReleased;
            frmCreate.OnMouseMove += FrmCreate_OnMouseMove;
            frmCreate.OnMouseReleased += FrmCreate_OnMouseReleased;

            // Palavras
            JsonHelper.Load("data/ui/create_character/words.json", out words);

            // Toca o som
            Sound.PlayMusic("res/music/create.ogg");
        }
        
        /// <summary>
        /// Clique do mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCreate_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            var gp = sender.GlobalPosition();

            for (int i = 0; i < 2; i++)
            {
                var posG = gp + new Vector2(sender.Size.x - 40, 220 + 24 * i);
                if (e.X >= posG.x && e.X <= posG.x + 20)
                    if (e.Y >= posG.y && e.Y <= posG.y + 20)
                    {
                        currentGender = (byte)i;
                        return;
                    }
            }
        }

        /// <summary>
        /// Movimento do mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCreate_OnMouseMove(ControlBase sender, Vector2 e)
        {
            var gp = sender.GlobalPosition();

            for (int i = 0; i < 2; i++)
            {
                var posG = gp + new Vector2(sender.Size.x - 40, 220 + 24 * i);
                if (e.x >= posG.x && e.x <= posG.x + 20)
                    if (e.y >= posG.y && e.y <= posG.y + 20)
                        Game.SetCursor(SFML.Window.Cursor.CursorType.Hand);
            }
        }

        /// <summary>
        /// Altera a sprite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSRight_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            var c = currentGender == 0 ? CharacterClass.Items[currentClass].MaleSprite : CharacterClass.Items[currentClass].FemaleSprite;
            currentSprite++;
            if (currentSprite >= c.Length) currentSprite = 0;
        }

        /// <summary>
        /// Altera a sprite
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSLeft_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            var c = currentGender == 0 ? CharacterClass.Items[currentClass].MaleSprite : CharacterClass.Items[currentClass].FemaleSprite;
            currentSprite--;
            if (currentSprite < 0) currentSprite = c.Length - 1;
        }

        /// <summary>
        /// Altera a classe ++
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRight_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            currentClass++;
            if (currentClass >= CharacterClass.Items.Count) currentClass = 0;
            txtClass.Text = CharacterClass.Items[currentClass].Name[(int)Game.CurrentLanguage];
            currentSprite = 0;
        }

        /// <summary>
        /// Altera a classe --
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLeft_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            currentClass--;
            if (currentClass < 0) currentClass = CharacterClass.Items.Count - 1;
            txtClass.Text = CharacterClass.Items[currentClass].Name[(int)Game.CurrentLanguage];
            currentSprite = 0;
        }

        /// <summary>
        /// Desenha no form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        private void FrmCreate_OnDraw(ControlBase sender, RenderTarget target)
        {
            var gp = sender.GlobalPosition();

            var text = words.GetText(0);
            DrawText(target, text, 14, txtName.GlobalPosition() + new Vector2(1, -20), Color.White);

            text = words.GetText(1);
            DrawText(target, text, 14, btnLeft.GlobalPosition() + new Vector2(1, -20), Color.White);

            DrawLine(target, gp + new Vector2(20, 160), gp + new Vector2(sender.Size.x - 20, 160), new Color(60, 60, 60));


            // Descrição
            text = CharacterClass.Items[currentClass].Description[(int)Game.CurrentLanguage];
            string[] wrap = GetWordWrap(text, (int)sender.Size.x - 40);
            for (int i = 0; i < wrap.Length; i++)
                DrawText(target, wrap[i], 12, gp + new Vector2((sender.Size.x - GetTextWidth(wrap[i])) / 2, 170 + 14 * i), Color.White);


            // Sprite
            var recpos = gp + new Vector2((sender.Size.x - 32) / 2, 218);
            DrawRoundedRectangle(target, recpos, new Vector2(32, 64), Color.White, 8, 8);

            var sprID = currentGender == 0 ? CharacterClass.Items[currentClass].MaleSprite[currentSprite] : CharacterClass.Items[currentClass].FemaleSprite[currentSprite];
            var spr = GlobalResources.Character[sprID];
            var sprSize = spr.size / 4;

            DrawTexture(target, spr, new Rectangle(recpos + new Vector2(16,-1), sprSize * .85f),
                new Rectangle(Vector2.Zero, sprSize), Color.White, new Vector2(sprSize.x / 2, 0));


            // Sexo
            string[] g = { "M", "F" };
            for(int i = 0; i < 2; i++)
            {
                var posG = gp + new Vector2(sender.Size.x - 40, 220 + 24 * i);
                DrawRoundedRectangle(target, posG, new Vector2(20), Color.White, 4, 8, currentGender == i ? 2 : 0, new Color(93, 162, 251));
                DrawText(target, g[i], 14, posG + new Vector2((20 - GetTextWidth(g[i], 14)) / 2, 1), Color.Black);
            }
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
