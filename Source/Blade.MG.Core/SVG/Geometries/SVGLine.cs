using Blade.MG.SVG.Geometries.Paths;
using Microsoft.Xna.Framework;

namespace Blade.MG.SVG.Geometries
{
    public class SVGLine : SVGGeometry, ISVGPath
    {
        public float X1;  // Start-X
        public float Y1;  // Start-Y
        public float X2;  // End-X
        public float Y2;  // End-Y


        public SVGLine()
        {

        }

        public SVGLine(float x1, float y1, float x2, float y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public SVGLine(string id, float x1, float y1, float x2, float y2)
        {
            ID = id ?? "";

            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public SVGLine(string id, float x1, float y1, float x2, float y2, Color stroke, float? strokeWidth = null, SVGFill fill = null)
        {
            ID = id ?? "";
            Stroke = stroke;
            StrokeWidth = strokeWidth;
            Fill = fill;

            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public override LineSegments ToLineSegments()
        {
            SVGPath pathGeometry = ToPath();
            return pathGeometry.ToLineSegments();
        }

        /// <summary>
        /// https://www.w3.org/TR/SVG/shapes.html#LineElement
        /// </summary>
        /// <returns></returns>
        public SVGPath ToPath()
        {
            //Mathematically, a ‘line’ element can be mapped to an equivalent ‘path’ element as follows, after converting coordinates into user coordinate system user units according to Units to generate values x1, y1, x2, and y2:

            //perform an absolute moveto operation to absolute location (x1,y1)
            //perform an absolute lineto operation to absolute location (x2,y2)

            SVGPath pathGeometry = new SVGPath();
            pathGeometry.ID = this.ID;
            pathGeometry.Fill = this.Fill;
            pathGeometry.Stroke = this.Stroke;
            pathGeometry.StrokeWidth = this.StrokeWidth;

            pathGeometry.Commands.Add(new PathMove { IsRelative = false, StartPoint = new Vector2(X1, Y1) });
            pathGeometry.Commands.Add(new PathLine { IsRelative = false, EndPoints = new List<Vector2> { new Vector2(X2, Y2) } });

            return pathGeometry;
        }
    }
}
