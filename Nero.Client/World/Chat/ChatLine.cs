using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero.Client.World.Chat
{
    struct ChatLine
    {
        public Color Color;
        public string Text;

        public ChatLine(string Text, Color Color)
        {
            this.Text = Text;
            this.Color = Color;
        }
    }
}
