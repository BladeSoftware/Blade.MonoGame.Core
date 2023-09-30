using Microsoft.Xna.Framework;

namespace Blade.MG.Core.Primitives
{
    public class Edge2D
    {
        public Vector2 P1;
        public Vector2 P2;

        public float Dx;
        public float Dy;

        public double XStep;
        public int Winding;
        public bool isHorizontal;

        public float CurrentX;


        public Edge2D(Vector2 p1, Vector2 p2)
        {
            //p1 = new Vector2((float)Math.Round(p1.X), (float)Math.Round(p1.Y));
            //p2 = new Vector2((float)Math.Round(p2.X), (float)Math.Round(p2.Y));

            if (p1.Y <= p2.Y)
            {
                Winding = 1;
            }
            else
            {
                Winding = -1;
            }

            // Make sure Points are Sorted by Y Ascending
            if (p2.Y < p1.Y)
            {
                P1 = p2;
                P2 = p1;
            }
            else
            {
                P1 = p1;
                P2 = p2;
            }

            Dx = P2.X - P1.X;
            Dy = P2.Y - P1.Y;

            XStep = Dx / Dy;

            //int p1x = (int)P1.X;
            //int p2x = (int)P2.X;
            int p1y = (int)P1.Y;
            int p2y = (int)P2.Y;

            isHorizontal = (p1y == p2y);

            // isPixel = isHorizontal && p1x==p2x;
        }

        public float FnX(float y)
        {
            if (y < P1.Y || y > P2.Y)
            {
                return float.NaN;
            }

            CurrentX = P1.X + (float)((double)(y - P1.Y) * XStep);

            return CurrentX;
        }

        public override string ToString()
        {
            return $"({P1.X.ToString("F2")}, {P1.Y.ToString("F2")})-({P2.X.ToString("F2")}, {P2.Y.ToString("F2")}) : CurrentX={CurrentX.ToString("F2")} : Winding={Winding} : IsHorizontal={isHorizontal}";
        }
    }
}
