using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Nero.Client.Helpers;
using Nero.Client.Player;
using Nero.Client.Scenes.GameplayComponents;
using Nero.Client.World;
using Nero.Control;
using Nero.SFML.Window;

namespace Nero.Client.Scenes
{
    using static Renderer;
    class GameplayScene : SceneBase
    {
        /// <summary>
        /// Carrega os recursos
        /// </summary>
        public override void LoadContent()
        {
            Map.Map.Current = new Map.Map();
            Camera.Initialize();

            // Controles
            var controls = Utils.GetTypesInNamespace<ControlBase>(Assembly.GetExecutingAssembly(), "Nero.Client.Scenes.GameplayComponents");
            foreach (var c in controls)
            {
                var control = (ControlBase)Activator.CreateInstance(c, this);
                control.Name = c.Name;
            }
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

            Camera.Begin();

            Character.My?.Draw(target);
            Character.My?.DrawTexts(target);

            // Editor de mapa
            if (FindControl<frmEditor_Map>().Visible)
            {
                if (FindControl<frmEditor_Map>().btnGrid.Checked)
                {
                    var end = (Vector2)Camera.End(Map.Map.Current);
                    var start = (Vector2)Camera.Start();
                    var size = (end - start + Vector2.One) * 32;

                    // Y Grid
                    for (int i = (int)start.y; i <= end.y + 1; i++)
                        DrawLine(target, new Vector2(start.x * 32, i * 32), new Vector2(start.x * 32 + size.x, i * 32), Color.White);

                    // X Grid
                    for (int i = (int)start.x; i <= end.x + 1; i++)
                        DrawLine(target, new Vector2(i * 32, start.y * 32), new Vector2(i * 32, start.y * 32 + size.y), Color.White);
                }
            }

            Camera.End();

            if (FindControl<frmEditor_Map>().Visible)
            {
                var cam_mouse = (Camera.GetMousePosition() / 32);
                DrawText(target, $"Mouse X: {cam_mouse.x} Y:{cam_mouse.y}", 14, new Vector2(10, 10 + 16), Color.White, 1, new Color(30, 30, 30));
            }

            DrawText(target, $"FPS: {Game.FPS}", 14, new Vector2(10), Color.White, 1, new Color(30, 30, 30));

            base.Draw(target, states);
        }

        /// <summary>
        /// Atualiza a cena
        /// </summary>
        public override void Update()
        {
            // Movimentos
            if (TextBox.Focus == null)
            {
                // Up
                if (Keyboard.IsKeyPressed(Keyboard.Key.W) || Keyboard.IsKeyPressed(Keyboard.Key.Up))
                { MoveHelper.Request(Directions.Up); goto breakMove; }

                // Down
                if (Keyboard.IsKeyPressed(Keyboard.Key.S) || Keyboard.IsKeyPressed(Keyboard.Key.Down))
                { MoveHelper.Request(Directions.Down); goto breakMove; }

                // Left
                if (Keyboard.IsKeyPressed(Keyboard.Key.A) || Keyboard.IsKeyPressed(Keyboard.Key.Left))
                { MoveHelper.Request(Directions.Left); goto breakMove; }

                // Right
                if (Keyboard.IsKeyPressed(Keyboard.Key.D) || Keyboard.IsKeyPressed(Keyboard.Key.Right))
                { MoveHelper.Request(Directions.Right); goto breakMove; }

            breakMove:;
            }

            base.Update();
            if (Character.My != null)
            {
                Character.My.Update();
                Camera.Position = Character.My.Position * 32 + Character.My.OffSet + new Vector2(16);
            }
        }

        /// <summary>
        /// Tecla ap�s pressionada
        /// </summary>
        /// <param name="e"></param>
        public override void KeyReleased(KeyEventArgs e)
        {
            if (TextBox.Focus != null) return;

            switch (e.Code)
            {
                case Keyboard.Key.Insert:
                    if (Character.My.AccessLevel > AccessLevels.Player)
                        FindControl<frmAdmin>().Show();
                    break;
            }

            base.KeyReleased(e);
        }

        /// <summary>
        /// Resize da janela
        /// </summary>
        public override void Resize()
        {
            base.Resize();
            Camera.Resize();
        }

        public void SetEditor<T>() where T : Form
        {
            foreach (var i in forms)
                if (!(i is T)) i.Hide();

            var find = forms.Find(i => i is T);
            find?.Show();
        }
    }
}
