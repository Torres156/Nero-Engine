using Nero.Client.Scenes.GameplayComponents;
using Nero.Client.World.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Client.Helpers
{
    using static Renderer;
    class ChatHelper
    {
        public const int MAX_LINES = 50;

        public static List<ChatLine> Chat_Normal;
        public static List<ChatLine> Chat_System;

        /// <summary>
        /// Inicializa o chat
        /// </summary>
        public static void Initialize()
        {
            Chat_Normal = new List<ChatLine>();
            Chat_System = new List<ChatLine>();
        }

        /// <summary>
        /// Adiciona o chat
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="color"></param>
        public static void Add(ChatTypes type, string Text, Color color)
        {
            if (Chat_Normal == null || Chat_System == null)
                Initialize();

            var scrlNormal = Game.GetScene().FindControl<pChat>().scrlNormal;
            bool updateSelect = scrlNormal.Value == scrlNormal.Maximum;

            var lines = GetTextWrap(Text, 300 - 16);
            foreach (var i in lines)
            {
                // Chat normal
                if (Chat_Normal.Count < MAX_LINES)
                    Chat_Normal.Add(new ChatLine(i, color));
                else
                {
                    // Re-Organized lines
                    for (int l = 0; l < MAX_LINES - 1; l++)
                        Chat_Normal[l] = Chat_Normal[l + 1];

                    // Ultima linha
                    Chat_Normal[MAX_LINES - 1] = new ChatLine(i, color);
                }

                // Chat System
                if (type == ChatTypes.System)
                {
                    if (Chat_System.Count < MAX_LINES)
                        Chat_System.Add(new ChatLine(i, color));
                    else
                    {
                        // Re-Organized lines
                        for (int l = 0; l < MAX_LINES - 1; l++)
                            Chat_System[l] = Chat_Normal[l + 1];

                        // Ultima linha
                        Chat_System[MAX_LINES - 1] = new ChatLine(i, color);
                    }                    
                }
            }

            Game.GetScene().FindControl<pChat>().CheckScrollMaximum();

            if (updateSelect)
                scrlNormal.Value = scrlNormal.Maximum;
            Game.GetScene().FindControl<pChat>().GoSystemMaximum();
        }
    }
}
