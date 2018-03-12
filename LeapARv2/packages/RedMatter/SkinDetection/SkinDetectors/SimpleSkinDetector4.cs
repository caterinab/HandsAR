using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedMatter.SkinDetection
{
    public class SimpleSkinDetector4:ISkinDetector
    {
        #region ISkinDetector Members

        public bool IsSkin(System.Drawing.Color color)
        {
            if (((double)color.B/(double)color.G<1.249)&
                ((double)DrawUtils.ChannelSum(color)/(double)(3*color.R)>0.696)&
                (0.3333-(double)color.B/(double)DrawUtils.ChannelSum(color)>0.014)&
                ((double)color.G/(double)(3*DrawUtils.ChannelSum(color))<0.108))
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
