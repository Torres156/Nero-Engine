using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Client
{
    using static Renderer;
    class FloatMessage
    {
        const int TIMER_DELETE = 3000; // 3 segundos

        #region Static
        public static List<FloatMessage> messages;
        static List<FloatMessage> deleteMessages = new List<FloatMessage>();

        /// <summary>
        /// Atualiza as mensagens
        /// </summary>
        public static void Update()
        {
            foreach (var i in messages)
                if (Environment.TickCount64 > i.timerDelete)
                    deleteMessages.Add(i);
                else
                    i.Position = new Vector2(i.Position.x, i.Position.y - 40 * Game.DeltaTime);

            // Deleta as mensagens
            if (deleteMessages.Count > 0)
            {
                foreach (var i in deleteMessages)                
                    if (messages.Contains(i))
                        messages.Remove(i);

                deleteMessages.Clear();
            }            
        }

        /// <summary>
        /// Desenha as mensagens
        /// </summary>
        /// <param name="target"></param>
        public static void Draw(RenderTarget target)
        {
            foreach (var i in messages)
                if (i.timerDelete > 0)
                    DrawText(target, i.Text, 14, i.Position - new Vector2(GetTextWidth(i.Text, 14), 7), i.Color,1, new Color(30,30,30));
        }

        /// <summary>
        /// Adiciona uma nova mensagem
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="position"></param>
        public static void Add(string text, Color color, Vector2 position)
        {
            var m = new FloatMessage();
            m.Text = text;
            m.Color = color;
            m.Position = position;
            m.timerDelete = Environment.TickCount64 + TIMER_DELETE;
            messages?.Add(m);
        }
        #endregion

        public string Text { get; private set; } = "";
        public Vector2 Position { get; private set; }
        public Color Color { get; private set; } = Color.White;

        // Client only
        long timerDelete;
    }
}
