using Microsoft.Xna.Framework;

namespace Blade.MG.SVG.Geometries.Paths
{
    //-----------------------------------------------------------------------------------------------------------
    // Code based on: https://mortoray.com/2017/02/16/rendering-an-svg-elliptical-arc-as-bezier-curves/
    // https://github.com/fuse-open/fuselibs/blob/master/Source/Fuse.Drawing.Surface/SurfaceUtil.uno
    //-----------------------------------------------------------------------------------------------------------
    public class PathEllipticArc : PathCommand
    {
        public Vector2 Size;
        public float RotationAngle; // Rotation in Degrees
        public bool IsLargeArc;
        public bool SweepDirection; // True = The arc is drawn in a positive-angle direction
        public Vector2 EndPoint;

        public void EllipticArcToBezierCurve(Vector2 from, out PathBezierSmoothCubic spline)
        {
            spline = new PathBezierSmoothCubic();

            if (EllipticArcOutOfRange(from, EndPoint, Size))
            {
                //TODO: Test
                spline.ControlPoints.Add(from);
                spline.ControlPoints.Add(EndPoint);
                spline.EndPoint = EndPoint;

                //curves.Add(new LineSegment { Type = LineSegmentType.Straight, To = arc.To });
                return;
            }

            var radius = Size.Abs();
            var xAngle = RotationAngle; //arc.B.X;
            var center = Vector2.Zero;
            var angles = Vector2.Zero;

            Vector2 p1 = from;
            Vector2 p2 = EndPoint;
            if (IsRelative)
            {
                p2 += p1;
            }

            EndpointToCenterArcParams(from, EndPoint, ref radius, xAngle,
                IsLargeArc, //arc.Flags.HasFlag(LineSegmentFlags.EllipticArcLarge),
                SweepDirection, //arc.Flags.HasFlag(LineSegmentFlags.EllipticArcSweep),
                out center, out angles);

            EllipticArcToBezierCurve(center, radius, xAngle, angles.X, angles.Y, false, spline);
        }

        private static bool EllipticArcOutOfRange(Vector2 from, Vector2 endPoint, Vector2 radius)
        {
            //F.6.2 Out-of-range parameters
            var len = (endPoint - from).Length();
            if (len < _zeroTolerance)
                return true;

            radius = radius.Abs();
            if (radius.X < _zeroTolerance || radius.Y < _zeroTolerance)
                return true;

            return false;
        }

        const float _zeroTolerance = 1e-05f;

        public static void EllipticArcToBezierCurve(Vector2 center, Vector2 radius, float xAngle, float startAngle, float deltaAngle, bool moveToStart, PathBezierSmoothCubic spline) // IList<LineSegment> curves)
        {
            double segments = 4; // Number of Bezier Curves to decompose the Arc into

            var s = startAngle;
            var e = s + deltaAngle;
            bool neg = e < s;
            float sign = neg ? -1 : 1;
            var remain = Math.Abs(e - s);

            var prev = EllipticArcPoint(center, radius, xAngle, s);

            //if (moveToStart)
            //    curves.Add(new LineSegment { Type = LineSegmentType.Move, To = prev });


            while (remain > _zeroTolerance)
            {
                var step = (float)Math.Min(remain, Math.PI / segments);
                var signStep = (float)(step * sign);

                var p1 = prev;
                var p2 = EllipticArcPoint(center, radius, xAngle, s + signStep);

                var alphaT = (float)(Math.Tan(signStep / 2f));
                var alpha = (float)(Math.Sin(signStep) * (Math.Sqrt(4 + 3 * alphaT * alphaT) - 1) / 3);
                var q1 = p1 + alpha * EllipticArcDerivative(center, radius, xAngle, s);
                var q2 = p2 - alpha * EllipticArcDerivative(center, radius, xAngle, s + signStep);

                spline.ControlPoints.Add(q1);
                spline.ControlPoints.Add(q2);

                spline.ControlPoints.Add(p2);
                //spline.EndPoint = new Vector3(p2, 0f);

                //curves.Add(new LineSegment
                //{
                //    Type = LineSegmentType.BezierCurve,
                //    To = p2,
                //    A = q1,
                //    B = q2
                //});

                //// TODO: Don't store as line segments, store as CubicBezier paths then split each Bezier into multiple line segments
                //curves.Add(new LineSegment
                //{
                //    From = new Vector3(p1, 0f),
                //    To = new Vector3(p2, 0f)
                //});

                s += signStep;
                remain -= step;
                prev = p2;
            }

            spline.ControlPoints.RemoveAt(spline.ControlPoints.Count - 1);
            spline.EndPoint = prev;

        }

        public void EndpointToCenterArcParams(Vector2 startPoint, out Vector2 r_, out Vector2 c, out Vector2 angles)
        {
            var p1 = new Vector2(startPoint.X, startPoint.Y);
            var p2 = new Vector2(EndPoint.X, EndPoint.Y);
            r_ = new Vector2(Size.X, Size.Y);

            EndpointToCenterArcParams(p1, p2, ref r_, RotationAngle, IsLargeArc, SweepDirection, out c, out angles);

        }

        /**
            Perform the endpoint to center arc parameter conversion as detailed in the SVG 1.1 spec.
            F.6.5 Conversion from endpoint to center parameterization

            @param r must be a ref in case it needs to be scaled up, as per the SVG spec
        */
        public static void EndpointToCenterArcParams(Vector2 p1, Vector2 p2, ref Vector2 r_, float xAngle, bool flagA, bool flagS, out Vector2 c, out Vector2 angles)
        {
            double rX = Math.Abs(r_.X);
            double rY = Math.Abs(r_.Y);

            //(F.6.5.1)
            double dx2 = (p1.X - p2.X) / 2.0;
            double dy2 = (p1.Y - p2.Y) / 2.0;
            double x1p = Math.Cos(xAngle) * dx2 + Math.Sin(xAngle) * dy2;
            double y1p = -Math.Sin(xAngle) * dx2 + Math.Cos(xAngle) * dy2;

            //(F.6.5.2)
            double rxs = rX * rX;
            double rys = rY * rY;
            double x1ps = x1p * x1p;
            double y1ps = y1p * y1p;
            // check if the radius is too small `pq < 0`, when `dq > rxs * rys` (see below)
            // cr is the ratio (dq : rxs * rys) 
            double cr = x1ps / rxs + y1ps / rys;
            if (cr > 1)
            {
                //scale up rX,rY equally so cr == 1
                var s = Math.Sqrt(cr);
                rX = s * rX;
                rY = s * rY;
                rxs = rX * rX;
                rys = rY * rY;
            }
            double dq = (rxs * y1ps + rys * x1ps);
            double pq = (rxs * rys - dq) / dq;
            double q = Math.Sqrt(Math.Max(0, pq)); //use Max to account for float precision
            //if (flagA == flagS)
            if (flagA != flagS)
                q = -q;
            double cxp = q * rX * y1p / rY;
            double cyp = -q * rY * x1p / rX;

            //(F.6.5.3)
            double cx = Math.Cos(xAngle) * cxp - Math.Sin(xAngle) * cyp + (p1.X + p2.X) / 2;
            double cy = Math.Sin(xAngle) * cxp + Math.Cos(xAngle) * cyp + (p1.Y + p2.Y) / 2;

            //(F.6.5.5)
            double theta = svgAngle(1, 0, (x1p - cxp) / rX, (y1p - cyp) / rY);
            //(F.6.5.6)
            double delta = svgAngle(
                (x1p - cxp) / rX, (y1p - cyp) / rY,
                (-x1p - cxp) / rX, (-y1p - cyp) / rY);
            delta = Modulus(delta, Math.PI * 2);
            if (flagS)
                delta -= 2 * Math.PI;

            r_ = new Vector2((float)rX, (float)rY);
            c = new Vector2((float)cx, (float)cy);
            angles = new Vector2((float)theta, (float)delta);
        }

        private static float svgAngle(double ux, double uy, double vx, double vy)
        {
            var u = new Vector2((float)ux, (float)uy);
            var v = new Vector2((float)vx, (float)vy);
            //(F.6.5.4)
            var dot = Vector2.Dot(u, v);
            var len = u.Length() * v.Length();
            var ang = (float)Math.Acos(Math.Clamp(dot / len, -1, 1)); //floating point precision, slightly over values appear
            if ((u.X * v.Y - u.Y * v.X) < 0)
            {
                ang = -ang;
            }

            return ang;
        }

        public static Vector2 EllipticArcPoint(Vector2 c, Vector2 r, float xAngle, float t)
        {
            return new Vector2(
                (float)(c.X + r.X * Math.Cos(xAngle) * Math.Cos(t) - r.Y * Math.Sin(xAngle) * Math.Sin(t)),
                (float)(c.Y + r.X * Math.Sin(xAngle) * Math.Cos(t) + r.Y * Math.Cos(xAngle) * Math.Sin(t))
                );
        }

        public static Vector2 EllipticArcDerivative(Vector2 c, Vector2 r, float xAngle, float t)
        {
            return new Vector2(
                (float)(-r.X * Math.Cos(xAngle) * Math.Sin(t) - r.Y * Math.Sin(xAngle) * Math.Cos(t)),
                (float)(-r.X * Math.Sin(xAngle) * Math.Sin(t) + r.Y * Math.Cos(xAngle) * Math.Cos(t)));
        }


        static public bool AngleInRange(float angle, float start, float end)
        {
            if (end < start)
            {
                var t = end;
                end = start;
                start = t;
            }

            var delta = end - start;
            if (delta >= 2 * Math.PI)
                return true;

            angle = Modulus(angle, (float)(2 * Math.PI));
            var pStartAngle = Modulus(start, 2 * Math.PI);
            var pEndAngle = pStartAngle + delta;

            if (angle >= pStartAngle && angle <= pEndAngle)
                return true;
            if (angle <= (pEndAngle - Math.PI * 2))
                return true;
            return false;
        }

        static public Vector2 BezierCurveDerivative(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t)
        {
            var t2 = t * t;
            return 3 * (-(p0 - 3 * p1 - p3 + 3 * p2) * t2 + 2 * (p0 - 2 * p1 + p2) * t - p0 + p1);
        }


        // Calculate a Mathematical Modulus
        private static float Modulus(float a, float b)
        {
            return a - b * (float)Math.Floor(a / b);
        }

        private static double Modulus(double a, double b)
        {
            return a - b * Math.Floor(a / b);
        }

    }
}
