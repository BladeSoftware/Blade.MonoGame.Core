using Microsoft.Xna.Framework;

namespace Blade.MG.Core.Primitives
{
    public class RectangleF
    {
        public float MinX;
        public float MaxX;
        public float MinY;
        public float MaxY;

        public float CenterX => (MinX + MaxX) / 2f;
        public float CenterY => (MinY + MaxY) / 2f;

        public RectangleF()
        {

        }

        public void Union(Vector2 vector2)
        {
            if (vector2.X < MinX) MinX = vector2.X;
            if (vector2.X > MaxX) MaxX = vector2.X;
            if (vector2.Y < MinY) MinY = vector2.Y;
            if (vector2.Y > MaxY) MaxY = vector2.Y;
        }

        public void Union(float x, float y)
        {
            if (x < MinX) MinX = x;
            if (x > MaxX) MaxX = x;
            if (y < MinY) MinY = y;
            if (y > MaxY) MaxY = y;
        }

        public Rectangle ToRectangle()
        {
            int minX = (int)Math.Floor(MinX);
            int maxX = (int)Math.Floor(MaxX);
            int minY = (int)Math.Floor(MinY);
            int maxY = (int)Math.Floor(MaxY);

            return new Rectangle(minX, minY, (maxX - minX + 1), (maxY - minY + 1));
        }
    }

}
