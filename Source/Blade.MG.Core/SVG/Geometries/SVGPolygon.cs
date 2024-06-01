using Blade.MG.SVG.Geometries.Paths;
using Microsoft.Xna.Framework;

namespace Blade.MG.SVG.Geometries
{
    public class SVGPolygon : SVGGeometry, ISVGPath
    {
        public List<Vector2> Points = new List<Vector2>();

        public SVGPolygon()
        {

        }

        public SVGPolygon(List<Vector2> points)
        {
            Points = points;
        }

        public SVGPolygon(string id, List<Vector2> points)
        {
            ID = id ?? "";

            Points = points;
        }

        public SVGPolygon(string id, List<Vector2> points, Color stroke, float? strokeWidth = null, SVGFill fill = null)
        {
            ID = id ?? "";
            Stroke = stroke;
            StrokeWidth = strokeWidth;
            Fill = fill;

            Points = points;
        }


        public override LineSegments ToLineSegments()
        {
            SVGPath pathGeometry = ToPath();
            return pathGeometry.ToLineSegments();
        }

        /// <summary>
        /// https://www.w3.org/TR/SVG/shapes.html#PolygonElement
        /// </summary>
        /// <returns></returns>
        public SVGPath ToPath()
        {
            //Mathematically, a ‘polygon’ element can be mapped to an equivalent ‘path’ element as follows:

            //perform an absolute moveto operation to the first coordinate pair in the list of points
            //for each subsequent coordinate pair, perform an absolute lineto operation to that coordinate pair
            //perform a closepath command

            SVGPath pathGeometry = new SVGPath();
            pathGeometry.ID = this.ID;
            pathGeometry.Fill = this.Fill;
            pathGeometry.Stroke = this.Stroke;
            pathGeometry.StrokeWidth = this.StrokeWidth;

            pathGeometry.Commands.Add(new PathMove { IsRelative = false, StartPoint = Points[0] });
            pathGeometry.Commands.Add(new PathLine { IsRelative = false, EndPoints = new List<Vector2>(Points.GetRange(1, Points.Count - 1)) });
            pathGeometry.Commands.Add(new PathClose());

            return pathGeometry;
        }
    }
}
