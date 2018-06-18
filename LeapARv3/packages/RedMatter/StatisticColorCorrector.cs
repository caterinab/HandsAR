using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter
{
    /// <summary>
    /// Class for statistic color correction of source image.
    /// </summary>
    public class StatisticColorCorrector
    {
        /// <summary>
        /// Source image.
        /// </summary>
        private Bitmap sourceBitmap;

        public Bitmap SourceBitmap
        {
            get { return sourceBitmap; }
            set { sourceBitmap = value; }
        }

        /// <summary>
        /// Bitmap for correction.
        /// </summary>
        private Bitmap correctBitmap;

        public Bitmap CorrectBitmap
        {
            get { return correctBitmap; }
            set { correctBitmap = value; }
        }

        /// <summary>
        /// Result corrected image.
        /// </summary>
        public Bitmap ResultImage
        {
            get
            {
                return CorrectImage(sourceBitmap, correctBitmap);
            }
        }

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="bmp">Source Image</param>
        public StatisticColorCorrector(Bitmap bmp)
        {
            this.sourceBitmap = bmp;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="bmp">Source Image</param>
        /// <param name="correct">Image for correction.</param>
        public StatisticColorCorrector(Bitmap bmp, Bitmap correct)
        {
            this.sourceBitmap = bmp;
            this.correctBitmap = correct;
        }

        #endregion

        /// <summary>
        /// Color correction.
        /// </summary>
        /// <param name="sourceImage">Source image.</param>
        /// <param name="correctionBitmap">Image for correction.</param>
        /// <returns>Corrected image.</returns>
        private static Bitmap CorrectImage(Bitmap sourceImage, Bitmap correctionBitmap)
        {
            Bitmap bmp = new Bitmap(sourceImage);
            Color meanSourceColor = DrawUtils.GetMeanValue(sourceImage);
            Color dispSourceColor = DrawUtils.GetDispersion(sourceImage);
            Color meanCorrectColor = DrawUtils.GetMeanValue(correctionBitmap);
            Color dispCorrectColor = DrawUtils.GetDispersion(correctionBitmap);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    Color oldColor = sourceImage.GetPixel(i, j);
                    double r = meanCorrectColor.R + (oldColor.R - meanSourceColor.R) * dispCorrectColor.R / dispSourceColor.R;
                    double g = meanCorrectColor.G + (oldColor.G - meanSourceColor.G) * dispCorrectColor.G / dispSourceColor.G;
                    double b = meanCorrectColor.B + (oldColor.B - meanSourceColor.B) * dispCorrectColor.B / dispSourceColor.B;
                    bmp.SetPixel(i, j, DrawUtils.GetCorrectColor((int)r, (int)g, (int)b));
                }
            }
            return bmp;
        }
    }
}
