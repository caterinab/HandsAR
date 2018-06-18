using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedMatter.SkinDetection
{
    public class SimpleSkinDetector3:ISkinDetector
    {
        #region ISkinDetector Members

        public bool IsSkin(System.Drawing.Color color)
        {
            if (((double)color.G / (double)color.G - (double)color.R / (double)color.B <= -0.0905) &
                ((double)(DrawUtils.ChannelSum(color)) / (double)(3 * color.R) + (double)(color.R - color.G) / (double)(DrawUtils.ChannelSum(color)) <= 0.9498))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region IImageSelector Members

        public bool IsSelectedPoint(System.Drawing.Color color)
        {
            return IsSkin(color);
        }

        #endregion
    }
}
