using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blade.MG.SVG
{
    // https://dev.w3.org/SVG/tools/svgweb/samples/svg-files/
    // https://www.rapidtables.com/web/tools/svg-viewer-editor.html
    // https://www.drawsvg.org/drawsvg.html
    // https://editor.method.ac/

    public class SVGDocument : SVGGroup
    {
        public RenderTarget2D CachedTexture2D = null;

        public SVGDocument()
        {

        }
        public SVGDocument(Color stroke, float? strokeWidth = null, SVGFill fill = null)
        {
            this.Stroke = stroke;
            this.StrokeWidth = strokeWidth;
            this.Fill = fill;
        }

        public Rectangle Measure(Matrix? matrix = null)
        {
            Matrix m = matrix ?? Matrix.Identity;
            var rectangle = SVGRenderer.MeasurePath(this, m);

            return rectangle;
        }

        public Rectangle Draw(SpriteBatch spriteBatch, Matrix matrix, float depth = 0f, bool cacheImage = true)
        {

            if (cacheImage)
            {
                if (CachedTexture2D == null)
                {
                    //// Remove the Scale Component
                    //matrix.Decompose(out Vector3 scale1, out Quaternion rotation1, out Vector3 translation1);
                    //var newMatrix = Matrix.Multiply(Matrix.CreateFromQuaternion(rotation1), Matrix.CreateTranslation(translation1));

                    //var bounds = Measure(newMatrix);
                    //CacheTexture(spriteBatch.GraphicsDevice, newMatrix, new Point(bounds.Width, bounds.Height));

                    //var bounds = Measure(matrix);
                    //CacheTexture(spriteBatch.GraphicsDevice, matrix, new Point(bounds.Width, bounds.Height));

                    //var bounds = Measure(matrix);
                    CacheTexture(spriteBatch.GraphicsDevice, matrix, Point.Zero);
                }

                DecomposeMatrix2D(ref matrix, out var position, out var rotation, out var scale);
                spriteBatch.Draw(CachedTexture2D, position, CachedTexture2D.Bounds, Color.White, rotation, new Vector2(0f, 0f), scale, SpriteEffects.None, depth);

                return new Rectangle(new Point((int)position.X, (int)position.Y), new Point((int)(CachedTexture2D.Width), (int)(CachedTexture2D.Height)));
                //return new Rectangle(new Point((int)position.X, (int)position.Y), new Point((int)(CachedTexture2D.Width * scale.X), (int)(CachedTexture2D.Height * scale.Y)));
            }
            else
            {
                matrix.Decompose(out Vector3 scale1, out Quaternion rotation1, out Vector3 translation1);
                //var newMatrix = Matrix.Multiply(Matrix.CreateScale(scale1), Matrix.CreateTranslation(translation1));
                var newMatrix = Matrix.CreateScale(scale1);
                //var newMatrix = Matrix.Multiply(Matrix.CreateScale(scale1), Matrix.CreateFromQuaternion(rotation1));

                //var rectangle = SVGRenderer.MeasurePath(this, newMatrix);

                //var position = matrix.GetPosition() + new Vector3(rectangle.X + rectangle.Width / 2f, rectangle.Y + rectangle.Height / 2f);
                //var matrix2 = Matrix.Multiply(matrix, Matrix.CreateTranslation(-rectangle.X - rectangle.Width / 2f + position.X, -rectangle.Y - rectangle.Height / 2f + position.Y, 0f));
                //var matrix2 = Matrix.Multiply(matrix, Matrix.CreateTranslation(-rectangle.X - rectangle.Width / 2f, - rectangle.Y - rectangle.Height / 2f , 0f));
                var matrix2 = matrix;

                SVGRenderer.DrawPath(this, spriteBatch, matrix2, out var dimensions, false);
                //SVGRenderer.DrawPath(this, spriteBatch, Matrix.Multiply(Matrix.CreateScale(16f), matrix));

                //var rectangle = dimensions.ToRectangle();
                //var position = new Vector3(dimensions.CenterX, dimensions.CenterY, 0f);
                //Primitives2D.FillCircle(spriteBatch, position, 3, Color.Yellow);

                return dimensions.ToRectangle();
            }
        }

        //public void Draw(SpriteBatch spriteBatch, Matrix matrix, float depth = 0f)
        //{
        //    if (cachedTexture2D == null)
        //    {
        //        cachedTexture2D = ToTexture2D(spriteBatch, matrix);
        //    }

        //    GameHelper.DecomposeMatrix2D(ref matrix, out var position, out var rotation, out var scale);

        //    spriteBatch.Draw(cachedTexture2D, position, cachedTexture2D.Bounds, Color.White, rotation, new Vector2(0f, 0f), 1f, SpriteEffects.None, depth);
        //    //spriteBatch.Draw(cachedTexture2D, position, cachedTexture2D.Bounds, Color.White, rotation, new Vector2(0f, 0f), scale, SpriteEffects.None, depth);
        //}


        public void CacheTexture(GraphicsDevice graphicsDevice, Matrix matrix, Point size)
        {
            if (CachedTexture2D != null)
            {
                CachedTexture2D.Dispose();
            }

            CachedTexture2D = ToTexture2D(graphicsDevice, matrix, size);
        }

        public RenderTarget2D ToTexture2D(GraphicsDevice graphicsDevice, Matrix matrix, Point size, int padding = 0)
        {
            var rectangle = SVGRenderer.MeasurePath(this, matrix);


            if (size == Point.Zero)
            {
                size = new Point(rectangle.Width + padding * 2, rectangle.Height + padding * 2);

                // Move to Top-Left corner
                matrix = Matrix.Multiply(matrix, Matrix.CreateTranslation(-rectangle.X + padding, -rectangle.Y + padding, 0f));
            }
            else
            {
                float newWidth = size.X - padding * 2;
                float newHeight = size.Y - padding * 2;

                float scaleX = newWidth / (float)rectangle.Width;
                float scaleY = newHeight / (float)rectangle.Height;
                float scale = scaleX <= scaleY ? scaleX : scaleY;

                matrix = Matrix.Multiply(Matrix.CreateScale(scale), matrix);

                // Move to Top-Left corner
                int sizeX = (int)(rectangle.Width * scale) + 2 * padding;
                int sizeY = (int)(rectangle.Height * scale) + 2 * padding;
                matrix = Matrix.Multiply(matrix, Matrix.CreateTranslation(-rectangle.X * scale + padding + ((size.X - sizeX) / 2f), -rectangle.Y * scale + padding + ((size.Y - sizeY) / 2f), 0f));
            }

            RenderTarget2D renderTarget2D = new RenderTarget2D(graphicsDevice, size.X, size.Y);

            using var sb = new SpriteBatch(graphicsDevice);
            var saveRenderTargets = graphicsDevice.GetRenderTargets();

            try
            {

                graphicsDevice.SetRenderTarget(renderTarget2D);
                graphicsDevice.Clear(Color.Transparent);
                //sb.GraphicsDevice.Clear(Color.CornflowerBlue);

                sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, null, null, null);
                //sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, null, null, null);
                SVGRenderer.DrawPath(this, sb, matrix, out var dimensions, false);

            }
            finally
            {
                sb.End();
            }

            graphicsDevice.SetRenderTargets(saveRenderTargets);

            return renderTarget2D;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matrix"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="scale"></param>
        private void DecomposeMatrix2D(ref Matrix matrix, out Vector2 position, out float rotation, out Vector2 scale)
        {
            Vector3 position3, scale3;
            Quaternion rotationQ;

            matrix.Decompose(out scale3, out rotationQ, out position3);

            Vector2 direction = Vector2.Transform(Vector2.UnitX, rotationQ);
            rotation = (float)Math.Atan2((float)(direction.Y), (float)(direction.X));
            position = new Vector2(position3.X, position3.Y);
            scale = new Vector2(scale3.X, scale3.Y);
        }


    }
}
