﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter.Fractal
{
    /// <summary>
    /// Julia set.
    /// </summary>
    public class JuliaFractal:Fractal
    {
        private Complex c = new Complex(0.05,-0.2);

        public Complex C
        {
            get { return c; }
            set { c = value; }
        }

        public JuliaFractal(int widthImage, int heightImage)
            : base(widthImage, heightImage)
        {

        }

        public override void BuildFractal()
        {
            for (int i = 0; i < FractalBitmap.Width; i++)
            {
                for (int j = 0; j < FractalBitmap.Height; j++)
                {
                    int count = 0;
                    Complex z = new Complex(X1 + ((double)((X2 - X1) * i) / (double)FractalBitmap.Width), Y1 + ((double)((Y2 - Y1) * j) / (double)FractalBitmap.Height));
                    Complex delta = z;
                    Complex z1 = z;
                    while ((count < MaxIterations) & (z.Abs < 1000000) & (delta.Abs > 0.0000001))
                    {
                        z = z * z + z + c;
                        delta = z1 - z;
                        z1 = z;
                        count++;
                    }
                    FractalBitmap.SetPixel(i, j, Color.FromArgb((int)((double)count * 255 / (double)MaxIterations), 0, 0));

                }
            }
        }
    }
}
