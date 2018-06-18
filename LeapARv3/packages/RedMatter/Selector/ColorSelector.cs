using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RedMatter
{
    /// <summary>
    /// Select color.
    /// </summary>
    public class ColorSelector:IImageSelector
    {
        /// <summary>
        /// Color for selection.
        /// </summary>
        private Color selectedColor;

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }

        public ColorSelector(Color selectColor)
        {
            selectedColor = selectColor;
        }

        #region IImageSelector Members

        public bool IsSelectedPoint(System.Drawing.Color color)
        {
            if (DrawUtils.IsEqualColors(color, selectedColor))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
