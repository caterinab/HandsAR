using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter.SkinDetection
{
    /// <summary>
    /// Class for skin detection.
    /// </summary>
    public class BaseDetector
    {
        /// <summary>
        /// Source image.
        /// </summary>
        private Bitmap sourceImage;

        public Bitmap SourceImage
        {
            get { return sourceImage; }
            set { sourceImage = value; }
        }
        /// <summary>
        /// Detection method.
        /// </summary>
        private ISkinDetector detector;

        public ISkinDetector Detector
        {
            get { return detector; }
            set { detector = value; }
        }

        #region Constructors

        public BaseDetector(Bitmap bmp)
        {
            this.sourceImage = bmp;
        }

        public BaseDetector(Bitmap bmp, ISkinDetector detector)
        {
            this.sourceImage = bmp;
            this.detector = detector;
        }

        #endregion

        public Bitmap SkinDetectionImage
        {
            get
            {
                ImageSelector selector = new ImageSelector(sourceImage, detector); 
                return selector.SelectImage;
            }
        }
    }
}
