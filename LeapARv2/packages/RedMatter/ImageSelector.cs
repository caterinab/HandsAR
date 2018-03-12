using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter
{
    /// <summary>
    /// Class for selection pixels of image.
    /// </summary>
    public class ImageSelector
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
        /// Selection method;
        /// </summary>
        private IImageSelector selector;

        public IImageSelector Selector
        {
            get { return selector; }
            set { selector = value; }
        }

        /// <summary>
        /// Color for background of selection.
        /// </summary>
        private Color backColor = Color.Black;

        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }

        public Bitmap SelectImage
        {
            get
            {
                Bitmap bmp = new Bitmap(sourceImage);
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        Color pixel = bmp.GetPixel(i, j);
                        if (selector.IsSelectedPoint(pixel) == true)
                        {
                            bmp.SetPixel(i, j, pixel);
                        }
                        else
                        {
                            bmp.SetPixel(i, j, backColor);
                        }
                    }
                }
                return bmp;
            }
        }

        #region Constructors

        public ImageSelector(Bitmap bmp)
        {
            this.sourceImage = bmp;
        }

        public ImageSelector(Bitmap bmp, IImageSelector selector)
        {
            this.sourceImage = bmp;
            this.selector = selector;
        }

        #endregion
    }
}
