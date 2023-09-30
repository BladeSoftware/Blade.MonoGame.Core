using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blade.MG.Primitives
{
    public static partial class Primitives2D
    {
        #region Triangles
        public static void DrawTriangle(SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, Vector2 p3, Color color, float lineWidth = 1f)
        {
            Primitives2D.DrawLine(spriteBatch, p1, p2, color, lineWidth);
            Primitives2D.DrawLine(spriteBatch, p2, p3, color, lineWidth);
            Primitives2D.DrawLine(spriteBatch, p3, p1, color, lineWidth);
        }
        public static void DrawTriangle(SpriteBatch spriteBatch, Vector3 p1, Vector3 p2, Vector3 p3, Color color, float lineWidth = 1f)
        {
            Primitives2D.DrawLine(spriteBatch, p1, p2, color, lineWidth);
            Primitives2D.DrawLine(spriteBatch, p2, p3, color, lineWidth);
            Primitives2D.DrawLine(spriteBatch, p3, p1, color, lineWidth);
        }
        #endregion

        #region Quads
        public static void DrawQuad(SpriteBatch spriteBatch, Quad2D quad, Color color, float lineWidth = 1f)
        {
            Primitives2D.DrawLine(spriteBatch, quad.TL, quad.TR, color, lineWidth);
            Primitives2D.DrawLine(spriteBatch, quad.TR, quad.BR, color, lineWidth);
            Primitives2D.DrawLine(spriteBatch, quad.BR, quad.BL, color, lineWidth);
            Primitives2D.DrawLine(spriteBatch, quad.BL, quad.TL, color, lineWidth);
        }
        #endregion

    }
}
