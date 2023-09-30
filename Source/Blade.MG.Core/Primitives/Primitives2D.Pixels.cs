using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blade.MG.Core.Primitives
{
    public static partial class Primitives2D
    {

        #region Pixels
        public static void DrawPixel(SpriteBatch spriteBatch, Vector2 position, Color color, float scale = 1f, float layerDepth = 0f)
        {
            DrawPixel(spriteBatch, position.X, position.Y, color, scale);
        }

        public static void DrawPixel(SpriteBatch spriteBatch, Vector3 position, Color color, float scale = 1f, float layerDepth = 0f)
        {
            DrawPixel(spriteBatch, position.X, position.Y, color, scale);
        }

        public static void DrawPixel(SpriteBatch spriteBatch, float x, float y, Color color, float scale = 1f, float layerDepth = 0f)
        {
            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x - scale / 2f, y - scale / 2f), null, color, 0f, new Vector2(0, 0), scale, SpriteEffects.None, layerDepth);
        }
        #endregion

    }
}
