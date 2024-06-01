using Blade.MG.SVG.Geometries.Paths;
using Microsoft.Xna.Framework;

namespace Blade.MG.SVG.Geometries
{

    public class SVGCircle : SVGGeometry, ISVGPath
    {
        public float Cx;  // Center X
        public float Cy;  // Center Y
        public float R;   // Radius

        public SVGCircle()
        {

        }

        public SVGCircle(float cx, float cy, float r)
        {
            Cx = cx;
            Cy = cy;
            R = r;
        }

        public SVGCircle(string id, float cx, float cy, float r)
        {
            ID = id ?? "";

            Cx = cx;
            Cy = cy;
            R = r;
        }

        public SVGCircle(string id, float cx, float cy, float r, Color stroke, float? strokeWidth = null, SVGFill fill = null)
        {
            ID = id ?? "";
            Stroke = stroke;
            StrokeWidth = strokeWidth;
            Fill = fill;

            Cx = cx;
            Cy = cy;
            R = r;
        }

        public override LineSegments ToLineSegments()
        {
            SVGPath pathGeometry = ToPath();
            return pathGeometry.ToLineSegments();
        }


        /// <summary>
        /// https://www.w3.org/TR/SVG/shapes.html#CircleElement
        /// </summary>
        /// <returns></returns>
        public SVGPath ToPath()
        {
            //Mathematically, a ‘circle’ element is mapped to an equivalent ‘path’ element that consists of four elliptical arc segments, each covering a quarter of the circle. 
            // The path begins at the "3 o'clock" point on the radius and proceeds in a clock-wise direction (before any transformations). 
            // The rx and ry parameters to the arc commands are both equal to the used value of the r property, after conversion to local user units, while the x-axis-rotation, the large-arc-flag, and the sweep-flag are all set to zero. The coordinates are computed as follows, where cx, cy, and r are the used values of the equivalent properties, converted to user units:
            //- A move-to command to the point cx+r,cy;
            //- arc to cx,cy+r;
            //- arc to cx-r,cy;
            //- arc to cx,cy-r;
            //- arc with a segment-completing close path operation.

            SVGPath pathGeometry = new SVGPath();
            pathGeometry.ID = this.ID;
            pathGeometry.Fill = this.Fill;
            pathGeometry.Stroke = this.Stroke;
            pathGeometry.StrokeWidth = this.StrokeWidth;


            pathGeometry.Commands.Add(new PathMove { IsRelative = false, StartPoint = new Vector2(Cx + R, Cy) });

            pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(Cx, Cy + R), Size = new Vector2(R, R), SweepDirection = false, IsLargeArc = false });
            pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(Cx - R, Cy), Size = new Vector2(R, R), SweepDirection = false, IsLargeArc = false });
            pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(Cx, Cy - R), Size = new Vector2(R, R), SweepDirection = false, IsLargeArc = false });
            pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(Cx + R, Cy), Size = new Vector2(R, R), SweepDirection = false, IsLargeArc = false });

            return pathGeometry;
        }
    }
}
