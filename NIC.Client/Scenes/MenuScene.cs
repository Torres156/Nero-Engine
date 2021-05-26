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
        int _hoverLogin = -1;        // Hover no botãos em texto


        /// <summary>
        /// Carrega os recursos
        /// </summary>
        public override void LoadContent()
        {
            background = new Texture("res/ui/background-title.png", true);
            background.Smooth = true;

            pLogin = new Panel(this)
            {
                Anchor = Anchors.Center,
                Size = new Vector2(300,350),
            };

            txtAccount = new TextBox(pLogin)
            {
                Anchor = Anchors.TopCenter,
                Size = new Vector2(260, 20),
                Position = new Vector2(0, 80),
                FillColor = Color.White,
                TextColor = new Color(30,30,30),                
            };

            txtPassword = new TextBox(pLogin)
            {
                Anchor = Anchors.TopCenter,
                Size = new Vector2(260, 20),
                Position = new Vector2(0, 130),
                FillColor = Color.White,
                TextColor = new Color(30, 30, 30),
                Password = true,
            };

            chkSave = new CheckBox(pLogin)
            {
                Text = "Salvar conta?",
                Position = new Vector2(20,160),
            };

            btnEnter = new Button(pLogin)
            {
                Anchor = Anchors.TopCenter,
                Size = new Vector2(150,30),
                Text = "Entrar",
                Position = new Vector2(0,200),
                Border_Rounded = 12
            };

            btnExit = new Button(pLogin)
            {
                Anchor = Anchors.TopRight,
                Size = new Vector2(20),
                Text = "X",
                TextSize = 16,
                Position = new Vector2(4),
                FillColor = Color.Transparent,
                TextColor = Color.White,
            };


            pLogin.OnDraw += PLogin_OnDraw;
            pLogin.OnMouseMove += PLogin_OnMouseMove;
            btnExit.OnMouseReleased += (sender, e) => Game.Running = false;
        }

        private void PLogin_OnMouseMove(Control sender, Vector2 e)
        {
            _hoverLogin = -1;
            var gp = sender.GlobalPosition();

            var text = "Ainda não possui uma conta?";
            var pos = gp + new Vector2(28 + GetTextWidth(text), 290);

            if (new Rectangle(pos, new Vector2(GetTextWidth("Crie já!"), 14)).Contains(e))
                _hoverLogin = 0;

            pos = gp + new Vector2(20, 310);
            text = "Acesse nosso site!";
            if (new Rectangle(pos, new Vector2(GetTextWidth(text), 14)).Contains(e))
                _hoverLogin = 1;

            if (_hoverLogin > -1)
                Game.SetCursor(Nero.SFML.Window.Cursor.CursorType.Hand);

        }

        private void PLogin_OnDraw(Control sender, RenderTarget target)
        {
            var gp = sender.GlobalPosition();

            var text = "Login";
            DrawText(target, text, 20, gp + new Vector2((sender.Size.x - GetTextWidth(text, 20)) / 2, 10), Color.White);

            text = "Conta:";
            DrawText(target, text, 14, txtAccount.GlobalPosition() + new Vector2(1,-19), Color.White);

            text = "Senha:";
            DrawText(target, text, 14, txtPassword.GlobalPosition() + new Vector2(1, -19), Color.White);

            var cline = new Color(80, 80, 80);
            DrawLine(target, gp + new Vector2(20, 270), gp + new Vector2(sender.Size.x - 20, 270), cline);

            text = "Ainda não possui uma conta?";
            DrawText(target, text, 12, gp + new Vector2(20,290), Color.White);
            DrawText(target, "Crie já!", 12, gp + new Vector2(28 + GetTextWidth(text),290), _hoverLogin == 0 ? new Color(175, 209, 253) : new Color(94, 159, 244));

            text = "Acesse nosso site!";
            DrawText(target, text, 12, gp + new Vector2(20, 310), _hoverLogin == 1 ? new Color(175, 209, 253) : new Color(94, 159, 244));
            
        }

        /// <summary>
        /// Descarrega os recursos
        /// </summary>
        public override void UnloadContent()
        {
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
