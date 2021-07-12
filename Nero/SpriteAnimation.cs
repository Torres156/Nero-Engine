using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    public class SpriteAnimation
    {
        Dictionary<string, Rectangle[]> collection;
        public int frame_timer = 150;
        public Vector2 Position = Vector2.Zero;
        public Vector2 origin = Vector2.Zero;
        public bool repeat = true;
        public Color color = Color.White;
        public float rotation = 0f;
        public float Scale = 1f;

        /// <summary>
        /// Atual chave
        /// </summary>
        public string CurrentKey { get; private set; }

        int frame_current = 0;
        int frame_currenttimer = 0;
        string frame_key = "";
        public Texture texture { get; private set; }

        /// <summary>
        /// Construtor
        /// </summary>
        public SpriteAnimation(Texture texture)
        {
            collection = new Dictionary<string, Rectangle[]>();
            this.texture = texture;
        }

        /// <summary>
        /// Executa a animação
        /// </summary>
        /// <param name="target"></param>
        /// <param name="key"></param>
        public void Play(RenderTarget target, string key, bool random = false)
        {
            if (!collection.ContainsKey(key))
                return;

            // Reseta caso seja outra animação
            if (frame_key != key)
            {
                frame_current = 0;
                frame_key = key;
            }

            var frames = collection[frame_key];

            // Reset
            if (frame_current >= frames.Length)
            {
                if (repeat)
                    frame_current = 0;
                else
                    return;
            }

            CurrentKey = key;

            // Desenha a sprite
            Renderer.DrawTexture(target, texture, new Rectangle(Position, frames[frame_current].size * Scale), frames[frame_current], color, origin, rotation,
                RenderStates.Default);

            // Processa os frames
            if (Environment.TickCount > frame_currenttimer)
            {
                frame_current++;
                frame_currenttimer = Environment.TickCount + frame_timer;
            }
        }

        public void Add(string key_name, params Rectangle[] rectangles)
            => collection.Add(key_name, rectangles);


        public bool HasEnded()
        {
            if (!collection.ContainsKey(frame_key))
                return true;

            var frames = collection[frame_key];

            // Reset
            if (frame_current >= frames.Length)
                return true;

            return false;
        }

        public void Reset()
        {
            frame_current = 0;
            frame_currenttimer = Environment.TickCount + frame_timer;
        }
    }
}
