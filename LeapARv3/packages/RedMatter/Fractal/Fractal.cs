using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter.Fractal
{
    public abstract class Fractal
    {
        private Bitmap fractalBitmap;

        public Bitmap FractalBitmap
        {
            get 
            {
                return fractalBitmap; 
            }
            set
            {
                fractalBitmap = value;
            }
        }

        private int width, height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        private int maxIterations = 64;

        public int MaxIterations
        {
            get { return maxIterations; }
            set { maxIterations = value; }
        }

        private double x1, y1, x2, y2;

        public double Y2
        {
            get { return y2; }
            set { y2 = value; }
        }

        public double X2
        {
            get { return x2; }
            set { x2 = value; }
        }

        public double Y1
        {
            get { return y1; }
            set { y1 = value; }
        }

        public double X1
        {
            get { return x1; }
            set { x1 = value; }
        }

        public Fractal(int widthImage, int heightImage)
        {
            this.height = heightImage;
            this.width = widthImage;
            fractalBitmap = new Bitmap(widthImage, heightImage);
        }

        public abstract void BuildFractal();
    }
}
