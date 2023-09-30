using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blade.MG.Core.Primitives
{
    public static partial class Primitives2D
    {

        #region Rectangles
        public static void DrawRect(SpriteBatch spriteBatch, Rectangle rectangle, Color color, float lineWidth = 1f)
        {
            DrawRect(spriteBatch, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, color, lineWidth);
        }

        public static void DrawRect(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Color color, float lineWidth = 1f)
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

                Primitives2D.HalfWidth(lineWidth, out int hw, out int fw);

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

    }
}
