using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedMatter.SkinDetection
{
    public class SimpleSkinDetector1: ISkinDetector
    {
        #region ISkinDetector Members

        public bool IsSkin(System.Drawing.Color color)
        {
            if (((double)color.R/(double)color.B > 1.185)&
               ((double)(color.R*color.B)/(double)(DrawUtils.ChannelSum(color)*DrawUtils.ChannelSum(color))>0.107)&
               ((double)(color.R*color.G)/(double)(DrawUtils.ChannelSum(color)*DrawUtils.ChannelSum(color))>0.112))
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
