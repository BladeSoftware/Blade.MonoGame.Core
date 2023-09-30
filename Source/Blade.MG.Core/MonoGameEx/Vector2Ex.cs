namespace Microsoft.Xna.Framework
{
    public static class Vector2Ex
    {
        public static Vector2 Abs(this Vector2 vector2)
        {
            return new Vector2(Math.Abs(vector2.X), Math.Abs(vector2.Y));
        }

        public static Vector2 Perpendicular(Vector2 vector2)
        {
            return new Vector2(-vector2.Y, vector2.X);
        }


        public static Vector2 Cross(Vector2 v, float a)
        {
            return new Vector2(a * v.Y, -a * v.X);
        }

        public static Vector2 Cross(float a, Vector2 v)
        {
            return new Vector2(-a * v.Y, a * v.X);
        }


        /// <summary>
        /// The 2D cross product, unlike the 3D version, does not return a vector but a scalar. 
        /// This scalar value actually represents the magnitude of the orthogonal vector along the z-axis, if the cross product were to actually be performed in 3D.
        /// 
        /// https://gamedevelopment.tutsplus.com/tutorials/how-to-create-a-custom-2d-physics-engine-oriented-rigid-bodies--gamedev-8032
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Cross(Vector2 a, Vector2 b)
        {
            // Vector2.Dot(Vector2Ex.Perpendicular(a), b);
            return a.X * b.Y - a.Y * b.X;
        }

    }
}
