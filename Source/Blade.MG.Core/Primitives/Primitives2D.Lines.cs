using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blade.MG.Core.Primitives
{
    public static partial class Primitives2D
    {
        #region Lines
        public static void DrawLine(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, Color color, float lineWidth = 1f, float layerDepth = 0f)
        {
            DrawLine(spriteBatch, p1.X, p1.Y, p2.X, p2.Y, color, lineWidth, layerDepth);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Vector3 p1, Vector3 p2, Color color, float lineWidth = 1f, float layerDepth = 0f)
        {
            DrawLine(spriteBatch, p1.X, p1.Y, p2.X, p2.Y, color, lineWidth, layerDepth);
        }

        public static void DrawLine(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Color color, float lineWidth = 1f, float layerDepth = 0f)
        {
            if (lineWidth > 1f)
            {
                DrawThickLine(spriteBatch, x1, y1, x2, y2, color, lineWidth, layerDepth);
                return;
            }

            float rotation = 0f;
            float xd = x2 - x1;
            float yd = y2 - y1;
            float len = (float)Math.Sqrt(xd * xd + yd * yd);

            if (len == 0)
            {
                return;
            }

            xd /= len;
            yd /= len;

            if (xd < -1f) xd = -1f;
            if (xd > 1f) xd = 1f;

            rotation = (float)Math.Acos(xd);

            if (yd < 0)
            {
                rotation = MathHelper.TwoPi - rotation;
            }

            x1 -= xd * (lineWidth / 2f);
            y1 -= yd * (lineWidth / 2f);

            x1 -= -yd * (lineWidth / 2f);
            y1 -= xd * (lineWidth / 2f);

            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1, y1), null, color, rotation, new Vector2(0, 0), new Vector2(len, lineWidth), SpriteEffects.None, layerDepth);
        }

        private static void DrawThickLine(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Color color, float lineWidth = 1f, float layerDepth = 0f)
        {
            if (lineWidth == 0)
            {
                return;
            }


            // Build list of lines
            List<Edge2D> lines = new List<Edge2D>();

            float dx = x2 - x1;
            float dy = y2 - y1;
            float len = (float)Math.Sqrt(dx * dx + dy * dy);

            if (len == 0)
            {
                return; // Draw Pixel ?
            }

            // Calculate Parallel
            float dx2 = -dy / len;
            float dy2 = dx / len;

            float halfWidth = lineWidth / 2f;
            float dxHalf = dx2 * halfWidth;
            float dyHalf = dy2 * halfWidth;

            Vector2 p1 = new Vector2(x1 - dxHalf, y1 - dyHalf);
            Vector2 p2 = new Vector2(x2 - dxHalf, y2 - dyHalf);
            Vector2 p3 = new Vector2(x2 + dxHalf, y2 + dyHalf);
            Vector2 p4 = new Vector2(x1 + dxHalf, y1 + dyHalf);

            lines.Add(new Edge2D(p1, p2));
            lines.Add(new Edge2D(p2, p3));
            lines.Add(new Edge2D(p3, p4));
            lines.Add(new Edge2D(p4, p1));


            float minY = p1.Y;
            float maxY = p1.Y;
            if (p2.Y < minY) minY = p2.Y;
            if (p2.Y > maxY) maxY = p2.Y;
            if (p3.Y < minY) minY = p3.Y;
            if (p3.Y > maxY) maxY = p3.Y;
            if (p4.Y < minY) minY = p4.Y;
            if (p4.Y > maxY) maxY = p4.Y;


            int yStart = (int)minY;
            int yEnd = (int)maxY;

            for (int y = yStart; y <= yEnd; y++)
            {
                float minX = float.MaxValue;
                float maxX = float.MinValue;

                for (int i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];

                    if (!float.IsNaN(line.FnX((float)y)))
                    {
                        if (line.CurrentX < minX) minX = line.CurrentX;
                        if (line.CurrentX > maxX) maxX = line.CurrentX;
                    }
                }

                if (minX != float.MaxValue)
                {
                    Primitives2D.DrawHLine(spriteBatch, y, minX, maxX, color, 1f, layerDepth);
                }
            }

        }

        /// <summary>
        /// create a smooth joint between two line segments:
        /// - Line 1 = lp0 to lp1
        /// - Line 2 = lp1 to lp2  <- Draws this line
        /// - Line 3 = lp2 to lp3
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="lp0"></param>
        /// <param name="lp1"></param>
        /// <param name="lp2"></param>
        /// <param name="lp3"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        /// <param name="depth"></param>
        /// <param name="dimensions"></param>
        /// <param name="measureOnly"></param>
        public static void DrawSmoothLine(SpriteBatch spriteBatch, Vector2 lp0, Vector2 lp1, Vector2 lp2, Vector2 lp3, Color color, float lineWidth, float depth = 0, RectangleF dimensions = null, bool measureOnly = false)
        {
            if (lineWidth == 0)
            {
                return;
            }

            float halfWidth = Math.Max(0f, lineWidth - 1f) / 2f;

            // Build list of total lines
            List<Edge2D> lines = new List<Edge2D>();

            // --- Previous Line Segment ---
            float dx = lp1.X - lp0.X;
            float dy = lp1.Y - lp0.Y;
            float len0 = (float)Math.Sqrt(dx * dx + dy * dy);

            // Calculate Parallel
            float dx0 = -dy / len0;
            float dy0 = dx / len0;

            float dxHalf0 = dx0 * halfWidth;
            float dyHalf0 = dy0 * halfWidth;

            // --- Current Line Segment ---
            dx = lp2.X - lp1.X;
            dy = lp2.Y - lp1.Y;
            float len1 = (float)Math.Sqrt(dx * dx + dy * dy);

            if (len1 == 0)
            {
                // Draw a Pixel
                if (dimensions != null)
                {
                    // Measure
                    dimensions.Union(lp1);
                }

                if (!measureOnly)
                {
                    // Draw
                    Primitives2D.DrawPixel(spriteBatch, lp1.X, lp1.Y, color);
                }

                return;
            }

            float dx1 = -dy / len1;
            float dy1 = dx / len1;

            float dxHalf1 = dx1 * halfWidth;
            float dyHalf1 = dy1 * halfWidth;


            // --- Next Line Segment ---
            dx = lp3.X - lp2.X;
            dy = lp3.Y - lp2.Y;
            float len2 = (float)Math.Sqrt(dx * dx + dy * dy);

            float dx2 = -dy / len2;
            float dy2 = dx / len2;

            float dxHalf2 = dx2 * halfWidth;
            float dyHalf2 = dy2 * halfWidth;


            Vector2 p01 = new Vector2(lp0.X - dxHalf0, lp0.Y - dyHalf0);
            Vector2 p02 = new Vector2(lp1.X - dxHalf0, lp1.Y - dyHalf0);
            Vector2 p03 = new Vector2(lp1.X + dxHalf0, lp1.Y + dyHalf0);
            Vector2 p04 = new Vector2(lp0.X + dxHalf0, lp0.Y + dyHalf0);

            Vector2 p11 = new Vector2(lp1.X - dxHalf1, lp1.Y - dyHalf1);
            Vector2 p12 = new Vector2(lp2.X - dxHalf1, lp2.Y - dyHalf1);
            Vector2 p13 = new Vector2(lp2.X + dxHalf1, lp2.Y + dyHalf1);
            Vector2 p14 = new Vector2(lp1.X + dxHalf1, lp1.Y + dyHalf1);

            Vector2 p21 = new Vector2(lp2.X - dxHalf2, lp2.Y - dyHalf2);
            Vector2 p22 = new Vector2(lp3.X - dxHalf2, lp3.Y - dyHalf2);
            Vector2 p23 = new Vector2(lp3.X + dxHalf2, lp3.Y + dyHalf2);
            Vector2 p24 = new Vector2(lp2.X + dxHalf2, lp2.Y + dyHalf2);


            Vector2 p1 = p11;
            Vector2 p2 = p12;
            Vector2 p3 = p13;
            Vector2 p4 = p14;

            //// Smooth the starting point  // TODO: Use a better smoothing method
            //if (len0 != 0)
            //{
            //    p1 = (p02 + p11) / 2f;
            //    p4 = (p03 + p14) / 2f;
            //}

            //// Smooth the ending point    // TODO: Use a better smoothing method
            //if (len2 != 0)
            //{
            //    p2 = (p12 + p21) / 2f;
            //    p3 = (p13 + p24) / 2f;
            //}

            //---Debug+

            //// Previous Line
            //DrawLine(game, spriteBatch, p01, p02, Color.Red, 1f, 0f);
            //DrawLine(game, spriteBatch, p02, p03, Color.Red, 1f, 0f);
            //DrawLine(game, spriteBatch, p03, p04, Color.Red, 1f, 0f);
            //DrawLine(game, spriteBatch, p04, p01, Color.Red, 1f, 0f);

            //// Current Line
            //DrawLine(game, spriteBatch, p11, p12, Color.Red, 1f, 0f);
            //DrawLine(game, spriteBatch, p12, p13, Color.Red, 1f, 0f);
            //DrawLine(game, spriteBatch, p13, p14, Color.Red, 1f, 0f);
            //DrawLine(game, spriteBatch, p14, p11, Color.Red, 1f, 0f);

            //// Next Line
            //DrawLine(game, spriteBatch, p21, p22, Color.Red, 1f, 0f);
            //DrawLine(game, spriteBatch, p22, p23, Color.Red, 1f, 0f);
            //DrawLine(game, spriteBatch, p23, p24, Color.Red, 1f, 0f);
            //DrawLine(game, spriteBatch, p24, p21, Color.Red, 1f, 0f);

            //// Smoothed Current Line
            //DrawLine(game, spriteBatch, p1, p2, Color.Yellow, 1f, 0f);
            //DrawLine(game, spriteBatch, p2, p3, Color.Yellow, 1f, 0f);
            //DrawLine(game, spriteBatch, p3, p4, Color.Yellow, 1f, 0f);
            //DrawLine(game, spriteBatch, p4, p1, Color.Yellow, 1f, 0f);

            //---Debug-


            float ix = 0;
            float iy = 0;

            //len0 = 0;
            //len2 = 0;

            // Smooth the starting point
            if (len0 != 0)
            {
                //p1 = (p02 + p11) / 2f;
                if (GetLineIntersection(p01.X, p01.Y, p02.X, p02.Y, p11.X, p11.Y, p12.X, p12.Y, ref ix, ref iy))
                {
                    p1 = new Vector2(ix, iy);
                }

                //p4 = (p03 + p14) / 2f;
                if (GetLineIntersection(p04.X, p04.Y, p03.X, p03.Y, p13.X, p13.Y, p14.X, p14.Y, ref ix, ref iy))
                {
                    p4 = new Vector2(ix, iy);
                }
            }

            // Smooth the ending point
            if (len2 != 0)
            {
                //    p2 = (p12 + p21) / 2f;
                if (GetLineIntersection(p21.X, p21.Y, p22.X, p22.Y, p11.X, p11.Y, p12.X, p12.Y, ref ix, ref iy))
                {
                    p2 = new Vector2(ix, iy);
                }

                //    p3 = (p13 + p24) / 2f;
                if (GetLineIntersection(p24.X, p24.Y, p23.X, p23.Y, p13.X, p13.Y, p14.X, p14.Y, ref ix, ref iy))
                {
                    p3 = new Vector2(ix, iy);
                }
            }


            ////---Debug+

            //// New Smoothed Current Line
            //DrawLine(game, spriteBatch, p1, p2, Color.White, 1f, 0f);
            //DrawLine(game, spriteBatch, p2, p3, Color.White, 1f, 0f);
            //DrawLine(game, spriteBatch, p3, p4, Color.White, 1f, 0f);
            //DrawLine(game, spriteBatch, p4, p1, Color.White, 1f, 0f);

            ////---Debug-


            lines.Add(new Edge2D(p1, p2));
            lines.Add(new Edge2D(p2, p3));
            lines.Add(new Edge2D(p3, p4));
            lines.Add(new Edge2D(p4, p1));

            float minY = p1.Y;
            float maxY = p1.Y;
            if (p2.Y < minY) minY = p2.Y;
            if (p2.Y > maxY) maxY = p2.Y;
            if (p3.Y < minY) minY = p3.Y;
            if (p3.Y > maxY) maxY = p3.Y;
            if (p4.Y < minY) minY = p4.Y;
            if (p4.Y > maxY) maxY = p4.Y;


            // if (minY < -2000 || maxY > 2000) { }

            int yStart = Math.Max((int)Math.Floor(minY), 0);
            int yEnd; // = (int)maxY;

            if (measureOnly)
            {
                yEnd = Math.Min((int)maxY, 4096); // Arbitrarily limit viewport to 4k when measuring
            }
            else
            {
                yEnd = Math.Min((int)maxY, spriteBatch.GraphicsDevice.Viewport.Height);
            }


            for (int y = yStart; y <= yEnd; y++)
            {
                float minX = float.MaxValue;
                float maxX = float.MinValue;

                for (int i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];

                    if (!float.IsNaN(line.FnX((float)y)))
                    {
                        if (line.CurrentX < minX) minX = line.CurrentX;
                        if (line.CurrentX > maxX) maxX = line.CurrentX;
                    }
                }

                if (minX != float.MaxValue)
                {
                    if (dimensions != null)
                    {
                        // Measure
                        dimensions.Union(minX, y);
                        dimensions.Union(maxX, y);
                    }

                    if (!measureOnly)
                    {
                        // Draw
                        Primitives2D.DrawHLine(spriteBatch, y, minX, maxX, color, 1f, depth);
                    }
                }

            }
        }


        public static void DrawHLine(SpriteBatch spriteBatch, float y, float x1, float x2, Color color, float lineWidth = 1f, float layerDepth = 0f)
        {
            float rotation = 0f;
            float len = x2 - x1;

            if (len == 0) return;

            if (len < 0)
            {
                float t = x1;
                x1 = x2;
                x2 = t;
                len = -len;
            }

            float hw = Math.Max(0f, lineWidth - 1f) / 2f;

            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1, y - hw), null, color, rotation, new Vector2(0, 0), new Vector2(len, lineWidth), SpriteEffects.None, layerDepth);
            //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1, y), null, Color.Yellow, rotation, new Vector2(0, 0), new Vector2(1,1), SpriteEffects.None, layerDepth);
        }

        public static void DrawVLine(SpriteBatch spriteBatch, float x, float y1, float y2, Color color, float lineWidth = 1f, float layerDepth = 0f)
        {
            float rotation = 0f;
            float len = y2 - y1;

            if (len == 0) return;

            if (len < 0)
            {
                float t = y1;
                y1 = y2;
                y2 = t;
                len = -len;
            }

            float hw = Math.Max(0f, lineWidth - 1f) / 2f;

            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x - hw, y1), null, color, rotation, new Vector2(0, 0), new Vector2(lineWidth, len), SpriteEffects.None, layerDepth);
        }

        #endregion
    }
}
