using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter.SkinDetection
{
    /// <summary>
    /// Interface for methods of skin detection.
    /// </summary>
    public interface ISkinDetector: IImageSelector
    {
        bool IsSkin(Color color);
    }
}
