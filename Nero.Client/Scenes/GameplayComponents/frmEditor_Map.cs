using Nero.Client.World;
using Nero.Control;
using Nero.SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Scenes.GameplayComponents
{
    using static Renderer;
    class frmEditor_Map : Form
    {
        // Constants
        const int MODE_TILE = 0;        // Modo tileset
        const int MODE_ATTRIBUTES = 1;  // Modo atributo

        // Publics
        public Rectangle SelectTile = new Rectangle(Vector2.Zero, Vector2.One); // Seleção do tile
        public int CurrentLayer = 0;                                            // Camada usada


        // Privates
        int hoverLayer = -1;
        LanguageWords words = new LanguageWords();
        bool _press;
        RenderTexture render;


        // Controls
        public Panel pTile;             // Painel de tile
        public Panel pAttribute;        // Painel de atributos
        public TextBox txtTileID;       // ID do Tile
        public ComboBox cmbTileType;    // Tipo de tile
        Button btnSave;                 // Botão de salvar
        Button btnProperties;           // Botão de propriedades
        public Button btnGrid;          // Botão de grid
        public Button btnLight;         // Botão de luz
        Button btnFill;                 // Pinta tudo
        public Button btnTile;          // Botão de tileset
        public Button btnAttribute;     // Botão de atributos
        HScroll hsLeft;                 // Scroll horizontal
        VScroll vsTop;                  // Scroll vertical


        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public frmEditor_Map(Bond bond) : base(bond)
        {
            Name = "frmEditor_Map";
            Size = new Vector2(404, 374);
            Anchor = Anchors.Center;
            SetTitle(Languages.PT_BR, "Editor de mapa");
            SetTitle(Languages.EN_USA, "Map Editor");
            OutlineColor = new Color(60, 60, 60);
            OutlineThickness = 1;
            Visible = false;


            pTile = new Panel(this)
            {
                Name = "pTile",
                Size = new Vector2(Size.x - 10, Size.y - 70),
                Position = new Vector2(5, 25),
                OutlineThickness = 1,
                OutlineColor = new Color(90, 90, 90),
                FillColor = new Color(20, 20, 20),
            };

            pAttribute = new Panel(this)
            {
                Size = new Vector2(Size.x - 10, Size.y - 65),
                Position = new Vector2(5, 25),
                OutlineThickness = 1,
                OutlineColor = new Color(90, 90, 90),
                FillColor = new Color(20, 20, 20),
                Visible = false,
            };

            txtTileID = new TextBox(pTile)
            {
                Position = new Vector2(55, 2),
                Size = new Vector2(30, 19),
                isNumeric = true,
                Maximum = GlobalResources.Tileset.Count - 1,
                Minimum = 1,
                Anchor = Anchors.BottomLeft,
                FillColor = Color.White,
                TextColor = new Color(30, 30, 30),
            };

            cmbTileType = new ComboBox(pTile)
            {
                Size = new Vector2(100, 19),
                Position = new Vector2(90, 2),
                Anchor = Anchors.BottomLeft,
            };
            cmbTileType.Item.Add("Normal");
            cmbTileType.Item.Add("Autotile");
            cmbTileType.Item.Add("Water");
            cmbTileType.SelectIndex = 0;

            btnSave = new Button(this)
            {
                Texture = new Texture("res/ui/mapeditor/save.png"),
                Size = new Vector2(20),
                Position = new Vector2(4, 0),
                FillColor = Color.Transparent,
            };

            btnProperties = new Button(this)
            {
                Texture = new Texture("res/ui/mapeditor/prop.png"),
                Size = new Vector2(20),
                Position = new Vector2(4 + 22, 0),
                FillColor = Color.Transparent,
            };

            btnGrid = new Button(this)
            {
                Texture = new Texture("res/ui/mapeditor/grid.png"),
                Size = new Vector2(20),
                Position = new Vector2(4 + 22 * 3, 0),
                FillColor = Color.Transparent,
                isChecked = true,
            };

            btnLight = new Button(this)
            {
                Texture = new Texture("res/ui/mapeditor/light.png"),
                Size = new Vector2(20),
                Position = new Vector2(4 + 22 * 4, 0),
                FillColor = Color.Transparent,
                isChecked = true,
            };

            btnFill = new Button(this)
            {
                Texture = new Texture("res/ui/mapeditor/fill.png"),
                Size = new Vector2(20),
                Position = new Vector2(4 + 22 * 5, 0),
                FillColor = Color.Transparent,
            };

            btnTile = new Button(this)
            {
                Texture = new Texture("res/ui/mapeditor/tile.png"),
                Size = new Vector2(20),
                Position = new Vector2(4 + 22 * 7, 0),
                FillColor = Color.Transparent,
                isChecked = true,
                Checked = true,
            };

            btnAttribute = new Button(this)
            {
                Texture = new Texture("res/ui/mapeditor/attribute.png"),
                Size = new Vector2(20),
                Position = new Vector2(4 + 22 * 8, 0),
                FillColor = Color.Transparent,
                isChecked = true,
            };

            vsTop = new VScroll(pTile)
            {
                Position = new Vector2(262, 4),
                Size = new Vector2(8, 258),
                Maximum = 0,
            };

            hsLeft = new HScroll(pTile)
            {
                Position = new Vector2(4, 262),
                Size = new Vector2(258, 8),
                Maximum = 0,
            };

            txtTileID.Value = 1;

            render = new RenderTexture(256, 256);

            // Events
            pTile.OnDraw += PTile_OnDraw;
            pTile.OnMouseMove += PTile_OnMouseMove;
            pTile.OnMouseReleased += PTile_OnMouseReleased;
            pTile.OnMousePressed += PTile_OnMousePressed;
            txtTileID.OnValidate += TxtTileID_OnValidate;
            OnVisibleChanged += FrmEditor_Map_OnVisibleChanged;
            btnFill.OnMouseReleased += BtnFill_OnMouseReleased;

            // Words
            words.AddText("Camadas", "Layers");
        }

        /// <summary>
        /// Pinta o mapa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFill_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            if (pTile.Visible)
            {
                var size = cmbTileType.SelectIndex == 0 ? SelectTile.size : Vector2.One;
                var m = Map.Map.Current;
                int countX = m.Size.x / (int)size.x + 1;
                int countY = m.Size.y / (int)size.y + 1;

                for(int x = 0; x < countX; x++)                
                    for(int y = 0; y < countY; y++)
                    {
                        for (int x2 = 0; x2 < size.x; x2++)
                            for (int y2 = 0; y2 < size.y; y2++)
                                m.AddChunk(CurrentLayer, (Map.ChunkTypes)cmbTileType.SelectIndex, txtTileID.Value, SelectTile.position + new Vector2(x2, y2),
                                    new Vector2(x * size.x + x2, y * size.y + y2));
                    }
            }
        }

        /// <summary>
        /// Visibilidade
        /// </summary>
        /// <param name="sender"></param>
        private void FrmEditor_Map_OnVisibleChanged(ControlBase sender)
        {
            if (!Visible)
                Game.GetScene<GameplayScene>().ExitEditor();
        }

        /// <summary>
        /// Mouse pressionado
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PTile_OnMousePressed(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            var gp = sender.GlobalPosition();

            var off = new Vector2(hsLeft.Value, vsTop.Value) * 16;
            var rec_pos = gp + new Vector2(5);
            var epos = new Vector2(e.X, e.Y);

            if (new Rectangle(rec_pos, new Vector2(256)).Contains(epos))
            {
                var cur = ((epos - rec_pos + off) / 16).Floor();
                if (!_press)
                {
                    SelectTile = new Rectangle(cur, Vector2.One);
                    _press = true; ;
                }
                else
                {
                    var size = Vector2.Max(Vector2.One, Vector2.One + cur -  SelectTile.position);
                    SelectTile.size = size;
                }
            }

        }

        /// <summary>
        /// Valida o ID de tile
        /// </summary>
        /// <param name="sender"></param>
        private void TxtTileID_OnValidate(ControlBase sender)
        {
            var tex = GlobalResources.Tileset[txtTileID.Value];
            vsTop.Maximum = Math.Max(0, (int)tex.size.y - 512) / 32;
            vsTop.Value = 0;
            hsLeft.Maximum = Math.Max(0, (int)tex.size.x - 512) / 32;
            hsLeft.Value = 0;
            SelectTile = new Rectangle(Vector2.Zero, Vector2.One);
        }

        /// <summary>
        /// Clique do mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PTile_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            _press = false;
            if (hoverLayer > -1)
                CurrentLayer = hoverLayer;
        }

        /// <summary>
        /// Movimento do mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PTile_OnMouseMove(ControlBase sender, Vector2 e)
        {
            var gp = sender.GlobalPosition();
            hoverLayer = -1;
            for (int i = 0; i < (int)Map.Layers.count; i++)
            {
                var pos = gp + new Vector2(280, 30 + 24 * i);
                if (new Rectangle(pos, new Vector2(110, 20)).Contains(e))
                {
                    hoverLayer = i;
                    Game.SetCursor(SFML.Window.Cursor.CursorType.Hand);
                }
            }
        }

        //Desenho no painel
        private void PTile_OnDraw(ControlBase sender, RenderTarget target)
        {
            var gp = sender.GlobalPosition();

            var rec_pos = gp + new Vector2(5);
            DrawRectangle(target, rec_pos, new Vector2(256), Color.Transparent, 1, new Color(60, 60, 60));

            // Desenha a tileset
            if (txtTileID.Value > 0)
            {
                render.Clear(Color.Transparent);
                var off = new Vector2(hsLeft.Value, vsTop.Value) * 16;
                var tex = GlobalResources.Tileset[txtTileID.Value];
                DrawTexture(render, tex, new Rectangle(Vector2.Zero, Vector2.Min(tex.size * .5f, new Vector2(256))),
                    new Rectangle(off * 2, Vector2.Min(tex.size, new Vector2(512))), Color.White);

                var select_pos = SelectTile.position * 16 - off;
                DrawRectangle(render, select_pos, SelectTile.size * 16, Color.Transparent, 1, Color.Red);
                
                render.Display();
                using(var spr = new Sprite(render.Texture))
                {
                    spr.Position = rec_pos;
                    target.Draw(spr);
                }
            }

            DrawText(target, "TileID:", 12, gp + new Vector2(5, sender.Size.y - 18), Color.White);

            // Camadas
            DrawText(target, words.GetText(0), 16, gp + new Vector2(280 + (110 - GetTextWidth(words.GetText(0), 16)) / 2, 4), Color.White);
            DrawLine(target, gp + new Vector2(280, 25), gp + new Vector2(390, 25), new Color(60, 60, 60));

            string[] layerName = { "Ground", "Mask", "Mask Anim", "Mask2", "Mask2 Anim", "Fringe", "Fringe Anim", "Fringe2", "Fringe2 Anim" };
            for (int i = 0; i < layerName.Length; i++)
            {
                if (i == CurrentLayer)
                    DrawRoundedRectangle(target, gp + new Vector2(280, 30 + 24 * i), new Vector2(110, 20), new Color(255, 255, 255, 10), 8, 8, 1, new Color(93, 162, 251));
                else if (i == hoverLayer)
                    DrawRoundedRectangle(target, gp + new Vector2(280, 30 + 24 * i), new Vector2(110, 20), new Color(255, 255, 255, 10), 8, 8);

                DrawText(target, layerName[i], 12, gp + new Vector2(280 + (110 - GetTextWidth(layerName[i])) / 2, 30 + 24 * i + 2), Color.White);
            }
        }
    }
}
