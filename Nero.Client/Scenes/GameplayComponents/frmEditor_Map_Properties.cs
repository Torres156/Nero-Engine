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
        TextBox txtName, txtMusic, txtFogID, txtFogVelocity, txtFogOpacity;
        CheckBox chkFogBlendAdd;
        ComboBox cmbZone;
        Panel pMusic;
        ListBox lstMusic;
        Button btnMusicOK;
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

            txtName = new TextBox(this)
            {
                Size = new Vector2(150,20),
                Position = new Vector2(140,5),
                MaxLength = 30,
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            cmbZone = new ComboBox(this)
            {
                Size = new Vector2(150,20),
                Position = new Vector2(140, 5 + 25 * 1),
            };
            cmbZone.Item.Add("Normal");
            cmbZone.Item.Add("Peace");
            cmbZone.Item.Add("Chaotic");
            cmbZone.SelectIndex = 0;

            txtMusic = new TextBox(this)
            {
                Blocked = true,
                Size = new Vector2(150,20),
                Position = new Vector2(140,5 + 25 * 2),
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
                Size = new Vector2(40,20),
                Position = new Vector2(100,5),
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            txtFogOpacity = new TextBox(tabs.TabPanels[0])
            {
                Minimum = 0,
                Maximum = 100,
                Text = "20",
                isNumeric = true,
                Size = new Vector2(40, 20),
                Position = new Vector2(100, 5 + 25 * 1),
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            chkFogBlendAdd = new CheckBox(tabs.TabPanels[0])
            {
                Position = new Vector2(140 - 14,5 + 25 * 2),                
            };

            #region Last Control sequence
            pMusic = new Panel(this)
            {
                Size = new Vector2(Size.x - 20, 105),
                Position = new Vector2(10, txtMusic.Position.y + 22),
                Visible = false,
                OutlineThickness = 1,
                OutlineColor = new Color(60,60,60),
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
                Size = new Vector2(100,20),
                Border_Rounded = 8,
                Position = new Vector2(0,5),                
            };
            btnMusicOK.SetText(Languages.PT_BR, "Usar");
            btnMusicOK.SetText(Languages.EN_USA, "Use");
            btnMusicOK.OnMouseReleased += (sender, e) => { if (lstMusic.SelectIndex > -1) txtMusic.Text = lstMusic.SelectItem; pMusic.Hide(); };
            #endregion

            OnDraw += FrmEditor_Map_Properties_OnDraw;

            words.AddText("Nome:", "Name:");            // 0
            words.AddText("Zona:", "Zone:");            // 1
            words.AddText("Musica:", "Music:");         // 2
            words.AddText("Velocidade:", "Speed:");     // 3
            words.AddText("Opacidade:", "Opacity:");    // 4            
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
                DrawRectangle(target, rec_pos, new Vector2(100), new Color(30, 30, 30),1,new Color(60,60,60));

                if (txtFogID.Value > 0 && txtFogID.Value < GlobalResources.Fog.Count)
                {
                    var tex = GlobalResources.Fog[txtFogID.Value];
                    DrawTexture(target, tex, new Rectangle(rec_pos, new Vector2(100)));
                }


                DrawText(target, "Fog ID:", 12, rec_pos + new Vector2(0, 106), Color.White);
                DrawText(target, words.GetText(3), 12, gp + new Vector2(5, 5 + 25 * 0), Color.White);
                DrawText(target, words.GetText(4), 12, gp + new Vector2(5, 5 + 25 * 1), Color.White);
                DrawText(target, "Blend Add:", 12, gp + new Vector2(5, 5 + 25 * 2), Color.White);
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
