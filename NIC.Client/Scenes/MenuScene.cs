using Nero;
using Nero.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIC.Client.Scenes
{
    using static Renderer;
    class MenuScene : SceneBase
    {
        Texture background;


        // Login
        Panel pLogin;           // Painel de login
        TextBox txtAccount;     // Conta
        TextBox txtPassword;    // Senha
        CheckBox chkSave;       // Salvar Conta
        Button btnEnter;        // Entrar na conta
        Button btnExit;         // Sair do jogo
        int _hoverLogin = -1;   // Hover no botãos em texto


        // Registro
        Panel pRegister;        // Painel de registro
        TextBox txtRAccount;    // Conta
        TextBox txtRPassword;   // Senha
        TextBox txtRPassword2;  // Repetir senha
        Button btnRExit;        // Sair do registro
        Button btnRRegister;    // Registrar


        // Configurações
        Form frmSettings;
        HScroll hsSound;
        HScroll hsMusic;
        CheckBox chkVSync;
        CheckBox chkAutoTile;


        // Textos
        LanguageWords login_words = new LanguageWords();
        LanguageWords register_words = new LanguageWords();
        LanguageWords settings_words = new LanguageWords();


        /// <summary>
        /// Carrega os recursos
        /// </summary>
        public override void LoadContent()
        {
            FadeActive = false;
            background = new Texture("res/ui/background-title.png", true);
            background.Smooth = true;

            // Login
            Control.LoadJson("data/ui/menu/login/panel_login.json", out pLogin);
            Control.LoadJson("data/ui/menu/login/textbox_account.json", out txtAccount);
            Control.LoadJson("data/ui/menu/login/textbox_password.json", out txtPassword);
            Control.LoadJson("data/ui/menu/login/checkbox_save.json", out chkSave);
            Control.LoadJson("data/ui/menu/login/button_enter.json", out btnEnter);
            Control.LoadJson("data/ui/menu/login/button_exit.json", out btnExit);

            // Registro
            Control.LoadJson("data/ui/menu/register/panel_register.json", out pRegister);
            Control.LoadJson("data/ui/menu/register/textbox_account.json", out txtRAccount);
            Control.LoadJson("data/ui/menu/register/textbox_password.json", out txtRPassword);
            Control.LoadJson("data/ui/menu/register/textbox_password2.json", out txtRPassword2);
            Control.LoadJson("data/ui/menu/register/button_register.json", out btnRRegister);
            Control.LoadJson("data/ui/menu/register/button_exit.json", out btnRExit);


            // Configurações
            frmSettings = new Form(this)
            {
                Size = new Vector2(350,250),
                Anchor = Anchors.Center,
                canDragged = false,
                Visible = false,
            };
            frmSettings.SetTitle(Languages.PT_BR, "Configurações");
            frmSettings.SetTitle(Languages.EN_USA, "Settings");
            frmSettings.ShowDialog();

            hsSound = new HScroll(frmSettings)
            {
                Size = new Vector2(140, 10),
                Position = new Vector2(20, 60),
                Maximum = 100,
                Value = 100,
            };

            hsMusic = new HScroll(frmSettings)
            {
                Size = new Vector2(140, 10),
                Position = new Vector2(195, 60),
                Maximum = 100,
                Value = 100,
            };

            chkVSync = new CheckBox(frmSettings)
            {
                Position = new Vector2(21, 140),
                UseMultipleLanguage = false,
            };
            chkVSync.Text[0] = "V-Sync";

            chkAutoTile = new CheckBox(frmSettings)
            {
                Position = new Vector2(196, 140),
                UseMultipleLanguage = false,
            };
            chkAutoTile.Text[0] = "AutoTile";

            // Events
            pLogin.OnDraw += PLogin_OnDraw;
            pLogin.OnMouseMove += PLogin_OnMouseMove;
            pLogin.OnMouseReleased += PLogin_OnMouseReleased;
            btnExit.OnMouseReleased += (sender, e) => Game.Running = false;
            btnRExit.OnMouseReleased += (sender, e) => { pRegister.Hide(); pLogin.Show(); };
            pRegister.OnDraw += PRegister_OnDraw;
            frmSettings.OnDraw += FrmSettings_OnDraw;


            // Textbox nexts
            txtAccount.Next = txtPassword;
            txtPassword.Next = txtAccount;
            txtRAccount.Next = txtRPassword;
            txtRPassword.Next = txtRPassword2;
            txtRPassword2.Next = txtRAccount;


            // Words
            JsonHelper.Load("data/ui/menu/login/words.json", out login_words);
            JsonHelper.Load("data/ui/menu/register/words.json", out register_words);
            settings_words.AddText("Som:", "Sound:");
            settings_words.AddText("Música:", "Music:");
            settings_words.AddText("Gráficos", "Graphics");
        }

        /// <summary>
        /// Desenho no painel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        private void FrmSettings_OnDraw(Control sender, RenderTarget target)
        {
            var gp = sender.GlobalPosition();

            var text = "Audio";
            DrawText(target, text, 16, gp + new Vector2(20, 50), Color.White);

            text = $"{settings_words.GetText(0)} {hsSound.Value}%";
            DrawText(target, text, 12, hsSound.GlobalPosition() + new Vector2(0, -16), Color.White);

            text = $"{settings_words.GetText(1)} {hsMusic.Value}%";
            DrawText(target, text, 12, hsMusic.GlobalPosition() + new Vector2(0, -16), Color.White);

            text = settings_words.GetText(2);
            DrawText(target, text, 16, gp + new Vector2(20, 140), Color.White);
        }

        /// <summary>
        /// Desenho no painel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        private void PRegister_OnDraw(Control sender, RenderTarget target)
        {
            var gp = sender.GlobalPosition();

            var text = register_words.GetText(0);
            DrawText(target, text, 20, gp + new Vector2((sender.Size.x - GetTextWidth(text, 20)) / 2, 10), Color.White);

            text = register_words.GetText(1);
            DrawText(target, text, 14, txtRAccount.GlobalPosition() + new Vector2(1, -19), Color.White);

            text = register_words.GetText(2);
            DrawText(target, text, 14, txtRPassword.GlobalPosition() + new Vector2(1, -19), Color.White);

            text = register_words.GetText(3);
            DrawText(target, text, 14, txtRPassword2.GlobalPosition() + new Vector2(1, -19), Color.White);

            var acc = txtRAccount.Text.Trim();
            var pwd = txtRPassword.Text.Trim();
            var pwd2 = txtRPassword2.Text.Trim();
            var lstWarning = new List<string>();

            if (acc.Length < 3)
                lstWarning.Add(register_words.GetText(4));

            if (pwd.Length < 3)
                lstWarning.Add(register_words.GetText(5));

            if (pwd != pwd2)
                lstWarning.Add(register_words.GetText(6));

            if (lstWarning.Count > 0)
                for (int i = 0; i < lstWarning.Count; i++)
                    DrawText(target, lstWarning[i], 12, gp + new Vector2((sender.Size.x - GetTextWidth(lstWarning[i])) / 2, 220 + 14 * i), Color.Red);
        }

        /// <summary>
        /// Clique do mouse no painel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PLogin_OnMouseReleased(Control sender, Nero.SFML.Window.MouseButtonEvent e)
        {
            if (_hoverLogin > -1)
                switch(_hoverLogin)
                {
                    case 0:
                        pLogin.Hide();
                        pRegister.Show();
                        break;

                    case 1:
                        System.Diagnostics.Process.Start("explorer", "https://github.com/Walford19/Nero-Engine");
                        break;
                }
        }

        /// <summary>
        /// Movimento do mouse no painel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PLogin_OnMouseMove(Control sender, Vector2 e)
        {
            _hoverLogin = -1;
            var gp = sender.GlobalPosition();

            var text = login_words.GetText(3);
            var pos = gp + new Vector2(28 + GetTextWidth(text), 290);

            if (new Rectangle(pos, new Vector2(GetTextWidth(login_words.GetText(4)), 14)).Contains(e))
                _hoverLogin = 0;

            pos = gp + new Vector2(20, 310);
            text = login_words.GetText(5);
            if (new Rectangle(pos, new Vector2(GetTextWidth(text), 14)).Contains(e))
                _hoverLogin = 1;

            if (_hoverLogin > -1)
                Game.SetCursor(Nero.SFML.Window.Cursor.CursorType.Hand);

        }

        /// <summary>
        /// Desenho no painel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        private void PLogin_OnDraw(Control sender, RenderTarget target)
        {
            var gp = sender.GlobalPosition();

            var text = login_words.GetText(0);
            DrawText(target, text, 20, gp + new Vector2((sender.Size.x - GetTextWidth(text, 20)) / 2, 10), Color.White);

            text = login_words.GetText(1);
            DrawText(target, text, 14, txtAccount.GlobalPosition() + new Vector2(1, -19), Color.White);

            text = login_words.GetText(2);
            DrawText(target, text, 14, txtPassword.GlobalPosition() + new Vector2(1, -19), Color.White);

            var cline = new Color(80, 80, 80);
            DrawLine(target, gp + new Vector2(20, 270), gp + new Vector2(sender.Size.x - 20, 270), cline);

            text = login_words.GetText(3);
            DrawText(target, text, 12, gp + new Vector2(20, 290), Color.White);
            DrawText(target, login_words.GetText(4), 12, gp + new Vector2(28 + GetTextWidth(text), 290), _hoverLogin == 0 ? new Color(175, 209, 253) : new Color(94, 159, 244));

            text = login_words.GetText(5);
            DrawText(target, text, 12, gp + new Vector2(20, 310), _hoverLogin == 1 ? new Color(175, 209, 253) : new Color(94, 159, 244));

        }

        /// <summary>
        /// Descarrega os recursos
        /// </summary>
        public override void UnloadContent()
        {
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
            DrawTexture(target, background, new Rectangle(Vector2.Zero, Size));

            base.Draw(target, states);
        }
    }
}
