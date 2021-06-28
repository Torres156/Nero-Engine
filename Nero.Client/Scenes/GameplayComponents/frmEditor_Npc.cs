using Nero.Client.World;
using Nero.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Client.Scenes.GameplayComponents
{
    using static Renderer;
    class frmEditor_Npc : Form
    {
        ListBox lstIndex;
        TabControl tabs;
        TextBox txtName, txtLevel, txtExp, txtSpriteID, txtScale, txtHP, txtRegen, txtDamage, txtResistPhysic, txtResistMagic,
            txtAttackSpeed, txtMoveSpeed, txtFind;
        ComboBox cmbBehavior, cmbRange;
        Button btnSave;

        LanguageWords words = new LanguageWords();

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="bond"></param>
        public frmEditor_Npc(Bond bond) : base(bond)
        {
            Anchor = Anchors.Center;
            Size = new Vector2(500, 500);
            SetTitle(Languages.PT_BR, "Editor de NPC");
            SetTitle(Languages.EN_USA, "Edit Npc");
            Visible = false;

            txtFind = new TextBox(this)
            {
                Position = new Vector2(5,5),
                Size = new Vector2(150,20),
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            lstIndex = new ListBox(this)
            {
                Position = new Vector2(5,30),
                Size = new Vector2(150,Size.y - BAR_HEIGHT - 15 - 40 - 30),
            };

            btnSave = new Button(this)
            {
                Size = new Vector2(150, 25),
                Anchor = Anchors.BottomLeft,
                Position = new Vector2(lstIndex.Position.x + lstIndex.Size.x + 5, 5),
                Border_Rounded = 12,
            };
            btnSave.SetText(Languages.PT_BR, "Salvar");
            btnSave.SetText(Languages.EN_USA, "Save");

            tabs = new TabControl(this)
            {
                Size = new Vector2(Size.x - 10 - lstIndex.Size.x, lstIndex.Size.y - 30),
                Position = new Vector2(lstIndex.Position.x + lstIndex.Size.x + 5, 5),                
            };
            tabs.Add("General", "General");
            tabs.Add("Atributos", "Attributes");
            //tabs.Add("Missões", "Quests");

            txtName = new TextBox(tabs[0])
            {                
                Anchor = Anchors.TopRight,
                Position = new Vector2(5),
                Size = new Vector2(200,20),
                MaxLength = 20,
                FillColor = Color.White,
                TextColor = Color.Black,
            };

            cmbBehavior = new ComboBox(tabs[0])
            {
                Anchor = Anchors.TopRight,
                Position = new Vector2(5,5 + 25 * 1),
                Size = new Vector2(200,20),
            };
            cmbBehavior.Item.Add("Passive");
            cmbBehavior.Item.Add("Agressive");
            cmbBehavior.Item.Add("Friend");
            cmbBehavior.Item.Add("Guard");
            cmbBehavior.Item.Add("Boss");
            cmbBehavior.Item.Add("Boss Minion");
            cmbBehavior.SelectIndex = 0;

            txtLevel = new TextBox(tabs[0])
            {
                isNumeric = true,
                Minimum = 1,
                Text = "1",
                Maximum = Constants.MAX_LEVELS,
                FillColor = Color.White,
                TextColor = Color.Black,
                Anchor = Anchors.TopRight,
                Position = new Vector2(5, 5 + 25 * 2),
                Size = new Vector2(200, 20),                
            };

            cmbRange = new ComboBox(tabs[0])
            {
                Anchor = Anchors.TopRight,
                Position = new Vector2(5, 5 + 25 * 3),
                Size = new Vector2(200, 20),
            };
            cmbRange.Item.Add("Melee");
            cmbRange.Item.Add("1");
            cmbRange.Item.Add("2");
            cmbRange.Item.Add("3");
            cmbRange.Item.Add("4");
            cmbRange.Item.Add("5");
            cmbRange.Item.Add("6");
            cmbRange.SelectIndex = 0;

            txtExp = new TextBox(tabs[0])
            {
                isNumeric = true,
                Minimum = 0,
                Text = "0",
                Maximum = long.MaxValue,
                FillColor = Color.White,
                TextColor = Color.Black,
                Anchor = Anchors.TopRight,
                Position = new Vector2(5, 5 + 25 * 4),
                Size = new Vector2(200, 20),
            };

            txtSpriteID = new TextBox(tabs[0])
            {
                isNumeric = true,
                Minimum = 0,
                Text = "0",
                Maximum = GlobalResources.Character.Count - 1,
                FillColor = Color.White,
                TextColor = Color.Black,                
                Position = new Vector2(100, 190 + 25 * 0),
                Size = new Vector2(80, 20),
            };

            txtScale = new TextBox(tabs[0])
            {
                isNumeric = true,
                Minimum = 50,
                Text = "100",
                Maximum = 500,
                FillColor = Color.White,
                TextColor = Color.Black,
                Position = new Vector2(100, 190 + 25 * 1),
                Size = new Vector2(80, 20),
            };

            // # TAB 2
            txtHP = new TextBox(tabs[1])
            {
                Anchor = Anchors.TopRight,
                isNumeric = true,
                Minimum = 1,
                Text = "1",
                Maximum = long.MaxValue,
                FillColor = Color.White,
                TextColor = Color.Black,
                Position = new Vector2(5),
                Size = new Vector2(200, 20),
            };

            txtRegen = new TextBox(tabs[1])
            {
                Anchor = Anchors.TopRight,
                isNumeric = true,
                Minimum = 0,
                Text = "0",
                Maximum = int.MaxValue,
                FillColor = Color.White,
                TextColor = Color.Black,
                Position = new Vector2(5, 5 + 25 * 1),
                Size = new Vector2(200, 20),
            };

            txtDamage = new TextBox(tabs[1])
            {
                Anchor = Anchors.TopRight,
                isNumeric = true,
                Minimum = 0,
                Text = "0",
                Maximum = long.MaxValue,
                FillColor = Color.White,
                TextColor = Color.Black,
                Position = new Vector2(5, 5 + 25 * 2),
                Size = new Vector2(200, 20),
            };

            txtResistPhysic = new TextBox(tabs[1])
            {
                Anchor = Anchors.TopRight,
                isNumeric = true,
                Minimum = 0,
                Text = "0",
                Maximum = long.MaxValue,
                FillColor = Color.White,
                TextColor = Color.Black,
                Position = new Vector2(5, 5 + 25 * 3),
                Size = new Vector2(200, 20),
            };

            txtResistMagic = new TextBox(tabs[1])
            {
                Anchor = Anchors.TopRight,
                isNumeric = true,
                Minimum = 0,
                Text = "0",
                Maximum = long.MaxValue,
                FillColor = Color.White,
                TextColor = Color.Black,
                Position = new Vector2(5, 5 + 25 * 4),
                Size = new Vector2(200, 20),
            };

            txtAttackSpeed = new TextBox(tabs[1])
            {
                Anchor = Anchors.TopRight,
                isNumeric = true,
                Minimum = 100,
                Text = "1000",
                Maximum = 2000,
                FillColor = Color.White,
                TextColor = Color.Black,
                Position = new Vector2(5, 5 + 25 * 5),
                Size = new Vector2(200, 20),
            };

            txtMoveSpeed = new TextBox(tabs[1])
            {
                Anchor = Anchors.TopRight,
                isNumeric = true,
                Minimum = 50,
                Text = "80",
                Maximum = 10000,
                FillColor = Color.White,
                TextColor = Color.Black,
                Position = new Vector2(5, 5 + 25 * 6),
                Size = new Vector2(200, 20),
            };

            // Eventos
            tabs.OnDrawTabPanel += Tabs_OnDrawTabPanel;
            OnVisibleChanged += FrmEditor_Npc_OnVisibleChanged;
            lstIndex.OnSelectIndex += LstIndex_OnSelectIndex;
            btnSave.OnMouseReleased += BtnSave_OnMouseReleased;
            txtFind.OnTextChanged += (sender) => UpdateList();

            // Palavras
            words.AddText("Nome:", "Name:");                                // 0
            words.AddText("Comportamento:", "Behavior:");                   // 1
            words.AddText("Nível:", "Level:");                              // 2
            words.AddText("Alcance:", "Range:");                            // 3
            words.AddText("Experiência:", "Experience:");                   // 4
            words.AddText("Aparência", "Appearance");                       // 5
            words.AddText("Gráfico:", "Graphics:");                         // 6
            words.AddText("Escala:", "Scale:");                             // 7
            words.AddText("Vida:", "Life:");                                // 8
            words.AddText("Regeneração:", "Regeneration:");                 // 9
            words.AddText("Dano:", "Damage:");                              // 10
            words.AddText("Resistência F.:", "Resist P.:");                 // 11
            words.AddText("Resistência M.:", "Resist M.:");                 // 12            
            words.AddText("Digite um nome!", "Enter a name!");              // 13
            words.AddText("Velocidade de ataque:", "Attack Speed:");        // 14
            words.AddText("Velocidade de Movimento:", "Move Speed:");       // 15
            words.AddText("Não pode repetir nome!", "Cannot repeat name!"); // 16
        }

        private void BtnSave_OnMouseReleased(ControlBase sender, SFML.Window.MouseButtonEvent e)
        {
            var Index = lstIndex.SelectIndex;
            if (Index < 0)
                return;

            var npcs = GetNpcs().ToList();
            var isNotNew = npcs.Count > 0 && Index < npcs.Count;
            var n = isNotNew ? npcs[Index] : new Npc();
            if (txtName.Text.Trim().Length == 0)
            {
                Game.GetScene().Alert(words.GetText(13));
                return;
            }

            if (Npc.Items.Any(i => i != n && (i.Name.Equals(txtName.Text, StringComparison.CurrentCultureIgnoreCase)) || i.Name.Equals("New npc")))
            {
                Game.GetScene().Alert(words.GetText(16));
                return;
            }

            n.Name = txtName.Text;
            n.Behavior = (NpcBehavior)cmbBehavior.SelectIndex;
            n.Level = (int)txtLevel.Value;
            n.Range = cmbRange.SelectIndex;
            n.Exp = txtExp.Value;
            n.SpriteID = (int)txtSpriteID.Value;
            n.Scale = (int)txtScale.Value;
            n.HP = txtHP.Value;
            n.Regen = (int)txtRegen.Value;
            n.Damage = (int)txtDamage.Value;
            n.ResistPhysic = (int)txtResistPhysic.Value;
            n.ResistMagic = (int)txtResistMagic.Value;
            n.AttackSpeed = (int)txtAttackSpeed.Value;
            n.MoveSpeed = (int)txtMoveSpeed.Value;

            lstIndex.Item[Index] = Array.IndexOf(Npc.Items, n) + " - " + n.Name;
            Network.Sender.SaveNpc(Array.IndexOf(Npc.Items, n));
        }

        private void LstIndex_OnSelectIndex(ControlBase sender)
        {
            var Index = lstIndex.SelectIndex;
            if (Index < 0)
                return;

            var npcs = GetNpcs().ToList();
            var isNotNew = npcs.Count > 0 && Index < npcs.Count;
            var n = isNotNew ? npcs[Index] : new Npc();
            
            txtName.Text = n.Name;
            cmbBehavior.SelectIndex = (int)n.Behavior;
            txtLevel.Value = n.Level;
            cmbRange.SelectIndex = n.Range;
            txtExp.Value = n.Exp;
            txtSpriteID.Value = n.SpriteID;
            txtScale.Value = n.Scale;
            txtHP.Value = n.HP;
            txtRegen.Value = n.Regen;
            txtDamage.Value = n.Damage;
            txtResistPhysic.Value = n.ResistPhysic;
            txtResistMagic.Value = n.ResistMagic;
            txtAttackSpeed.Value = n.AttackSpeed;
            txtMoveSpeed.Value = n.MoveSpeed;
        }

        private void FrmEditor_Npc_OnVisibleChanged(ControlBase sender)
        {
            if (!Visible)
            {
                Game.GetScene<GameplayScene>().ExitEditor();
            }
            else // Visivel
            {
                UpdateList();                
            }
        }

        void UpdateList()
        {
            var text = txtFind.Text.Trim();
            lstIndex.Clear();
            foreach (var i in GetNpcs())
                lstIndex.Add(Array.IndexOf(Npc.Items, i) + " - " + i.Name);    

            lstIndex.SelectIndex = 0;
        }

        IEnumerable<Npc> GetNpcs()
        {
            var text = txtFind.Text.Trim();
            return Npc.Items.Where(i => i.Name.Contains(text, StringComparison.CurrentCultureIgnoreCase));
        }

        /// <summary>
        /// Desenha nos paineis
        /// </summary>
        /// <param name="target"></param>
        /// <param name="panel"></param>
        /// <param name="Index"></param>
        private void Tabs_OnDrawTabPanel(RenderTarget target, Panel panel, int Index)
        {
            var gp = panel.GlobalPosition();
            var Size = panel.Size;
            if (Index == 0)
            {
                DrawText(target, words.GetText(0), 14, new Vector2(gp.x + 5, txtName.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(1), 14, new Vector2(gp.x + 5, cmbBehavior.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(2), 14, new Vector2(gp.x + 5, txtLevel.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(3), 14, new Vector2(gp.x + 5, cmbRange.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(4), 14, new Vector2(gp.x + 5, txtExp.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(6), 14, new Vector2(gp.x + 5, txtSpriteID.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(7), 14, new Vector2(gp.x + 5, txtScale.GlobalPosition().y), Color.White);


                var c = new Color(200, 200, 200);
                DrawLine(target, gp + new Vector2(5, 145), gp + new Vector2(Size.x - 5, 145), c);
                DrawText(target, words.GetText(5),20, gp + new Vector2(5, 150), Color.White);

                var rec_pos = gp + new Vector2(Size.x - 5 - 100, 175);
                DrawRectangle(target, rec_pos, new Vector2(100), new Color(0, 0, 0, 100), 1, new Color(80, 80, 80));
                if (txtSpriteID.Value > 0)
                {
                    var tex = GlobalResources.Character[(int)txtSpriteID.Value];
                    var size = tex.size / 4;

                    DrawTexture(target, tex, new Rectangle(rec_pos + new Vector2(50, 100), Vector2.Min(size, new Vector2(100))),
                        new Rectangle(Vector2.Zero, size), Color.White, new Vector2(size.x / 2, size.y));
                }
            }
            else if (Index == 1)
            {
                DrawText(target, words.GetText(8), 14, new Vector2(gp.x + 5, txtHP.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(9), 14, new Vector2(gp.x + 5, txtRegen.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(10), 14, new Vector2(gp.x + 5, txtDamage.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(11), 14, new Vector2(gp.x + 5, txtResistPhysic.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(12), 14, new Vector2(gp.x + 5, txtResistMagic.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(14), 14, new Vector2(gp.x + 5, txtAttackSpeed.GlobalPosition().y), Color.White);
                DrawText(target, words.GetText(15), 14, new Vector2(gp.x + 5, txtMoveSpeed.GlobalPosition().y), Color.White);
            }
        }
    }
}
