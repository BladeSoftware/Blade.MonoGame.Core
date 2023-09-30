using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blade.MG.Core.Primitives
{
    public static partial class Primitives2D
    {
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

    }
}
