using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter
{
    public class NonBlackSelector:IImageSelector
    {
        #region IImageSelector Members

        public bool IsSelectedPoint(System.Drawing.Color color)
        {
            if (DrawUtils.IsEqualColors(color, Color.Black))
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
