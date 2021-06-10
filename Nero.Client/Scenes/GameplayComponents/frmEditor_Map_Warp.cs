using Nero.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Scenes.GameplayComponents
{
    using static Renderer;
    class frmEditor_Map_Warp : Form
    {
        TextBox txtMapID, txtX, txtY;
        Button btnSave;
        bool save = false;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public frmEditor_Map_Warp(Bond bond) : base(bond)
        {
            Size = new Vector2(300, 200);
            Anchor = Anchors.Center;
            SetTitle(Languages.PT_BR, "Teleporte");
            SetTitle(Languages.EN_USA, "Warp");
            canDragged = false;
            Visible = false;
            OutlineThickness = 1;
            OutlineColor = new Color(70, 70, 70);

            txtMapID = new TextBox(this)
            {
                isNumeric = true,
                MaxLength = 10,
                Minimum = 1,
                Maximum = Constants.MAX_CHARACTERS,
                Size = new Vector2(260, 20),
                Anchor = Anchors.TopCenter,
                Position = new Vector2(0, 20),
                Value = 1,
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            txtX = new TextBox(this)
            {
                isNumeric = true,
                MaxLength = 10,
                Minimum = 0,
                Maximum = 999,
                Size = new Vector2(100, 20),
                Anchor = Anchors.TopCenter,
                Position = new Vector2(-54, 70),
                Value = 0,
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            txtY = new TextBox(this)
            {
                isNumeric = true,
                MaxLength = 10,
                Minimum = 0,
                Maximum = 999,
                Size = new Vector2(100, 20),
                Anchor = Anchors.TopCenter,
                Position = new Vector2(54, 70),
                Value = 0,
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            btnSave = new Button(this)
            {
                Size = new Vector2(100,30),
                Anchor = Anchors.BottomCenter,
                Position = new Vector2(0,10),
                Border_Rounded = 8,
            };
            btnSave.SetText(Languages.PT_BR, "Salvar");
            btnSave.SetText(Languages.EN_USA, "Save");

            OnDraw += FrmEditor_Map_Warp_OnDraw;
            OnVisibleChanged += FrmEditor_Map_Warp_OnVisibleChanged;
            btnSave.OnMouseReleased += (sender, e) => Hide();
        }

        private void FrmEditor_Map_Warp_OnVisibleChanged(ControlBase sender)
        {
            if (!Visible)
                if (!save)
                    Game.GetScene().FindControl<frmEditor_Map>().CurrentAttribute = Map.AttributeTypes.Block;
                else
                    Game.GetScene().FindControl<frmEditor_Map>().args = new string[] 
                    { txtMapID.Value.ToString(), txtX.Value.ToString(), txtY.Value.ToString()};
        }

        private void FrmEditor_Map_Warp_OnDraw(ControlBase sender, RenderTarget target)
        {
            DrawText(target, "Map:", 14, txtMapID.GlobalPosition() + new Vector2(1, -20), Color.White);
            DrawText(target, "X:", 14, txtX.GlobalPosition() + new Vector2(1, -20), Color.White);
            DrawText(target, "Y:", 14, txtY.GlobalPosition() + new Vector2(1, -20), Color.White);
        }
    }
}
