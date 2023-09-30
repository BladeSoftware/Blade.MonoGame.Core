using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blade.MG.Primitives
{
    public static partial class Primitives2D
    {

        #region Rectangles
        public static void DrawRect(SpriteBatch spriteBatch, Rectangle rectangle, Color color, float lineWidth = 1f, bool bounded = false)
        {
            DrawRect(spriteBatch, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, color, lineWidth, bounded);
        }

        public static void DrawRect(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Color color, float lineWidth = 1f, bool bounded = false)
        {

            if (lineWidth <= 1f)
            {
                Primitives2D.DrawHLine(spriteBatch, y1, x1, x2, color, 1f);
                Primitives2D.DrawHLine(spriteBatch, y2 - 1, x1, x2, color, 1f);

                Primitives2D.DrawVLine(spriteBatch, x2 - 1, y1 + 1, y2 - 1, color, 1f);
                Primitives2D.DrawVLine(spriteBatch, x1, y1 + 1, y2 - 1, color, 1f);
            }
            else
            {

                if (bounded)
                {
                    // Expand the rectangle to allow the edges to be cetered within the thick edges.
                    CalculateLineWidthOffsets(lineWidth, out int halfWidth, out int fullWidth);

                    x1 += halfWidth;
                    y1 += halfWidth;
                    x2 -= halfWidth;
                    y2 -= halfWidth;
                }

                Primitives2D.CalculateLineWidthOffsets(lineWidth, out int hw, out int fw);

                int left = (int)x1 - hw;
                int top = (int)y1 - hw;
                int right = (int)x2 + hw;
                int bottom = (int)y2 + hw;


                //FillRect(spriteBatch, left - 1, top - 1, right + 1, bottom + 1, Color.Red);


                // Top
                FillRect(spriteBatch, left, top, right, top + fw, color);

                // Bottom
                FillRect(spriteBatch, left, bottom, right, bottom - fw, color);

                // Left
                FillRect(spriteBatch, left, top + fw, left + fw, bottom - fw, color);

                // Right
                FillRect(spriteBatch, right - fw, top + fw, right, bottom - fw, color);

            }

        }

        public static void FillRect(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Color color)
        {
            float rotation = 0f;
            float width = x2 - x1;
            float height = y2 - y1;

            if (width == 0 || height == 0) return;
            if (width < 0)
            {
                float t = x1;
                x1 = x2;
                x2 = t;
                width = -width;
            }

            if (height < 0)
            {
                float t = y1;
                y1 = y2;
                y2 = t;
                height = -height;
            }

            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1, y1), null, color, rotation, new Vector2(0, 0), new Vector2(width, height), SpriteEffects.None, 0f);
        }

        public static void FillRect(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            float rotation = 0f;

            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(rectangle.Left, rectangle.Top), null, color, rotation, new Vector2(0, 0), new Vector2(rectangle.Width, rectangle.Height), SpriteEffects.None, 0f);

        }
        #endregion

        #region Rounded Rectangles

        public static void DrawRoundedRect(SpriteBatch spriteBatch, Rectangle rectangle, float radius, Color color, float lineWidth = 1f, bool bounded = false)
        {
            DrawRoundedRect(spriteBatch, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, radius, color, lineWidth, bounded);
        }


        /// <summary>
        /// If bounded = true, then the entire rectangle with thick edges is contained within the (x1,y1)-(x2,y2) bounds
        /// If bounded = false, then the thick edges are centered on the specified bounds, i.e. they will extend beyound the bounds
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        /// <param name="lineWidth"></param>
        /// <param name="bounded"></param>
        public static void DrawRoundedRect(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float radius, Color color, float lineWidth = 1f, bool bounded = false)
        {
            if (!bounded && lineWidth > 1f)
            {
                // Expand the rectangle to allow the edges to be cetered within the thick edges.
                CalculateLineWidthOffsets(lineWidth, out int halfWidth, out int fullWidth);

                x1 -= halfWidth;
                y1 -= halfWidth;
                x2 += halfWidth;
                y2 += halfWidth;
            }


            float width = x2 - x1;
            float height = y2 - y1;

            if (width == 0 || height == 0) return;
            if (width < 0)
            {
                float t1 = x1;
                x1 = x2;
                x2 = t1;
                //width = -width;
            }

            //if (height < 0)
            //{
            //    height = -height;
            //}

            if (radius < 0)
            {
                radius = 0;
            }

            //if (lineWidth > radius)
            //{
            //    lineWidth = radius;
            //}

            float cx1 = x1 + radius;
            float cy1 = y1 + radius;
            float cx2 = x2 - radius;
            float cy2 = y2 - radius;

            Vector2 cTL = new Vector2(cx1, cy1);

            float radInner = radius - lineWidth;
            float x = radius;

            for (int a = 0; a < radius; a++)
            {
                Vector2 p2 = new Vector2(cx1 - x, cy1 - a);
                Vector2 p3 = new Vector2(cx1 - a, cy1 - x);

                if (p2.X >= p3.X)
                {
                    break;
                }

                float dist3 = Vector2.Distance(p2, cTL) - radius;

                while (dist3 > 1f)
                {
                    p2 = p2 with { X = p2.X + 1f };
                    p3 = p3 with { Y = p3.Y + 1f };

                    dist3 = Vector2.Distance(p2, cTL) - radius;
                    x -= 1;
                }

                float alias = 1f - dist3;
                float lineTotal = 0f;

                do
                {
                    var pixelColor = color with { A = (byte)((float)color.A * alias) };

                    var p2TL = p2;
                    var p2TR = p2 with { X = -p2.X + cx1 + cx2 - 1 };
                    var p2BL = p2 with { Y = -p2.Y + cy1 + cy2 - 1 };
                    var p2BR = p2 with { X = -p2.X + cx1 + cx2 - 1, Y = -p2.Y + cy1 + cy2 - 1 };

                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);


                    if (p3.Y < p2.Y)
                    {
                        var p3TL = p3;
                        var p3TR = p3 with { X = -p3.X + cx1 + cx2 - 1 };
                        var p3BL = p3 with { Y = -p3.Y + cy1 + cy2 - 1 };
                        var p3BR = p3 with { X = -p3.X + cx1 + cx2 - 1, Y = -p3.Y + cy1 + cy2 - 1 };

                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3TL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3TR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3BL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3BR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    }

                    lineTotal += alias;


                    p2 = p2 with { X = p2.X + 1.0f };
                    p3 = p3 with { Y = p3.Y + 1.0f };

                    dist3 = Vector2.Distance(p2, cTL);

                    float diff = dist3 - radInner;
                    alias = Math.Min(diff, 1f);
                    //if (alias < 1f)
                    //{
                    //    alias = 1f - alias;
                    //}

                }
                while (dist3 >= radInner && p2.X <= cTL.X && p2.X <= p3.X);

            }

            // Draw Top and Bottom Lines
            float len = (x2 - radius) - (x1 + radius) - 2;
            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1 + radius + 1, y1), null, color, 0f, new Vector2(0, 0), new Vector2(len, lineWidth), SpriteEffects.None, 0f);
            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1 + radius + 1, y2 - lineWidth), null, color, 0f, new Vector2(0, 0), new Vector2(len, lineWidth), SpriteEffects.None, 0f);

            // Draw Left and Right Lines
            len = (y2 - radius) - (y1 + radius) - 2;
            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1, y1 + radius + 1), null, color, 0f, new Vector2(0, 0), new Vector2(lineWidth, len), SpriteEffects.None, 0f);
            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x2 - lineWidth, y1 + radius + 1), null, color, 0f, new Vector2(0, 0), new Vector2(lineWidth, len), SpriteEffects.None, 0f);

        }


        public static void FillRoundedRect(SpriteBatch spriteBatch, Rectangle rectangle, float radius, Color color)
        {
            FillRoundedRect(spriteBatch, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, radius, color);
        }

        public static void FillRoundedRect(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float radius, Color color)
        {
            float width = x2 - x1;
            float height = y2 - y1;

            if (width == 0 || height == 0) return;
            if (width < 0)
            {
                float t1 = x1;
                x1 = x2;
                x2 = t1;
                //width = -width;
            }

            if (radius < 0)
            {
                radius = 0;
            }

            float lineWidth = 0f;

            float cx1 = x1 + radius;
            float cy1 = y1 + radius;
            float cx2 = x2 - radius + 1;
            float cy2 = y2 - radius + 1;

            Vector2 cTL = new Vector2(cx1 + 0.5f, cy1 + 0.5f);

            bool crossover = false;
            Vector2 p3 = Vector2.Zero;

            float oy = float.NegativeInfinity;

            float x = radius;
            for (int a = 0; a < radius; a++)
            {
                Vector2 p2 = new Vector2(cx1 - x - 0.5f, cy1 - a);
                if (!crossover)
                {
                    p3 = new Vector2(cx1 - a, cy1 - x - 0.5f);
                }

                if (!crossover && p2.X >= p3.X - lineWidth)
                {
                    crossover = true;
                }

                if (p2.X > p3.X)
                {
                    break;
                }


                float dist3 = Vector2.Distance(p2, cTL) - radius;

                while (dist3 > 1f)
                {
                    p2 = p2 with { X = p2.X + 1f };
                    p3 = p3 with { Y = p3.Y + 1f };

                    dist3 = Vector2.Distance(p2, cTL) - radius;
                    x -= 1;
                }

                // First pixel in row is anti-aliased
                float alias = 1f - dist3;
                var pixelColor = color with { A = (byte)((float)color.A * alias) };

                var p2TL = p2;
                var p2TR = p2 with { X = -p2.X + cx1 + cx2 - 1 };
                var p2BL = p2 with { Y = -p2.Y + cy1 + cy2 - 2 };
                var p2BR = p2 with { X = -p2.X + cx1 + cx2 - 1, Y = -p2.Y + cy1 + cy2 - 2 };

                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TL with { X = p2TL.X + 1f }, null, color, 0f, new Vector2(0, 0), new Vector2(p2TR.X - p2TL.X - 1f, 1f), SpriteEffects.None, 0f);
                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BL with { X = p2BL.X + 1f }, null, color, 0f, new Vector2(0, 0), new Vector2(p2TR.X - p2TL.X - 1f, 1f), SpriteEffects.None, 0f);

                if (p2.Y > p3.Y)
                {
                    var p3TL = p3;
                    var p3TR = p3 with { X = -p3.X + cx1 + cx2 - 2 };
                    var p3BL = p3 with { Y = -p3.Y + cy1 + cy2 - 1 };
                    var p3BR = p3 with { X = -p3.X + cx1 + cx2 - 2, Y = -p3.Y + cy1 + cy2 - 1 };

                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3TL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3TR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3BL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3BR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

                    if (oy != p3.Y)
                    {
                        oy = p3.Y;

                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3TL with { X = p3TL.X + 1f }, null, color, 0f, new Vector2(0, 0), new Vector2(p3TR.X - p3TL.X - 1f, 1f), SpriteEffects.None, 0f);
                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3BL with { X = p3TL.X + 1f }, null, color, 0f, new Vector2(0, 0), new Vector2(p3TR.X - p3TL.X - 1f, 1f), SpriteEffects.None, 0f);
                    }
                }


            }

            // Draw Middle Rectangle
            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1, y1 + radius + 1f), null, color, 0f, new Vector2(0, 0), new Vector2(width, height - radius * 2f - 2f), SpriteEffects.None, 0f);

        }

        public static void FillRoundedRectCornersInverted(SpriteBatch spriteBatch, Rectangle rectangle, float radius, Color color)
        {
            FillRoundedRectCornersInverted(spriteBatch, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, radius, color);
        }

        public static void FillRoundedRectCornersInverted(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float radius, Color color)
        {
            float width = x2 - x1;
            float height = y2 - y1;

            if (width == 0 || height == 0) return;
            if (width < 0)
            {
                float t1 = x1;
                x1 = x2;
                x2 = t1;
                //width = -width;
            }

            if (radius < 0)
            {
                radius = 0;
            }

            float lineWidth = 0f;

            float cx1 = x1 + radius;
            float cy1 = y1 + radius;
            float cx2 = x2 - radius + 1;
            float cy2 = y2 - radius + 1;

            Vector2 cTL = new Vector2(cx1 + 0.5f, cy1 + 0.5f);

            bool crossover = false;
            Vector2 p3 = Vector2.Zero;

            float oy = float.NegativeInfinity;

            float x = radius;
            for (int a = 0; a < radius; a++)
            {
                Vector2 p2 = new Vector2(cx1 - x - 0.5f, cy1 - a);
                if (!crossover)
                {
                    p3 = new Vector2(cx1 - a, cy1 - x - 0.5f);
                }

                if (!crossover && p2.X >= p3.X - lineWidth)
                {
                    crossover = true;
                }

                if (p2.X > p3.X)
                {
                    break;
                }


                float dist3 = Vector2.Distance(p2, cTL) - radius;

                while (dist3 > 1f)
                {
                    p2 = p2 with { X = p2.X + 1f };
                    p3 = p3 with { Y = p3.Y + 1f };

                    dist3 = Vector2.Distance(p2, cTL) - radius;
                    x -= 1;
                }

                // First pixel in row is anti-aliased
                float alias = 1f - dist3;
                var pixelColor = color with { A = (byte)((float)color.A * alias) };

                var p2TL = p2;
                var p2TR = p2 with { X = -p2.X + cx1 + cx2 - 1 };
                var p2BL = p2 with { Y = -p2.Y + cy1 + cy2 - 2 };
                var p2BR = p2 with { X = -p2.X + cx1 + cx2 - 1, Y = -p2.Y + cy1 + cy2 - 2 };

                //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

                //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TL with { X = p2TL.X + 1f }, null, color, 0f, new Vector2(0, 0), new Vector2(p2TR.X - p2TL.X - 1f, 1f), SpriteEffects.None, 0f);
                //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BL with { X = p2BL.X + 1f }, null, color, 0f, new Vector2(0, 0), new Vector2(p2TR.X - p2TL.X - 1f, 1f), SpriteEffects.None, 0f);

                float len = p2TL.X - x1 + 1;
                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TL with { X = x1 }, null, color, 0f, new Vector2(0, 0), new Vector2(len, 1f), SpriteEffects.None, 0f);
                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TR with { X = x2 - len }, null, color, 0f, new Vector2(0, 0), new Vector2(len, 1f), SpriteEffects.None, 0f);
                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BL with { X = x1 }, null, color, 0f, new Vector2(0, 0), new Vector2(len, 1f), SpriteEffects.None, 0f);
                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BR with { X = x2 - len }, null, color, 0f, new Vector2(0, 0), new Vector2(len, 1f), SpriteEffects.None, 0f);

                if (p2.Y > p3.Y && oy != p3.Y)
                {
                    oy = p3.Y;

                    var p3TL = p3;
                    var p3TR = p3 with { X = -p3.X + cx1 + cx2 - 2 };
                    var p3BL = p3 with { Y = -p3.Y + cy1 + cy2 - 1 };
                    var p3BR = p3 with { X = -p3.X + cx1 + cx2 - 2, Y = -p3.Y + cy1 + cy2 - 1 };

                    len = p3TL.X - x1 + 1;

                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3TL with { X = x1 }, null, color, 0f, new Vector2(0, 0), new Vector2(len, 1f), SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3TR with { X = x2 - len }, null, color, 0f, new Vector2(0, 0), new Vector2(len, 1f), SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3BL with { X = x1 }, null, color, 0f, new Vector2(0, 0), new Vector2(len, 1f), SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3BR with { X = x2 - len }, null, color, 0f, new Vector2(0, 0), new Vector2(len, 1f), SpriteEffects.None, 0f);

                }


            }

        }

        #endregion

    }
}
