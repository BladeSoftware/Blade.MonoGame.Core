using Blade.MG.SVG.Geometries.Paths;
using Microsoft.Xna.Framework;

namespace Blade.MG.SVG.Geometries
{
    public class SVGEllipse : SVGGeometry, ISVGPath
    {
        public float Cx;  // Center X
        public float Cy;  // Center Y
        public float Rx;  // Radius X
        public float Ry;  // Radius Y


        public SVGEllipse()
        {

        }


        public SVGEllipse(float cx, float cy, float rx, float ry)
        {
            Cx = cx;
            Cy = cy;
            Rx = rx;
            Ry = ry;
        }

        public SVGEllipse(string id, float cx, float cy, float rx, float ry)
        {
            ID = id ?? "";

            Cx = cx;
            Cy = cy;
            Rx = rx;
            Ry = ry;
        }

        public SVGEllipse(string id, float cx, float cy, float rx, float ry, Color stroke, float? strokeWidth = null, SVGFill fill = null)
        {
            ID = id ?? "";
            Stroke = stroke;
            StrokeWidth = strokeWidth;
            Fill = fill;

            Cx = cx;
            Cy = cy;
            Rx = rx;
            Ry = ry;
        }

        public override LineSegments ToLineSegments()
        {
            SVGPath pathGeometry = ToPath();
            return pathGeometry.ToLineSegments();
        }

        /// <summary>
        /// https://www.w3.org/TR/SVG/shapes.html#EllipseElement
        /// </summary>
        /// <returns></returns>
        public SVGPath ToPath()
        {
            //Mathematically, an ‘ellipse’ element is mapped to an equivalent ‘path’ element that consists of four elliptical arc segments, each covering a quarter of the ellipse. 
            // The path begins at the "3 o'clock" point on the radius and proceeds in a clock-wise direction (before any transformation). 
            // The rx and ry parameters to the arc commands are the used values of the equivalent properties after conversion to local user units, while the x-axis-rotation, the large-arc-flag, and the sweep-flag are all set to zero. The coordinates are computed as follows, where cx, cy, rx, and ry are the used values of the equivalent properties, converted to user units:

            //A move-to command to the point cx+rx,cy;
            //arc to cx,cy+ry;
            //arc to cx-rx,cy;
            //arc to cx,cy-ry;
            //arc with a segment-completing close path operation.

            SVGPath pathGeometry = new SVGPath();
            pathGeometry.ID = this.ID;
            pathGeometry.Fill = this.Fill;
            pathGeometry.Stroke = this.Stroke;
            pathGeometry.StrokeWidth = this.StrokeWidth;

            pathGeometry.Commands.Add(new PathMove { IsRelative = false, StartPoint = new Vector2(Cx + Rx, Cy) });

            pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(Cx, Cy + Ry), Size = new Vector2(Rx, Ry), SweepDirection = false, IsLargeArc = false });
            pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(Cx - Rx, Cy), Size = new Vector2(Rx, Ry), SweepDirection = false, IsLargeArc = false });
            pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(Cx, Cy - Ry), Size = new Vector2(Rx, Ry), SweepDirection = false, IsLargeArc = false });
            pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(Cx + Rx, Cy), Size = new Vector2(Rx, Ry), SweepDirection = false, IsLargeArc = false });

            return pathGeometry;
        }
    }
}
