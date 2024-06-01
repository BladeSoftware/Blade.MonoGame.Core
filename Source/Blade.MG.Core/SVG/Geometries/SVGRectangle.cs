using Blade.MG.SVG.Geometries.Paths;
using Microsoft.Xna.Framework;

namespace Blade.MG.SVG.Geometries
{
    /*
    * https://www.w3.org/TR/SVG/shapes.html#Introduction
    */

    public class SVGRectangle : SVGGeometry, ISVGPath
    {
        public float X;       // Left Edge
        public float Y;       // Top Edge
        public float Width;   // Width
        public float Height;  // Height
        public float Rx;      // Corner Radius X
        public float Ry;      // Corner Radius Y

        public SVGRectangle()
        {

        }

        public SVGRectangle(float x, float y, float w, float h, float rx = 0, float ry = 0)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
            Rx = rx;
            Ry = ry;
        }

        public SVGRectangle(string id, float x, float y, float w, float h, float rx = 0, float ry = 0)
        {
            ID = id ?? "";

            X = x;
            Y = y;
            Width = w;
            Height = h;
            Rx = rx;
            Ry = ry;
        }

        public SVGRectangle(string id, float x, float y, float w, float h, float rx, float ry, Color stroke, float? strokeWidth = null, SVGFill fill = null)
        {
            ID = id ?? "";
            Stroke = stroke;
            StrokeWidth = strokeWidth;
            Fill = fill;

            X = x;
            Y = y;
            Width = w;
            Height = h;
            Rx = rx;
            Ry = ry;
        }

        public override LineSegments ToLineSegments()
        {
            SVGPath pathGeometry = ToPath();
            return pathGeometry.ToLineSegments();
        }

        /// <summary>
        /// https://www.w3.org/TR/SVG/shapes.html#RectElement
        /// </summary>
        /// <returns></returns>
        public SVGPath ToPath()
        {
            //Mathematically, a ‘rect’ element is mapped to an equivalent ‘path’ element as follows, after generating absolute used values x, y, width, height, rx, and rx in user units for the user coordinate system, for each of the equivalent geometric properties following the rules specified above and in Units:
            //perform an absolute moveto operation to location (x+rx,y);
            //perform an absolute horizontal lineto with parameter x+width-rx;
            //if both rx and ry are greater than zero, perform an absolute elliptical arc operation to coordinate (x+width,y+ry), where rx and ry are used as the equivalent parameters to the elliptical arc command, the x-axis-rotation and large-arc-flag are set to zero, the sweep-flag is set to one;
            //perform an absolute vertical lineto parameter y+height-ry;
            //if both rx and ry are greater than zero, perform an absolute elliptical arc operation to coordinate (x+width-rx,y+height), using the same parameters as previously;
            //perform an absolute horizontal lineto parameter x+rx;
            //if both rx and ry are greater than zero, perform an absolute elliptical arc operation to coordinate (x,y+height-ry), using the same parameters as previously;
            //perform an absolute vertical lineto parameter y+ry
            //if both rx and ry are greater than zero, perform an absolute elliptical arc operation with a segment-completing close path operation, using the same parameters as previously.

            bool hasRoundedCorners = (Rx > 0 && Ry > 0);

            SVGPath pathGeometry = new SVGPath();
            pathGeometry.ID = this.ID;
            pathGeometry.Fill = this.Fill;
            pathGeometry.Stroke = this.Stroke;
            pathGeometry.StrokeWidth = this.StrokeWidth;


            pathGeometry.Commands.Add(new PathMove { IsRelative = false, StartPoint = new Vector2(X + Rx, Y) });
            pathGeometry.Commands.Add(new PathHLine { IsRelative = false, EndPointX = X + Width - Rx });

            if (hasRoundedCorners)
            {
                pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(X + Width, Y + Ry), Size = new Vector2(Rx, Ry), SweepDirection = false, IsLargeArc = false });
            }

            pathGeometry.Commands.Add(new PathVLine { IsRelative = false, EndPointY = Y + Height - Ry });

            if (hasRoundedCorners)
            {
                pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(X + Width - Rx, Y + Height), Size = new Vector2(Rx, Ry), SweepDirection = false, IsLargeArc = false });
            }

            pathGeometry.Commands.Add(new PathHLine { IsRelative = false, EndPointX = X + Rx });

            if (hasRoundedCorners)
            {
                pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(X, Y + Height - Ry), Size = new Vector2(Rx, Ry), SweepDirection = false, IsLargeArc = false });
            }

            pathGeometry.Commands.Add(new PathVLine { IsRelative = false, EndPointY = Y + Ry });

            if (hasRoundedCorners)
            {
                pathGeometry.Commands.Add(new PathEllipticArc { IsRelative = false, EndPoint = new Vector2(X + Rx, Y), Size = new Vector2(Rx, Ry), SweepDirection = false, IsLargeArc = false });
            }

            return pathGeometry;
        }
    }
}
