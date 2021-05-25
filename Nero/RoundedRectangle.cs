using System;
using System.Collections.Generic;
using System.Text;

namespace Nero
{
    public class RoundedRectangle : SFML.Graphics.Shape
    {
        public Vector2 Size
        {
            get => size;
            set
            {
                if (size != value)
                {
                    size = value;
                    Update();
                }
            }
        }
        Vector2 size;

        public float Radius
        {
            get => radius;
            set
            {
                if (radius != value)
                {
                    radius = value;
                    Update();
                }
            }
        }
        float radius = 0f;

        public uint cornerPointCount
        {
            get => cornerpoint;
            set
            {
                if (cornerpoint != value)
                {
                    cornerpoint = value;
                    Update();
                }
            }
        }
        uint cornerpoint = 6;

        public RoundedRectangle()
        { }

        public RoundedRectangle(Vector2 Size, float radius, uint cornerPointCount)
        {
            this.Size = Size;
            this.Radius = radius;
            this.cornerPointCount = cornerPointCount;
        }

        public override Vector2 GetPoint(uint index)
        {
            if (index >= GetPointCount())
                return new Vector2(0);

            float deltaAngle = 90f / (cornerpoint - 1);
            var center = new Vector2();
            uint centerIndex = index / cornerpoint;

            switch (centerIndex)
            {
                case 0: center.x = size.x - radius; center.y = radius; break;
                case 1: center.x = radius; center.y = radius; break;
                case 2: center.x = radius; center.y = size.y - radius; break;
                case 3: center.x = size.x - radius; center.y = size.y - radius; break;
            }

            return new Vector2(radius * MathF.Cos(deltaAngle * (index - centerIndex) * MathF.PI / 180) + center.x,
                -radius * MathF.Sin(deltaAngle * (index - centerIndex) * MathF.PI / 180) + center.y);
        }

        public override uint GetPointCount()
        {
            return cornerpoint * 4;
        }
    }
}
