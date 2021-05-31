using Nero.Control;
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


        // Texturas
        

        // Publics
        public Rectangle SelectTile = new Rectangle(Vector2.Zero, Vector2.One); // Seleção do tile
        public int CurrentLayer = 0;                                            // Camada usada


        // Privates
        int hoverLayer = -1;
        LanguageWords words = new LanguageWords();


        // Controls
        Panel pTile;                // Painel de tile
        Panel pAttribute;           // Painel de atributos
        TextBox txtTileID;          // ID do Tile
        ComboBox cmbTileType;       // Tipo de tile
        Button btnSave;             // Botão de salvar
        Button btnProperties;       // Botão de propriedades
        public Button btnGrid;      // Botão de grid
        public Button btnLight;     // Botão de luz
        Button btnFill;             // Pinta tudo
        public Button btnTile;      // Botão de tileset
        public Button btnAttribute; // Botão de atributos


        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public frmEditor_Map(Bond bond) : base(bond)
        {
            Name = "frmEditor_Map";
            Size = new Vector2(400, 360);
            Anchor = Anchors.Center;
            SetTitle(Languages.PT_BR, "Editor de mapa");
            SetTitle(Languages.EN_USA, "Map Editor");
            OutlineColor = new Color(60, 60, 60);
            OutlineThickness = 1;
            Visible = false;


            pTile = new Panel(this)
            {
                Name = "pTile",
                Size = new Vector2(Size.x - 10, Size.y - 65),
                Position = new Vector2(5, 25),
                OutlineThickness = 1,
                OutlineColor = new Color(90,90,90),
                FillColor = new Color(20,20,20),
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
                Position = new Vector2(55,2),
                Size = new Vector2(30,19),
                isNumeric = true,
                Maximum = 0,
                Anchor = Anchors.BottomLeft,
                Value = 0,
                FillColor = Color.White,
                TextColor = new Color(30,30,30),
            };

            cmbTileType = new ComboBox(pTile)
            {
                Size = new Vector2(100,19),
                Position = new Vector2(90,2),
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


            // Events
            pTile.OnDraw += PTile_OnDraw;
            pTile.OnMouseMove += PTile_OnMouseMove;
            pTile.OnMouseReleased += PTile_OnMouseReleased;

            // Words
            words.AddText("Camadas", "Layers");
        }
        
        /// <summary>
        /// Clique do mouse
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PTile_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
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
            for(int i = 0; i < (int)Map.Layers.count; i++)
            {
                var pos = gp + new Vector2(270, 30 + 24 * i);
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

            DrawText(target, "TileID:", 12, gp + new Vector2(5, sender.Size.y - 18), Color.White);

            // Camadas
            DrawText(target, words.GetText(0), 16, gp + new Vector2(270 + (110 - GetTextWidth(words.GetText(0), 16)) / 2, 4), Color.White);
            DrawLine(target, gp + new Vector2(270, 25), gp + new Vector2(380, 25), new Color(60, 60, 60));

            string[] layerName = { "Ground", "Mask", "Mask Anim", "Mask2", "Mask2 Anim", "Fringe", "Fringe Anim", "Fringe2", "Fringe2 Anim" };
            for(int i = 0; i < layerName.Length; i++)
            {
                if (i == CurrentLayer)
                    DrawRoundedRectangle(target, gp + new Vector2(270, 30 + 24 * i), new Vector2(110, 20), new Color(255,255,255,10),8,8, 1, new Color(93, 162, 251));
                else if(i == hoverLayer)
                    DrawRoundedRectangle(target, gp + new Vector2(270, 30 + 24 * i), new Vector2(110, 20), new Color(255, 255, 255, 10), 8, 8);

                DrawText(target, layerName[i], 12, gp + new Vector2(270 + (110 - GetTextWidth(layerName[i])) / 2, 30 + 24 * i + 2), Color.White);
            }
        }
    }
}
