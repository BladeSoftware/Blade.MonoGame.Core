using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blade.MG.Primitives
{
    public static partial class Primitives2D
    {
        #region Circle Bitmaps
        public static void DrawCircleFast(SpriteBatch spriteBatch, Vector2 position, float radius, Color color)
        {
            DrawCircleFast(spriteBatch, position.X, position.Y, radius, color);
        }

        public static void DrawCircleFast(SpriteBatch spriteBatch, Vector3 position, float radius, Color color)
        {
            DrawCircleFast(spriteBatch, position.X, position.Y, radius, color);
        }

        public static void DrawCircleFast(SpriteBatch spriteBatch, float x, float y, float radius, Color color)
        {
            spriteBatch.Draw(CircleTexture(spriteBatch), new Vector2(x, y), null, color, 0f, new Vector2(50, 50), radius / 50f, SpriteEffects.None, 0f);
        }


        public static void FillCircleFast(SpriteBatch spriteBatch, Vector2 position, float radius, Color color)
        {
            FillCircleFast(spriteBatch, position.X, position.Y, radius, color);
        }

        public static void FillCircleFast(SpriteBatch spriteBatch, Vector3 position, float radius, Color color)
        {
            FillCircleFast(spriteBatch, position.X, position.Y, radius, color);
        }

        public static void FillCircleFast(SpriteBatch spriteBatch, float x, float y, float radius, Color color)
        {
            spriteBatch.Draw(FilledCircleTexture(spriteBatch), new Vector2(x, y), null, color, 0f, new Vector2(50, 50), radius / 50f, SpriteEffects.None, 0f);
        }

        #endregion

        #region Circles
        public static void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, Color color)
        {
            DrawCircle(spriteBatch, center.X, center.Y, radius, color);
        }
        public static void DrawCircle(SpriteBatch spriteBatch, Vector3 center, float radius, Color color)
        {
            DrawCircle(spriteBatch, center.X, center.Y, radius, color);
        }

        public static void DrawCircle(SpriteBatch spriteBatch, float x, float y, float radius, Color color)
        {
            if (spriteBatch == null)
            {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            int target = 0;
            int a = (int)radius;
            int b = 0;
            int t;


            Vector2 pixelScale = new Vector2(1f, 1f);
            float hw = 1f / 2f;

            int r2 = a * a; // radius^2;
            while (a >= b)
            {

                b = (int)Math.Round(Math.Sqrt(r2 - a * a));

                // SWAP(target, b);
                t = target; target = b; b = t;

                while (b < target)
                {
                    int af = (100 * a) / 100;
                    int bf = (100 * b) / 100;

                    Color color2 = Color.Red;

                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x + af - hw, y + b - hw), null, color, 0f, new Vector2(0, 0), pixelScale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x + bf - hw, y + a - hw), null, color, 0f, new Vector2(0, 0), pixelScale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x - af - hw, y + b - hw), null, color, 0f, new Vector2(0, 0), pixelScale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x - bf - hw, y + a - hw), null, color, 0f, new Vector2(0, 0), pixelScale, SpriteEffects.None, 0f);

                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x - af - hw, y - b - hw), null, color, 0f, new Vector2(0, 0), pixelScale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x - bf - hw, y - a - hw), null, color, 0f, new Vector2(0, 0), pixelScale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x + af - hw, y - b - hw), null, color, 0f, new Vector2(0, 0), pixelScale, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x + bf - hw, y - a - hw), null, color, 0f, new Vector2(0, 0), pixelScale, SpriteEffects.None, 0f);


                    b += 1;
                }
                a -= 1;
            };

        }

        public static void DrawCircle(SpriteBatch spriteBatch, Vector2 center, float radius, Color color, float lineWidth, bool bounded = false)
        {
            DrawCircle(spriteBatch, center.X, center.Y, radius, color, lineWidth, bounded);
        }

        public static void DrawCircle(SpriteBatch spriteBatch, float cx, float cy, float radius, Color color, float lineWidth, bool bounded = false)
        {
            if (spriteBatch == null)
            {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            float x1 = cx - radius;
            float y1 = cy - radius;
            float x2 = cx + radius;
            float y2 = cy + radius;

            if (!bounded && lineWidth > 1f)
            {
                // Expand the rectangle to allow the edges to be cetered within the thick edges.
                CalculateLineWidthOffsets(lineWidth, out int halfWidth, out int fullWidth);

                radius += halfWidth;

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
            if (lineWidth > radius)
            {
                lineWidth = radius;
            }

            float cx1 = x1 + radius;
            float cy1 = y1 + radius;
            float cx2 = x2 - radius;
            float cy2 = y2 - radius;

            Vector2 cTL = new Vector2(cx1, cy1);

            float radInner = radius - lineWidth;
            float x = radius;

            for (int a = 1; a < radius; a++)
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


        }

        public static void FillCircle(SpriteBatch spriteBatch, Vector3 vector3, float radius, Color color)
        {
            FillCircle(spriteBatch, vector3.X, vector3.Y, radius, color);
        }

        public static void FillCircle(SpriteBatch spriteBatch, Vector2 vector2, float radius, Color color)
        {
            FillCircle(spriteBatch, vector2.X, vector2.Y, radius, color);
        }

        public static void FillCircle(SpriteBatch spriteBatch, float x, float y, float radius, Color color)
        {
            if (spriteBatch == null)
            {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            int target = 0;
            int a = (int)radius;
            int b = 0;
            int t;

            int r2 = a * a; // radius^2;
            while (a >= b)
            {

                b = (int)Math.Round(Math.Sqrt(r2 - a * a));

                // SWAP(target, b);
                t = target; target = b; b = t;

                while (b < target)
                {
                    int af = (100 * a) / 100;
                    int bf = (100 * b) / 100;

                    DrawHLine(spriteBatch, y - a, x - bf, x + bf, color);
                    DrawHLine(spriteBatch, y - b, x - af, x + af, color);
                    DrawHLine(spriteBatch, y + b, x - af, x + af, color);
                    DrawHLine(spriteBatch, y + a, x - bf, x + bf, color);

                    b += 1;
                }
                a -= 1;
            };

        }
        #endregion

    }
}
