using Blade.MG;
using Blade.MG.SVG.Geometries.Paths;
using Microsoft.Xna.Framework;

namespace Blade.MG.SVG.Geometries
{
    /// <summary>
    /// https://www.w3.org/TR/SVG/paths.html
    /// https://developer.mozilla.org/en-US/docs/Web/SVG/Tutorial/Paths
    /// </summary>

    /* TODO: Cater for Stroke / Fill etc.
     LineGeometry myLineGeometry = new LineGeometry();
     myLineGeometry.StartPoint = new Point(10,20);
     myLineGeometry.EndPoint = new Point(100,130);

     Path myPath = new Path();
     myPath.Stroke = Brushes.Black;
     myPath.StrokeThickness = 1;
     myPath.Data = myLineGeometry;
     */
    public class SVGPath : SVGGeometry, ISVGPath
    {
        public static int BezierSegments = 32;  // Number of line segments to decompose Beziers into

        public List<PathCommand> Commands = new List<PathCommand>();


        public SVGPath()
        {

        }

        public SVGPath(string path)
        {
            Commands = PathParser.ParsePath(path);

        }

        public SVGPath(string id, string path)
        {
            ID = id ?? "";
            Commands = PathParser.ParsePath(path);
        }

        public SVGPath(string id, string path, Color stroke, float? strokeWidth = null, SVGFill fill = null)
        {
            ID = id ?? "";
            Stroke = stroke;
            StrokeWidth = strokeWidth;
            Fill = fill;

            Commands = PathParser.ParsePath(path);
        }


        public void ParsePath(string path)
        {
            Commands = PathParser.ParsePath(path);
        }

        public override LineSegments ToLineSegments()
        {
            // TODO: Cache LineSegments
            return ToLineSegments(this);
        }

        private static LineSegments ToLineSegments(SVGPath path)
        {
            LineSegments lineSegments = new LineSegments();
            LineSegment lineShapes = new LineSegment();
            lineSegments.Shapes.Add(lineShapes);
            lineShapes.SrcGeometry = path;

            PollyLine currentLine = new PollyLine();
            lineShapes.PollyLines.Add(currentLine);
            currentLine.Points.Add(Vector2.Zero); // Start at the Top-Left corner

            Vector2 point = new Vector2();
            Vector2 startPoint = new Vector2();
            PathCommand previousCommand = null;

            foreach (var command in path.Commands)
            {
                if (command is PathMove)
                {
                    PathMove pathMove = command as PathMove;
                    point = pathMove.IsRelative ? point + pathMove.StartPoint : pathMove.StartPoint;
                    startPoint = point;

                    if (currentLine.Points.Count > 1)
                    {
                        currentLine = new PollyLine();
                        lineShapes.PollyLines.Add(currentLine);
                    }
                    else
                    {
                        currentLine.Points.Clear();
                    }

                    currentLine.Points.Add(point);

                }
                else if (command is PathLine)
                {
                    PathLine pathLine = command as PathLine;
                    foreach (var endPoint in pathLine.EndPoints)
                    {
                        Vector2 p2 = pathLine.IsRelative ? point + endPoint : endPoint;
                        //Primitives2D.DrawLine(Game, spriteBatch, Vector3.Transform(point, matrix), Vector3.Transform(p2, matrix), Color.Yellow);
                        currentLine.Points.Add(p2);
                        point = p2;
                    }
                }
                else if (command is PathHLine)
                {
                    PathHLine pathLine = command as PathHLine;
                    Vector2 p2 = pathLine.IsRelative ? point + new Vector2(pathLine.EndPointX, 0f) : new Vector2(pathLine.EndPointX, point.Y);
                    //Primitives2D.DrawLine(Game, spriteBatch, Vector3.Transform(point, matrix), Vector3.Transform(p2, matrix), Color.Yellow);
                    currentLine.Points.Add(p2);
                    point = p2;
                }
                else if (command is PathVLine)
                {
                    PathVLine pathLine = command as PathVLine;
                    Vector2 p2 = pathLine.IsRelative ? point + new Vector2(0f, pathLine.EndPointY) : new Vector2(point.X, pathLine.EndPointY);
                    //Primitives2D.DrawLine(Game, spriteBatch, Vector3.Transform(point, matrix), Vector3.Transform(p2, matrix), Color.Yellow);
                    currentLine.Points.Add(p2);
                    point = p2;
                }
                else if (command is PathBezierCubic || command is PathBezierSmoothCubic)
                {
                    PathBezierBase cubicBezier = command as PathBezierBase;

                    List<Vector2> controlPoints = new List<Vector2>();

                    if (command is PathBezierSmoothCubic && (previousCommand is PathBezierCubic || previousCommand is PathBezierSmoothCubic))
                    {
                        Vector2 lastControl = ((PathBezierBase)previousCommand).LastControlPoint;
                        Vector2 reflection = point + (point - lastControl);
                        controlPoints.Add(point);
                        controlPoints.Add(reflection);
                    }
                    else
                    {
                        controlPoints.Add(point);
                    }

                    if (cubicBezier.IsRelative)
                    {
                        for (int i = 0; i < cubicBezier.ControlPoints.Count; i++)
                        {
                            controlPoints.Add(point + cubicBezier.ControlPoints[i]);
                        }
                        controlPoints.Add(point + cubicBezier.EndPoint);
                    }
                    else
                    {
                        controlPoints.AddRange(cubicBezier.ControlPoints);
                        controlPoints.Add(cubicBezier.EndPoint);
                    }


                    var points = Splines.CalcSpline(Splines.SplineType.Bezier_Cubic, BezierSegments, controlPoints);

                    //Vector3 p1 = Vector3.Transform(point, matrix);
                    foreach (var endPoint in points)
                    {
                        //Vector3 p2 = Vector3.Transform(endPoint, matrix);

                        //Primitives2D.DrawLine(Game, spriteBatch, p1, p2, Color.Yellow);
                        //p1 = p2;
                        currentLine.Points.Add(endPoint);
                    }

                    point = points.Last();

                }
                else if (command is PathBezierQuadratic || command is PathBezierSmoothQuadratic)
                {
                    PathBezierBase quadraticBezier = command as PathBezierBase;

                    List<Vector2> controlPoints = new List<Vector2>();

                    if (command is PathBezierSmoothQuadratic && (previousCommand is PathBezierQuadratic || previousCommand is PathBezierSmoothQuadratic))
                    {
                        Vector2 lastControl = ((PathBezierBase)previousCommand).LastControlPoint;
                        Vector2 reflection = point + (point - lastControl);
                        controlPoints.Add(point);
                        controlPoints.Add(reflection);
                    }
                    else
                    {
                        controlPoints.Add(point);
                    }

                    if (quadraticBezier.IsRelative)
                    {
                        for (int i = 0; i < quadraticBezier.ControlPoints.Count; i++)
                        {
                            controlPoints.Add(point + quadraticBezier.ControlPoints[i]);
                        }
                        controlPoints.Add(point + quadraticBezier.EndPoint);
                    }
                    else
                    {
                        controlPoints.AddRange(quadraticBezier.ControlPoints);
                        controlPoints.Add(quadraticBezier.EndPoint);
                    }

                    var points = Splines.CalcSpline(Splines.SplineType.Bezier_Quadric, BezierSegments, controlPoints);

                    //Vector3 p1 = Vector3.Transform(point, matrix);
                    foreach (var endPoint in points)
                    {
                        //Vector3 p2 = Vector3.Transform(endPoint, matrix);
                        //Primitives2D.DrawLine(Game, spriteBatch, p1, p2, Color.Yellow);
                        //p1 = p2;

                        currentLine.Points.Add(endPoint);
                    }
                    point = points.Last();

                }
                else if (command is PathEllipticArc)
                {
                    // https://mortoray.com/2017/02/16/rendering-an-svg-elliptical-arc-as-bezier-curves/
                    PathEllipticArc arc = command as PathEllipticArc;

                    arc.EllipticArcToBezierCurve(point, out var spline);


                    List<Vector2> controlPoints = new List<Vector2>();
                    controlPoints.Add(point);

                    //if (arc.IsRelative)
                    //{
                    //    for (int i = 0; i < spline.ControlPoints.Count; i++)
                    //    {
                    //        controlPoints.Add(point + spline.ControlPoints[i]);
                    //    }
                    //    controlPoints.Add(point + spline.EndPoint);
                    //}
                    //else
                    //{
                    controlPoints.AddRange(spline.ControlPoints);
                    controlPoints.Add(spline.EndPoint);
                    //}

                    var points = Splines.CalcSpline(Splines.SplineType.Bezier_Cubic, BezierSegments, controlPoints);

                    //Vector3 p1 = Vector3.Transform(point, matrix);
                    foreach (var endPoint in points)
                    {
                        //Vector3 p2 = Vector3.Transform(endPoint, matrix);

                        //Primitives2D.DrawLine(Game, spriteBatch, p1, p2, Color.Yellow);
                        //p1 = p2;
                        currentLine.Points.Add(endPoint);
                    }

                    point = points.Last();

                }
                else if (command is PathClose)
                {
                    //Primitives2D.DrawLine(Game, spriteBatch, Vector3.Transform(point, matrix), Vector3.Transform(startPoint, matrix), Color.Yellow);
                    currentLine.Points.Add(startPoint);
                    point = startPoint;
                }

                previousCommand = command;
            }

            return lineSegments;
        }

        public SVGPath ToPath()
        {
            return this;
        }
    }
}
