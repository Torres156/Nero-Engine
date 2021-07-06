using Nero.Client.World;
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
        Button btnCopy, btnDelete, btnSave;
        CheckBox chkBlock;
        ComboBox cbDir;

        bool isUpdate = false;

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
            Visible = false;


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
                Size = new Vector2(150, Size.y - BAR_HEIGHT - 15 - 30 - 25),
            };


            txtNpcID = new TextBox(this)
            {
                Anchor = Anchors.TopRight,
                FillColor = Color.White,
                TextColor = Color.Black,
                isNumeric = true,
                Size = new Vector2(150, 20),
                Position = new Vector2(5, 5 + 25),
                Minimum = 0,
                Maximum = Constants.MAX_NPCS - 1,
                Text = "0",
            };

            btnCopy = new Button(this)
            {
                Anchor = Anchors.BottomLeft,
                Size = new Vector2(70, 25),
                Position = new Vector2(5, 0),
                Border_Rounded = 12,
            };
            btnCopy.SetText(Languages.PT_BR, "Copiar");
            btnCopy.SetText(Languages.EN_USA, "Copy");

            btnDelete = new Button(this)
            {
                Anchor = Anchors.BottomLeft,
                Size = new Vector2(70, 25),
                Position = new Vector2(85, 0),
                Border_Rounded = 12,
            };
            btnDelete.SetText(Languages.PT_BR, "Deletar");
            btnDelete.SetText(Languages.EN_USA, "Delete");

            btnSave = new Button(this)
            {
                Anchor = Anchors.BottomRight,
                Size = new Vector2(70, 25),
                Position = new Vector2(5, 0),
                Border_Rounded = 12,
            };
            btnSave.SetText(Languages.PT_BR, "Salvar");
            btnSave.SetText(Languages.EN_USA, "Save");

            chkBlock = new CheckBox(this)
            {
                Anchor = Anchors.TopRight,
                Position = new Vector2(5, 5 + 25 * 2),
            };

            cbDir = new ComboBox(this)
            {
                Anchor = Anchors.TopRight,
                Position = new Vector2(5, 5 + 25 * 3),
                Size = new Vector2(150, 20),
            };
            cbDir.Item.AddRange(new string[] { "UP", "DOWN", "LEFT", "RIGHT" }.AsEnumerable());
            cbDir.SelectIndex = 0;

            // Eventos
            OnDraw += FrmEditor_Spawn_OnDraw;
            OnVisibleChanged += FrmEditor_Spawn_OnVisibleChanged;
            lstIndex.OnSelectIndex += LstIndex_OnSelectIndex;
            txtFind.OnTextChanged += TxtFind_OnTextChanged;
            btnSave.OnMouseReleased += BtnSave_OnMouseReleased;
            btnCopy.OnMouseReleased += BtnCopy_OnMouseReleased;
            btnDelete.OnMouseReleased += BtnDelete_OnMouseReleased;

            // Palavras
            words.AddText("Bloqueio M.:", "M. Block:");
            words.AddText("Direção:", "Direction:");
        }

        private void BtnDelete_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            var Index = lstIndex.SelectIndex;
            if (Index < 0)
                return;

            var spawns = GetSpawns().ToList();
            if (Index >= spawns.Count)
                return;

            var s = spawns[Index];
            lstIndex.Item.RemoveAt(Index);
            lstIndex.SelectIndex = Math.Max(0, Index - 1);
            SpawnFactory.Items.Remove(s);
            isUpdate = true;
        }

        private void BtnCopy_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            var Index = lstIndex.SelectIndex;
            if (Index < 0)
                return;

            var spawns = GetSpawns().ToList();
            if (Index >= spawns.Count)
                return;

            var s = spawns[Index];
            lstIndex.Item[lstIndex.Item.Count - 1] = Npc.Items[s.NpcID].Name;                        
            lstIndex.Add("New npc");
            lstIndex.SelectIndex = lstIndex.Item.Count - 2;
            SpawnFactory.Items.Add(s);
            isUpdate = true;
        }

        private void BtnSave_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            var Index = lstIndex.SelectIndex;
            if (Index < 0)
                return;

            var spawns = GetSpawns().ToList();
            var isNotNew = spawns.Count > 0 && Index < spawns.Count;
            var s = isNotNew ? spawns[Index] : new SpawnFactoryItem();

            s.NpcID = (int)txtNpcID.Value;
            s.BlockMove = chkBlock.Checked;
            s.Direction = (Directions)cbDir.SelectIndex;

            lstIndex.Item[Index] = Npc.Items[s.NpcID].Name;
            if (!isNotNew)
            {
                SpawnFactory.Items.Add(s);
                lstIndex.Add("New npc");
            }
            isUpdate = true;
        }

        private void TxtFind_OnTextChanged(ControlBase sender)
        {
            UpdateList();
        }

        private void LstIndex_OnSelectIndex(ControlBase sender)
        {
            var Index = lstIndex.SelectIndex;
            if (Index < 0) return;

            var spawns = GetSpawns().ToList();
            var isNotNew = spawns.Count > 0 && Index < spawns.Count;
            var s = isNotNew ? spawns[Index] : new SpawnFactoryItem();

            txtNpcID.Value = s.NpcID;
            chkBlock.Checked = s.BlockMove;
            cbDir.SelectIndex = (int)s.Direction;
        }

        private void FrmEditor_Spawn_OnVisibleChanged(ControlBase sender)
        {
            if (!Visible)
            {
                if (isUpdate)
                {
                    Network.Sender.UpdateSpawnFactory();
                }

                Game.GetScene<GameplayScene>().ExitEditor();
            }
            else // Visivel
            {
                isUpdate = false;
                UpdateList();
            }
        }

        void UpdateList()
        {
            var text = txtFind.Text.Trim();
            lstIndex.Clear();
            foreach (var i in GetSpawns())
                lstIndex.Add(Npc.Items[i.NpcID].Name);

            lstIndex.Add("New spawn");
            lstIndex.SelectIndex = lstIndex.Item.Count - 1;
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
            DrawText(target, words[1], 12, new Vector2(gp.x + 160, cbDir.GlobalPosition().y), Color.White);
        }

        IEnumerable<SpawnFactoryItem> GetSpawns()
        {
            var text = txtFind.Text.Trim();
            return SpawnFactory.Items.Where(i => Npc.Items[i.NpcID].Name.Contains(text, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
