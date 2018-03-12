using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter.SkinDetection
{
    /// <summary>
    /// Class for building integral projection for detected binary image.
    /// </summary>
    public class IntegralProjection
    {
        /// <summary>
        /// Binary image.
        /// </summary>
        private Bitmap detectedImage;

        public Bitmap DetectedImage
        {
            get { return detectedImage; }
            set { detectedImage = value; }
        }

        /// <summary>
        /// Horizontal integral projection.
        /// </summary>
        private int[] horizontalProjection;

        public int[] HorizontalProjection
        {
            get { return horizontalProjection; }
        }

        /// <summary>
        /// Vertical integral projection.
        /// </summary>
        private int[] verticalProjection;

        public int[] VerticalProjection
        {
            get { return verticalProjection; }
        }

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="bmp">Image for detection.</param>
        public IntegralProjection(Bitmap bmp)
        {
            this.detectedImage = bmp;
            verticalProjection = new int[bmp.Height];
            horizontalProjection = new int[bmp.Width];

            BuildProjections();
        }

        #endregion

        /// <summary>
        /// Building integral projections.
        /// </summary>
        private void BuildProjections()
        {
            for (int i = 0; i < detectedImage.Width; i++)
            {
                int sum = 0;
                for (int j = 0; j < detectedImage.Height; j++)
                {
                    if (DrawUtils.IsEqualColors(detectedImage.GetPixel(i, j), Color.White))
                    {
                        sum++;
                    }
                }
                horizontalProjection[i] = sum;
            }

            for (int i = 0; i < detectedImage.Height; i++)
            {
                int sum = 0;
                for (int j = 0; j < detectedImage.Width; j++)
                {
                    if (DrawUtils.IsEqualColors(detectedImage.GetPixel(j, i), Color.White))
                    {
                        sum++;
                    }
                }
                verticalProjection[i] = sum;
            }
        }
    }
}
