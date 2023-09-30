using System.Runtime.CompilerServices;

namespace Microsoft.Xna.Framework
{
    public static class RayEx
    {
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Vector3 Point(this Ray ray, float t)
        {
            return new Vector3(ray.Position.X + t * ray.Direction.X, ray.Position.Y + t * ray.Direction.Y, ray.Position.Z + t * ray.Direction.Z);
        }

    }
}
