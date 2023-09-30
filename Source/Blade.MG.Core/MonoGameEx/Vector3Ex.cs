namespace Microsoft.Xna.Framework
{
    public static class Vector3Ex
    {
        public static Vector3 Abs(this Vector3 vector3)
        {
            return new Vector3(Math.Abs(vector3.X), Math.Abs(vector3.Y), Math.Abs(vector3.Z));
        }

        public static Vector2 ToVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.X, vector3.Y);
        }

        //public static Vector3 operator +(this Vector3 value)
        //{
        //    return value;
        //}

    }
}
