using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedMatter.SkinDetection
{
    public class SimpleSkinDetector2: ISkinDetector
    {
        #region ISkinDetector Members

        public bool IsSkin(System.Drawing.Color color)
        {
            if(((double)3*color.B*color.R*color.R/(double)(DrawUtils.ChannelSum(color)*DrawUtils.ChannelSum(color)*DrawUtils.ChannelSum(color))>0.1276)&
                ((double)(color.R*color.B+color.G*color.G)/(double)(color.G*color.B)>2.14)&
                ((double)(DrawUtils.ChannelSum(color))/(double)(3*color.R)+(double)(color.R-color.G)/(double)(DrawUtils.ChannelSum(color))<2.7775))
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
