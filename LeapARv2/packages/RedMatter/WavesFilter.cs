using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter
{
    /// <summary>
    /// Ellipse vignette.
    /// </summary>
    public class WavesFilter
    {
        private Bitmap sourceBitmap;

        /// <summary>
        /// Source image.
        /// </summary>
        public Bitmap SourceBitmap
        {
            get { return sourceBitmap; }
            set { sourceBitmap = value; }
        }

        private Bitmap resultImage;

        /// <summary>
        /// Result image.
        /// </summary>
        public Bitmap ResultImage
        {
            get
            {
                BuildImage();
                return resultImage;
            }
            set { resultImage = value; }
        }

        private Color vignetteColor = Color.Black;

        public Color VignetteColor
        {
            get { return vignetteColor; }
            set { vignetteColor = value; }
        }

        private int period = 20;

        public int Period
        {
            get { return period; }
            set { period = value; }
        }

        private double fade = 1;

        public double Fade
        {
            get { return fade; }
            set 
            {
                if (fade > 1)
                {
                    fade = 1;
                }
                if (fade < 0)
                {
                    fade = 0;
                }
                fade = value; 
            }
        }

        public WavesFilter(Bitmap bmp)
        {
            sourceBitmap = bmp;
            resultImage = new Bitmap(sourceBitmap);
            //BuildImage();
        }

        private void BuildImage()
        {
            for (int i = 0; i < sourceBitmap.Width - 1; i++)
            {
                for (int j = 0; j < sourceBitmap.Height - 1; j++)
                {
                    int dX = Math.Min(i, sourceBitmap.Width - i);
                    int dY = Math.Min(j, sourceBitmap.Height - j);

                    double k = fade * Math.Sin(j * period);
                    Color color = sourceBitmap.GetPixel(i, j);
                    resultImage.SetPixel(i, j, DrawUtils.SuperpositionColor(DrawUtils.GetGrayColor(color), color, k));
                }
            }
        }
    }
}
