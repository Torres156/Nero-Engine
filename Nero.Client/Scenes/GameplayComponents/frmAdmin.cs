using Nero.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Scenes.GameplayComponents
{
    using static Renderer;
    class frmAdmin : Form
    {
        Button btnEditMap,btnEditNpc, btnEditSpawn;
        
        LanguageWords words = new LanguageWords();

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public frmAdmin(Bond bond) : base(bond)
        {
            Size = new Vector2(250, 200);
            OutlineThickness = 1;
            OutlineColor = new Color(60, 60, 60);
            Anchor = Anchors.Center;
            SetTitle(Languages.PT_BR, "Administração");
            SetTitle(Languages.EN_USA, "Administration");            
            Visible = false;


            btnEditMap = new Button(this)
            {
                Size = new Vector2(100,20),
                Position = new Vector2(20),  
                Border_Rounded = 8,
            };
            btnEditMap.SetText(Languages.PT_BR, "Editar Mapa");
            btnEditMap.SetText(Languages.EN_USA, "Edit Map");

            btnEditNpc = new Button(this)
            {
                Size = new Vector2(100, 20),
                Position = new Vector2(20,20+ 22 * 1),
                Border_Rounded = 8,
            };
            btnEditNpc.SetText(Languages.PT_BR, "Editar Npc");
            btnEditNpc.SetText(Languages.EN_USA, "Edit Npc");

            btnEditSpawn = new Button(this)
            {
                Size = new Vector2(100, 20),
                Position = new Vector2(20, 20 + 22 * 2),
                Border_Rounded = 8,
            };
            btnEditSpawn.SetText(Languages.PT_BR, "Editar Spawn");
            btnEditSpawn.SetText(Languages.EN_USA, "Edit Spawn");


            // Events
            btnEditMap.OnMouseReleased += BtnEditMap_OnMouseReleased;
            btnEditNpc.OnMouseReleased += BtnEditNpc_OnMouseReleased;
            btnEditSpawn.OnMouseReleased += BtnEditSpawn_OnMouseReleased;
            OnDraw += FrmAdmin_OnDraw;

            words.AddText("Editores", "Editors");
        }

        /// <summary>
        /// Edita os Spawns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditSpawn_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            Hide();
            Network.Sender.RequestSpawnFactory();            
        }

        /// <summary>
        /// Edita os NPC
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditNpc_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            Hide();
            Game.GetScene<GameplayScene>().SetEditor<frmEditor_Npc>();
        }

        /// <summary>
        /// Desenha no form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        private void FrmAdmin_OnDraw(ControlBase sender, RenderTarget target)
        {
            var gp = GlobalPosition();
            DrawText(target, words.GetText(0), 14, gp + new Vector2(20, 40), Color.White);
        }

        /// <summary>
        /// Edita o mapa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditMap_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            Hide();
            Game.GetScene<GameplayScene>().SetEditor<frmEditor_Map>();
        }
    }
}
