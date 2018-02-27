// ===============================
// AUTHOR     : Mirko Pani
// CREATE DATE     : 24/10/17
// PURPOSE     : Classe di supporto per generare quaternioni da una matrice di rotazione
// SPECIAL NOTES: 
// ===============================
// Change History:
//
//==================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomLeap
{
    class QuaternionHelper
    {
        //Genera un quaternione a partire dalla matrice di rotazione  (i tre vettori)
        public static Leap.LeapQuaternion generateQuaternion(Leap.Vector x, Leap.Vector y, Leap.Vector z)
        {
            /*   double wr = Math.Sqrt(1 + x[0] + y[1] + z[2]) / 2;
               double xr = (z[1] - y[2]) / (4 * wr);
               double yr = (x[2] - z[0]) / (4 * wr);
               double zr = (y[0] - x[1]) / (4 * wr);*/

            double wr, xr, yr, zr;

            float trace = x[0] + y[1] + z[2]; // I removed + 1.0f; see discussion with Ethan
            if (trace > 0)
            {// I changed M_EPSILON to 0
                float s = 0.5f / (float)Math.Sqrt(trace + 1.0f);
                wr = 0.25f / s;
                xr = (z[1] - y[2]) * s;
                yr = (x[2] - z[0]) * s;
                zr = (y[0] - x[1]) * s;
            }
            else
            {
                if (x[0] > y[1] && x[0] > z[2])
                {
                    float s = 2.0f * (float)Math.Sqrt(1.0f + x[0] - y[1] - z[2]);
                    wr = (z[1] - y[2]) / s;
                    xr = 0.25f * s;
                    yr = (x[1] + y[0]) / s;
                    zr = (x[2] + z[0]) / s;
                }
                else if (y[1] > z[2])
                {
                    float s = 2.0f * (float)Math.Sqrt(1.0f + y[1] - x[0] - z[2]);
                    wr = (x[2] - z[0]) / s;
                    xr = (x[1] + y[0]) / s;
                    yr = 0.25f * s;
                    zr = (y[2] + z[1]) / s;
                }
                else
                {
                    float s = 2.0f * (float)Math.Sqrt(1.0f + z[2] - x[0] - y[1]);
                    wr = (y[0] - x[1]) / s;
                    xr = (x[2] + z[0]) / s;
                    yr = (y[2] + z[1]) / s;
                    zr = 0.25f * s;
                }
            }


            /*if (double.IsNaN(xr) || double.IsNaN(yr) || double.IsNaN(zr))
            {
                throw new ArithmeticException();
            }*/

            return new Leap.LeapQuaternion((float)-xr, (float)-yr, (float)-zr, (float)wr);
        }
    }
}

