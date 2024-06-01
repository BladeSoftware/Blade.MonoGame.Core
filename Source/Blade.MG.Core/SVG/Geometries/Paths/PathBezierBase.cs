using Microsoft.Xna.Framework;

namespace Blade.MG.SVG.Geometries.Paths
{
    public abstract class PathBezierBase : PathCommand
    {
        public List<Vector2> ControlPoints = new List<Vector2>();
        public Vector2 EndPoint;

        public Vector2 LastControlPoint => ControlPoints[ControlPoints.Count - 1];
    }
}
