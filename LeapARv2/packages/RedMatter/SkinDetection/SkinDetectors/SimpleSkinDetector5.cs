using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedMatter.SkinDetection
{
    public class SimpleSkinDetector5:ISkinDetector
    {
        #region ISkinDetector Members

        public bool IsSkin(System.Drawing.Color color)
        {
            if (((double)color.G/(double)color.B - (double)color.R/(double)color.G<=-0.0905)&
                ((double)(color.G*DrawUtils.ChannelSum(color))/(double)(color.B*(color.R-color.G))>3.4857)&
                ((double)(DrawUtils.ChannelSum(color)*DrawUtils.ChannelSum(color)*DrawUtils.ChannelSum(color))/(double)(3*color.G*color.R*color.R)<=7.397)&
                ((double)DrawUtils.ChannelSum(color)/(double)(9*color.R)-0.333 > -0.0976))
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
