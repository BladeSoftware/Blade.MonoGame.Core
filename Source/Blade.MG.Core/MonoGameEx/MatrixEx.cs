namespace Microsoft.Xna.Framework
{
    public static class MatrixEx
    {

        public static Matrix CreateRotationZ(float radians, Vector3 center)
        {
            return Matrix.Multiply(Matrix.Multiply(Matrix.CreateTranslation(-center), Matrix.CreateRotationZ(radians)), Matrix.CreateTranslation(center));
        }


        public static Vector3 GetPosition(this Matrix m)
        {
            return new Vector3(m.M41, m.M42, m.M43);
        }

        public static Vector3 GetScale(this Matrix m)
        {
            return new Vector3(m.M11 + m.M21 + m.M31, m.M12 + m.M22 + m.M32, m.M13 + m.M23 + m.M33);
        }

        /// <summary>
        /// Given two sets of co-ordinate axis e(e1,e2,e3) & f(f1,f2,f3)
        /// Returns the transformation matrix that will transform points 
        /// from co-ordinate system e to f
        /// Assumes everything is normalised!
        /// </summary>
        /// <param name="e1"></param>
        /// <param name="e2"></param>
        /// <param name="e3"></param>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <param name="f3"></param>
        /// <returns></returns>
        public static Matrix CreateAlignAxis(Vector3 e1, Vector3 e2, Vector3 e3, Vector3 f1, Vector3 f2, Vector3 f3)
        {
            Matrix ret; // = new Matrix();
            Matrix t; // = new Matrix();
            Vector3 g;

            // Find the angle between e1 & f1
            float a = Vector3.Dot(e1, f1); // Returns co-sine of the angle between e1 & f1
            if (a < -1) a = -1;
            if (a > 1) a = 1;

            if (a == 1) // Angle = 0 degrees
            {
                ret = Matrix.Identity;
            }
            else if (a == -1) // Angle = 180 degrees.
            {
                ret = Matrix.CreateFromAxisAngle(e2, MathHelper.Pi);
            }
            else
            {
                a = (float)Math.Acos(a); // cos(a)^-1 

                // Find a vector perpendicular to e1 & f1
                g = Vector3.Cross(e1, f1);
                g.Normalize();

                // Rotate around Vector 'g' by Angle 'a'
                ret = Matrix.CreateFromAxisAngle(g, a);
            }


            // Applying Matrix lRet to e1,e2,e3 should leave e1 = f1
            // e1 = Not required as it should equal f1
            e2 = Vector3.TransformNormal(e2, ret);
            // e3 = CMatrix.MulHF(lRet, e3) ' Not required as e3 is not used...


            // Find the angle between e2 & f2
            a = Vector3.Dot(e2, f2); // Returns co-sine of the angle between e2 & f2
            if (a < -1) a = -1;
            if (a > 1) a = 1;

            if (a == 1) // Angle = 0 degrees
            {
                t = Matrix.Identity;
            }
            else if (a == -1) // Angle = 180 degrees.
            {
                t = Matrix.CreateFromAxisAngle(e3, MathHelper.Pi);
            }
            else
            {
                a = (float)Math.Acos(a); // cos(a)^-1

                // Find a vector perpendicular to e2 & f2
                g = Vector3.Cross(e2, f2);
                g.Normalize();

                // Rotate around Vector 'g' by Angle 'a'
                t = Matrix.CreateFromAxisAngle(g, a);
            }

            // Concatinate matrix ret & T to give the final transformation
            var ret2 = Matrix.Multiply(ret, t);

            return ret2;
        }

        /*
                public static Matrix MultiplyNoPerspective(Matrix matrix1, Matrix matrix2)
                {
                    Matrix ret;
                    MultiplyNoPerspective(ref matrix1, ref matrix2, out ret);
                    return ret;
                }

                public static void MultiplyNoPerspective(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
                {
                    result.M11 = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31; // + matrix1.M14 * matrix2.M41;
                    result.M12 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32; // + matrix1.M14 * matrix2.M42;
                    result.M13 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33; // + matrix1.M14 * matrix2.M43;
                    result.M14 = 0f; //matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;

                    result.M21 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31; // + matrix1.M24 * matrix2.M41;
                    result.M22 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32; // + matrix1.M24 * matrix2.M42;
                    result.M23 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33; // + matrix1.M24 * matrix2.M43;
                    result.M24 = 0f; //matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;

                    result.M31 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31; // + matrix1.M34 * matrix2.M41;
                    result.M32 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32; // + matrix1.M34 * matrix2.M42;
                    result.M33 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33; // + matrix1.M34 * matrix2.M43;
                    result.M34 = 0f; //matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;

                    result.M41 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix2.M41; //matrix1.M44 * matrix2.M41;
                    result.M42 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix2.M42; //matrix1.M44 * matrix2.M42;
                    result.M43 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix2.M43; //matrix1.M44 * matrix2.M43;
                    result.M44 = 1f; //matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
                }

                public static Matrix Invert(Matrix matrix)
                {
                    Invert(ref matrix, out matrix);
                    return matrix;

                }

                public static void Invert(ref Matrix matrix, out Matrix result)
                {
                    ///
                    // Use Laplace expansion theorem to calculate the inverse of a 4x4 matrix
                    // 
                    // 1. Calculate the 2x2 determinants needed the 4x4 determinant based on the 2x2 determinants 
                    // 3. Create the adjugate matrix, which satisfies: A * adj(A) = det(A) * I
                    // 4. Divide adjugate matrix with the determinant to find the inverse

                    float det1, det2, det3, det4, det5, det6, det7, det8, det9, det10, det11, det12;
                    float detMatrix;
                    findDeterminants(ref matrix, out detMatrix, out det1, out det2, out det3, out det4, out det5, out det6,
                                     out det7, out det8, out det9, out det10, out det11, out det12);

                    float invDetMatrix = 1f / detMatrix;

                    Matrix ret; // Allow for matrix and result to point to the same structure

                    ret.M11 = (matrix.M22 * det12 - matrix.M23 * det11) * invDetMatrix;
                    ret.M12 = (-matrix.M12 * det12 + matrix.M13 * det11) * invDetMatrix;
                    ret.M13 = (det4) * invDetMatrix;
                    ret.M14 = 0;
                    ret.M21 = (-matrix.M21 * det12 + matrix.M23 * det9) * invDetMatrix;
                    ret.M22 = (matrix.M11 * det12 - matrix.M13 * det9) * invDetMatrix;
                    ret.M23 = (-det2) * invDetMatrix;
                    ret.M24 = 0;
                    ret.M31 = (matrix.M21 * det11 - matrix.M22 * det9) * invDetMatrix;
                    ret.M32 = (-matrix.M11 * det11 + matrix.M12 * det9) * invDetMatrix;
                    ret.M33 = (det1) * invDetMatrix;
                    ret.M34 = 0;
                    ret.M41 = (-matrix.M21 * det10 + matrix.M22 * det8 - matrix.M23 * det7) * invDetMatrix;
                    ret.M42 = (matrix.M11 * det10 - matrix.M12 * det8 + matrix.M13 * det7) * invDetMatrix;
                    ret.M43 = (-matrix.M41 * det4 + matrix.M42 * det2 - matrix.M43 * det1) * invDetMatrix;
                    ret.M44 = 1;

                    result = ret;
                }

                /// <summary>
                /// Helper method for using the Laplace expansion theorem using two rows expansions to calculate major and 
                /// minor determinants of a 4x4 matrix. This method is used for inverting a matrix.
                /// </summary>
                private static void findDeterminants(ref Matrix matrix, out float major,
                                                     out float minor1, out float minor2, out float minor3, out float minor4, out float minor5, out float minor6,
                                                     out float minor7, out float minor8, out float minor9, out float minor10, out float minor11, out float minor12)
                {
                    double det1 = (double)matrix.M11 * (double)matrix.M22 - (double)matrix.M12 * (double)matrix.M21;
                    double det2 = (double)matrix.M11 * (double)matrix.M23 - (double)matrix.M13 * (double)matrix.M21;
                    double det4 = (double)matrix.M12 * (double)matrix.M23 - (double)matrix.M13 * (double)matrix.M22;
                    double det7 = (double)matrix.M31 * (double)matrix.M42 - (double)matrix.M32 * (double)matrix.M41;
                    double det8 = (double)matrix.M31 * (double)matrix.M43 - (double)matrix.M33 * (double)matrix.M41;
                    double det9 = (double)matrix.M31;
                    double det10 = (double)matrix.M32 * (double)matrix.M43 - (double)matrix.M33 * (double)matrix.M42;
                    double det11 = (double)matrix.M32;
                    double det12 = (double)matrix.M33;

                    major = (float)(det1 * det12 - det2 * det11 + det4 * det9);
                    minor1 = (float)det1;
                    minor2 = (float)det2;
                    minor3 = 0f;
                    minor4 = (float)det4;
                    minor5 = 0f;
                    minor6 = 0f;
                    minor7 = (float)det7;
                    minor8 = (float)det8;
                    minor9 = (float)det9;
                    minor10 = (float)det10;
                    minor11 = (float)det11;
                    minor12 = (float)det12;
                }
        */

        //// Monogame versions for reference
        //public static void Multiply(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
        //{
        //    result.M11 = matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21 + matrix1.M13 * matrix2.M31 + matrix1.M14 * matrix2.M41;
        //    result.M12 = matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22 + matrix1.M13 * matrix2.M32 + matrix1.M14 * matrix2.M42;
        //    result.M13 = matrix1.M11 * matrix2.M13 + matrix1.M12 * matrix2.M23 + matrix1.M13 * matrix2.M33 + matrix1.M14 * matrix2.M43;
        //    result.M14 = matrix1.M11 * matrix2.M14 + matrix1.M12 * matrix2.M24 + matrix1.M13 * matrix2.M34 + matrix1.M14 * matrix2.M44;

        //    result.M21 = matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21 + matrix1.M23 * matrix2.M31 + matrix1.M24 * matrix2.M41;
        //    result.M22 = matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22 + matrix1.M23 * matrix2.M32 + matrix1.M24 * matrix2.M42;
        //    result.M23 = matrix1.M21 * matrix2.M13 + matrix1.M22 * matrix2.M23 + matrix1.M23 * matrix2.M33 + matrix1.M24 * matrix2.M43;
        //    result.M24 = matrix1.M21 * matrix2.M14 + matrix1.M22 * matrix2.M24 + matrix1.M23 * matrix2.M34 + matrix1.M24 * matrix2.M44;

        //    result.M31 = matrix1.M31 * matrix2.M11 + matrix1.M32 * matrix2.M21 + matrix1.M33 * matrix2.M31 + matrix1.M34 * matrix2.M41;
        //    result.M32 = matrix1.M31 * matrix2.M12 + matrix1.M32 * matrix2.M22 + matrix1.M33 * matrix2.M32 + matrix1.M34 * matrix2.M42;
        //    result.M33 = matrix1.M31 * matrix2.M13 + matrix1.M32 * matrix2.M23 + matrix1.M33 * matrix2.M33 + matrix1.M34 * matrix2.M43;
        //    result.M34 = matrix1.M31 * matrix2.M14 + matrix1.M32 * matrix2.M24 + matrix1.M33 * matrix2.M34 + matrix1.M34 * matrix2.M44;

        //    result.M41 = matrix1.M41 * matrix2.M11 + matrix1.M42 * matrix2.M21 + matrix1.M43 * matrix2.M31 + matrix1.M44 * matrix2.M41;
        //    result.M42 = matrix1.M41 * matrix2.M12 + matrix1.M42 * matrix2.M22 + matrix1.M43 * matrix2.M32 + matrix1.M44 * matrix2.M42;
        //    result.M43 = matrix1.M41 * matrix2.M13 + matrix1.M42 * matrix2.M23 + matrix1.M43 * matrix2.M33 + matrix1.M44 * matrix2.M43;
        //    result.M44 = matrix1.M41 * matrix2.M14 + matrix1.M42 * matrix2.M24 + matrix1.M43 * matrix2.M34 + matrix1.M44 * matrix2.M44;
        //}

        //public static void Invert(ref Matrix matrix, out Matrix result)
        //{
        //    ///
        //    // Use Laplace expansion theorem to calculate the inverse of a 4x4 matrix
        //    // 
        //    // 1. Calculate the 2x2 determinants needed the 4x4 determinant based on the 2x2 determinants 
        //    // 3. Create the adjugate matrix, which satisfies: A * adj(A) = det(A) * I
        //    // 4. Divide adjugate matrix with the determinant to find the inverse

        //    float det1, det2, det3, det4, det5, det6, det7, det8, det9, det10, det11, det12;
        //    float detMatrix;
        //    findDeterminants(ref matrix, out detMatrix, out det1, out det2, out det3, out det4, out det5, out det6,
        //                     out det7, out det8, out det9, out det10, out det11, out det12);

        //    float invDetMatrix = 1f / detMatrix;

        //    Matrix ret; // Allow for matrix and result to point to the same structure

        //    ret.M11 = (matrix.M22 * det12 - matrix.M23 * det11 + matrix.M24 * det10) * invDetMatrix;
        //    ret.M12 = (-matrix.M12 * det12 + matrix.M13 * det11 - matrix.M14 * det10) * invDetMatrix;
        //    ret.M13 = (matrix.M42 * det6 - matrix.M43 * det5 + matrix.M44 * det4) * invDetMatrix;
        //    ret.M14 = (-matrix.M32 * det6 + matrix.M33 * det5 - matrix.M34 * det4) * invDetMatrix;
        //    ret.M21 = (-matrix.M21 * det12 + matrix.M23 * det9 - matrix.M24 * det8) * invDetMatrix;
        //    ret.M22 = (matrix.M11 * det12 - matrix.M13 * det9 + matrix.M14 * det8) * invDetMatrix;
        //    ret.M23 = (-matrix.M41 * det6 + matrix.M43 * det3 - matrix.M44 * det2) * invDetMatrix;
        //    ret.M24 = (matrix.M31 * det6 - matrix.M33 * det3 + matrix.M34 * det2) * invDetMatrix;
        //    ret.M31 = (matrix.M21 * det11 - matrix.M22 * det9 + matrix.M24 * det7) * invDetMatrix;
        //    ret.M32 = (-matrix.M11 * det11 + matrix.M12 * det9 - matrix.M14 * det7) * invDetMatrix;
        //    ret.M33 = (matrix.M41 * det5 - matrix.M42 * det3 + matrix.M44 * det1) * invDetMatrix;
        //    ret.M34 = (-matrix.M31 * det5 + matrix.M32 * det3 - matrix.M34 * det1) * invDetMatrix;
        //    ret.M41 = (-matrix.M21 * det10 + matrix.M22 * det8 - matrix.M23 * det7) * invDetMatrix;
        //    ret.M42 = (matrix.M11 * det10 - matrix.M12 * det8 + matrix.M13 * det7) * invDetMatrix;
        //    ret.M43 = (-matrix.M41 * det4 + matrix.M42 * det2 - matrix.M43 * det1) * invDetMatrix;
        //    ret.M44 = (matrix.M31 * det4 - matrix.M32 * det2 + matrix.M33 * det1) * invDetMatrix;

        //    result = ret;
        //}

        ///// <summary>
        ///// Helper method for using the Laplace expansion theorem using two rows expansions to calculate major and 
        ///// minor determinants of a 4x4 matrix. This method is used for inverting a matrix.
        ///// </summary>
        //private static void findDeterminants(ref Matrix matrix, out float major,
        //                                     out float minor1, out float minor2, out float minor3, out float minor4, out float minor5, out float minor6,
        //                                     out float minor7, out float minor8, out float minor9, out float minor10, out float minor11, out float minor12)
        //{
        //    double det1 = (double)matrix.M11 * (double)matrix.M22 - (double)matrix.M12 * (double)matrix.M21;
        //    double det2 = (double)matrix.M11 * (double)matrix.M23 - (double)matrix.M13 * (double)matrix.M21;
        //    double det3 = (double)matrix.M11 * (double)matrix.M24 - (double)matrix.M14 * (double)matrix.M21;
        //    double det4 = (double)matrix.M12 * (double)matrix.M23 - (double)matrix.M13 * (double)matrix.M22;
        //    double det5 = (double)matrix.M12 * (double)matrix.M24 - (double)matrix.M14 * (double)matrix.M22;
        //    double det6 = (double)matrix.M13 * (double)matrix.M24 - (double)matrix.M14 * (double)matrix.M23;
        //    double det7 = (double)matrix.M31 * (double)matrix.M42 - (double)matrix.M32 * (double)matrix.M41;
        //    double det8 = (double)matrix.M31 * (double)matrix.M43 - (double)matrix.M33 * (double)matrix.M41;
        //    double det9 = (double)matrix.M31 * (double)matrix.M44 - (double)matrix.M34 * (double)matrix.M41;
        //    double det10 = (double)matrix.M32 * (double)matrix.M43 - (double)matrix.M33 * (double)matrix.M42;
        //    double det11 = (double)matrix.M32 * (double)matrix.M44 - (double)matrix.M34 * (double)matrix.M42;
        //    double det12 = (double)matrix.M33 * (double)matrix.M44 - (double)matrix.M34 * (double)matrix.M43;

        //    major = (float)(det1 * det12 - det2 * det11 + det3 * det10 + det4 * det9 - det5 * det8 + det6 * det7);
        //    minor1 = (float)det1;
        //    minor2 = (float)det2;
        //    minor3 = (float)det3;
        //    minor4 = (float)det4;
        //    minor5 = (float)det5;
        //    minor6 = (float)det6;
        //    minor7 = (float)det7;
        //    minor8 = (float)det8;
        //    minor9 = (float)det9;
        //    minor10 = (float)det10;
        //    minor11 = (float)det11;
        //    minor12 = (float)det12;
        //}

    }
}
