using Nero.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Scenes.GameplayComponents
{
    using static Renderer;
    class frmAdmin : Form
    {
        Button btnEditMap;

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
            canDragged = false;
            Visible = false;


            btnEditMap = new Button(this)
            {
                Size = new Vector2(100,20),
                Position = new Vector2(20),                
            };
            btnEditMap.SetText(Languages.PT_BR, "Editar mapa");
            btnEditMap.SetText(Languages.EN_USA, "Edit map");


            // Events
            btnEditMap.OnMouseReleased += BtnEditMap_OnMouseReleased;
            OnDraw += FrmAdmin_OnDraw;

            words.AddText("Editores", "Editors");
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
