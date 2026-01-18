using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blade.MG.Primitives
{
    public static partial class Primitives2D
    {
        #region Arcs

        /// <summary>
        /// Draws an arc (portion of a circle outline) using line segments.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch to draw with.</param>
        /// <param name="center">The center point of the arc.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="startAngle">The starting angle in radians.</param>
        /// <param name="sweepAngle">The angle to sweep in radians.</param>
        /// <param name="color">The color of the arc.</param>
        /// <param name="thickness">The thickness of the arc line.</param>
        /// <param name="segments">Number of line segments to approximate the arc (default auto-calculated).</param>
        public static void DrawArc(SpriteBatch spriteBatch, Vector2 center, float radius, float startAngle, float sweepAngle, Color color, float thickness, int segments = 0)
        {
            if (radius <= 0 || thickness <= 0 || sweepAngle == 0)
                return;

            // Auto-calculate segments based on arc size if not specified
            if (segments <= 0)
            {
                segments = Math.Max(8, (int)(Math.Abs(sweepAngle) * radius * 0.5f));
            }

            var pixel = PixelTexture(spriteBatch.GraphicsDevice);
            float angleStep = sweepAngle / segments;

            Vector2 previousPoint = new(
                center.X + radius * MathF.Cos(startAngle),
                center.Y + radius * MathF.Sin(startAngle)
            );

            for (int i = 1; i <= segments; i++)
            {
                float angle = startAngle + angleStep * i;
                Vector2 currentPoint = new(
                    center.X + radius * MathF.Cos(angle),
                    center.Y + radius * MathF.Sin(angle)
                );

                DrawLine(spriteBatch, previousPoint, currentPoint, color, thickness);
                previousPoint = currentPoint;
            }
        }

        ///// <summary>
        ///// Draws a line between two points.
        ///// </summary>
        //public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness)
        //{
        //    var pixel = PixelTexture(spriteBatch.GraphicsDevice);
        //    Vector2 delta = end - start;
        //    float length = delta.Length();

        //    if (length < 0.001f)
        //        return;

        //    float rotation = MathF.Atan2(delta.Y, delta.X);
        //    Vector2 origin = new(0f, 0.5f);
        //    Vector2 scale = new(length, thickness);

        //    spriteBatch.Draw(pixel, start, null, color, rotation, origin, scale, SpriteEffects.None, 0f);
        //}

        #endregion
    }
}
