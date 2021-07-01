using Nero.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Client.Scenes.GameplayComponents
{
    using static Renderer;
    class frmEditor_Spawn : Form
    {
        ListBox lstIndex;
        TextBox txtFind, txtNpcID;
        Button btnCopy, btnDelete;
        CheckBox chkBlock;
        ComboBox cbDir;

        LanguageWords words = new LanguageWords();

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public frmEditor_Spawn(Bond bond) : base(bond)
        {
            Anchor = Anchors.Center;
            Size = new Vector2(400, 250);
            SetTitle(Languages.PT_BR, "Editor de Spawn");
            SetTitle(Languages.EN_USA, "Editor Spawn");

            txtFind = new TextBox(this)
            {
                Position = new Vector2(5, 5),
                Size = new Vector2(150, 20),
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            lstIndex = new ListBox(this)
            {
                Position = new Vector2(5, 30),
                Size = new Vector2(150, Size.y - BAR_HEIGHT - 15 - 30 - 25 ),
            };

            txtNpcID = new TextBox(this)
            {
                Anchor = Anchors.TopRight,
                FillColor = Color.White,
                TextColor = Color.Black,
                isNumeric = true,
                Size = new Vector2(150,20),
                Position = new Vector2(5,5 + 25),
                Minimum = 0,
                Maximum = Constants.MAX_NPCS - 1,
                Text = "0",                
            };

            btnCopy = new Button(this)
            {
                Anchor = Anchors.BottomLeft,
                Size = new Vector2(70,25),
                Position = new Vector2(5,0),
                Border_Rounded = 12,
            };
            btnCopy.SetText(Languages.PT_BR, "Copiar");
            btnCopy.SetText(Languages.EN_USA, "Copy");

            btnDelete = new Button(this)
            {
                Anchor = Anchors.BottomLeft,
                Size = new Vector2(70, 25),
                Position = new Vector2(85,0),
                Border_Rounded = 12,
            };
            btnDelete.SetText(Languages.PT_BR, "Deletar");
            btnDelete.SetText(Languages.EN_USA, "Delete");

            chkBlock = new CheckBox(this)
            {
                Anchor = Anchors.TopRight,
                Position = new Vector2(5,5 + 25 * 2),
            };

            cbDir = new ComboBox(this)
            {
                Anchor = Anchors.TopRight,
                Position = new Vector2(5, 5 + 25 * 2),
            };
            cbDir.Item.AddRange(new string[] {"" }.AsEnumerable());

            // Eventos
            OnDraw += FrmEditor_Spawn_OnDraw;

            // Palavras
            words.AddText("Bloqueio M.:", "M. Block:");
        }

        /// <summary>
        /// Desenha na janela
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="target"></param>
        private void FrmEditor_Spawn_OnDraw(ControlBase sender, RenderTarget target)
        {
            var gp = sender.GlobalPosition();

            DrawText(target, "Npc ID:", 12, new Vector2(gp.x + 160, txtNpcID.GlobalPosition().y), Color.White);
            DrawText(target, words[0], 12, new Vector2(gp.x + 160, chkBlock.GlobalPosition().y), Color.White);
        }
    }
}
