using Blade.MG.Input;
using Blade.MG.Primitives;
using Blade.MG.SVG;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Text;

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

            //Tests();

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

            InputManager.Update();

            // Check Input
            if (InputManager.Keyboard.IsKeyDown(Keys.Escape))
            {
#if !__IOS__
                this.Exit();
#endif
                return;
            }


            TestInput();


            if (InputManager.Keyboard.KeyPressed(Keys.Q)) bounded = !bounded;
            if (InputManager.Keyboard.KeyPressed(Keys.W)) rounded = !rounded;
            if (InputManager.Keyboard.KeyPressed(Keys.E)) showConstructorLines = !showConstructorLines;

        }


        RenderTarget2D svgImg = null;

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


            if (svgImg == null)
            {
                var svg = new SVGDocument();
                //var path = svg.AddPath("M1837 557L768 1627l-557-558 90-90 467 466 979-978 90 90z");

                var path = svg.AddPath("M16.972 6.251 c -0.967 -0.538 -2.185 -0.188 -2.72 0.777 l -3.713 6.682 -2.125 -2.125 c -0.781 -0.781 -2.047 -0.781 -2.828 0 -0.781 0.781 -0.781 2.047 0 2.828 l4 4c.378.379.888.587 1.414.587l.277-.02c.621-.087 1.166-.46 1.471-1.009l5-9c.537-.966.189-2.183-.776-2.72z");

                //var path = svg.AddPath("M311.050164,164.241829 C311.515694,164.468113 311.892625,164.838464 312.122929,165.295868 L316.099751,173.194183 L324.973849,174.469632 C326.268261,174.655674 327.164093,175.837503 326.974745,177.10932 C326.89957,177.614264 326.657566,178.080933 326.285988,178.437488 L319.8697,184.594364 L321.377373,193.280949 C321.597288,194.54801 320.730161,195.75033 319.440591,195.966406 C318.9286,196.052194 318.402102,195.970261 317.94215,195.733219 L309.999845,191.640063 L302.057539,195.733219 C300.899043,196.330263 299.467297,195.891509 298.859646,194.753235 C298.618394,194.301311 298.535005,193.784004 298.622316,193.280949 L300.129989,184.594364 L293.713702,178.437488 C292.777796,177.539421 292.760056,176.065936 293.674077,175.146367 C294.036966,174.781275 294.511926,174.543496 295.02584,174.469632 L303.899939,173.194183 L307.87676,165.295868 C308.456835,164.143789 309.877617,163.671879 311.050164,164.241829 Z");
                //var path = svg.AddPath("M311.050164,164.241829 C311.515694,164.468113 311.892625,164.838464 312.122929,165.295868 L316.099751,173.194183 L324.973849,174.469632 C326.268261,174.655674 327.164093,175.837503 326.974745,177.10932 C326.89957,177.614264 326.657566,178.080933 326.285988,178.437488 L319.8697,184.594364 L321.377373,193.280949 C321.597288,194.54801 320.730161,195.75033 319.440591,195.966406 C318.9286,196.052194 318.402102,195.970261 317.94215,195.733219 L309.999845,191.640063 L302.057539,195.733219 C300.899043,196.330263 299.467297,195.891509 298.859646,194.753235 C298.618394,194.301311 298.535005,193.784004 298.622316,193.280949 L300.129989,184.594364 L293.713702,178.437488 C292.777796,177.539421 292.760056,176.065936 293.674077,175.146367 C294.036966,174.781275 294.511926,174.543496 295.02584,174.469632 L303.899939,173.194183 L307.87676,165.295868 C308.456835,164.143789 309.877617,163.671879 311.050164,164.241829 Z M305.472113,175.320323 L295.368684,176.772461 L302.673821,183.78225 L300.95729,193.67219 L309.999845,189.012009 L319.042399,193.67219 L317.325868,183.78225 L324.631006,176.772461 L314.527576,175.320323 L309.999845,166.327854 L305.472113,175.320323 Z");

                path.Fill = new SVGFill { FillColor = Color.LightBlue, FillRule = FillRule.Nonzero };
                path.Stroke = Color.DarkMagenta;
                path.StrokeWidth = 20;

                svgImg = svg.ToTexture2D(GraphicsDevice, Matrix.CreateScale(1f), new Point(500, 500), 0);
            }

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
                        Primitives2D.DrawRoundedRect(spriteBatch, new Rectangle(center.X - w / 2, center.Y - h / 2, w, h), 18, Color.Purple, 20, bounded);
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

                    int svgWidth = 140;
                    int svgHeight = 140;

                    spriteBatch.Draw(svgImg, new Rectangle(center.X - svgWidth / 2, center.Y - svgHeight / 2, svgWidth, svgHeight), svgImg.Bounds, Color.White);

                }
                finally
                {
                    spriteBatch.End();
                }
            }

        }

        private void Tests()
        {
            Color color;

            color = ColorHelper.FromHexColor("#010203");
            color = ColorHelper.FromHexColor("#01020304");

            color = ColorHelper.FromJsonColor("""{"R":1,"G":2,"B":3}""");
            color = ColorHelper.FromJsonColor("""{"R":1,"G":2,"B":3,"A":4}""");
            color = ColorHelper.FromJsonColor("""{"R":1 "G":2 "B":3}""");
            color = ColorHelper.FromJsonColor("""{"R":1 "G":2 "B":3 "A":4}""");

            color = ColorHelper.FromString("""{"R":1 "G":2 "B":3 "A":4}""");

            string s = ColorHelper.ToHexColor(color);
            s = ColorHelper.ToJsonColor(color);

            color = ColorHelper.FromString(s);


        }

        private void TestInput()
        {

            //if (InputManager.Mouse.PrimaryButton.Pressed)
            //{
            //}

            //if (InputManager.Mouse.SecondaryButton.Pressed)
            //{
            //}


            //if (InputManager.Touch.IsConnected)
            //{

            //}

            //if (InputManager.Touch.TryGetGesture(out var gesture))
            //{

            //}


            //var x = InputManager.Mouse.PrimaryButton.I

            //var thumbStickLeft = InputManager.GamePad(0).ThumbStickLeft;
            //var thumbStickRight = InputManager.GamePad(0).ThumbStickRight;
            //var triggerLeft = InputManager.GamePad(0).TriggerLeft;
            //var triggerRight = InputManager.GamePad(0).TriggerRight;

            //bool x = InputManager.GamePad(PlayerIndex.One).DPad.Up.IsUp;
            //x = InputManager.GamePad(PlayerIndex.One).DPad.Up.IsDown;
            //x = InputManager.GamePad(PlayerIndex.One).DPad.Up.Pressed;
            //x = InputManager.GamePad(PlayerIndex.One).DPad.Up.Released;


            //InputManager.GamePad(0).PacketNumber;

            //InputManager.GamePad(PlayerIndex.One).Dpad;

            //InputManager.MouseState.PrimaryButton
            //InputManager.MouseState.PreviousMouseState.PrimaryButton == ButtonState.Pressed

        }

    }

}
