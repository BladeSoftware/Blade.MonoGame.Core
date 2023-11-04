using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blade.MG.Primitives
{
    public static partial class Primitives2D
    {
        #region Textures

        //private static Texture2D Pixel(Game game) => Content.Load<Texture2D>("Images/pixel");
        //private static Texture2D Circle(Game game) => Content.Load<Texture2D>("Images/circle");
        //private static Texture2D CircleSolid(Game game) => Content.Load<Texture2D>("Images/circle_solid");

        private static Texture2D pixelTexture = null;
        public static Texture2D PixelTexture(GraphicsDevice graphicsDevice)
        {
            if (pixelTexture == null)
            {
                pixelTexture = new Texture2D(graphicsDevice, 1, 1);
                pixelTexture.SetData<Color>(new Color[] { new Color(Color.White, 1f) }, 0, 1);  // Color, array Start Index, Count
            }

            return pixelTexture;
        }

        private static Texture2D circleTexture = null;
        public static Texture2D CircleTexture(GraphicsDevice graphicsDevice)
        {
            // return Content.Load<Texture2D>("Images/circle");
            if (circleTexture == null)
            {
                using (var newSpriteBatch = new SpriteBatch(graphicsDevice))
                {
                    var saveRenderTargets = newSpriteBatch.GraphicsDevice.GetRenderTargets();

                    circleTexture = new RenderTarget2D(newSpriteBatch.GraphicsDevice, 100, 100);
                    newSpriteBatch.GraphicsDevice.SetRenderTarget((RenderTarget2D)circleTexture);
                    newSpriteBatch.GraphicsDevice.Clear(Color.Transparent);

                    newSpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, null);
                    DrawCircle(newSpriteBatch, 100f / 2f, 100f / 2f, 50f, Color.Red);
                    newSpriteBatch.End();

                    newSpriteBatch.GraphicsDevice.SetRenderTargets(saveRenderTargets);
                }
            }

            return circleTexture;
        }

        private static Texture2D filledCircleTexture = null;
        public static Texture2D FilledCircleTexture(GraphicsDevice graphicsDevice)
        {
            if (filledCircleTexture == null)
            {
                using (var newSpriteBatch = new SpriteBatch(graphicsDevice))
                {
                    var saveRenderTargets = newSpriteBatch.GraphicsDevice.GetRenderTargets();

                    filledCircleTexture = new RenderTarget2D(newSpriteBatch.GraphicsDevice, 100, 100);
                    newSpriteBatch.GraphicsDevice.SetRenderTarget((RenderTarget2D)filledCircleTexture);
                    newSpriteBatch.GraphicsDevice.Clear(Color.Transparent);

                    newSpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, null);
                    FillCircle(newSpriteBatch, 100f / 2f, 100f / 2f, 50f, Color.Red);
                    newSpriteBatch.End();

                    newSpriteBatch.GraphicsDevice.SetRenderTargets(saveRenderTargets);
                }
            }

            return filledCircleTexture;
        }

        // Draw a White and Grey Checker pattern that represents transparency
        private static Texture2D transparencyGridTexture = null;
        public static Texture2D TransparencyGridTexture(GraphicsDevice graphicsDevice)
        {
            if (transparencyGridTexture == null)
            {
                using (var newSpriteBatch = new SpriteBatch(graphicsDevice))
                {
                    var saveRenderTargets = newSpriteBatch.GraphicsDevice.GetRenderTargets();

                    transparencyGridTexture = new RenderTarget2D(newSpriteBatch.GraphicsDevice, 16, 16);
                    newSpriteBatch.GraphicsDevice.SetRenderTarget((RenderTarget2D)transparencyGridTexture);
                    newSpriteBatch.GraphicsDevice.Clear(Color.White);

                    newSpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, null);
                    Primitives2D.FillRect(newSpriteBatch, 0, 0, 8, 8, new Color(191, 191, 191));
                    Primitives2D.FillRect(newSpriteBatch, 8, 8, 16, 16, new Color(191, 191, 191));
                    newSpriteBatch.End();

                    newSpriteBatch.GraphicsDevice.SetRenderTargets(saveRenderTargets);
                }
            }

            return transparencyGridTexture;
        }


        public static Texture2D NewRenderTarget(GraphicsDevice graphicsDevice, int width, int height, Color clearColor)
        {
            Texture2D texture = null;

            using (var newSpriteBatch = new SpriteBatch(graphicsDevice))
            {
                var saveRenderTargets = newSpriteBatch.GraphicsDevice.GetRenderTargets();

                texture = new RenderTarget2D(newSpriteBatch.GraphicsDevice, width, height);
                newSpriteBatch.GraphicsDevice.SetRenderTarget((RenderTarget2D)texture);
                newSpriteBatch.GraphicsDevice.Clear(clearColor);

                newSpriteBatch.GraphicsDevice.SetRenderTargets(saveRenderTargets);
            }

            return texture;
        }

        #endregion

    }
}
