using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    using static Renderer;
    public class Animation : Drawable
    {
        TextureAnimation animation;

        /// <summary>
        /// Frame Inicial
        /// </summary>
        public int Frame_Start = 0;

        /// <summary>
        /// Frame Final
        /// </summary>
        public int Frame_End = 0;

        /// <summary>
        /// Origem
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// Modo Blend
        /// </summary>
        public bool Blend = false;

        /// <summary>
        /// Posição de desenho
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Escala
        /// </summary>
        public float Scale = 1f;

        /// <summary>
        /// Cor de desenho
        /// </summary>
        public Color Color = Color.White;

        /// <summary>
        /// Velocidade de passar de frame
        /// </summary>
        public int Speed = 40;

        /// <summary>
        /// Repetição
        /// </summary>
        public int Repeat = 0;

        /// <summary>
        /// Repetição infinita
        /// </summary>
        public bool Repeat_Unlimited = false;

        /// <summary>
        /// Atual repetição
        /// </summary>
        int repeat_current = 0;

        /// <summary>
        /// Atual frame
        /// </summary>
        int Frame_current = 0;

        /// <summary>
        /// Tempo de frame
        /// </summary>
        int Frame_Timer = 0;

        /// <summary>
        /// Destroi a animação
        /// </summary>
        public bool Destroy { get; private set; } = false;

  
        /// <summary>
        /// Construtor
        /// </summary>
        public Animation()
        {

        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="animation"></param>
        public Animation(TextureAnimation animation)
        {
            SetTextureAnimation(animation);
        }

        public void SetTextureAnimation(TextureAnimation animation)
        {
            if (this.animation == animation) return;
            this.animation = animation;
            Frame_Timer = Environment.TickCount + Speed;
            Frame_current = 0;
            var texture = animation.Texture[Frame_current];

            Frame_End = animation.Texture.Length - 1;
        }

        /// <summary>
        /// Desenha a animação
        /// </summary>
        /// <param name="target"></param>
        /// <param name="states"></param>
        public void Draw(RenderTarget target, RenderStates states)
        {
            if (Destroy) return;
            Update();

            if (Frame_current > Frame_End) return;
            var texture = animation.Texture[Frame_current];
            DrawTexture(target, texture, new Rectangle(Position, texture.size * Scale),
                new Rectangle(Vector2.Zero, texture.size), Color, Origin, 0,
                Blend ? new RenderStates(BlendMode.Add) : RenderStates.Default);
        }

        /// <summary>
        /// Atualiza o desenho
        /// </summary>
        void Update()
        {
            if (Frame_current < Frame_Start) Frame_Start = 0;
            if (Frame_End >= animation.FrameCount) Frame_End = animation.FrameCount - 1; // Debug

            if (Environment.TickCount > Frame_Timer)
            {
                Frame_current++;
                if (Frame_current > Frame_End)
                {
                    if (Repeat_Unlimited)
                        Frame_current = Frame_Start;
                    else
                    {
                        if (Repeat == 0) // Sem repetição                    
                            Destroy = true;
                        else
                        {
                            repeat_current++;
                            if (repeat_current > Repeat)
                                Destroy = true;
                            else
                                Frame_current = Frame_Start;
                        }
                    }
                }
                Frame_Timer = Environment.TickCount + Speed;
            }
        }

        /// <summary>
        /// Reseta a animação
        /// </summary>
        public void Reset()
        {
            Destroy = false;
            Frame_current = Frame_Start;
            Frame_Timer = Environment.TickCount + Speed;
        }


    }
}
