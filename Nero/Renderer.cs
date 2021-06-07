using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nero
{
    using SFML.Graphics;
    public static class Renderer
    {
        static readonly Sprite _sprite = new Sprite();
        static readonly LargeSprite _spritelarge = new LargeSprite();
        static readonly RectangleShape rec = new RectangleShape();
        static readonly CircleShape cir = new CircleShape();
        static readonly RoundedRectangle roundrec = new RoundedRectangle();
        static Vertex[] lines = new Vertex[2];
        static Vertex[] gradients = new Vertex[4];


        internal static Font gameFont;        
        internal static Text _text;
        internal static Texture Shadow;

        /// <summary>
        /// Desenha a textura
        /// </summary>
        /// <param name="target"></param>
        /// <param name="texture"></param>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="origin"></param>
        /// <param name="color"></param>
        /// <param name="states"></param>
        public static void DrawTexture(RenderTarget target, Texture texture, Rectangle destination, Rectangle source, Color color, Vector2 origin, float rotation, RenderStates states)
        {
            if (texture == null) return;

            if (texture.type == TextureTypes.Normal)
            {
                var scale = destination.size / source.size;

                _sprite.Texture = texture.GetTexture();
                _sprite.Position = destination.position.Round();
                _sprite.Scale = scale;
                _sprite.TextureRect = (IntRect)source;
                _sprite.Color = color;
                _sprite.Origin = origin;
                _sprite.Rotation = rotation;
                target.Draw(_sprite, states);
            }
            else
            {
                var scale = destination.size / source.size;

                _spritelarge.Texture = texture.GetLargeTexture();
                _spritelarge.Position = destination.position.Round();
                _spritelarge.Scale = scale;
                _spritelarge.Color = color;
                _spritelarge.Origin = origin;
                _spritelarge.Rotation = rotation;
                target.Draw(_spritelarge, states);
            }
        }

        /// <summary>
        /// Desenha a textura
        /// </summary>
        /// <param name="target"></param>
        /// <param name="texture"></param>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="color"></param>
        /// <param name="origin"></param>
        /// <param name="rotation"></param>
        public static void DrawTexture(RenderTarget target, Texture texture, Rectangle destination, Rectangle source, Color color, Vector2 origin, float rotation)
            => DrawTexture(target, texture, destination, source, color, origin, rotation, RenderStates.Default);

        /// <summary>
        /// Desenha a textura
        /// </summary>
        /// <param name="target"></param>
        /// <param name="texture"></param>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="color"></param>
        /// <param name="origin"></param>
        public static void DrawTexture(RenderTarget target, Texture texture, Rectangle destination, Rectangle source, Color color, Vector2 origin)
            => DrawTexture(target, texture, destination, source, color, origin, 0);

        /// <summary>
        /// Desenha a textura
        /// </summary>
        /// <param name="target"></param>
        /// <param name="texture"></param>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="color"></param>
        public static void DrawTexture(RenderTarget target, Texture texture, Rectangle destination, Rectangle source, Color color)
            => DrawTexture(target, texture, destination, source, color, Vector2.Zero);

        /// <summary>
        /// Desenha a textura
        /// </summary>
        /// <param name="target"></param>
        /// <param name="texture"></param>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        public static void DrawTexture(RenderTarget target, Texture texture, Rectangle destination, Rectangle source)
            => DrawTexture(target, texture, destination, source, Color.White);

        /// <summary>
        /// Desenha a textura
        /// </summary>
        /// <param name="target"></param>
        /// <param name="texture"></param>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        public static void DrawTexture(RenderTarget target, Texture texture, Rectangle destination)
            => DrawTexture(target, texture, destination, new Rectangle(Vector2.Zero, texture.size), Color.White);

        /// <summary>
        /// Desenha a textura
        /// </summary>
        /// <param name="target"></param>
        /// <param name="texture"></param>
        /// <param name="destination"></param>
        public static void DrawTexture(RenderTarget target, Texture texture, Rectangle destination, Color color)
            => DrawTexture(target, texture, destination, new Rectangle(Vector2.Zero, texture.size), color);

        /// <summary>
        /// Desenha a textura
        /// </summary>
        /// <param name="target"></param>
        /// <param name="texture"></param>
        public static void DrawTexture(RenderTarget target, Texture texture, Vector2 position)
            => DrawTexture(target, texture, new Rectangle(position, texture.size));

        /// <summary>
        /// Desenha a textura
        /// </summary>
        /// <param name="target"></param>
        /// <param name="texture"></param>
        public static void DrawTexture(RenderTarget target, Texture texture, Vector2 position, Color color)
            => DrawTexture(target, texture, new Rectangle(position, texture.size), color);

        /// <summary>
        /// Desenha o texto
        /// </summary>
        /// <param name="target"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="outlineThickness"></param>
        /// <param name="outlineColor"></param>
        public static void DrawText(RenderTarget target, string text, int charactersize, Vector2 position, Color color, float outlineThickness, Color outlineColor)
        {
            if (_text == null)
                throw new Exception("A font não foi carregada!");

            if (text == null || text.Trim().Length == 0)
                return;

            _text.DisplayedString = text;
            _text.CharacterSize = (uint)charactersize;
            _text.Position = position.ToInt();
            _text.FillColor = color;
            _text.OutlineThickness = outlineThickness;
            _text.OutlineColor = outlineColor;
            target.Draw(_text);
        }

        /// <summary>
        /// Desenha o texto
        /// </summary>
        /// <param name="target"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        public static void DrawText(RenderTarget target, string text, int charactersize, Vector2 position, Color color)
            => DrawText(target, text, charactersize, position, color, 0, Color.Black);

        /// <summary>
        /// Desenha um degrade - QUADS vertex
        /// </summary>
        /// <param name="target"></param>
        /// <param name="pos"></param>
        /// <param name="color"></param>
        public static void DrawGradient(RenderTarget target, Vector2[] pos, Color[] color)
        {
            for (int i = 0; i < 4; i++)
                gradients[i] = new Vertex(pos[i], color[i]);

            target.Draw(gradients, PrimitiveType.Quads);
        }

        /// <summary>
        /// Desenha um degrade - QUADS vertex
        /// </summary>
        /// <param name="target"></param>
        /// <param name="pos1"></param>
        /// <param name="color1"></param>
        /// <param name="pos2"></param>
        /// <param name="color2"></param>
        /// <param name="pos3"></param>
        /// <param name="color3"></param>
        /// <param name="pos4"></param>
        /// <param name="color4"></param>
        public static void DrawGradient(RenderTarget target, Vector2 pos1, Color color1, Vector2 pos2, Color color2, Vector2 pos3, Color color3,
            Vector2 pos4, Color color4)
            => DrawGradient(target, new Vector2[] { pos1, pos2, pos3, pos4 }, new Color[] { color1, color2, color3, color4 });

        /// <summary>
        /// Desenha uma linha
        /// </summary>
        /// <param name="target"></param>
        /// <param name="pos1"></param>
        /// <param name="color1"></param>
        /// <param name="pos2"></param>
        /// <param name="color2"></param>
        public static void DrawLine(RenderTarget target, Vector2 pos1, Color color1, Vector2 pos2, Color color2)
        {
            lines[0] = new Vertex(pos1, color1);
            lines[1] = new Vertex(pos2, color2);
            target.Draw(lines, PrimitiveType.Lines);
        }

        /// <summary>
        /// Desenha uma linha
        /// </summary>
        /// <param name="target"></param>
        /// <param name="pos1"></param>
        /// <param name="pos2"></param>
        /// <param name="color"></param>
        public static void DrawLine(RenderTarget target, Vector2 pos1, Vector2 pos2, Color color)
            => DrawLine(target, pos1, color, pos2, color);

        /// <summary>
        /// Desenha um retângulo
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="fillColor"></param>
        /// <param name="outlineThickness"></param>
        /// <param name="outlineColor"></param>
        public static void DrawRectangle(RenderTarget target, Vector2 position, Vector2 size, Color fillColor, float outlineThickness, Color outlineColor)
        {
            rec.Position = position.Floor();
            rec.Size = size;
            rec.FillColor = fillColor;
            rec.OutlineThickness = outlineThickness;
            rec.OutlineColor = outlineColor;
            target.Draw(rec);
        }

        /// <summary>
        /// Desenha um retângulo
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="fillColor"></param>
        public static void DrawRectangle(RenderTarget target, Vector2 position, Vector2 size, Color fillColor)
            => DrawRectangle(target, position, size, fillColor, 0, Color.Transparent);

        /// <summary>
        /// Desenha um retângulo arredondado
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="fillColor"></param>
        /// <param name="outlineThickness"></param>
        /// <param name="outlineColor"></param>
        public static void DrawRoundedRectangle(RenderTarget target, Vector2 position, Vector2 size, Color fillColor, float radius, uint pointcount, float outlineThickness, Color outlineColor)
        {
            roundrec.Position = position.Floor();
            roundrec.Size = size;
            roundrec.FillColor = fillColor;
            roundrec.Radius = radius;
            roundrec.cornerPointCount = pointcount;
            roundrec.OutlineThickness = outlineThickness;
            roundrec.OutlineColor = outlineColor;
            target.Draw(roundrec);
        }

        /// <summary>
        /// Desenha um retângulo arredondado
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <param name="fillColor"></param>
        /// <param name="radius"></param>
        /// <param name="pointcount"></param>
        public static void DrawRoundedRectangle(RenderTarget target, Vector2 position, Vector2 size, Color fillColor, float radius, uint pointcount)
            => DrawRoundedRectangle(target, position, size, fillColor, radius, pointcount, 0, Color.Transparent);

        /// <summary>
        /// Desenha um circulo
        /// </summary>
        /// <param name=""></param>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <param name="fillColor"></param>
        /// <param name="outlineThickness"></param>
        /// <param name="outlineColor"></param>
        public static void DrawCircle(RenderTarget target, Vector2 position, float radius, Color fillColor, float outlineThickness, Color outlineColor)
        {
            cir.SetPointCount(360);
            cir.Position = position.Floor();
            cir.Radius = radius;
            cir.Origin = new Vector2(radius);
            cir.FillColor = fillColor;
            cir.OutlineThickness = outlineThickness;
            cir.OutlineColor = outlineColor;
            target.Draw(cir);
        }

        /// <summary>
        /// Desenha a sombra para um retângulo
        /// </summary>
        /// <param name="target"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        public static void DrawShadowRectangle(RenderTarget target, Vector2 position, Vector2 size, byte opacity = 255, float scale = 0.4f)
        {
            if (opacity == 0 || scale == 0) return;
            if (Shadow == null) return;

            var texsize = (Shadow.size / 3);
            var sizescale = texsize * scale;

            // TOP LEFT
            DrawTexture(target, Shadow, new Rectangle(position - sizescale, sizescale),
                new Rectangle(Vector2.Zero, texsize), new Color(255, 255, 255, opacity));

            // TOP CENTER
            DrawTexture(target, Shadow, new Rectangle(position - new Vector2(0, sizescale.y) , new Vector2(size.x, sizescale.y)),
                new Rectangle(new Vector2(texsize.x, 0), texsize), new Color(255, 255, 255, opacity));

            // TOP RIGHT
            DrawTexture(target, Shadow, new Rectangle(position + new Vector2(size.x, -sizescale.y), sizescale),
                new Rectangle(new Vector2(texsize.x * 2, 0), texsize), new Color(255, 255, 255, opacity));

            // LEFT
            DrawTexture(target, Shadow, new Rectangle(position + new Vector2(-sizescale.x, 0), new Vector2(sizescale.x, size.y)),
                new Rectangle(new Vector2(0, texsize.y), texsize), new Color(255, 255, 255, opacity));

            // LEFT
            DrawTexture(target, Shadow, new Rectangle(position + new Vector2(size.x, 0), new Vector2(sizescale.x, size.y)),
                new Rectangle(new Vector2(texsize.x * 2, texsize.y), texsize), new Color(255, 255, 255, opacity));

            // BOTTOM LEFT
            DrawTexture(target, Shadow, new Rectangle(position + new Vector2(-sizescale.x, size.y), sizescale),
                new Rectangle(new Vector2(0, texsize.y * 2), texsize), new Color(255, 255, 255, opacity));

            // BOTTOM
            DrawTexture(target, Shadow, new Rectangle(position + new Vector2(0, size.y), new Vector2(size.x, sizescale.y)),
                new Rectangle(new Vector2(texsize.x, texsize.y * 2), texsize), new Color(255, 255, 255, opacity));

            // BOTTOM RIGHT
            DrawTexture(target, Shadow, new Rectangle(position + new Vector2(size.x, size.y), sizescale),
                new Rectangle(texsize * 2, texsize), new Color(255, 255, 255, opacity));

        }

        /// <summary>
        /// Comprimento do Texto
        /// </summary>
        /// <param name="text"></param>
        /// <param name="characterSize"></param>
        /// <returns></returns>
        public static float GetTextWidth(string text, uint characterSize = 12)
        {
            if (text.Trim().Length == 0)
                return 0;

            _text.CharacterSize = characterSize;
            _text.DisplayedString = text;

            return _text.FindCharacterPos((uint)text.Length).X.Floor();
        }

        /// <summary>
        /// Word Wrap
        /// </summary>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="characterSize"></param>
        /// <returns></returns>
        public static string[] GetWordWrap(string text, int width, uint characterSize = 12)
        {
            var collection = new List<string>();
            if (text.Length > 0)
            {
                var words = text.Split();
                string line = "";
                foreach (var i in words)
                {
                    if (i == "[n]")
                    {
                        collection.Add(line.Trim());
                        line = "";
                        continue;
                    }

                    if (GetTextWidth(line + i, characterSize) > width)
                    {
                        collection.Add(line.Trim());
                        line = i + " ";
                    }
                    else
                        line += i + " ";
                }
                if (line.Length > 0) collection.Add(line.Trim());
            }
            return collection.ToArray();
        }
    }
}
