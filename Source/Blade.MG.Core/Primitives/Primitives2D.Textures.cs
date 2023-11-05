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
                    var saveRenderTargets = graphicsDevice.GetRenderTargets();

                    circleTexture = new RenderTarget2D(graphicsDevice, 100, 100);
                    graphicsDevice.SetRenderTarget((RenderTarget2D)circleTexture);
                    graphicsDevice.Clear(Color.Transparent);

                    newSpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, null);
                    DrawCircle(newSpriteBatch, 100f / 2f, 100f / 2f, 50f, Color.Red);
                    newSpriteBatch.End();

                    graphicsDevice.SetRenderTargets(saveRenderTargets);
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
                    var saveRenderTargets = graphicsDevice.GetRenderTargets();

                    filledCircleTexture = new RenderTarget2D(graphicsDevice, 100, 100);
                    graphicsDevice.SetRenderTarget((RenderTarget2D)filledCircleTexture);
                    graphicsDevice.Clear(Color.Transparent);

                    newSpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, null);
                    FillCircle(newSpriteBatch, 100f / 2f, 100f / 2f, 50f, Color.Red);
                    newSpriteBatch.End();

                    graphicsDevice.SetRenderTargets(saveRenderTargets);
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
                    var saveRenderTargets = graphicsDevice.GetRenderTargets();

                    transparencyGridTexture = new RenderTarget2D(graphicsDevice, 16, 16);
                    graphicsDevice.SetRenderTarget((RenderTarget2D)transparencyGridTexture);
                    graphicsDevice.Clear(Color.White);

                    newSpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, null);
                    Primitives2D.FillRect(newSpriteBatch, 0, 0, 8, 8, new Color(191, 191, 191));
                    Primitives2D.FillRect(newSpriteBatch, 8, 8, 16, 16, new Color(191, 191, 191));
                    newSpriteBatch.End();

                    graphicsDevice.SetRenderTargets(saveRenderTargets);
                }
            }

            return transparencyGridTexture;
        }


        public static Texture2D NewRenderTarget(GraphicsDevice graphicsDevice, Vector2 size, Color? clearColor = null)
        {
            return NewRenderTarget(graphicsDevice, (int)size.X, (int)size.Y, clearColor);
        }

        public static RenderTarget2D NewRenderTarget(GraphicsDevice graphicsDevice, int width, int height, Color? clearColor = null)
        {
            var texture = new RenderTarget2D(graphicsDevice, width, height);

            if (clearColor != null)
            {
                var saveRenderTargets = graphicsDevice.GetRenderTargets();

                graphicsDevice.SetRenderTarget(texture);
                graphicsDevice.Clear(clearColor.Value);

                graphicsDevice.SetRenderTargets(saveRenderTargets);
            }

            return texture;
        }

        public static RenderTargetBinding[] SwitchRenderTarget(GraphicsDevice graphicsDevice, RenderTarget2D newRenderTarget2D)
        {
            var saveRenderTargets = graphicsDevice.GetRenderTargets();
            graphicsDevice.SetRenderTarget(newRenderTarget2D);

            return saveRenderTargets;
        }

        public static void RestoreRenderTarget(GraphicsDevice graphicsDevice, RenderTargetBinding[] savedRenderTargets)
        {
            graphicsDevice.SetRenderTargets(savedRenderTargets);
        }

        #endregion

    }
}
