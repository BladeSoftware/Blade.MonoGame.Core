using Blade.MG;
using Blade.MG.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blade.MG.SVG
{

    public enum FillRule
    {
        Nonzero,
        EvenOdd
    }

    public static class SVGRenderer
    {
        private static float StartDepth = 1f; //0.0001f;
        private static float DepthDelta = -0.0001f;

        public static Rectangle MeasurePath(SVGGeometry shape, Matrix matrix)
        {
            float depth = StartDepth;
            var dimensions = new RectangleF { MinX = float.MaxValue, MinY = float.MaxValue, MaxX = float.MinValue, MaxY = float.MinValue };

            DrawPath(shape, null, matrix, shape.Stroke, shape.StrokeWidth, shape.Fill, ref depth, ref dimensions, true);

            return dimensions.ToRectangle();
        }

        public static void DrawPath(List<SVGGeometry> shapes, SpriteBatch spriteBatch, Matrix matrix, out RectangleF dimensions, bool measureOnly = false)
        {
            float depth = StartDepth;

            dimensions = new RectangleF { MinX = float.MaxValue, MinY = float.MaxValue, MaxX = float.MinValue, MaxY = float.MinValue };

            foreach (var shape in shapes)
            {
                DrawPath(shape, spriteBatch, matrix, shape.Stroke, shape.StrokeWidth, shape.Fill, ref depth, ref dimensions, measureOnly);
            }
        }

        public static void DrawPath(SVGGeometry shape, SpriteBatch spriteBatch, Matrix matrix, out RectangleF dimensions, bool measureOnly = false)
        {
            float depth = StartDepth;

            dimensions = new RectangleF { MinX = float.MaxValue, MinY = float.MaxValue, MaxX = float.MinValue, MaxY = float.MinValue };

            DrawPath(shape, spriteBatch, matrix, shape.Stroke, shape.StrokeWidth, shape.Fill, ref depth, ref dimensions, measureOnly);
        }

        private static void DrawPath(SVGGeometry shape, SpriteBatch spriteBatch, Matrix matrix, Color? inheritedStroke, float? inheritedStrokeWidth, SVGFill inheritedFill, ref float depth, ref RectangleF dimensions, bool measureOnly = false)
        {
            Color stroke = shape.Stroke ?? inheritedStroke ?? Color.White;
            float strokeWidth = shape.StrokeWidth ?? inheritedStrokeWidth ?? 1f;
            SVGFill fill = shape.Fill ?? inheritedFill;

            if (shape is SVGGroup)
            {
                foreach (var subShape in ((SVGGroup)shape).Shapes)
                {
                    //DrawPath(subShape, spriteBatch, matrix, ref depth, dimensions, measureOnly);
                    DrawPath(subShape, spriteBatch, matrix, stroke, strokeWidth, fill, ref depth, ref dimensions, measureOnly);
                }
            }
            else
            {
                var lines = shape.ToLineSegments();
                DrawPath(lines, spriteBatch, matrix, stroke, strokeWidth, fill, ref depth, ref dimensions, measureOnly);
            }
        }

        private static void DrawPath(LineSegments lines, SpriteBatch spriteBatch, Matrix matrix, Color inheritedStroke, float? inheritedStrokeWidth, SVGFill inheritedFill, ref float depth, ref RectangleF dimensions, bool measureOnly = false)
        {
            foreach (var segment in lines.Shapes)
            {
                depth += DepthDelta;

                Color stroke = segment?.SrcGeometry?.Stroke ?? inheritedStroke;
                float strokeWidth = segment?.SrcGeometry?.StrokeWidth ?? inheritedStrokeWidth ?? 1f;
                SVGFill fill = segment?.SrcGeometry?.Fill ?? inheritedFill;


                //-- Simple Edge 
                //foreach (var pollyLine in segment.PollyLines)
                //{
                //    if (fill != null)
                //    {
                //        FillPath(game, spriteBatch, segment, pollyLine, matrix, fill.Value, FillRule.EvenOdd, depth);
                //        depth += DepthDelta;
                //    }

                //    bool isClosed = pollyLine.Points[0] == pollyLine.Points[pollyLine.Points.Count - 1];

                //    Vector2 p1 = Vector2.Transform(pollyLine.Points[0], matrix);
                //    for (int i = 1; i < pollyLine.Points.Count; i++)
                //    {
                //        Vector2 p2 = Vector2.Transform(pollyLine.Points[i], matrix);
                //        Primitives2D.DrawLine(game, spriteBatch, p1, p2, stroke, strokeWidth, depth);

                //        p1 = p2;
                //    }
                //}

                // Smoothed Edges
                foreach (var pollyLine in segment.PollyLines)
                {
                    if (fill != null && !measureOnly) //Don't fill if we're just measuring the path
                    {
                        FillPath(spriteBatch, segment, pollyLine, matrix, fill, stroke, strokeWidth, depth);
                        depth += DepthDelta;
                    }

                    int numPoints = pollyLine.Points.Count;
                    if (numPoints < 2) continue;

                    bool isClosed = pollyLine.Points[0] == pollyLine.Points[numPoints - 1];

                    Vector2 p0 = isClosed ? Vector2.Transform(pollyLine.Points[numPoints - 2], matrix) : Vector2.Transform(pollyLine.Points[0], matrix);
                    Vector2 p1 = Vector2.Transform(pollyLine.Points[0], matrix);
                    Vector2 p2 = Vector2.Transform(pollyLine.Points[1], matrix);


                    //int j = 0;

                    for (int i = 2; i < pollyLine.Points.Count + 1; i++)
                    {
                        //j++;

                        Vector2 p3 = i < numPoints ? Vector2.Transform(pollyLine.Points[i], matrix) : Vector2.Transform(pollyLine.Points[1], matrix);

                        Primitives2D.DrawSmoothLine(spriteBatch, p0, p1, p2, p3, stroke, strokeWidth, depth, ref dimensions, measureOnly);

                        //var col = Color.White;
                        //if (j == 0) col = Color.Red;
                        //if (j == 1) col = Color.Green;
                        //if (j == 2) col = Color.Purple;

                        //Primitives2D.DrawSmoothLine(spriteBatch, p0, p1, p2, p3, col, strokeWidth, depth, dimensions, measureOnly);


                        p0 = p1;
                        p1 = p2;
                        p2 = p3;
                    }
                }


            }
        }


        private static void FillPath(SpriteBatch spriteBatch, LineSegment segment, PollyLine pollyLine, Matrix matrix, SVGFill fill, Color stroke, float strokeWidth, float depth)
        {
            // Build list of total lines
            List<Edge2D> lines = new List<Edge2D>();

            Vector2 p1 = Vector2.Transform(pollyLine.Points[0], matrix);

            float minY = p1.Y;
            float maxY = p1.Y;

            for (int i = 1; i < pollyLine.Points.Count; i++)
            {
                Vector2 p2 = Vector2.Transform(pollyLine.Points[i], matrix);
                if (p2.Y < minY)
                {
                    minY = p2.Y;
                }
                if (p2.Y > maxY)
                {
                    maxY = p2.Y;
                }

                lines.Add(new Edge2D(p1, p2));

                //Primitives2D.DrawLine(game, spriteBatch, p1, p2, stroke, strokeWidth);

                p1 = p2;
            }

            int yStart = (int)minY;
            int yEnd = (int)maxY;


            // List of Active Lines
            List<Edge2D> activeEdges = new List<Edge2D>();

            for (int y = yStart; y <= yEnd; y++)
            {
                float minX = float.MaxValue;
                float maxX = float.MinValue;

                // Build list of Active Lines
                activeEdges.Clear();

                for (int i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];

                    if (!float.IsNaN(line.FnX((float)y)))
                    //if (!line.isHorizontal && !float.IsNaN(line.FnX((float)y)))
                    {
                        activeEdges.Add(line);

                        if (line.CurrentX < minX) minX = line.CurrentX;
                        if (line.CurrentX > maxX) maxX = line.CurrentX;
                    }
                }

                //Primitives2D.DrawHLine(game, spriteBatch, y, 20f, 200f, fill, 1f);

                //if (y == 582) fill = Color.Purple;  // 580 - 585 Debugging
                //if (y == 604) fill = Color.Purple;  // 580 - 585 Debugging
                //if (y == 516) fill = Color.Purple;  // 580 - 585 Debugging

                if (activeEdges.Count > 1)
                {
                    //Primitives2D.DrawHLine(game, spriteBatch, y, minX, maxX, fill, 1f);

                    // Sort by X
                    //activeEdges.Sort((a, b) => a.CurrentX.CompareTo(b.CurrentX));
                    activeEdges.Sort((a, b) =>
                    {
                        if (a.CurrentX < b.CurrentX) return -1;
                        if (a.CurrentX > b.CurrentX) return +1;
                        if (a.Winding < b.Winding) return -1;
                        if (a.Winding > b.Winding) return +1;
                        return 0;
                    });

                    // Remove duplicate points
                    for (int j = activeEdges.Count - 2; j >= 0; j--)
                    {
                        if (activeEdges[j].CurrentX == activeEdges[j + 1].CurrentX && activeEdges[j].Winding == activeEdges[j + 1].Winding)
                        {
                            activeEdges.RemoveAt(j);
                        }
                    }


                    // Fill Shape
                    int windingCount = activeEdges[0].Winding;

                    if (fill.FillRule == FillRule.Nonzero)
                    {
                        // Rule: nonzero
                        for (int i = 1; i < activeEdges.Count; i++)
                        {
                            if (windingCount != 0)
                            {
                                float startX = activeEdges[i - 1].CurrentX;
                                float endX = activeEdges[i].CurrentX;
                                Primitives2D.DrawHLine(spriteBatch, y, startX, endX, fill.FillColor, 1f, depth);
                                //DrawHLine(spriteBatch, y, startX, endX, fill, stroke, strokeWidth, depth);
                            }

                            windingCount += activeEdges[i].Winding;
                        }
                    }
                    else
                    {
                        // Rule: evenodd
                        for (int i = 1; i < activeEdges.Count; i++)
                        {
                            windingCount += activeEdges[i].Winding;

                            if ((windingCount % 2) == 0)
                            {
                                float startX = activeEdges[i - 1].CurrentX;
                                float endX = activeEdges[i].CurrentX;
                                Primitives2D.DrawHLine(spriteBatch, y, startX, endX, fill.FillColor, 1f, depth);
                                //DrawHLine(spriteBatch, y, startX, endX, fill, stroke, strokeWidth, depth);
                            }

                        }
                    }

                }
            }

        }

        //private static void DrawHLine(SpriteBatch spriteBatch, float y, float minX, float maxX, Color fill, Color stroke, float strokeWidth, float depth)
        //{
        //    Primitives2D.DrawHLine(spriteBatch, y, minX, maxX, fill, 1f, depth);

        //    // Antialias the line ends
        //    float offsetL = (float)(minX - Math.Floor(minX));
        //    float offsetR = (float)(maxX - Math.Floor(maxX));

        //    float halfWidth = strokeWidth / 2f;

        //    //if (offsetL < 0.5f)
        //    {
        //        float alphaL = 1f - offsetL;
        //        Color colorL = new Color(stroke, alphaL);
        //        //colorL = new Color(Color.Red, alphaL);
        //        //colorL = Color.Red;

        //        Primitives2D.DrawHLine(spriteBatch, y, minX - halfWidth, minX + halfWidth, colorL, 1f, depth + 0.0000001f);
        //        //Primitives2D.DrawPixel(spriteBatch, minX - 0.5f, y, colorL, 1f, depth + 0.0000001f);
        //    }
        //    //if (offsetR > 0.5f)
        //    {
        //        float alphaR = 1f - offsetR;
        //        Color colorR = new Color(stroke, alphaR);
        //        //colorR = new Color(Color.Red, alphaR);

        //        Primitives2D.DrawHLine(spriteBatch, y, maxX - halfWidth, maxX + halfWidth, colorR, 1f, depth + 0.0000001f);
        //        //Primitives2D.DrawPixel(spriteBatch, maxX + 0.5f, y, colorR, 1f, depth + 0.0000001f);
        //    }

        //}

    }
}
