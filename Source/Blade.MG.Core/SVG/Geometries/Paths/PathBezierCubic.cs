using Microsoft.Xna.Framework;

namespace Blade.MG.SVG.Geometries.Paths
{
    public class PathBezierCubic : PathBezierBase
    {
        public PathBezierCubic()
        {

        }

        public PathBezierCubic(bool isRelative, Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint)
        {
            IsRelative = isRelative;
            ControlPoints.Add(controlPoint1);
            ControlPoints.Add(controlPoint2);
            EndPoint = endPoint;
        }
    }
}
