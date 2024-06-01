using Microsoft.Xna.Framework;

namespace Blade.MG
{

    /// <summary>
    /// Calculates Splines and Patches
    /// </summary>
    /// <remarks>
    /// Most of this work is derived from a Post by Steve Noskowicz showing the basis matrices of various splines and their use. (Q10706@email.mot.com)
    /// At Last check, the Posting was still archived at http://news.povray.org/povray.binaries.tutorials/attachment/%3CXns91B880592482seed7@povray.org%3E/Splines.bas.txt
    /// I've extended the original to allow for Spline-Patches
    /// 
    /// This is the comments taken from the original post :
    ///
    /// -------------------------INTERPOLATION MINI GUIDE-------------------------
    /// 
    /// This is not a tutorial on interpolation. It shows a very common algorithm used
    /// for interpolation which I made general purpose to compare several
    /// interpolation types.  I added comments to explain some subtle aspects.
    ///
    /// I included some common types: Bézier, B-spline and Catmul-Rom. Some others I
    /// ran across are: parabolic, simple cubic, Hermite and beta with tension and bias.
    /// I completely derived the linear basis matrix myself (big deal).
    ///
    /// I also derived the Kochanek-Bartels from their 84 SIGGRAPH paper to provide
    /// global control of tension, bias, continuity  (it's been 25 years since I've
    /// done matrix math so it took a few passes). NO NURBS, I haven't gotten in that
    /// deep.
    ///
    /// I am not a math major nor an expert on splines and interpolation.  For the
    /// last 2-1/2 years I read comp.graphics.algorithms, did some searches and
    /// grabbed related things.  In Jan. '96, 4 months ago,  I decided to finally code
    /// something and found that it was rather simple, thanks mostly to Leon DeBoer's
    /// posting.
    ///
    /// This routine does interpolation on a 2D list in the array: IT(c,n) where c=1
    /// for the X coordinate, c=2 for Y and "n" the vector number. The same type of
    /// interpolation is done on both X and Y coordinates though this is not required
    /// in general and some interesting effects may be possible by using different
    /// interpolation types for the dimensions - room for a little art. There are
    /// comments for going to more dimensions later.  I set up this routine to study
    /// the interpolation types and debug the code with the goal of moving to key
    /// frame animation.
    ///
    /// There is a problem with terminology which I will now define.  Since Laser
    /// (light show) Graphics is "vector" graphics, the coordinate sets (X,Y here) are
    /// called vectors.  As this code is in my image editor, I use that term.  I think
    /// I use both vector and control point in my comments.  They are the same.  This
    /// algorithm considers all input image vectors as control points, even for
    /// Hermite (once you understand the Hermite, you'll see why this is incorrect for
    /// it).  For key frame animation, the input control points are called key frames
    /// or "keys".
    ///
    /// This is parametric interpolation, which means that the coordinate values are
    /// interpolated vs. a parameter, in this case J.  Y is NOT interpolated as a
    /// function of X as we might think to do.
    ///
    /// Because some types need data points before and after an interval in order to
    /// calculate a curve, the first and last vectors need extra data.  A common
    /// solution is simply to repeat the end vectors to provide the data.  This is
    /// done here.  This forces the curve to always hit the 1st and last vectors.
    /// This along with the way each type works can confuse what is going on. The
    /// extra values could be made changeable to gain more control of the curve at the
    /// end vectors.
    ///
    /// This is a general purpose routine, a single interpolation type won't need all
    /// the terms to be calculated.  Any basis matrix entry of zero means that the
    /// corresponding term in the coef. equation can be removed.  An entire zero row
    /// means that the corresponding power of T calculation can be dropped. This
    /// routine calculates the polynomial equation in matrix form:
    /// 
    ///                                        | n-1 |
    ///      1                                 | n   |           Steve Noskowicz
    /// P = --- * [T^3  T^2  T  1] * [basis] * | n+1 |         Q10706@email.mot.com
    ///      D                                 | n+2 |          May-13:Nov-25 1996
    /// 
    ///
    /// NOTE:
    /// For types which hit the control points (linear, cubic, Catmul-Rom), when T=0
    /// (or 1) control vectors are being "re-calculated".  For these types, time can
    /// be saved by eliminating those calculations and passing the original vectors
    /// through.  Also, if T goes to 1, you are double calculating.
    /// </remarks>
    public class Splines
    {

        public enum SplineType
        {
            Linear,
            SimpleCubic,
            Parabolic,
            CatMull_Rom,
            BSpline,
            Bezier_Cubic,
            Bezier_Quadric,
            Hermite,
            Beta,
            Kochanek_Bartels
        }

        private class SplineContext
        {
            public float[,] Mat = new float[4, 4];
            public int NStart;
            public int NEnd;
            public int NStep;
            public int JS;
        }

        private static Dictionary<SplineType, SplineContext> splineContext = new Dictionary<SplineType, SplineContext>();

        static Splines()
        {

            //throw new NotImplementedException("Fix Splines");

            //Splines = (ESplineType[])Enum.GetValues(typeof(ESplineType));

            //            var splineTypes = (SplineType[])Enum.GetValues(typeof(SplineType));


            //for (int i = 0; i < Enum<Splines>.GetValues; i++)
            //{
            //    SplineVars.Add(InitSpline(Splines[i]));
            //}
        }

        private static SplineContext CalcSplineVars(SplineType splineType)
        {
            if (splineContext.TryGetValue(splineType, out var context))
            {
                return context;
            }

            var newContext = InitSpline(splineType);
            splineContext.Add(splineType, newContext);

            return newContext;
        }

        private static void InitRow(float[,] Mat, float Divisor, int Row, float c1, float c2, float c3, float c4)
        {
            Mat[Row, 0] = c1 / Divisor;
            Mat[Row, 1] = c2 / Divisor;
            Mat[Row, 2] = c3 / Divisor;
            Mat[Row, 3] = c4 / Divisor;
        }

        private static void InitSpline_Linear(SplineContext Vars)
        {
            float Divisor = 0;
            //   Linear Interpolation
            //
            // As T goes from 0 to 1 you go from n to n+1
            //           n-1     n   n+1   n+2
            //   T^3       0     0     0     0     /
            //   T^2       0     0     0     0    /
            //   T^1       0    -1     1     0   /  1
            //   T^0       0     1     0     0  /
            //
            Divisor = 1;
            //divisor    This allows the Basis matrix to be all integers.
            InitRow(Vars.Mat, Divisor, 0, +0, +0, +0, +0);
            InitRow(Vars.Mat, Divisor, 1, +0, +0, +0, +0);
            InitRow(Vars.Mat, Divisor, 2, +0, -1, +1, +0);
            InitRow(Vars.Mat, Divisor, 3, +0, +1, +0, +0);

            Vars.NStart = 1;
            // Start at the first vector
            Vars.NEnd = -1;
            // End at next to last
            Vars.NStep = 1;
            // Step one at a time
            Vars.JS = 0;
            // Interpolate from zero
        }

        private static void InitSpline_SimpleCubic(SplineContext Vars)
        {
            float Divisor = 0;
            // Simple Cubic
            //
            // Derived from code found on the net written by Toby Orloff & Jim Larson
            // U of Minn Geometry Supercomputer Project "omni_interp" source code.
            // The derivative=0 at control points which gives straight
            // lines between control points,.
            // Hits every vector, but slows down at them.
            // As T goes from 0 TO 1 you go from n TO n+1
            //          n-1     n   n+1    n+2
            //   T^3      0    +2    -2      0     /
            //   T^2      0    -3    +3      0    /
            //   T^1      0     0     0      0   /  1
            //   T^0      0    +1     0      0  /
            //
            Divisor = 1;
            //divisor    This allows the Basis matrix to be all integers.
            InitRow(Vars.Mat, Divisor, 0, +0, +2, -2, +0);
            InitRow(Vars.Mat, Divisor, 1, +0, -3, +3, +0);
            InitRow(Vars.Mat, Divisor, 2, +0, +0, +0, +0);
            InitRow(Vars.Mat, Divisor, 3, +0, +1, +0, +0);

            Vars.NStart = 1;
            // Start at the first vector
            Vars.NEnd = -1;
            // End at next to last
            Vars.NStep = 1;
            // Step one at a time
            Vars.JS = 0;
            // Interpolate from zero
        }

        private static void InitSpline_Parabolic(SplineContext Vars)
        {
            float Divisor = 0;
            //  Parabolic basis Matrix from Leon de Boer.
            //
            // Doesn't hit key vectors.  As T goes from 0 to 1 you go from 
            // the midpoint of (n-1) & n to the midpoint n & (n+1).  
            //          n-1     n   n+1   n+2
            //   T^3      0     0     0     0     /
            //   T^2     +1    -2     1     0    /
            //   T^1     -2    +2     0     0   /  2
            //   T^0      1     1     0     0  /
            //
            Divisor = 2;
            //divisor    This allows the Basis matrix to be all integers.
            InitRow(Vars.Mat, Divisor, 0, +0, +0, +0, +0);
            InitRow(Vars.Mat, Divisor, 1, +1, -2, +1, +0);
            InitRow(Vars.Mat, Divisor, 2, -2, +2, +0, +0);
            InitRow(Vars.Mat, Divisor, 3, +1, +1, +0, +0);

            Vars.NStart = 1;
            // Start at the first vector
            Vars.NEnd = 0;
            // End at next to last
            Vars.NStep = 1;
            // Step one at a time
            Vars.JS = 0;
            // Interpolate from zero
        }

        private static void InitSpline_CatmullRom(SplineContext Vars)
        {
            float Divisor = 0;
            // Catmull-Rom
            //
            // As T goes from 0 to 1 you go from n to n+1
            //          n-1      n   n+1   n+2
            //   T^3     -1     +3    -3    +1     /
            //   T^2     +2     -5     4    -1    /
            //   T^1     -1      0     1     0   /  2
            //   T^0      0      2     0     0  /
            //
            Divisor = 2;
            //divisor    This allows the Basis matrix to be all integers.
            InitRow(Vars.Mat, Divisor, 0, -1, +3, -3, +1);
            InitRow(Vars.Mat, Divisor, 1, +2, -5, +4, -1);
            InitRow(Vars.Mat, Divisor, 2, -1, +0, +1, +0);
            InitRow(Vars.Mat, Divisor, 3, +0, +2, +0, +0);

            Vars.NStart = 1;
            // Start at the first vector
            Vars.NEnd = -1;
            // End at next to last
            Vars.NStep = 1;
            // Step one at a time
            Vars.JS = 0;
            // Interpolate from zero
        }

        private static void InitSpline_BSpline(SplineContext Vars)
        {
            float Divisor = 0;
            //  B-Spline basis Matrix
            //
            // Doesn't hit any input vectors
            // As T goes from 0 to 1 you go from near n to near n+1
            //          n-1     n   n+1   n+2
            //   T^3     -1    +3    -3    +1     /
            //   T^2     +3    -6     3     0    /
            //   T^1     -3     0     3     0   /  6
            //   T^0      1     4     1     0  /
            //
            Divisor = 6;
            //divisor    This allows the Basis matrix to be all integers.
            InitRow(Vars.Mat, Divisor, 0, -1, +3, -3, +1);
            InitRow(Vars.Mat, Divisor, 1, +3, -6, +3, +0);
            InitRow(Vars.Mat, Divisor, 2, -3, +0, +3, +0);
            InitRow(Vars.Mat, Divisor, 3, +1, +4, +1, +0);

            Vars.NStart = 1;
            // Start at the first vector
            Vars.NEnd = -1;
            // End at next to last
            Vars.NStep = 1;
            // Step one at a time
            Vars.JS = 0;
            // Interpolate from zero
        }

        private static void InitSpline_BezierQuadratic(SplineContext Vars)
        {
            float Divisor = 0;
            // Bézier (quadratic)
            //
            //  It uses the second degree Berstein basis function.
            // There is ONE control point between interpolated points.
            // The end velocities are both determined by the end-to-center vector.
            // As T goes from 0 to 1 you go from n to n+2
            //          n-1    n    n+1    n+2
            //   T^3    +0    -0    +0     +0    /
            //   T^2    -0    +1    -2     +1   /
            //   T^1    +0    -2    +2     -0  /  1
            //   T^0    +0    +1    +0     +0 /
            Divisor = 1;
            //divisor    This allows the Basis matrix to be all integers.
            InitRow(Vars.Mat, Divisor, 0, +0, +0, +0, +0);
            InitRow(Vars.Mat, Divisor, 1, +0, +1, -2, +1);
            InitRow(Vars.Mat, Divisor, 2, +0, -2, +2, +0);
            InitRow(Vars.Mat, Divisor, 3, +0, +1, +0, +0);

            Vars.NStart = 1;
            // Start at the first vector
            Vars.NEnd = -2;
            // End at next to last
            Vars.NStep = 2;
            // Step two at a time
            Vars.JS = 0;
            // Interpolate from zero
        }

        private static void InitSpline_BezierCubic(SplineContext Vars)
        {
            float Divisor = 0;
            //   Bézier (Cubic)
            //
            // Hits every third vector so STEP=3 & start@ second vector (#3)
            // As T goes from 0 TO 1 you go from n-1 TO n+2
            //         n-1     n    n+1    n+2
            //   T^3    -1    +3     -3     +1     /
            //   T^2    +3    -6     +3     +0    /
            //   T^1    -3    +3     +0     +0   /  1
            //   T^0    +1    +0     +0     +0  /
            //
            Divisor = 1;
            //divisor    This allows the Basis matrix to be all integers.
            InitRow(Vars.Mat, Divisor, 0, -1, +3, -3, +1);
            InitRow(Vars.Mat, Divisor, 1, +3, -6, +3, +0);
            InitRow(Vars.Mat, Divisor, 2, -3, +3, +0, +0);
            InitRow(Vars.Mat, Divisor, 3, +1, +0, +0, +0);

            //' MJA Test
            //Divisor = 6  'divisor    This allows the Basis matrix to be all integers.
            //InitRow(0, +1, +4, +1, +0)
            //InitRow(1, +0, +1, +4, +1)
            //InitRow(2, +0, +0, +1, +4)
            //InitRow(3, +0, +0, +0, +1)

            Vars.NStart = 2;
            // Start at the second vector
            Vars.NEnd = -2;
            // End at next to last
            Vars.NStep = 3;
            // Step three at a time
            Vars.JS = 0;
            // Interpolate from zero
        }

        private static void InitSpline_Hermit(SplineContext Vars)
        {
            float Divisor = 0;
            //  Hermite (non-spline)    From Llew Mason
            //
            // Bézier is similar and easier to use for graphics because the extra
            // control points are relative to the adjacent vectors.
            // I arranged the basis matrix columns to be similar to the Bézier.
            // The two tangent vector controls are in the middle (R1 & R2)
            // However, they must have values near zero, not near the adjacent vectors.
            // As T goes from 0 TO 1 you go from n-1 to n+2
            // The R1 & R2 are the velocity from/to end vectors respectively.
            //                 R1    R2
            //         n-1     n    n+1   n+2
            //   T^3     2    +1     +1    -2     /  The order of columns changed
            //   T^2    -3    -2     -1    +3    /     from regular Hermite.
            //   T^1     0     1      0     0   /  1
            //   T^0    +1     0      0     0  /
            //
            Divisor = 1;
            //divisor    This allows the Basis matrix to be all integers.
            InitRow(Vars.Mat, Divisor, 0, +2, +1, +1, -2);
            InitRow(Vars.Mat, Divisor, 1, -3, -2, -1, +3);
            InitRow(Vars.Mat, Divisor, 2, +0, +1, +0, +0);
            InitRow(Vars.Mat, Divisor, 3, +1, +0, +0, +0);

            Vars.NStart = 2;
            // Start at the second vector
            Vars.NEnd = -2;
            // End at next to last
            Vars.NStep = 3;
            // Step three at a time
            Vars.JS = 0;
            // Interpolate from zero
        }

        private static void InitSpline_Beta(SplineContext Vars)
        {
            float Divisor = 0;
            //  Beta Spline with Tension & Bias from Llew Mason.
            //
            // As tension goes from 0 to big, attraction is toward control point n.
            // As tension goes to about -6, curve "repels" from control point n.
            // As Bias goes from 1 to infinity attraction moves from point n toward n-1.
            // As bias goes from 1 to 0, attraction moves toward n+1.
            // As T goes from 0 to 1 you go from near n to near n+1
            //          n-1        n               n+1      n+2
            //   T^3  -2B^3   2(T+B^3+B^2+B)  -2(T+B^2+B+1)  2     /
            //   T^2  +6B^3  -3(T+2B^3+2B^2)    3(T+2B^2)    0    /
            //   T^1  -6B^3      6(B^3-B)           6B       0   /  T+2B^3+4B^2+4B+2
            //   T^0   2B^3     T+4(B^2+B)          2        0  /
            // For B=1 & T=0  this traces the B-Spline
            //
            float FB = 1;
            // Bias
            float FT = 0;
            // Tension
            //
            Divisor = FT + 2f * (float)Math.Pow(FB, 3) + 4f * (float)Math.Pow(FB, 2) + 4f * FB + 2f;
            //  The matrix divisor for Beta
            InitRow(Vars.Mat, Divisor, 0, -2f * (float)Math.Pow(FB, 3), +2f * (FT + (float)Math.Pow(FB, 3) + (float)Math.Pow(FB, 2) + FB), -2f * (FT + (float)Math.Pow(FB, 2) + FB + 1), 2);
            InitRow(Vars.Mat, Divisor, 1, +6f * (float)Math.Pow(FB, 3), -3f * (FT + 2f * (float)Math.Pow(FB, 3) + 2f * (float)Math.Pow(FB, 2)), +3f * (FT + 2f * (float)Math.Pow(FB, 2)), 0);
            InitRow(Vars.Mat, Divisor, 2, -6f * (float)Math.Pow(FB, 3), +6f * ((float)Math.Pow(FB, 3) - FB), +6f * FB, 0);
            InitRow(Vars.Mat, Divisor, 3, +2f * (float)Math.Pow(FB, 3), (FT + 4f * ((float)Math.Pow(FB, 2) + FB)), +2f, 0);

            Vars.NStart = 1;
            // Start at the first vector
            Vars.NEnd = -1;
            // End at next to last
            Vars.NStep = 1;
            // Step one at a time
            Vars.JS = 0;
            // Interpolate from zero
        }

        private static void InitSpline_KochanekBartels(SplineContext Vars)
        {
            float Divisor = 0;
            float FFA = 0;
            float FFB = 0;
            float FFC = 0;
            float FFD = 0;
            float FT = 0;
            float FB = 0;
            float FC = 0;

            //  Kochanek-Bartels
            //
            // Basis matrix for global Tension, Continuity  & Bias
            //   T H I S  I S  I T   I figured it out from the SIGGRAPH paper !
            // As T goes from 0 to 1 you go from n to n+1
            //        n-1      n        n+1       n+2
            //   T^3  -A    4+A-B-C   -4+B+C-D     D     /
            //   T^2 +2A  -6-2A+2B+C  6-2B-C+D    -D    /
            //   T^1  -A     A-B         B         0   /  2
            //   T^0   0      2          0         0  /
            //
            FT = 0;
            // Tension       T=+1-->Tight             T=-1--> Round
            FB = 0;
            // Bias          B=+1-->Post Shoot        B=-1--> Pre shoot
            FC = -1;
            // Continuity    C=+1-->Inverted corners  C=-1--> Box corners
            //
            // When T=B=C=0 this is the Catmul-Rom.
            // When T=1 & B=C=0 this is the Simple Cubic.
            // When T=B=0 & C=-1 this is the linear interp.
            //
            // Here, the three parameters are folded into the basis matrix.  If you want 
            // independent control of the segment start and end, make two T, C & Bs. 
            // One for A & B (beginning) and one for C & D (end).  For local control of
            // each point, you'll need an array of T, C & Bs for each individual point.
            // If you want the local control as shown on the video or in the paper, you use
            // the "A" & "B" for the current segment and the "C" & "D" from the previous
            // segment.
            //
            FFA = (1 - FT) * (1 + FC) * (1 + FB);
            FFB = (1 - FT) * (1 - FC) * (1 - FB);
            FFC = (1 - FT) * (1 - FC) * (1 + FB);
            FFD = (1 - FT) * (1 + FC) * (1 - FB);

            //  Here, the basis matrix coefficients are defined
            Divisor = 2;
            // divisor for K-B
            InitRow(Vars.Mat, Divisor, 0, -FFA, (+4 + FFA - FFB - FFC), (-4 + FFB + FFC - FFD), FFD);
            InitRow(Vars.Mat, Divisor, 1, +2 * FFA, (-6 - 2 * FFA + 2 * FFB + FFC), (+6 - 2 * FFB - FFC + FFD), -FFD);
            InitRow(Vars.Mat, Divisor, 2, -FFA, (FFA - FFB), FFB, +0);
            InitRow(Vars.Mat, Divisor, 3, +0, +2, +0, +0);

            Vars.NStart = 1;
            // Start at the first vector
            Vars.NEnd = -1;
            // End at next to last
            Vars.NStep = 1;
            // Step one at a time
            Vars.JS = 0;
            // Interpolate from zero
        }

        private static SplineContext InitSpline(SplineType SplineType)
        {
            SplineContext Vars = new SplineContext();

            switch (SplineType)
            {
                case SplineType.Linear:
                    InitSpline_Linear(Vars);
                    break;
                case SplineType.SimpleCubic:
                    InitSpline_SimpleCubic(Vars);
                    break;
                case SplineType.Parabolic:
                    InitSpline_Parabolic(Vars);
                    break;
                case SplineType.Bezier_Cubic:
                    InitSpline_BezierCubic(Vars);
                    break;
                case SplineType.Bezier_Quadric:
                    InitSpline_BezierQuadratic(Vars);
                    break;
                case SplineType.BSpline:
                    InitSpline_BSpline(Vars);
                    break;
                case SplineType.CatMull_Rom:
                    InitSpline_CatmullRom(Vars);
                    break;
                case SplineType.Hermite:
                    InitSpline_Hermit(Vars);
                    break;
                case SplineType.Kochanek_Bartels:
                    InitSpline_KochanekBartels(Vars);
                    break;
                case SplineType.Beta:
                    InitSpline_Beta(Vars);
                    break;
                default:
                    InitSpline_CatmullRom(Vars);
                    break;
            }

            return Vars;
        }

        public static List<Vector2> CalcSpline(SplineType SplineType, int Resolution, List<Vector2> CtrlPts)
        {
            List<Vector2> Path;
            List<Vector2> Pts;
            // New list of Points from the ControlPts
            SplineContext Vars;
            Vector2 FA; //= new Vector2();
            Vector2 FB; //= new Vector2();
            Vector2 FC; //= new Vector2();
            Vector2 FD; //= new Vector2();
            Vector2 F; //= new Vector2();
            float T;
            float Tdelta;

            // Init our Spline Specific Variables
            //      Vars = InitSpline(SplineType)
            Vars = CalcSplineVars(SplineType);

            // Init our Ctrl Points Collection
            Pts = new List<Vector2>();

            // Copy The First Control Point to the start (So that it is duplicated)
            Pts.Add(CtrlPts[0]);

            // Insert all the Control Points
            Pts.AddRange(CtrlPts);

            // Copy the last Control Point twice to the end
            Pts.Add(Pts[Pts.Count - 1]);
            Pts.Add(Pts[Pts.Count - 1]);

            // Init our Result Collection
            Path = new List<Vector2>();

            //  Add the first point
            Path.Add(Pts[0]);
            // (MJA)

            Tdelta = 1f / (float)Resolution;

            int nStart = Vars.NStart;
            int nEnd = (Pts.Count - 3 + Vars.NEnd);
            int nStep = Vars.NStep;

            for (int n = nStart; n <= nEnd; n += nStep)
            {

                // These are the coefficients a, b, c, d, in  aT^3 + bT^2 + cT + d
                FA.X = Vars.Mat[0, 0] * Pts[n - 1].X + Vars.Mat[0, 1] * Pts[n].X + Vars.Mat[0, 2] * Pts[n + 1].X + Vars.Mat[0, 3] * Pts[n + 2].X;
                FB.X = Vars.Mat[1, 0] * Pts[n - 1].X + Vars.Mat[1, 1] * Pts[n].X + Vars.Mat[1, 2] * Pts[n + 1].X + Vars.Mat[1, 3] * Pts[n + 2].X;
                FC.X = Vars.Mat[2, 0] * Pts[n - 1].X + Vars.Mat[2, 1] * Pts[n].X + Vars.Mat[2, 2] * Pts[n + 1].X + Vars.Mat[2, 3] * Pts[n + 2].X;
                FD.X = Vars.Mat[3, 0] * Pts[n - 1].X + Vars.Mat[3, 1] * Pts[n].X + Vars.Mat[3, 2] * Pts[n + 1].X + Vars.Mat[3, 3] * Pts[n + 2].X;
                //
                FA.Y = Vars.Mat[0, 0] * Pts[n - 1].Y + Vars.Mat[0, 1] * Pts[n].Y + Vars.Mat[0, 2] * Pts[n + 1].Y + Vars.Mat[0, 3] * Pts[n + 2].Y;
                FB.Y = Vars.Mat[1, 0] * Pts[n - 1].Y + Vars.Mat[1, 1] * Pts[n].Y + Vars.Mat[1, 2] * Pts[n + 1].Y + Vars.Mat[1, 3] * Pts[n + 2].Y;
                FC.Y = Vars.Mat[2, 0] * Pts[n - 1].Y + Vars.Mat[2, 1] * Pts[n].Y + Vars.Mat[2, 2] * Pts[n + 1].Y + Vars.Mat[2, 3] * Pts[n + 2].Y;
                FD.Y = Vars.Mat[3, 0] * Pts[n - 1].Y + Vars.Mat[3, 1] * Pts[n].Y + Vars.Mat[3, 2] * Pts[n + 1].Y + Vars.Mat[3, 3] * Pts[n + 2].Y;


                // Calculate one segment of the interpolation here.

                //'  Add the first point
                //If n = Vars.NStart Then
                //  Path.Add(New Vec3(FD.x, FD.y, FD.z))
                //End If

                T = 0.0f;

                // Interpolate one segment
                for (int J = Vars.JS + 1; J <= Resolution; J++)
                {
                    //          T = J / Resolution  ' The interpolation parameter steps from delta to 1.0
                    T += Tdelta;

                    // The simple polynomial evaluation first pre-computes T^2 & T^3
                    //   T2 = T * T  :  T3 = T2 * T
                    // Then the polynomials
                    //   FX = FAX*T3 + FBX*T2 + FCX*T + FDX
                    //   FY = FAY*T3 + FBY*T2 + FCY*T + FDY
                    //   FZ = FAZ*T3 + FBZ*T2 + FCZ*T + FDZ
                    //
                    // However, using Horner's Rule saves calculations & time.
                    F.X = ((FA.X * T + FB.X) * T + FC.X) * T + FD.X;    // interpolated x value (using Horner)
                    F.Y = ((FA.Y * T + FB.Y) * T + FC.Y) * T + FD.Y;    // interpolated y value

                    Path.Add(new Vector2(F.X, F.Y));
                }
                //  J   sub-step "between" vectors
            }
            //  n   Move to the next vector set.

            return Path;
        }

        public static List<Vector3> CalcSpline(SplineType SplineType, int Resolution, List<Vector3> CtrlPts)
        {
            List<Vector3> Path;
            List<Vector3> Pts;
            // New list of Points from the ControlPts
            SplineContext Vars;
            Vector3 FA; // = new Vector3();
            Vector3 FB; // = new Vector3();
            Vector3 FC; // = new Vector3();
            Vector3 FD; // = new Vector3();
            Vector3 F; // = new Vector3();
            float T;
            float Tdelta;

            // Init our Spline Specific Variables
            //      Vars = InitSpline(SplineType)
            Vars = CalcSplineVars(SplineType);

            // Init our Ctrl Points Collection
            Pts = new List<Vector3>();

            // Copy The First Control Point to the start (So that it is duplicated)
            Pts.Add(CtrlPts[0]);

            // Insert all the Control Points
            Pts.AddRange(CtrlPts);

            // Copy the last Control Point twice to the end
            Pts.Add(Pts[Pts.Count - 1]);
            Pts.Add(Pts[Pts.Count - 1]);

            // Init our Result Collection
            Path = new List<Vector3>();

            //  Add the first point
            Path.Add(Pts[0]);
            // (MJA)

            Tdelta = 1f / (float)Resolution;

            int nStart = Vars.NStart;
            int nEnd = (Pts.Count - 3 + Vars.NEnd);
            int nStep = Vars.NStep;

            for (int n = nStart; n <= nEnd; n += nStep)
            {

                // These are the coefficients a, b, c, d, in  aT^3 + bT^2 + cT + d
                FA.X = Vars.Mat[0, 0] * Pts[n - 1].X + Vars.Mat[0, 1] * Pts[n].X + Vars.Mat[0, 2] * Pts[n + 1].X + Vars.Mat[0, 3] * Pts[n + 2].X;
                FB.X = Vars.Mat[1, 0] * Pts[n - 1].X + Vars.Mat[1, 1] * Pts[n].X + Vars.Mat[1, 2] * Pts[n + 1].X + Vars.Mat[1, 3] * Pts[n + 2].X;
                FC.X = Vars.Mat[2, 0] * Pts[n - 1].X + Vars.Mat[2, 1] * Pts[n].X + Vars.Mat[2, 2] * Pts[n + 1].X + Vars.Mat[2, 3] * Pts[n + 2].X;
                FD.X = Vars.Mat[3, 0] * Pts[n - 1].X + Vars.Mat[3, 1] * Pts[n].X + Vars.Mat[3, 2] * Pts[n + 1].X + Vars.Mat[3, 3] * Pts[n + 2].X;
                //
                FA.Y = Vars.Mat[0, 0] * Pts[n - 1].Y + Vars.Mat[0, 1] * Pts[n].Y + Vars.Mat[0, 2] * Pts[n + 1].Y + Vars.Mat[0, 3] * Pts[n + 2].Y;
                FB.Y = Vars.Mat[1, 0] * Pts[n - 1].Y + Vars.Mat[1, 1] * Pts[n].Y + Vars.Mat[1, 2] * Pts[n + 1].Y + Vars.Mat[1, 3] * Pts[n + 2].Y;
                FC.Y = Vars.Mat[2, 0] * Pts[n - 1].Y + Vars.Mat[2, 1] * Pts[n].Y + Vars.Mat[2, 2] * Pts[n + 1].Y + Vars.Mat[2, 3] * Pts[n + 2].Y;
                FD.Y = Vars.Mat[3, 0] * Pts[n - 1].Y + Vars.Mat[3, 1] * Pts[n].Y + Vars.Mat[3, 2] * Pts[n + 1].Y + Vars.Mat[3, 3] * Pts[n + 2].Y;
                //
                FA.Z = Vars.Mat[0, 0] * Pts[n - 1].Z + Vars.Mat[0, 1] * Pts[n].Z + Vars.Mat[0, 2] * Pts[n + 1].Z + Vars.Mat[0, 3] * Pts[n + 2].Z;
                FB.Z = Vars.Mat[1, 0] * Pts[n - 1].Z + Vars.Mat[1, 1] * Pts[n].Z + Vars.Mat[1, 2] * Pts[n + 1].Z + Vars.Mat[1, 3] * Pts[n + 2].Z;
                FC.Z = Vars.Mat[2, 0] * Pts[n - 1].Z + Vars.Mat[2, 1] * Pts[n].Z + Vars.Mat[2, 2] * Pts[n + 1].Z + Vars.Mat[2, 3] * Pts[n + 2].Z;
                FD.Z = Vars.Mat[3, 0] * Pts[n - 1].Z + Vars.Mat[3, 1] * Pts[n].Z + Vars.Mat[3, 2] * Pts[n + 1].Z + Vars.Mat[3, 3] * Pts[n + 2].Z;


                // Calculate one segment of the interpolation here.

                //'  Add the first point
                //If n = Vars.NStart Then
                //  Path.Add(New Vec3(FD.x, FD.y, FD.z))
                //End If

                T = 0.0f;

                // Interpolate one segment
                for (int J = Vars.JS + 1; J <= Resolution; J++)
                {
                    //          T = J / Resolution  ' The interpolation parameter steps from delta to 1.0
                    T += Tdelta;

                    // The simple polynomial evaluation first pre-computes T^2 & T^3
                    //   T2 = T * T  :  T3 = T2 * T
                    // Then the polynomials
                    //   FX = FAX*T3 + FBX*T2 + FCX*T + FDX
                    //   FY = FAY*T3 + FBY*T2 + FCY*T + FDY
                    //   FZ = FAZ*T3 + FBZ*T2 + FCZ*T + FDZ
                    //
                    // However, using Horner's Rule saves calculations & time.
                    F.X = ((FA.X * T + FB.X) * T + FC.X) * T + FD.X;     // interpolated x value (using Horner)
                    F.Y = ((FA.Y * T + FB.Y) * T + FC.Y) * T + FD.Y;     // interpolated y value
                    F.Z = ((FA.Z * T + FB.Z) * T + FC.Z) * T + FD.Z;     // interpolated z value

                    Path.Add(new Vector3(F.X, F.Y, F.Z));
                }
                //  J   sub-step "between" vectors
            }
            //  n   Move to the next vector set.

            return Path;
        }

        public static List<List<Vector3>> CalcPatch(SplineType SplineType, int Resolution, Vector3[,] CtrlPts)
        {
            List<List<Vector3>> Pnts = new List<List<Vector3>>();
            List<List<Vector3>> Result = new List<List<Vector3>>();
            List<Vector3> Ctrl;

            for (int x = 0; x <= 3; x++)
            {
                Ctrl = new List<Vector3>();
                for (int y = 0; y <= 3; y++)
                {
                    Ctrl.Add(CtrlPts[x, y]);
                }
                Pnts.Add(CalcSpline(SplineType, Resolution, Ctrl));
            }

            for (int x = 0; x <= Pnts[0].Count - 1; x++)
            {
                Ctrl = new List<Vector3>();
                for (int y = 0; y <= Pnts.Count - 1; y++)
                {
                    Ctrl.Add(Pnts[y][x]);
                }
                Result.Add(CalcSpline(SplineType, Resolution, Ctrl));
            }

            return Result;
        }
    }


}

