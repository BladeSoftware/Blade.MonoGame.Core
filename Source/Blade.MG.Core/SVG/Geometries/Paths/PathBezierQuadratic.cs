using Microsoft.Xna.Framework;

namespace Blade.MG.SVG.Geometries.Paths
{
    public class PathBezierQuadratic : PathBezierBase
    {
        public PathBezierQuadratic()
        {

        }

        public PathBezierQuadratic(bool isRelative, Vector2 controlPoint, Vector2 endPoint)
        {
            IsRelative = isRelative;
            ControlPoints.Add(controlPoint);
            EndPoint = endPoint;

        }
    }
}
