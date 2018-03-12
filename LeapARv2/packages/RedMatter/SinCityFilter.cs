using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter
{
    /// <summary>
    /// Filter for "Sin City" style.
    /// </summary>
    public class SinCityFilter
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

        private double tolerance = 50;

        /// <summary>
        /// Color tolerance.
        /// </summary>
        public double Tolerance
        {
            get { return tolerance; }
            set { tolerance = value; }
        }

        private Color mainColor;

        /// <summary>
        /// Color for filtering.
        /// </summary>
        public Color MainColor
        {
            get { return mainColor; }
            set { mainColor = value; }
        }


        public SinCityFilter(Bitmap bmp, Color main)
        {
            sourceBitmap = bmp;
            resultImage = new Bitmap(sourceBitmap);
            mainColor = main;
            BuildImage();
        }

        private void BuildImage()
        {
            for (int i = 1; i < sourceBitmap.Width-1; i++)
            {
                for (int j = 1; j < sourceBitmap.Height-1; j++)
                {
                    Color color = sourceBitmap.GetPixel(i, j);
                    double distance = DrawUtils.GetColorDistance(color, mainColor);
                    if (distance < tolerance)
                    {
                        double k = distance / tolerance;
                        resultImage.SetPixel(i, j, DrawUtils.SuperpositionColor(DrawUtils.GetGrayColor(color), color, k));
                    }
                    else
                    {
                        resultImage.SetPixel(i, j, DrawUtils.GetGrayColor(color));
                    }

                }
            }
        }
    }
}
