using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter.SkinDetection
{
    /// <summary>
    /// Parametric skin detection model.
    /// </summary>
    public class GaussianSkinDetector:ISkinDetector
    {
        /// <summary>
        /// Training data.
        /// </summary>
        private Bitmap sampleImage;

        public Bitmap SampleImage
        {
            get { return sampleImage; }
            set { sampleImage = value; }
        }
        private Color meanSample;
        private Color dispSample;

        /// <summary>
        /// Threshold of detection.
        /// </summary>
        private double threshold = 0.5;

        public double Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        #region Constructor

        public GaussianSkinDetector(Bitmap skinSample)
        {
            this.sampleImage = skinSample;
            meanSample = DrawUtils.GetMeanValue(sampleImage);
            dispSample = DrawUtils.GetDispersion(sampleImage);
        }

        #endregion



        #region ISkinDetector Members

        public bool IsSkin(System.Drawing.Color color)
        {
            double pR = (1.0 / (2 * Math.PI * dispSample.R / 255)) * Math.Exp((-0.5) * (color.R - dispSample.R) * (color.R - dispSample.R) / (256 * 256));
            double pG = (1.0 / (2 * Math.PI * dispSample.G / 255)) * Math.Exp((-0.5) * (color.G - dispSample.G) * (color.G - dispSample.G) / (256 * 256));
            double pB = (1.0 / (2 * Math.PI * dispSample.B / 255)) * Math.Exp((-0.5) * (color.B - dispSample.B) * (color.B - dispSample.B) / (256 * 256));
            if (pR + pG + pB < threshold)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region IImageSelector Members

        public bool IsSelectedPoint(System.Drawing.Color color)
        {
            return IsSkin(color);
        }

        #endregion
    }
}
