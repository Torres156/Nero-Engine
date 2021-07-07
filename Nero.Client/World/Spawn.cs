using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Client.World
{
    using static Renderer;
    class Spawn
    {
        #region Static
        public static Spawn[] Items;
        #endregion


        public int NpcID { get; set; }
        public Vector2 Position { get; set; }
        public long HP { get; set; }
        public Directions Direction { get; set; }

        // Client Only
        SpriteAnimation animation;
        public Vector2 OffSet;
        public bool Moving;
        long timerFrame;
        int FrameStep;

        /// <summary>
        /// Desenha o Spawn
        /// </summary>
        /// <param name="target"></param>
        public void Draw(RenderTarget target)
        {
            if (Npc.Items[NpcID].SpriteID == 0)
                return;

            if (animation == null)
                CreateAnimation();

            var pos = Position * 8 + OffSet + new Vector2(4);
            string[] dirs = { "up", "down", "left", "right" };
            string key = "normal_" + dirs[(int)Direction];
            if (timerFrame > 0)
                key = "move_" + dirs[(int)Direction];

            animation.Scale = Npc.Items[NpcID].Scale / 100f;
            animation.Position = pos.Floor();
            animation.Play(target, key);
        }

        /// <summary>
        /// Desenha os textos
        /// </summary>
        /// <param name="target"></param>
        public void DrawTexts(RenderTarget target)
        {
            if (Npc.Items[NpcID].SpriteID == 0)
                return;
            var n = Npc.Items[NpcID];
            var tex = GlobalResources.Character[n.SpriteID];
            var size = tex.size / 4;
            var pos = Position * 8 + OffSet.Floor() + new Vector2(4);
            var colorName = Color.White;            

            DrawText(target, n.Name, 14, pos - new Vector2(GetTextWidth(n.Name, 14) / 2, size.y + 20), colorName, 1, new Color(30, 30, 30));
        }

        /// <summary>
        /// Cria a animação
        /// </summary>
        void CreateAnimation()
        {
            if (Npc.Items[NpcID].SpriteID == 0)
                return;
            var tex = GlobalResources.Character[Npc.Items[NpcID].SpriteID];
            var size = tex.size / 4;
            animation = new SpriteAnimation(tex);
            animation.origin = new Vector2(size.x / 2, size.y);
            animation.repeat = true;
            animation.frame_timer = 150;

            // Normal
            animation.Add("normal_up", new Rectangle(new Vector2(0, size.y * 3), size));
            animation.Add("normal_down", new Rectangle(new Vector2(0, size.y * 0), size));
            animation.Add("normal_left", new Rectangle(new Vector2(0, size.y * 1), size));
            animation.Add("normal_right", new Rectangle(new Vector2(0, size.y * 2), size));

            // Move
            animation.Add("move_up", new Rectangle(new Vector2(size.x * 1, size.y * 3), size),
                new Rectangle(new Vector2(size.x * 3, size.y * 3), size));
            animation.Add("move_down", new Rectangle(new Vector2(size.x * 1, size.y * 0), size),
                new Rectangle(new Vector2(size.x * 3, size.y * 0), size));
            animation.Add("move_left", new Rectangle(new Vector2(size.x * 1, size.y * 1), size),
                new Rectangle(new Vector2(size.x * 3, size.y * 1), size));
            animation.Add("move_right", new Rectangle(new Vector2(size.x * 1, size.y * 2), size),
                new Rectangle(new Vector2(size.x * 3, size.y * 2), size));
        }

        /// <summary>
        ///  Atualiza o spawn
        /// </summary>
        public void Update()
        {
            ProcessMovement();

            if (timerFrame > 0 && Environment.TickCount64 > timerFrame)
                timerFrame = 0;
        }

        /// <summary>
        /// Processa o movimento
        /// </summary>
        void ProcessMovement()
        {
            if (!Moving)
                return;

            // Velocidade
            var n = Npc.Items[NpcID];
            float speed = n.MoveSpeed * Game.DeltaTime;

            if (OffSet.x > 0)
                OffSet.x = Math.Max(0, OffSet.x - speed);
            if (OffSet.x < 0)
                OffSet.x = Math.Min(0, OffSet.x + speed);

            if (OffSet.y > 0)
                OffSet.y = Math.Max(0, OffSet.y - speed);
            if (OffSet.y < 0)
                OffSet.y = Math.Min(0, OffSet.y + speed);

            if (OffSet == Vector2.Zero)
            {
                FrameStep++;
                if (FrameStep > 1) FrameStep = 0;
                Moving = false;
                timerFrame = Environment.TickCount64 + 150;
            }

        }
    }
}
