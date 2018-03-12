using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RedMatter.SkinDetection;

namespace RedMatter.SkinDetection
{
    /// <summary>
    /// Simple skin detector.
    /// </summary>
    public class SimpleSkinDetector: ISkinDetector
    {

        #region ISkinDetector Members

        public bool IsSkin(System.Drawing.Color color)
        {
            if (((color.R > 95) & (color.G > 40) & (color.B < 20) &
                (DrawUtils.MaxChannel(color)-DrawUtils.MinChannel(color)>15) &
                (Math.Abs(color.R-color.G)>15)&(color.R>color.G)&(color.R>color.B))||
                ((color.R > 220) & (color.G > 210) & (color.B > 170) & (Math.Abs(color.R - color.G) <= 15)&
                (color.R>color.B)&(color.G>color.B)))
            {
                return true;
            }
            return false;
        }

        public bool IsSelectedPoint(System.Drawing.Color color)
        {
            return IsSkin(color);
        }

        #endregion
    }
}
