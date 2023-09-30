using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

namespace Blade.MG.Core.Primitives
{
    public static partial class Primitives2D
    {
        internal static void HalfWidth(float width, out int halfWidth, out int fullWidth)
        {
            int hw = (int)Math.Floor(width);
            if (hw <= 1)
            {
                halfWidth = 0;
                fullWidth = 0;
                return;
            }

            if (int.IsOddInteger(hw))
            {
                halfWidth = (hw - 1) / 2;
                fullWidth = halfWidth * 2 + 1;

            }
            else
            {
                halfWidth = (hw / 2) - 1;
                fullWidth = halfWidth * 2 + 2;
            }
        }

        #region Textures

        //private static Texture2D Pixel(Game game) => Content.Load<Texture2D>("Images/pixel");
        //private static Texture2D Circle(Game game) => Content.Load<Texture2D>("Images/circle");
        //private static Texture2D CircleSolid(Game game) => Content.Load<Texture2D>("Images/circle_solid");

        private static Texture2D pixelTexture = null;
        public static Texture2D PixelTexture(GraphicsDevice graphicsDevice)
        {
            //return Content.Load<Texture2D>("Images/pixel");
            if (pixelTexture == null)
            {
                pixelTexture = new Texture2D(graphicsDevice, 1, 1);
                pixelTexture.SetData<Color>(new Color[] { new Color(Color.White, 1f) }, 0, 1);  // Color, array Start Index, Count
            }

            return pixelTexture;
        }

        private static Texture2D circleTexture = null;
        public static Texture2D CircleTexture(SpriteBatch spriteBatch)
        {
            // return Content.Load<Texture2D>("Images/circle");
            if (circleTexture == null)
            {
                using (var newSpriteBatch = new SpriteBatch(spriteBatch.GraphicsDevice))
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
        public static Texture2D FilledCircleTexture(SpriteBatch spriteBatch)
        {
            // return Content.Load<Texture2D>("Images/circle");
            if (filledCircleTexture == null)
            {
                using (var newSpriteBatch = new SpriteBatch(spriteBatch.GraphicsDevice))
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
            //return game.Content.Load<Texture2D>("Images/pixel");
            if (transparencyGridTexture == null)
            {
                //transparencyGridTexture = new Texture2D(graphicsDevice, 16, 16);

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
        #endregion


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

        #region Ellipse Bitmaps
        public static void DrawEllipseFast(SpriteBatch spriteBatch, Vector2 position, Vector2 radius, Color color)
        {
            DrawEllipseFast(spriteBatch, position.X, position.Y, radius.X, radius.Y, color);
        }

        public static void DrawEllipseFast(SpriteBatch spriteBatch, Vector3 position, float radiusX, float radiusY, Color color)
        {
            DrawEllipseFast(spriteBatch, position.X, position.Y, radiusX, radiusY, color);
        }

        public static void DrawEllipseFast(SpriteBatch spriteBatch, Vector3 position, Vector2 radius, Color color)
        {
            DrawEllipseFast(spriteBatch, position.X, position.Y, radius.X, radius.Y, color);
        }

        public static void DrawEllipseFast(SpriteBatch spriteBatch, Vector3 position, Vector3 radius, Color color)
        {
            DrawEllipseFast(spriteBatch, position.X, position.Y, radius.X, radius.Y, color);
        }

        public static void DrawEllipseFast(SpriteBatch spriteBatch, float x, float y, float radiusX, float radiusY, Color color)
        {
            spriteBatch.Draw(CircleTexture(spriteBatch), new Vector2(x, y), null, color, 0f, new Vector2(50, 50), new Vector2(radiusX / 50f, radiusY / 50f), SpriteEffects.None, 0f);
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


                    //            spriteBatch.Draw(Pixel(game), new Vector2(x + af, y + b), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x + bf, y + a), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x - af, y + b), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x - bf, y + a), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

                    //            spriteBatch.Draw(Pixel(game), new Vector2(x - af, y - b), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x - bf, y - a), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x + af, y - b), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x + bf, y - a), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

                    b += 1;
                }
                a -= 1;
            };

        }

        public static void DrawCircle(SpriteBatch spriteBatch, float x, float y, float radius, Color color, float lineWidth)
        {
            if (spriteBatch == null)
            {
                throw new ArgumentNullException(nameof(spriteBatch));
            }

            int target = 0;
            int a = (int)radius;
            int b = 0;
            int t;


            //Vector2 pixelScale = new Vector2(lineWidth, lineWidth);
            float hw = lineWidth / 2f;

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

                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x + af - hw, y + b - hw), null, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x + bf - hw, y + a - hw), null, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x - af - hw, y + b - hw), null, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x - bf - hw, y + a - hw), null, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x - af - hw, y - b - hw), null, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x - bf - hw, y - a - hw), null, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x + af - hw, y - b - hw), null, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x + bf - hw, y - a - hw), null, color, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);


                    //            spriteBatch.Draw(Pixel(game), new Vector2(x + af, y + b), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x + bf, y + a), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x - af, y + b), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x - bf, y + a), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

                    //            spriteBatch.Draw(Pixel(game), new Vector2(x - af, y - b), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x - bf, y - a), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x + af, y - b), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
                    //            spriteBatch.Draw(Pixel(game), new Vector2(x + bf, y - a), null, color, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

                    b += 1;
                }
                a -= 1;
            };

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

                    //IF Border<> No_Border THEN
                    //  BEGIN
                    //    VPutPixel(x+af, y + b, Border );
                    //VPutPixel(x + bf, y + a, Border);
                    //VPutPixel(x - af, y + b, Border);
                    //VPutPixel(x - bf, y + a, Border);
                    //VPutPixel(x - af, y - b, Border);
                    //VPutPixel(x - bf, y - a, Border);
                    //VPutPixel(x + af, y - b, Border);
                    //VPutPixel(x + bf, y - a, Border);
                    //END;


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

        #region Ellipses
        //        {---------------------------------------------------------------------------}
        //    PROCEDURE VEllipse(X1, Y1, Xr, Yr : Integer);
        //    VAR X, Y : Integer;
        //    A,ASquared,TwoASquared : Longint;
        //    B,BSquared,TwoBSquared : Longint;
        //    D,Dx,Dy                : Longint;

        //    PROCEDURE DoFast;
        //    BEGIN
        //      While(Y > 0) DO  { Fast Slope }
        //IF(D< 0) THEN
        //BEGIN
        //              Inc(X);
        //VPutPixel(X1+X, Y1+Y, Colour);
        //VPutPixel(X1-X, Y1+Y, Colour);
        //VPutPixel(X1+X, Y1-Y, Colour);
        //VPutPixel(X1-X, Y1-Y, Colour);
        //Dx := Dx + TwoBSquared;
        //              D := D + Dx;
        //            END
        //          ELSE
        //            BEGIN
        //              Dec(Y);
        //VPutPixel(X1+X+1, Y1+Y, Colour);
        //VPutPixel(X1-X-1, Y1+Y, Colour);
        //VPutPixel(X1+X+1, Y1-Y, Colour);
        //VPutPixel(X1-X-1, Y1-Y, Colour);
        //Dy := Dy - TwoASquared;
        //              D := D + ASquared - Dy;
        //            END;
        //      END;

        //    PROCEDURE DoSlow;
        //BEGIN
        //  While(Dx<Dy )  DO       { While Curve is slow }
        //          IF(D > 0) THEN
        //           BEGIN
        //              Dec(Y);
        //Dy := Dy - TwoASquared;
        //              D := D - Dy;
        //              VPutPixel(X1+X+1, Y1+Y, Colour);
        //VPutPixel(X1-X-1, Y1+Y, Colour);
        //VPutPixel(X1+X+1, Y1-Y, Colour);
        //VPutPixel(X1-X-1, Y1-Y, Colour);
        //Inc(X);
        //Dx := Dx + TwoBSquared;
        //              D := D + BSquared + Dx;
        //            END
        //          ELSE
        //            BEGIN
        //              INC(X);
        //VPutPixel(X1+X, Y1+Y, Colour);
        //VPutPixel(X1-X, Y1+Y, Colour);
        //VPutPixel(X1+X, Y1-Y, Colour);
        //VPutPixel(X1-X, Y1-Y, Colour);
        //Dx := Dx + TwoBSquared;
        //              D := D + BSquared + Dx;
        //            END;
        //      END;

        //BEGIN
        //  X := 0;
        //  Y := Yr;
        //  IF(Xr<Yr) THEN
        // BEGIN
        //      A := Minor(Xr, Yr);
        //B := Major(Xr, Yr);
        //END;
        //  IF(XR >= Yr) THEN
        //   BEGIN
        //      A := Major(Xr, Yr);
        //B := Minor(Xr, Yr);
        //END;
        //  ASquared := A* A;
        //TwoASquared := 2 * ASquared;

        //BSquared := B* B;
        //TwoBSquared := 2 * BSquared;
        //DX := 0;
        //  DY := TwoASquared* B;

        //VPutPixel(X1, Y1+Y, Colour);
        //VPutPixel(X1, Y1-Y, Colour);
        //VPutPixel(X1+Xr+1, Y1, Colour);
        //VPutPixel(X1-Xr-1, Y1, Colour);

        //IF(XR<Yr) THEN
        //BEGIN
        //      D := Trunc(BSquared - ASquared* B + ASquared );
        //DoFast;
        //      D := Trunc(D + (3* (ASquared - BSquared) /2 - (Dx + Dy)) /2);
        //      DoSlow;
        //    END;
        //  IF(XR >= Yr) THEN
        //   BEGIN
        //      D := Trunc(BSquared - ASquared* B + ASquared );
        //DoSlow;
        //      D := Trunc(D + (3* (ASquared - BSquared) /2 - (Dx + Dy)) /2);
        //      DoFast;
        //    END;
        //END;

        //{---------------------------------------------------------------------------}
        //PROCEDURE VFillEllipse(X1, Y1, Xr, Yr : Integer);
        //VAR X, Y                    : Integer;
        //    A,ASquared,TwoASquared : Longint;
        //    B,BSquared,TwoBSquared : Longint;
        //    D,Dx,Dy                : Longint;
        //    Hold                   : Byte;

        //    PROCEDURE DoFast;
        //BEGIN
        //  While(Y > 0) DO  { Fast Slope }
        //          IF(D< 0) THEN
        //          BEGIN
        //              Inc(X);
        //              { FCol,FCol2,Texture); }
        //              FunLine(X1-X, Y1+Y, X1+X);
        //FunLine(X1-X, Y1-Y, X1+X);
        //Dx := Dx + TwoBSquared;
        //              D := D + Dx;
        //            END
        //          ELSE
        //            BEGIN
        //              Dec(Y);
        //FunLine(X1-X-1, Y1+Y, X1+X+1);
        //FunLine(X1-X-1, Y1-Y, X1+X+1);
        //Dy := Dy - TwoASquared;
        //              D := D + ASquared - Dy;
        //            END;
        //      END;

        //    PROCEDURE DoSlow;
        //BEGIN
        //  While(Dx<Dy )  DO       { While Curve is slow }
        //          IF(D > 0) THEN
        //           BEGIN
        //              Dec(Y);
        //Dy := Dy - TwoASquared;
        //              D := D - Dy;
        //              FunLine(X1-X-1, Y1+Y, X1+X+1);
        //FunLine(X1-X-1, Y1-Y, X1+X+1);
        //Inc(X);
        //Dx := Dx + TwoBSquared;
        //              D := D + BSquared + Dx;
        //            END
        //          ELSE
        //            BEGIN
        //              INC(X);
        //FunLine(X1-X-1, Y1+Y, X1+X+1);
        //FunLine(X1-X-1, Y1-Y, X1+X+1);
        //Dx := Dx + TwoBSquared;
        //              D := D + BSquared + Dx;
        //            END;
        //      END;
        //BEGIN
        //  X := 0;
        //  Y := Yr;
        //  IF(Xr<Yr) THEN
        // BEGIN
        //      A := Minor(Xr, Yr);
        //B := Major(Xr, Yr);
        //END;
        //  IF(XR >= Yr) THEN
        //   BEGIN
        //      A := Major(Xr, Yr);
        //B := Minor(Xr, Yr);
        //END;
        //  ASquared := A* A;
        //TwoASquared := 2 * ASquared;

        //BSquared := B* B;
        //TwoBSquared := 2 * BSquared;
        //DX := 0;
        //  DY := TwoASquared* B;

        //IF(XR<Yr) THEN
        //BEGIN
        //      D := Trunc(BSquared - ASquared* B + ASquared );
        //DoFast;
        //      D := Trunc(D + (3* (ASquared - BSquared) /2 - (Dx + Dy)) /2);
        //      DoSlow;
        //    END;
        //  IF(XR >= Yr) THEN
        //   BEGIN
        //      D := Trunc(BSquared - ASquared* B + ASquared );
        //DoSlow;
        //      D := Trunc(D + (3* (ASquared - BSquared) /2 - (Dx + Dy)) /2);
        //      DoFast;
        //    END;
        //  IF Border<> No_Border THEN
        //    BEGIN
        //      Hold := Colour;
        //Colour := Border;
        //      Border := Hold;
        //      VEllipse(X1, Y1, Xr, Yr);
        //Hold := Border;
        //      BorDer := Colour;
        //      Colour := hold;
        //    END;
        //END;
        #endregion

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

        #region Rounded Rectangles

        public static void DrawRoundedRect(SpriteBatch spriteBatch, Rectangle rectangle, float radius, Color color, float lineWidth = 1f)
        {
            DrawRoundedRect(spriteBatch, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, radius, color, lineWidth);
        }

        //public static void DrawRoundedRect(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float radius, Color color, float lineWidth = 1f)
        //{
        //    if (x2 < x1)
        //    {
        //        float t = x2;
        //        x2 = x1;
        //        x1 = t;
        //    }

        //    if (y2 < y1)
        //    {
        //        float t = y2;
        //        y2 = y1;
        //        y1 = t;
        //    }

        //    float hw = lineWidth;

        //    double fi = 0f;
        //    double rad = (double)radius;
        //    double x = 0;

        //    float oxa = x1;
        //    float oxb = x2;

        //    for (int i = 0; i <= radius; i++)
        //    {

        //        double delta_fi = 1d / Math.Cos(fi) / rad;
        //        double s = rad * delta_fi;
        //        double delta_x = s * Math.Sin(fi);
        //        //double delta_y = s * Math.Cos(fi);

        //        fi += delta_fi;

        //        x += delta_x;
        //        float len = (float)x;

        //        float xa = x1 + len;
        //        float xb = x2 - len;

        //        DrawHLine(spriteBatch, y1 + radius - i, oxa, xa + hw, color);
        //        DrawHLine(spriteBatch, y1 + radius - i, xb - hw, oxb, color);

        //        DrawHLine(spriteBatch, y2 - radius + i, oxa, xa + hw, color);
        //        DrawHLine(spriteBatch, y2 - radius + i, xb - hw, oxb, color);

        //        oxa = xa;
        //        oxb = xb;
        //    }

        //    // Draw Middle Lines
        //    DrawHLine(spriteBatch, y1, x1 + radius, x2 - radius, color, lineWidth);
        //    DrawHLine(spriteBatch, y2, x1 + radius, x2 - radius, color, lineWidth);

        //    DrawVLine(spriteBatch, x1, y1 + radius, y2 - radius, color, lineWidth);
        //    DrawVLine(spriteBatch, x2, y1 + radius, y2 - radius, color, lineWidth);
        //}

        //public static void DrawRoundedRect(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float radius, Color color, float lineWidth = 1f)
        //{
        //    float width = x2 - x1;
        //    float height = y2 - y1;

        //    if (width == 0 || height == 0) return;
        //    if (width < 0)
        //    {
        //        float t1 = x1;
        //        x1 = x2;
        //        x2 = t1;
        //        //width = -width;
        //    }

        //    //if (height < 0)
        //    //{
        //    //    height = -height;
        //    //}

        //    // Draw Rounded Corners
        //    int target = 0;
        //    int a = (int)radius;
        //    int b = 0;
        //    int t;

        //    //Vector2 pixelScale = new Vector2(lineWidth, lineWidth);
        //    Vector2 pixelScale = new Vector2(1f, 1f);
        //    Vector2 pixelScaleH = new Vector2(lineWidth, 1f);

        //    float cx1 = x1 + radius;
        //    float cy1 = y1 + radius;
        //    float cx2 = x2 - radius + 1;
        //    float cy2 = y2 - radius + 1;

        //    float len = 0f;

        //    //int oa = int.MinValue;
        //    int oa = a;
        //    int ob = int.MinValue;

        //    int afMin = 0;
        //    int afMax = 0;

        //    int bfMin = 0;
        //    int bfMax = 0;

        //    int r2 = a * a; // radius^2;

        //    while (a >= b)
        //    {
        //        b = (int)Math.Round(Math.Sqrt(r2 - a * a));
        //        if (ob == int.MinValue) ob = b;

        //        // SWAP(target, b);
        //        t = target; target = b; b = t;

        //        while (b < target)
        //        {
        //            int af = (100 * a) / 100;
        //            int bf = (100 * b) / 100;

        //            if (ob == b)
        //            {
        //                if (af < afMin) afMin = af;
        //                if (af > afMax) afMax = af;
        //            }
        //            else
        //            {

        //                //if ((radius - b - 1) < lineWidth)
        //                if ((radius - b - 1) < 1)
        //                {
        //                    // Fill this line
        //                    float xLeft = cx1 - af - 1;
        //                    float xRight = cx2 + af - 1;
        //                    len = xRight - xLeft + 1;
        //                    Vector2 lineScale = new Vector2(len, lineWidth);

        //                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx1 - af - 1, cy1 - b - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);
        //                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx1 - af - 1, cy2 + b - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);
        //                }
        //                else
        //                {
        //                    // Draw borders Left and Right
        //                    len = float.Max(1f, lineWidth);
        //                    Vector2 lineScale = new Vector2(len, 1f);

        //                    // Top Left
        //                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx1 - af - 1, cy1 - b - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);

        //                    // Bottom Left
        //                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx1 - af - 1, cy2 + b - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);

        //                    // Top Right
        //                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx2 + af - len, cy1 - b - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);

        //                    // Bottom Right
        //                    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx2 + af - len, cy2 + b - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);
        //                }

        //                afMin = af;
        //                afMax = af;
        //            }

        //            if (oa == a)
        //            {
        //                if (bf < bfMin) bfMin = bf;
        //                if (bf > bfMax) bfMax = bf;
        //            }
        //            else
        //            {
        //                if (b != a)
        //                {
        //                    //if ((radius - a - 1) < lineWidth)
        //                    if ((radius - a - 1) < 1)
        //                    {
        //                        float xLeft = cx1 - bf - 1;
        //                        float xRight = cx2 + bf - 1;
        //                        len = xRight - xLeft + 1;
        //                        Vector2 lineScale = new Vector2(len, lineWidth);

        //                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx1 - bf - 1, cy1 - a - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);
        //                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx1 - bf - 1, cy2 + a - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);

        //                    }
        //                    else
        //                    {
        //                        //len = bfMax - bfMin + 1;
        //                        // Draw borders Left and Right
        //                        len = float.Max(1f, lineWidth);

        //                        Vector2 lineScale = new Vector2(len, 1f);

        //                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx1 - bf - 1, cy1 - a - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);
        //                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx1 - bf - 1, cy2 + a - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);

        //                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx2 + bf - len, cy1 - a - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);
        //                        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx2 + bf - len, cy2 + a - 1), null, color, 0f, new Vector2(0, 0), lineScale, SpriteEffects.None, 0f);
        //                    }
        //                }

        //                bfMin = bf;
        //                bfMax = bf;
        //            }


        //            oa = a;
        //            ob = b;

        //            b += 1;
        //        }
        //        a -= 1;
        //    }

        //    //// Draw Top and Bottom Lines
        //    //float len = (x2 - radius) - (x1 + radius - 1);
        //    //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1 + radius - 1, y1), null, color, 0f, new Vector2(0, 0), new Vector2(len, lineWidth), SpriteEffects.None, 0f);
        //    //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1 + radius - 1, y2 - lineWidth), null, color, 0f, new Vector2(0, 0), new Vector2(len, lineWidth), SpriteEffects.None, 0f);

        //    // Draw Left and Right Lines
        //    len = (y2 - radius) - (y1 + radius - 1) + 1;
        //    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1, y1 + radius - 1), null, color, 0f, new Vector2(0, 0), new Vector2(lineWidth, len), SpriteEffects.None, 0f);
        //    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x2 - lineWidth, y1 + radius - 1), null, color, 0f, new Vector2(0, 0), new Vector2(lineWidth, len), SpriteEffects.None, 0f);

        //}


        public static void DrawRoundedRect(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float radius, Color color, float lineWidth = 1f)
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

            //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx1, cy1), null, Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx2 - 1, cy1), null, Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx1, cy2 - 1), null, Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(cx2 - 1, cy2 - 1), null, Color.Black, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);


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

        //public static void TestingDrawRoundedRect(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float radius, Color color, float lineWidth = 1f)
        //{
        //    float width = x2 - x1;
        //    float height = y2 - y1;

        //    if (width == 0 || height == 0) return;
        //    if (width < 0)
        //    {
        //        float t1 = x1;
        //        x1 = x2;
        //        x2 = t1;
        //        //width = -width;
        //    }

        //    //if (height < 0)
        //    //{
        //    //    height = -height;
        //    //}

        //    if (radius < 0)
        //    {
        //        radius = 0;
        //    }
        //    if (lineWidth > radius)
        //    {
        //        lineWidth = radius;
        //    }

        //    float cx1 = x1 + radius;
        //    float cy1 = y1 + radius;
        //    float cx2 = x2 - radius + 1;
        //    float cy2 = y2 - radius + 1;

        //    Vector2 cTL = new Vector2(cx1 + 0.5f, cy1 + 0.5f);

        //    bool crossover = false;
        //    Vector2 p3 = Vector2.Zero;

        //    float radInner = radius - lineWidth;

        //    float x = radius;
        //    for (int a = 0; a < radius; a++)
        //    {
        //        Vector2 p2 = new Vector2(cx1 - x - 0.5f, cy1 - a);
        //        if (!crossover)
        //        {
        //            p3 = new Vector2(cx1 - a, cy1 - x - 0.5f);
        //        }

        //        if (!crossover && p2.X >= p3.X - lineWidth)
        //        {
        //            crossover = true;
        //        }

        //        if (p2.X > p3.X)
        //        {
        //            break;
        //        }

        //        float dist3 = Vector2.Distance(p2, cTL) - radius;

        //        while (dist3 > 1f)
        //        {
        //            p2 = p2 with { X = p2.X + 1f };
        //            p3 = p3 with { Y = p3.Y + 1f };

        //            dist3 = Vector2.Distance(p2, cTL) - radius;
        //            x -= 1;
        //        }

        //        // First pixel in row is anti-aliased
        //        float alias = 1f - dist3;
        //        float lineTotal = alias;
        //        var pixelColor = color with { A = (byte)((float)color.A * alias) };

        //        var p2TL = p2;
        //        var p2TR = p2 with { X = -p2.X + cx1 + cx2 - 1 };
        //        var p2BL = p2 with { Y = -p2.Y + cy1 + cy2 - 2 };
        //        var p2BR = p2 with { X = -p2.X + cx1 + cx2 - 1, Y = -p2.Y + cy1 + cy2 - 2 };

        //        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2TR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //        spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2BR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

        //        if (!crossover && p2.X <= p3.X)
        //        {
        //            var p3TL = p3;
        //            var p3TR = p3 with { X = -p3.X + cx1 + cx2 - 2 };
        //            var p3BL = p3 with { Y = -p3.Y + cy1 + cy2 - 1 };
        //            var p3BR = p3 with { X = -p3.X + cx1 + cx2 - 2, Y = -p3.Y + cy1 + cy2 - 1 };

        //            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3TL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3TR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3BL, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3BR, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //        }


        //        p2 = p2 with { X = p2.X + 1.0f };
        //        p3 = p3 with { Y = p3.Y + 1.0f };

        //        dist3 = Vector2.Distance(p2, cTL);

        //        while (dist3 >= radInner && p2.X <= cTL.X && p2.X <= p3.X + 1f)
        //        {
        //            float diff = dist3 - radInner;
        //            alias = Math.Min(diff, 1f);
        //            pixelColor = color with { A = (byte)((float)color.A * alias) };

        //            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2 with { X = -p2.X + cx1 + cx2 - 1 }, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2 with { Y = -p2.Y + cy1 + cy2 - 2 }, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //            spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p2 with { X = -p2.X + cx1 + cx2 - 1, Y = -p2.Y + cy1 + cy2 - 2 }, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

        //            if (!crossover)
        //            {
        //                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3 with { X = -p3.X + cx1 + cx2 - 2 }, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3 with { Y = -p3.Y + cy1 + cy2 - 1 }, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //                spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), p3 with { X = -p3.X + cx1 + cx2 - 2, Y = -p3.Y + cy1 + cy2 - 1 }, null, pixelColor, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
        //            }

        //            lineTotal += alias;


        //            p2 = p2 with { X = p2.X + 1.0f };
        //            p3 = p3 with { Y = p3.Y + 1.0f };

        //            dist3 = Vector2.Distance(p2, cTL);
        //        }

        //    }

        //  // Draw Top and Bottom Lines
        //  float len = (x2 - radius) - (x1 + radius - 1) - 3;
        //  spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1 + radius + 1, y1), null, color, 0f, new Vector2(0, 0), new Vector2(len, lineWidth), SpriteEffects.None, 0f);
        //  spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1 + radius + 1, y2 - lineWidth), null, color, 0f, new Vector2(0, 0), new Vector2(len, lineWidth), SpriteEffects.None, 0f);

        //  // Draw Left and Right Lines
        //  len = (y2 - radius) - (y1 + radius - 1) - 3;
        //  spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1, y1 + radius + 1), null, color, 0f, new Vector2(0, 0), new Vector2(lineWidth, len), SpriteEffects.None, 0f);
        //  spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x2 - lineWidth, y1 + radius + 1), null, color, 0f, new Vector2(0, 0), new Vector2(lineWidth, len), SpriteEffects.None, 0f);
        //}

        public static void FillRoundedRect(SpriteBatch spriteBatch, Rectangle rectangle, float radius, Color color)
        {
            FillRoundedRect(spriteBatch, rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom, radius, color);
        }


        ///// <summary>
        ///// </summary>
        ///// <remarks>Based on https://stackoverflow.com/a/43913660/22310480</remarks>
        //public static void FillRoundedRect(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float radius, Color color)
        //{
        //    if (x2 < x1)
        //    {
        //        float t = x2;
        //        x2 = x1;
        //        x1 = t;
        //    }

        //    if (y2 < y1)
        //    {
        //        float t = y2;
        //        y2 = y1;
        //        y1 = t;
        //    }

        //    double fi = 0f;
        //    double rad = (double)radius;
        //    double x = 0;

        //    for (int i = 1; i <= radius; i++)
        //    {

        //        double delta_fi = 1d / Math.Cos(fi) / rad;
        //        double s = rad * delta_fi;
        //        double delta_x = s * Math.Sin(fi);
        //        //double delta_y = s * Math.Cos(fi);

        //        fi += delta_fi;

        //        x += delta_x;
        //        float len = (float)x;

        //        DrawHLine(spriteBatch, y1 + radius - i, x1 + (float)len, x2 - (float)len, color);
        //        DrawHLine(spriteBatch, y2 - radius + i, x1 + (float)len, x2 - (float)len, color);
        //    }

        //    // Draw Middle Rectangle
        //    float rotation = 0f;
        //    float width = x2 - x1 + 1f;
        //    float height = y2 - y1 - 1f;
        //    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1, y1 + radius), null, color, rotation, new Vector2(0, 0), new Vector2(width, height - radius * 2f), SpriteEffects.None, 0f);

        //}

        //public static void FillRoundedRect(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float radius, Color color)
        //{
        //    float width = x2 - x1;
        //    float height = y2 - y1;

        //    if (width == 0 || height == 0) return;
        //    if (width < 0)
        //    {
        //        float t1 = x1;
        //        x1 = x2;
        //        x2 = t1;
        //        width = -width;
        //    }

        //    if (height < 0)
        //    {
        //        height = -height;
        //    }

        //    // Draw Rounded Corners
        //    int target = 0;
        //    int a = (int)radius;
        //    int b = 0;
        //    int t;

        //    float cx1 = x1 + radius;
        //    float cy1 = y1 + radius;
        //    float cx2 = x2 - radius + 1;
        //    float cy2 = y2 - radius + 1;

        //    int oa = int.MinValue;
        //    int ob = int.MinValue;


        //    int r2 = a * a; // radius^2;

        //    while (a >= b)
        //    {

        //        b = (int)Math.Round(Math.Sqrt(r2 - a * a));

        //        // SWAP(target, b);
        //        t = target; target = b; b = t;

        //        while (b < target)
        //        {
        //            int af = (100 * a) / 100;
        //            int bf = (100 * b) / 100;

        //            if (oa != a)
        //            {
        //                DrawHLine(spriteBatch, cy1 - a, cx1 - bf - 1, cx2 + bf, color);
        //                DrawHLine(spriteBatch, cy2 + a, cx1 - bf - 1, cx2 + bf, color);
        //            }

        //            if (ob != b && b != a)
        //            {
        //                DrawHLine(spriteBatch, cy1 - b, cx1 - af - 1, cx2 + af, color);
        //                DrawHLine(spriteBatch, cy2 + b, cx1 - af - 1, cx2 + af, color);
        //            }

        //            oa = a;
        //            ob = b;

        //            b += 1;
        //        }
        //        a -= 1;
        //    }

        //    // Draw Middle Rectangle
        //    spriteBatch.Draw(PixelTexture(spriteBatch.GraphicsDevice), new Vector2(x1, y1 + radius), null, color, 0f, new Vector2(0, 0), new Vector2(width, height - radius * 2f), SpriteEffects.None, 0f);
        //}

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

        //public static void FillRoundedRectCornersInverted(SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, float radius, Color color)
        //{
        //    float width = x2 - x1;
        //    float height = y2 - y1;

        //    if (width == 0 || height == 0) return;
        //    //if (width < 0)
        //    //{
        //    //    float t1 = x1;
        //    //    x1 = x2;
        //    //    x2 = t1;
        //    //    width = -width;
        //    //}

        //    //if (height < 0)
        //    //{
        //    //    height = -height;
        //    //}

        //    // Draw Rounded Corners
        //    int target = 0;
        //    int a = (int)radius;
        //    int b = 0;
        //    int t;


        //    int r2 = a * a; // radius^2;
        //    while (a >= b)
        //    {

        //        b = (int)Math.Round(Math.Sqrt(r2 - a * a));

        //        // SWAP(target, b);
        //        t = target; target = b; b = t;

        //        while (b < target)
        //        {
        //            int af = (100 * a) / 100;
        //            int bf = (100 * b) / 100;

        //            float cx1 = x1 + radius;
        //            float cy1 = y1 + radius;
        //            float cx2 = x2 - radius + 1;
        //            float cy2 = y2 - radius + 1;

        //            // Draw Top
        //            ////DrawHLine(game, spriteBatch, cy1 - b, cx1 - af - 1f, cx2 + af, color);
        //            ////DrawHLine(game, spriteBatch, cy1 - a, cx1 - bf - 1f, cx2 + bf, color);

        //            DrawHLine(spriteBatch, cy1 - b, x1, cx1 - af, color);
        //            DrawHLine(spriteBatch, cy1 - b, cx2 + af, x2, color);

        //            DrawHLine(spriteBatch, cy1 - a, x1, cx1 - bf, color);
        //            DrawHLine(spriteBatch, cy1 - a, cx2 + bf, x2, color);


        //            // Draw Bottom
        //            ////DrawHLine( spriteBatch, cy2 + b, cx1 - af - 1f, cx2 + af, color);
        //            ////DrawHLine( spriteBatch, cy2 + a, cx1 - bf - 1f, cx2 + bf, color);

        //            DrawHLine(spriteBatch, cy2 + b, x1, cx1 - af, color);
        //            DrawHLine(spriteBatch, cy2 + b, cx2 + af, x2, color);

        //            DrawHLine(spriteBatch, cy2 + a, x1, cx1 - bf, color);
        //            DrawHLine(spriteBatch, cy2 + a, cx2 + bf, x2, color);


        //            b += 1;
        //        }
        //        a -= 1;

        //    }

        //    //DrawHLine(spriteBatch, y1, x1, x2, color);
        //    //DrawHLine(spriteBatch, y2, x1, x2, color);
        //}

        #endregion

        // Returns true if the line segments intersect, otherwise false. 
        // In addition, if the lines intersect the intersection point will be stored in the floats i_x and i_y.
        private static bool get_line_segment_intersection(float p0_x, float p0_y, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, ref float i_x, ref float i_y)
        {
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x; s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x; s2_y = p3_y - p2_y;

            float det = (-s2_x * s1_y + s1_x * s2_y);
            if (det == 0)
            {
                return false;
            }

            float s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / det;
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / det;

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                // Collision detected
                i_x = p0_x + (t * s1_x);
                i_y = p0_y + (t * s1_y);
                return true;
            }

            return false; // No collision
        }

        // Returns true if the infinite lines intersect, otherwise false. 
        // In addition, if the lines intersect the intersection point will be stored in the floats i_x and i_y.
        private static bool get_line_intersection(float p0_x, float p0_y, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, ref float i_x, ref float i_y)
        {
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x; s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x; s2_y = p3_y - p2_y;

            float det = (-s2_x * s1_y + s1_x * s2_y);
            if (det == 0)
            {
                return false;
            }

            float s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / det;
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / det;

            if (s != 0 && t != 0)
            {
                // Collision detected
                i_x = p0_x + (t * s1_x);
                i_y = p0_y + (t * s1_y);
                return true;
            }

            return false; // No collision
        }

    }


    //public void DrawString(SpriteBatch spriteBatch, Rectangle rectangle, string text, SpriteFont spriteFont, Color? color, HorizontalAlignmentType horizontalAlignment = HorizontalAlignmentType.Left, VerticalAlignmentType verticalAlignment = VerticalAlignmentType.Center, Rectangle? clippingRect = null)
    //{
    //    SpriteFont font = spriteFont; // ?? Context.DefaultFont;
    //    Vector2 textSize = font.MeasureString(text);

    //    float x;
    //    float y;

    //    switch (horizontalAlignment)
    //    {
    //        case HorizontalAlignmentType.Left:
    //            x = rectangle.Left;
    //            break;

    //        case HorizontalAlignmentType.Center:
    //        case HorizontalAlignmentType.Stretch:
    //            x = ((rectangle.Left + rectangle.Right) / 2) - ((int)textSize.X / 2);
    //            if (x < rectangle.Left)
    //            {
    //                //x = rectangle.Left;
    //                x = rectangle.Right - textSize.X;
    //            }
    //            break;

    //        case HorizontalAlignmentType.Right:
    //            x = rectangle.Right - (int)textSize.X;
    //            break;

    //        default:
    //            x = rectangle.Left;
    //            break;
    //    }

    //    switch (verticalAlignment)
    //    {
    //        case VerticalAlignmentType.Top:
    //            y = rectangle.Top;
    //            break;

    //        case VerticalAlignmentType.Center:
    //        case VerticalAlignmentType.Stretch:
    //            y = ((rectangle.Top + rectangle.Bottom) / 2) - ((int)textSize.Y / 2);
    //            if (y < rectangle.Top)
    //            {
    //                y = rectangle.Top;
    //            }
    //            break;

    //        case VerticalAlignmentType.Bottom:
    //            y = rectangle.Bottom - (int)textSize.Y;
    //            break;

    //        default:
    //            y = rectangle.Left;
    //            break;
    //    }

    //    Vector2 textPosition = new Vector2(x, y);

    //    //if (clippingRect == null)
    //    //{
    //    //    ClipToRect(rectangle);
    //    //}
    //    //else
    //    //{
    //    //    ClipToRect(Rectangle.Intersect(clippingRect.Value, rectangle));
    //    //}


    //    spriteBatch.DrawString(font, text, textPosition, color ?? Color.White);
    //}

}
