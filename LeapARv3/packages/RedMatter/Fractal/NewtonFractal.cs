using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter.Fractal
{
    /// <summary>
    /// Newton fractal.
    /// </summary>
    public class NewtonFractal:Fractal
    {
        //private Bitmap fractalBitmap;

        //public Bitmap FractalBitmap
        //{
        //    get 
        //    {
        //        BuildFractal();
        //        return fractalBitmap; 
        //    }
        //}

        //private int width, height;

        //public int Height
        //{
        //    get { return height; }
        //    set { height = value; }
        //}

        //public int Width
        //{
        //    get { return width; }
        //    set { width = value; }
        //}

        //private int maxIterations = 64;

        //public int MaxIterations
        //{
        //    get { return maxIterations; }
        //    set { maxIterations = value; }
        //}

        //private double x1, y1, x2, y2;

        //public double Y2
        //{
        //    get { return y2; }
        //    set { y2 = value; }
        //}

        //public double X2
        //{
        //    get { return x2; }
        //    set { x2 = value; }
        //}

        //public double Y1
        //{
        //    get { return y1; }
        //    set { y1 = value; }
        //}

        //public double X1
        //{
        //    get { return x1; }
        //    set { x1 = value; }
        //}

        public NewtonFractal(int widthImage, int heightImage):base(widthImage, heightImage)
        {
            //this.height = heightImage;
            //this.width = widthImage;
            //fractalBitmap = new Bitmap(widthImage, heightImage);
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
                        z = z - (z * z * z - (new Complex(1, 0))) / (z * z * (new Complex(3, 0)));
                        delta = z1 - z;
                        z1 = z;
                        count++;
                    }
                    FractalBitmap.SetPixel(i,j,Color.FromArgb((int)((double)count*255/(double)MaxIterations),0,0));
                    //fractalBitmap.SetPixel(i, j, Color.FromArgb(count, count, count));
                }
            }
        }
    }
}
