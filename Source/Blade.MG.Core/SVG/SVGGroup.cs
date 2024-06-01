using Blade.MG.SVG.Geometries;

namespace Blade.MG.SVG
{
    public class SVGGroup : SVGGeometry
    {
        public List<SVGGeometry> Shapes = new List<SVGGeometry>();


        public SVGGroup()
        {

        }

        public SVGGroup(string id)
        {
            ID = id ?? "";
        }

        public SVGGeometry AddShape(SVGGeometry shape)
        {
            if (shape == null)
            {
                return null;
            }

            Shapes.Add(shape);

            return shape;
        }

        public SVGPath AddPath(string path)
        {
            if (path == null)
            {
                return null;
            }

            var shape = new SVGPath(path);
            Shapes.Add(shape);

            return shape;
        }

        public override LineSegments ToLineSegments()
        {
            var lineSegments = new LineSegments();

            foreach (var shape in Shapes)
            {
                LineSegment lineShapes = new LineSegment();
                lineSegments.Shapes.Add(lineShapes);
                lineShapes.SrcGeometry = shape;
                lineShapes.PollyLines = shape.ToLineSegments().Shapes[0].PollyLines;
            }

            return lineSegments;
        }

    }
}
