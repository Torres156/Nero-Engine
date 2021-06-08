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


        // Client Only
        public Vector2 OffSet;
        public bool Moving = false;


        // Privates
        int FrameStep = 0;


        /// <summary>
        /// Construtor
        /// </summary>
        public Character()
        {

        }

        /// <summary>
        /// Desenha o personagem
        /// </summary>
        /// <param name="target"></param>
        public void Draw(RenderTarget target)
        {
            var tex = GlobalResources.Character[SpriteID];
            var size = tex.size / 4;
            var pos = Position * 32 + OffSet + new Vector2(16);
            var origin = new Vector2(size.x / 2, size.y);

            // Frames no eixo Y
            int framey = 0; 
            switch(Direction)
            {
                case Directions.Up: framey = 3; break;
                case Directions.Left: framey = 1; break;
                case Directions.Right: framey = 2; break;
            }

            // Frames no eixo X
            int framex = 0; // Parado
            if (Moving && (OffSet.x <= -12 || OffSet.y <= -12 || OffSet.x >= 12 || OffSet.y >= 12))
                framex = 1 + FrameStep * 2;

            var source = new Rectangle(new Vector2(framex, framey) * size, size);
            var dest = new Rectangle(pos, size);
            DrawTexture(target, tex, dest, source, Color.White, origin);            
        }

        /// <summary>
        /// Desenha os textos
        /// </summary>
        /// <param name="target"></param>
        public void DrawTexts(RenderTarget target)
        {
            var tex = GlobalResources.Character[SpriteID];
            var size = tex.size / 4;
            var pos = Position * 32 + OffSet.Floor() + new Vector2(16);
            var colorName = Color.White;

            switch(AccessLevel)
            {
                case AccessLevels.GameMaster: colorName = new Color(254, 131, 65); break;
                case AccessLevels.Administrator: colorName = new Color(108, 181, 246, 255); break;
            }

            DrawText(target, Name, 14, pos - new Vector2(GetTextWidth(Name,14) / 2, size.y + 20), colorName, 1, new Color(30,30,30));            
        }

        /// <summary>
        ///  Atualiza o personagem
        /// </summary>
        public void Update()
        {
            ProcessMovement();
        }

        /// <summary>
        /// Processa o movimento
        /// </summary>
        void ProcessMovement()
        {
            if (!Moving)
                return;

            // Velocidade
            float speed = 200 * Game.DeltaTime;
            

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
            }

        }
    }
}

