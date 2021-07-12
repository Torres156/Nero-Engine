using Nero.Client.World;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nero.Client.Player
{
    using static Renderer;
    class Character
    {
        #region Static
        public static Character My;
        public static List<Character> Items = new List<Character>();

        /// <summary>
        /// Encontra um personagem
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Character Find(string name)
        {
            if (My.Name.Trim().ToLower() == name.Trim().ToLower())
                return My;

            return Items.Find(i => i.Name.Trim().ToLower() == name.Trim().ToLower());
        }
        #endregion

        // Publics
        public string Name = "";                                        // Nome
        public int ClassID = 0;                                         // Id da classe
        public int SpriteID = 0;                                        // Aparência
        public int Level = 1;                                           // Nível do personagem
        public long Experience = 0;                                     // Experiência
        public int[] StatPrimary = new int[(int)StatPrimaries.count];   // Atributos primários
        public Directions Direction = Directions.Down;                  // Direção do personagem
        public int Points = 0;                                          // Pontos de atributos
        public int MapID = 0;                                           // Id do mapa
        public Vector2 Position = Vector2.Zero;                         // Posição
        public AccessLevels AccessLevel = AccessLevels.Player;          // Acesso de administrador
        public int[] Vital;                                             // Atributos vitais

        // Client Only
        public Vector2 OffSet;
        public bool Moving = false;
        public long timerAttack;


        // Privates
        int FrameStep = 0;
        long timerFrame = 0;
        SpriteAnimation animation;       


        /// <summary>
        /// Construtor
        /// </summary>
        public Character()
        {
            Vital = new int[(int)Vitals.count];
        }

        /// <summary>
        /// Desenha o personagem
        /// </summary>
        /// <param name="target"></param>
        public void Draw(RenderTarget target)
        {
            if (animation == null)
                CreateAnimation();

            var pos = Position * 8 + OffSet + new Vector2(4);            
            string[] dirs = { "up", "down", "left", "right" };
            string key = "normal_" + dirs[(int)Direction];
            if (timerFrame > 0)
                key = "move_" + dirs[(int)Direction];

            // Ataque
            if (timerAttack > 0 && timerAttack - Environment.TickCount64 > 500)
                    key = "attack_" + dirs[(int)Direction];

            animation.Position = pos.Floor();
            animation.Play(target, key);
        }

        /// <summary>
        /// Desenha os textos
        /// </summary>
        /// <param name="target"></param>
        public void DrawTexts(RenderTarget target)
        {
            var tex = GlobalResources.Character[SpriteID];
            var size = tex.size / 4;
            var pos = Position * 8 + OffSet.Floor() + new Vector2(4);
            var colorName = Color.White;

            switch (AccessLevel)
            {
                case AccessLevels.GameMaster: colorName = new Color(254, 131, 65); break;
                case AccessLevels.Administrator: colorName = new Color(108, 181, 246, 255); break;
            }

            DrawText(target, Name, 14, pos - new Vector2(GetTextWidth(Name, 14) / 2, size.y + 20), colorName, 1, new Color(30, 30, 30));
        }

        /// <summary>
        /// Cria a animação
        /// </summary>
        void CreateAnimation()
        {
            var tex = GlobalResources.Character[SpriteID];
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


            // Attack
            animation.Add("attack_up", new Rectangle(new Vector2(size.x * 1, size.y * 3), size));
            animation.Add("attack_down", new Rectangle(new Vector2(size.x * 1, size.y * 0), size));
            animation.Add("attack_left", new Rectangle(new Vector2(size.x * 1, size.y * 1), size));
            animation.Add("attack_right", new Rectangle(new Vector2(size.x * 1, size.y * 2), size));
        }

        /// <summary>
        ///  Atualiza o personagem
        /// </summary>
        public void Update()
        {
            ProcessMovement();

            if (timerFrame > 0 && Environment.TickCount64 > timerFrame)
                timerFrame = 0;

            if (timerAttack > 0 && Environment.TickCount64 > timerAttack)            
                timerAttack = 0;
        }

        /// <summary>
        /// Processa o movimento
        /// </summary>
        void ProcessMovement()
        {
            if (!Moving)
                return;

            // Velocidade
            float speed = 200 * Game.FixedTime;

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

