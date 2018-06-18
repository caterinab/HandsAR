using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter.Fractal
{
    /// <summary>
    /// Mandelbrot set.
    /// </summary>
    public class MandelbrotFractal: Fractal
    {

        public MandelbrotFractal(int widthImage, int heightImage):base(widthImage, heightImage)
        {

        }

        public override void BuildFractal()
        {
            for (int i = 0; i < FractalBitmap.Width; i++)
            {
                for (int j = 0; j < FractalBitmap.Height; j++)
                {
                    int count = 0;
                    Complex c = new Complex(X1 + ((double)((X2 - X1) * i) / (double)FractalBitmap.Width), Y1 + ((double)((Y2 - Y1) * j) / (double)FractalBitmap.Height));
                    Complex z = new Complex(0, 0);
                    Complex z1 = z;
                    Complex delta = c;
                    while ((count < MaxIterations) & (z.Abs < 1000000) & (delta.Abs > 0.0000001))
                    {
                        z = z * z + c;
                        delta = z1 - z;
                        z1 = z;
                        count++;
                    }
                    int color = (int)((double)count * 255 / (double)MaxIterations);
                    FractalBitmap.SetPixel(i, j, Color.FromArgb(color, color, color));
                    //fractalBitmap.SetPixel(i, j, Color.FromArgb(count, count, count));
                }
            }
        }
    }
}
