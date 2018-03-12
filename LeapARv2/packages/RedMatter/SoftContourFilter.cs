using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter
{
    /// <summary>
    /// Soft contour filter.
    /// </summary>
    public class SoftContourFilter
    {
        private Bitmap sourceBitmap;

        public Bitmap SourceBitmap
        {
            get { return sourceBitmap; }
            set { sourceBitmap = value; }
        }

        private Bitmap resultImage;

        public Bitmap ResultImage
        {
            get { return resultImage; }
            set { resultImage = value; }
        }

        private double threshold = 50;

        public double Threshold
        {
            get { return threshold; }
            set { threshold = value; }
        }

        private double softness = 10;

        public double Softness
        {
            get { return softness; }
            set { softness = value; }
        }

        private double[,] core = new double[3, 3];

        public SoftContourFilter(Bitmap bmp)
        {
            sourceBitmap = bmp;
            resultImage = new Bitmap(sourceBitmap);
            InitCore();
            BuildImage();
        }

        private void InitCore()
        {
            core[0, 0] = -1;
            core[0, 1] = -2;
            core[0, 2] = -1;
            core[1, 0] = 0;
            core[1, 1] = 0;
            core[1, 2] = 0;
            core[2, 0] = 1;
            core[2, 1] = 2;
            core[2, 2] = 1;
        }

        private void BuildImage()
        {
            for (int i = 1; i < sourceBitmap.Width-1; i++)
            {
                for (int j = 1; j < sourceBitmap.Height-1; j++)
                {
                    double sx = 0;
                    sx = DrawUtils.GetGray(sourceBitmap.GetPixel(i - 1, j - 1)) * core[0, 0] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i, j - 1)) * core[0, 1] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i + 1, j - 1)) * core[0, 2] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i - 1, j)) * core[1, 0] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i, j)) * core[1, 1] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i + 1, j)) * core[1, 2] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i - 1, j + 1)) * core[2, 0] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i, j + 1)) * core[2, 1] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i + 1, j + 1)) * core[2, 2];

                    double sy = 0;
                    sy = DrawUtils.GetGray(sourceBitmap.GetPixel(i - 1, j - 1)) * core[0, 0] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i, j - 1)) * core[1, 0] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i + 1, j - 1)) * core[2, 0] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i - 1, j)) * core[0, 1] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i, j)) * core[1, 1] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i + 1, j)) * core[2, 1] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i - 1, j + 1)) * core[0, 2] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i, j + 1)) * core[1, 2] +
                            DrawUtils.GetGray(sourceBitmap.GetPixel(i + 1, j + 1)) * core[2, 2];

                    double level = Math.Sqrt(sx * sx + sy * sy);
                    if (level > threshold)
                    {
                        Color color = sourceBitmap.GetPixel(i, j);
                        int r = color.R - (int)(((level - threshold) / threshold) * softness);
                        int g = color.G - (int)(((level - threshold) / threshold) * softness);
                        int b = color.B - (int)(((level - threshold) / threshold) * softness);
                        resultImage.SetPixel(i, j, DrawUtils.GetCorrectColor(r, g, b));
                    }
                    else
                    {
                        //resultImage.SetPixel(i, j, Color.White);
                        resultImage.SetPixel(i, j, sourceBitmap.GetPixel(i, j));
                    }

                }
            }
        }

    }
}
