using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedMatter.Fractal
{
    /// <summary>
    /// Complex number.
    /// </summary>
    public class Complex
    {
        private double im;

        public double Im
        {
            get { return im; }
            set { im = value; }
        }
        private double re;

        public double Re
        {
            get { return re; }
            set { re = value; }
        }

        public Complex(double re, double im)
        {
            this.re = re;
            this.im = im;
        }

        public double Abs
        {
            get
            {
                return Math.Sqrt(re * re + im * im);
            }
        }

        public static Complex operator +(Complex c1, Complex c2)
        {
            return new Complex(c1.Re + c2.Re, c1.Im + c2.Im);
        }

        public static Complex operator -(Complex c1, Complex c2)
        {
            return new Complex(c1.Re - c2.Re, c1.Im - c2.Im);
        }

        public static Complex operator *(Complex c1, Complex c2)
        {
            return new Complex(c1.Re * c2.Re - c1.Im * c2.Im, c1.Im * c2.Re + c1.Re * c2.Im);
        }

        public static Complex operator /(Complex c1, Complex c2)
        {
            return new Complex((c1.Re * c2.Re + c1.Im * c2.Im) / (c2.Abs * c2.Abs), (c1.Im * c2.Re - c1.Re * c2.Im) / (c2.Abs * c2.Abs));
        }
    }
}
