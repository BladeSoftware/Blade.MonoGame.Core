using Blade.MG.Core.Input;
using Blade.MG.Core.Primitives;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Examples
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class TestGame : Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private Texture2D testTexture;
        private bool bounded = false;
        private bool rounded = false;
        private bool showConstructorLines = true;

        public TestGame()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();


            IsFixedTimeStep = true;


            graphicsDeviceManager.IsFullScreen = false;
            graphicsDeviceManager.HardwareModeSwitch = false;

            if (graphicsDeviceManager.IsFullScreen)
            {
                graphicsDeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphicsDeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }
            else
            {
                // --- Set a pre-defined window size
                graphicsDeviceManager.PreferredBackBufferWidth = 1280;
                graphicsDeviceManager.PreferredBackBufferHeight = 720;
            }

            // -- Set allowed Orientations
            graphicsDeviceManager.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;

            // -- Should the mouse cursor be visible
            IsMouseVisible = true;


            graphicsDeviceManager.ApplyChanges();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            //var spriteBatch = new SpriteBatch(GraphicsDevice);

            // Example loading a texture using the content manager
            testTexture = this.Content.Load<Texture2D>("Images/spaceship1");

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            InputManager.Instance.Update();

            // Check Input
            if (InputManager.KeyboardState.IsKeyDown(Keys.Escape))
            {
#if !__IOS__
                this.Exit();
#endif
                return;
            }

            if (InputManager.KeyPressed(Keys.Q)) bounded = !bounded;
            if (InputManager.KeyPressed(Keys.W)) rounded = !rounded;
            if (InputManager.KeyPressed(Keys.E)) showConstructorLines = !showConstructorLines;
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // ---===== Draw Background / Skyboxes etc. =====---
            GraphicsDevice.Clear(Color.Black);

            var view = GraphicsDevice.Viewport.TitleSafeArea;
            var center = view.Center;

            base.Draw(gameTime);


            using (SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice))
            {
                try
                {
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, null, null, null);


                    Primitives2D.DrawLine(spriteBatch, 0, 0, view.Width, view.Height, Color.Yellow, 3);
                    Primitives2D.DrawLine(spriteBatch, view.Width, 0, 0, view.Height, Color.Yellow, 3);

                    int w = 250;
                    int h = 250;

                    Primitives2D.DrawPixel(spriteBatch, center.X, center.Y, Color.Orange, 1);

                    if (rounded)
                    {
                        Primitives2D.DrawRoundedRect(spriteBatch, new Rectangle(center.X - w / 2, center.Y - h / 2, w, h), 12, Color.Purple, 20, bounded);
                    }
                    else
                    {
                        Primitives2D.DrawRect(spriteBatch, new Rectangle(center.X - w / 2, center.Y - h / 2, w, h), Color.Purple, 20, bounded);
                    }

                    if (showConstructorLines)
                    {
                        Primitives2D.DrawRect(spriteBatch, new Rectangle(center.X - w / 2, center.Y - h / 2, w, h), Color.Yellow, 1);
                    }


                    Primitives2D.DrawCircle(spriteBatch, center.ToVector2(), 250f, Color.Green, 25, bounded);

                    if (showConstructorLines)
                    {
                        Primitives2D.DrawCircle(spriteBatch, center.ToVector2(), 250f, Color.Yellow, 1);
                    }

                }
                finally
                {
                    spriteBatch.End();
                }
            }

        }

    }

}
