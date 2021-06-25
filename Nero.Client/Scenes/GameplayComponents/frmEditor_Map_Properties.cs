using Nero.Client.Map;
using Nero.Client.World;
using Nero.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Nero.Client.Scenes.GameplayComponents
{
    using static Renderer;
    class frmEditor_Map_Properties : Form
    {
        TextBox txtName, txtMusic, txtFogID, txtFogVelocity, txtFogOpacity, txtPanoramaID,
            txtWarpUp, txtWarpDown, txtWarpLeft, txtWarpRight, txtSizeX, txtSizeY;
        CheckBox chkFogBlendAdd;
        ComboBox cmbZone;
        Panel pMusic;
        ListBox lstMusic;
        Button btnMusicOK, btnMusicPlayStop;
        Button btnSave;
        TabControl tabs;


        LanguageWords words = new LanguageWords();

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public frmEditor_Map_Properties(Bond bond) : base(bond)
        {
            Size = new Vector2(300, 360);
            SetTitle(Languages.PT_BR, "Propriedades");
            SetTitle(Languages.EN_USA, "Properties");
            Anchor = Anchors.Center;
            canDragged = false;
            Visible = false;
            OutlineThickness = 1;


            // Controls
            txtName = new TextBox(this)
            {
                Size = new Vector2(150, 20),
                Position = new Vector2(140, 5),
                MaxLength = 30,
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            cmbZone = new ComboBox(this)
            {
                Size = new Vector2(150, 20),
                Position = new Vector2(140, 5 + 25 * 1),
            };
            cmbZone.Item.Add("Normal");
            cmbZone.Item.Add("Peace");
            cmbZone.Item.Add("Chaotic");
            cmbZone.SelectIndex = 0;

            txtMusic = new TextBox(this)
            {
                Blocked = true,
                Size = new Vector2(150, 20),
                Position = new Vector2(140, 5 + 25 * 2),
                Text = "None",
                FillColor = Color.White,
                TextColor = Color.Black,
            };
            txtMusic.OnMouseReleased += (sender, e) => pMusic.Toggle();

            tabs = new TabControl(this)
            {
                Size = new Vector2(Size.x - 20, 165),
                Position = new Vector2(10, 90),
                OutlineThickness = 1,
            };
            tabs.OnDrawTabPanel += Tabs_OnDrawTabPanel; ;
            tabs.Add("Fog", "Fog");
            tabs.Add("Panorama", "Panorama");
            tabs.Add("Teleporte", "Warp");
            tabs.Add("Tamanho", "Size");

            txtFogID = new TextBox(tabs.TabPanels[0])
            {
                Minimum = 0,
                Maximum = GlobalResources.Fog.Count - 1,
                Text = "0",
                isNumeric = true,
                Size = new Vector2(40, 20),
                Position = new Vector2(tabs.TabPanels[0].Size.x - 105 + 60, 110),
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            txtFogVelocity = new TextBox(tabs.TabPanels[0])
            {
                Minimum = 0,
                Maximum = 500,
                Text = "80",
                isNumeric = true,
                Size = new Vector2(40, 20),
                Position = new Vector2(100, 5),
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            txtFogOpacity = new TextBox(tabs.TabPanels[0])
            {
                Minimum = 0,
                Maximum = 255,
                Text = "20",
                isNumeric = true,
                Size = new Vector2(40, 20),
                Position = new Vector2(100, 5 + 25 * 1),
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            chkFogBlendAdd = new CheckBox(tabs.TabPanels[0])
            {
                Position = new Vector2(140 - 14, 5 + 25 * 2),
            };

            txtPanoramaID = new TextBox(tabs.TabPanels[1])
            {
                Anchor = Anchors.BottomCenter,
                Position = new Vector2(30, 5),
                Size = new Vector2(40, 20),
                isNumeric = true,
                Maximum = GlobalResources.Panorama.Count - 1,
                Minimum = 0,
                Text = "0",
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            txtWarpUp = new TextBox(tabs.TabPanels[2])
            {
                Anchor = Anchors.Center,
                Size = new Vector2(40, 20),
                isNumeric = true,
                Maximum = Constants.MAX_MAPS,
                Minimum = 0,
                Text = "0",
                FillColor = Color.White,
                TextColor = Color.Black,
                Align = TextAligns.Center,
                Position = new Vector2(0, -24)
            };

            txtWarpDown = new TextBox(tabs.TabPanels[2])
            {
                Anchor = Anchors.Center,
                Size = new Vector2(40, 20),
                isNumeric = true,
                Maximum = Constants.MAX_MAPS,
                Minimum = 0,
                Text = "0",
                FillColor = Color.White,
                TextColor = Color.Black,
                Align = TextAligns.Center,
                Position = new Vector2(0, 24)
            };

            txtWarpLeft = new TextBox(tabs.TabPanels[2])
            {
                Anchor = Anchors.Center,
                Size = new Vector2(40, 20),
                isNumeric = true,
                Maximum = Constants.MAX_MAPS,
                Minimum = 0,
                Text = "0",
                FillColor = Color.White,
                TextColor = Color.Black,
                Align = TextAligns.Center,
                Position = new Vector2(-44, 0)
            };

            txtWarpRight = new TextBox(tabs.TabPanels[2])
            {
                Anchor = Anchors.Center,
                Size = new Vector2(40, 20),
                isNumeric = true,
                Maximum = Constants.MAX_MAPS,
                Minimum = 0,
                Text = "0",
                FillColor = Color.White,
                TextColor = Color.Black,
                Align = TextAligns.Center,
                Position = new Vector2(44, 0)
            };

            txtSizeX = new TextBox(tabs.TabPanels[3])
            {
                Anchor = Anchors.Center,
                Size = new Vector2(40, 20),
                Position = new Vector2(-24, 0),
                isNumeric = true,
                Maximum = 999,
                Minimum = 10,
                Text = "10",
                FillColor = Color.White,
                TextColor = Color.Black,
                Align = TextAligns.Center,
            };

            txtSizeY = new TextBox(tabs.TabPanels[3])
            {
                Anchor = Anchors.Center,
                Size = new Vector2(40, 20),
                Position = new Vector2(24, 0),
                isNumeric = true,
                Maximum = 999,
                Minimum = 10,
                Text = "10",
                FillColor = Color.White,
                TextColor = Color.Black,
                Align = TextAligns.Center,
            };

            btnSave = new Button(this)
            {
                Anchor = Anchors.BottomCenter,
                Size = new Vector2(100, 30),
                Border_Rounded = 12,
                Position = new Vector2(0, 5),
            };
            btnSave.SetText(Languages.PT_BR, "Salvar");
            btnSave.SetText(Languages.EN_USA, "Save");
            btnSave.OnMouseReleased += BtnSave_OnMouseReleased;

            #region Last Control sequence
            pMusic = new Panel(this)
            {
                Size = new Vector2(Size.x - 20, 105),
                Position = new Vector2(10, txtMusic.Position.y + 22),
                Visible = false,
                OutlineThickness = 1,
                OutlineColor = new Color(60, 60, 60),
            };
            pMusic.OnVisibleChanged += (sender) =>
            {
                if (btnMusicPlayStop.Text[0] == "Parar")
                {
                    btnMusicPlayStop.Text = new[] { "Tocar", "Play" };
                    Sound.StopMusic();
                }
            };

            lstMusic = new ListBox(pMusic)
            {
                Size = new Vector2(pMusic.Size.x - 10, 70),
                Position = new Vector2(5),
            };
            lstMusic.Add("None");
            var musics = Directory.GetFiles("res/music/");
            foreach (var i in musics)
                lstMusic.Add(i.Substring("res/music/".Length));
            lstMusic.SelectIndex = 0;

            btnMusicOK = new Button(pMusic)
            {
                Anchor = Anchors.BottomCenter,
                Size = new Vector2(100, 20),
                Border_Rounded = 8,
                Position = new Vector2(0, 5),
            };
            btnMusicOK.SetText(Languages.PT_BR, "Usar");
            btnMusicOK.SetText(Languages.EN_USA, "Use");
            btnMusicOK.OnMouseReleased += (sender, e) =>
            {
                if (lstMusic.SelectIndex > -1)
                    txtMusic.Text = lstMusic.SelectItem; pMusic.Hide();
            };

            btnMusicPlayStop = new Button(pMusic)
            {
                Anchor = Anchors.BottomLeft,
                Size = new Vector2(80, 20),
                Border_Rounded = 8,
                Position = new Vector2(5)
            };
            btnMusicPlayStop.Text = new[] { "Tocar", "Play" };
            btnMusicPlayStop.OnMouseReleased += BtnMusicPlayStop_OnMouseReleased;
            #endregion

            OnDraw += FrmEditor_Map_Properties_OnDraw;
            OnVisibleChanged += FrmEditor_Map_Properties_OnVisibleChanged;

            words.AddText("Nome:", "Name:");            // 0
            words.AddText("Zona:", "Zone:");            // 1
            words.AddText("Musica:", "Music:");         // 2
            words.AddText("Velocidade:", "Speed:");     // 3
            words.AddText("Opacidade:", "Opacity:");    // 4
            words.AddText("Volume está no 0!",
                "Volume is on 0!");                     // 5
        }

        /// <summary>
        /// Tocar ou parar musica
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMusicPlayStop_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            if (btnMusicPlayStop.Text[0] == "Tocar")
            {
                if (lstMusic.SelectIndex > 0)
                {
                    if (WindowSettings.Instance.Volume_Music == 0)
                    {
                        Game.GetScene().Alert(words.GetText(5));
                        return;
                    }
                    var music_name = lstMusic.SelectItem;
                    Sound.PlayMusic("res/music/" + music_name, false);
                    btnMusicPlayStop.Text = new[] { "Parar", "Stop" };
                }
            }
            else if (btnMusicPlayStop.Text[0] == "Parar")
            {
                Sound.StopMusic();
                btnMusicPlayStop.Text = new[] { "Tocar", "Play" };
            }
        }

        /// <summary>
        /// Salva o mapa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            var m = MapInstance.Current;
            m.Name = txtName.Text;
            m.Zone = (ZoneTypes)cmbZone.SelectIndex;
            m.MusicName = txtMusic.Text;
            m.FogID = (int)txtFogID.Value;
            m.FogSpeed = (int)txtFogVelocity.Value;
            m.FogOpacity = (byte)txtFogOpacity.Value;
            m.FogBlend = chkFogBlendAdd.Checked;
            m.PanoramaID = (int)txtPanoramaID.Value;
            m.Warps = new[] { (int)txtWarpUp.Value, (int)txtWarpDown.Value, (int)txtWarpLeft.Value, (int)txtWarpRight.Value };
            m.SetSize((int)txtSizeX.Value, (int)txtSizeY.Value);

            Hide();
        }

        /// <summary>
        /// Quando abre a janela
        /// </summary>
        /// <param name="sender"></param>
        private void FrmEditor_Map_Properties_OnVisibleChanged(ControlBase sender)
        {
            if (Visible)
            {
                var m = MapInstance.Current;
                txtName.Text = m.Name;
                cmbZone.SelectIndex = (int)m.Zone;
                txtMusic.Text = m.MusicName;
                txtFogID.Value = m.FogID;
                txtFogVelocity.Value = m.FogSpeed;
                txtFogOpacity.Value = m.FogOpacity;
                chkFogBlendAdd.Checked = m.FogBlend;
                txtPanoramaID.Value = m.PanoramaID;
                txtWarpUp.Value = m.Warps[0];
                txtWarpDown.Value = m.Warps[1];
                txtWarpLeft.Value = m.Warps[2];
                txtWarpRight.Value = m.Warps[3];
                txtSizeX.Value = m.Size.x;
                txtSizeY.Value = m.Size.y;
            }
            else
            {
                if (btnMusicPlayStop.Text[0] == "Parar")
                {
                    btnMusicPlayStop.Text = new[] { "Tocar", "Play" };
                    Sound.StopMusic();
                }
            }
        }

        /// <summary>
        /// Desenha no painel das tabs
        /// </summary>
        /// <param name="target"></param>
        /// <param name="panel"></param>
        /// <param name="Index"></param>
        private void Tabs_OnDrawTabPanel(RenderTarget target, Panel panel, int Index)
        {
            var gp = panel.GlobalPosition();

            if (Index == 0) // Primeiro Painel
            {
                var rec_pos = gp + new Vector2(panel.Size.x - 5 - 100, 5);
                DrawRectangle(target, rec_pos, new Vector2(100), new Color(30, 30, 30), 1, new Color(60, 60, 60));

                if (txtFogID.Value > 0 && txtFogID.Value < GlobalResources.Fog.Count)
                {
                    var tex = GlobalResources.Fog[(int)txtFogID.Value];
                    DrawTexture(target, tex, new Rectangle(rec_pos, new Vector2(100)));
                }

                DrawText(target, "Fog ID:", 12, rec_pos + new Vector2(0, 106), Color.White);
                DrawText(target, words.GetText(3), 12, gp + new Vector2(5, 5 + 25 * 0), Color.White);
                DrawText(target, words.GetText(4), 12, gp + new Vector2(5, 5 + 25 * 1), Color.White);
                DrawText(target, "Blend Add:", 12, gp + new Vector2(5, 5 + 25 * 2), Color.White);
            }
            else if (Index == 1) // Segundo painel
            {
                var rec_pos = gp + new Vector2((panel.Size.x - 100) / 2, 5);
                DrawRectangle(target, rec_pos, new Vector2(100), new Color(30, 30, 30), 1, new Color(60, 60, 60));
                DrawText(target, "Graphic:", 12, rec_pos + new Vector2(0, 108), Color.White);

                if (txtPanoramaID.Value > 0 && txtPanoramaID.Value < GlobalResources.Panorama.Count)
                {
                    var tex = GlobalResources.Panorama[(int)txtPanoramaID.Value];
                    DrawTexture(target, tex, new Rectangle(rec_pos, new Vector2(100)));
                }
            }
            else if (Index == 2) // Terceiro painel
            {
                DrawText(target, "Up", 12, txtWarpUp.GlobalPosition() + new Vector2((txtWarpUp.Size.x - GetTextWidth("Up")) / 2, -20), Color.White);
                DrawText(target, "Down", 12, txtWarpDown.GlobalPosition() + new Vector2((txtWarpDown.Size.x - GetTextWidth("Down")) / 2, -20), Color.White);
                DrawText(target, "Left", 12, txtWarpLeft.GlobalPosition() + new Vector2((txtWarpLeft.Size.x - GetTextWidth("Left")) / 2, -20), Color.White);
                DrawText(target, "Right", 12, txtWarpRight.GlobalPosition() + new Vector2((txtWarpRight.Size.x - GetTextWidth("Right")) / 2, -20), Color.White);
            }
            else if (Index == 3) // Quarto painel
            {
                DrawText(target, "X", 12, txtSizeX.GlobalPosition() + new Vector2((txtSizeX.Size.x - GetTextWidth("X")) / 2, -20), Color.White);
                DrawText(target, "Y", 12, txtSizeY.GlobalPosition() + new Vector2((txtSizeY.Size.x - GetTextWidth("Y")) / 2, -20), Color.White);
            }
        }

        /// <summary>
        /// Desenha na janela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        private void FrmEditor_Map_Properties_OnDraw(ControlBase sender, RenderTarget target)
        {
            var gp = GlobalPosition();

            DrawText(target, words.GetText(0), 14, new Vector2(gp.x + 10, txtName.GlobalPosition().y), Color.White);
            DrawText(target, words.GetText(1), 14, new Vector2(gp.x + 10, cmbZone.GlobalPosition().y), Color.White);
            DrawText(target, words.GetText(2), 14, new Vector2(gp.x + 10, txtMusic.GlobalPosition().y), Color.White);
        }
    }
}
