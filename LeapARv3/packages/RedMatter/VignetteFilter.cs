using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter
{
    /// <summary>
    /// Add vignette for image.
    /// </summary>
    public class VignetteFilter
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

        private int vignetteWidth;

        public int VignetteWidth
        {
            get { return vignetteWidth; }
            set { vignetteWidth = value; }
        }

        private int fade;

        public int Fade
        {
            get { return fade; }
            set { fade = value; }
        }

        public VignetteFilter(Bitmap bmp)
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

                    if ((dY <= vignetteWidth) & (dX <= vignetteWidth))
                    {
                        double k = 1 - (double)(Math.Min(dY, dX) - vignetteWidth + fade) / (double)fade;
                        resultImage.SetPixel(i, j, DrawUtils.SuperpositionColor(vignetteColor, sourceBitmap.GetPixel(i, j), k));
                        continue;
                    }

                    if ((dX < (vignetteWidth - fade)) | (dY < (vignetteWidth - fade)))
                    {
                        resultImage.SetPixel(i, j, vignetteColor);
                    }
                    else
                    {
                        if ((dX < vignetteWidth)&(dY>vignetteWidth))
                        {
                            double k = 1 - (double)(dX - vignetteWidth + fade) / (double)fade;
                            resultImage.SetPixel(i, j, DrawUtils.SuperpositionColor(vignetteColor, sourceBitmap.GetPixel(i, j), k));
                        }
                        else
                        {
                            if ((dY < vignetteWidth)&(dX > vignetteWidth))
                            {
                                double k = 1 - (double)(dY - vignetteWidth + fade) / (double)fade;
                                resultImage.SetPixel(i, j, DrawUtils.SuperpositionColor(vignetteColor, sourceBitmap.GetPixel(i, j), k));
                            }
                            else
                            {
                                resultImage.SetPixel(i, j, sourceBitmap.GetPixel(i, j));
                            }
                        }
                    }
                }
            }
        }
    }
}
