using Microsoft.Xna.Framework;

namespace Blade.MG.SVG
{
    public abstract class SVGGeometry
    {
        public string ID = "";

        public Color? Stroke; // = Color.White;
        public float? StrokeWidth; // = 1f;
        public SVGFill Fill; // = null;

        public abstract LineSegments ToLineSegments();

    }
}
