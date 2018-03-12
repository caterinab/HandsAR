using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter
{
    /// <summary>
    /// Interface for select pixels on image.
    /// </summary>
    public interface IImageSelector
    {
        bool IsSelectedPoint(Color color);
    }
}
