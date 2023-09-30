namespace Blade.MG.Primitives
{
    public static partial class Primitives2D
    {

        /// <summary>
        /// Helper function for calculating the pixel offset for thick lines.
        /// e.g. For a Horizontal line, centered on Y=10
        ///   - Line Width = 1 : Half Width = 0, Full Width = 0 : Line is drawn on Y = 10
        ///   - Line Width = 2 : Half Width = 0, Full Width = 1 : Line is drawn on Y-0 = 10 to Y+1 = 11
        ///   - Line Width = 3 : Hald Width = 1, Full Width = 1 : Line is drawn on Y-1 = 9  to Y+1 = 11
        ///   etc.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="halfWidth"></param>
        /// <param name="fullWidth"></param>
        internal static void CalculateLineWidthOffsets(float width, out int halfWidth, out int fullWidth)
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




        /// <summary>
        /// Returns true if the line segments intersect, otherwise false. 
        /// In addition, if the lines intersect the intersection point will be stored in the floats i_x and i_y.
        /// </summary>
        private static bool GetLineSegmentIntersection(float p0_x, float p0_y, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, ref float i_x, ref float i_y)
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

        /// <summary>
        /// Returns true if the infinite lines intersect, otherwise false.  
        /// In addition, if the lines intersect the intersection point will be stored in the floats i_x and i_y.
        /// </summary>
        private static bool GetLineIntersection(float p0_x, float p0_y, float p1_x, float p1_y, float p2_x, float p2_y, float p3_x, float p3_y, ref float i_x, ref float i_y)
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

}
