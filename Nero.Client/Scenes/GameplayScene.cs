using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Nero.Client.Helpers;
using Nero.Client.Map;
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
            Map.MapInstance.Current = Map.MapInstance.Load(0);
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
            if (Map.MapInstance.Current != null)
            {
                Camera.Begin();

                // Ground
                Map.MapInstance.Current?.DrawGround(target);

                for (int y = Camera.Start().y; y <= Camera.End(MapInstance.Current).y; y++)
                {
                    // Players
                    foreach (var i in Character.Items)
                        if ((int)i.Position.y == y)
                            i.Draw(target);

                    // Me
                    if ((int)Character.My.Position.y == y)
                        Character.My?.Draw(target);
                }

                // Fringe
                Map.MapInstance.Current?.DrawFringe(target);


                // # Textos #

                // Players
                foreach (var i in Character.Items)
                    i.DrawTexts(target);
                Character.My?.DrawTexts(target);


                // Editor de mapa
                if (FindControl<frmEditor_Map>().Visible)
                {
                    if (FindControl<frmEditor_Map>().pAttribute.Visible)
                        MapInstance.Current.DrawAttributes(target);

                    if (FindControl<frmEditor_Map>().btnGrid.Checked)
                    {
                        var end = (Vector2)Camera.End(Map.MapInstance.Current);
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
            }

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
            if (Game.Window.HasFocus() && TextBox.Focus == null)
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

            // Personagem
            foreach (var i in Character.Items)
                i.Update();
            if (Character.My != null)
            {
                Character.My.Update();
                Camera.Position = Character.My.Position * 32 + Character.My.OffSet + new Vector2(16);
            }

            // Mapa
            Map.MapInstance.Current?.Update();

            base.Update();
        }

        /// <summary>
        /// Tecla após pressionada
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

        /// <summary>
        /// Seta o editor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void SetEditor<T>() where T : Form
        {
            foreach (var i in forms)
                if (!(i is T)) i.Hide();

            var find = forms.Find(i => i is T);
            find?.Show();
        }

        /// <summary>
        /// Quando sai do editor
        /// </summary>
        public void ExitEditor()
        {

        }

        /// <summary>
        /// Mouse pressionado
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public override bool MousePressed(MouseButtonEvent e)
        {
            var result = base.MousePressed(e);

            if (!result)
            {
                var mp = Camera.GetMousePosition() / 32;
                var form_MapEditor = FindControl<frmEditor_Map>();


                // Modo desenho
                if (form_MapEditor.Visible)
                {
                    if (e.Button == Mouse.Button.Left)
                    {
                        if (form_MapEditor.pTile.Visible)
                            if (form_MapEditor.cmbTileType.SelectIndex == 0)
                            {
                                for (int x2 = 0; x2 < form_MapEditor.SelectTile.size.x; x2++)
                                    for (int y2 = 0; y2 < form_MapEditor.SelectTile.size.y; y2++)
                                        Map.MapInstance.Current.AddChunk(form_MapEditor.CurrentLayer, (Map.ChunkTypes)form_MapEditor.cmbTileType.SelectIndex, form_MapEditor.txtTileID.Value,
                                            form_MapEditor.SelectTile.position + new Vector2(x2, y2), mp.ToVector2() + new Vector2(x2, y2));
                            }
                            else
                                Map.MapInstance.Current.AddChunk(form_MapEditor.CurrentLayer, (Map.ChunkTypes)form_MapEditor.cmbTileType.SelectIndex,
                                    form_MapEditor.txtTileID.Value, form_MapEditor.SelectTile.position, mp.ToVector2());
                        else if (form_MapEditor.pAttribute.Visible)
                        {                           
                            MapInstance.Current.AddAttribute(mp.ToVector2(), form_MapEditor.CurrentAttribute, form_MapEditor.args);
                        }
                    }
                    else if (e.Button == Mouse.Button.Right)
                        if (form_MapEditor.pTile.Visible)
                            Map.MapInstance.Current.RemoveChunk(form_MapEditor.CurrentLayer, mp.ToVector2());
                        else if (form_MapEditor.pAttribute.Visible)
                            MapInstance.Current.RemoveAttribute(mp.ToVector2(), form_MapEditor.CurrentAttribute);

                }
            }

            return result;
        }
    }
}
