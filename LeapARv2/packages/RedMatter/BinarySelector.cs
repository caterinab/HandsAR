using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter
{
        /// <summary>
        /// Class for building binary image.
        /// </summary>
        public class BinarySelector
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
            private Color firstColor = Color.Black;

            public Color FirstColor
            {
                get { return firstColor; }
                set { firstColor = value; }
            }

            /// <summary>
            /// Color for select image.
            /// </summary>
            private Color secondColor = Color.White;

            public Color SecondColor
            {
              get { return secondColor; }
              set { secondColor = value; }
            }

            /// <summary>
            /// Building selection image.
            /// </summary>
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
                                bmp.SetPixel(i, j, secondColor);
                            }
                            else
                            {
                                bmp.SetPixel(i, j, firstColor);
                            }
                        }
                    }
                    return bmp;
                }
            }

            #region Constructors

            public BinarySelector(Bitmap bmp)
            {
                this.sourceImage = bmp;
            }

            public BinarySelector(Bitmap bmp, IImageSelector selector)
            {
                this.sourceImage = bmp;
                this.selector = selector;
            }

            #endregion
    }
}
