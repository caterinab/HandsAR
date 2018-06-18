using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace RedMatter.EXIF
{
    /// <summary>
    /// Class for writing EXIF tags.
    /// This class hasn't been verified!!!!!!
    /// </summary>
    public class PropertyWriter
    {
        /// <summary>
        /// Source image.
        /// </summary>
        private Bitmap sourceBitmap;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="image">Source image.</param>
        public PropertyWriter(Bitmap image)
        {
            sourceBitmap = image;
        }

        /// <summary>
        /// Set tag value.
        /// </summary>
        /// <param name="id">Tag identificator.</param>
        /// <param name="value">String value.</param>
        public void SetTagById(int id, string value)
        {
            PropertyItem pi = sourceBitmap.GetPropertyItem(id);
            Type type = (Type)pi.Type;
            switch (type)
            {
                case Type.ASCII: pi.Value = EXIFTypes.ToASCII(value); break;
                case Type.BYTE: pi.Value = EXIFTypes.ToByte(value); break;
                case Type.SHORT: pi.Value = EXIFTypes.ToShort(value); break;
                case Type.LONG: pi.Value = EXIFTypes.ToLong(value); break;
                case Type.RATIONAL: pi.Value = EXIFTypes.ToRational(value); break;
                case Type.UNDEFINED: pi.Value = EXIFTypes.ToRational(value); break;
                case Type.SLONG: pi.Value = EXIFTypes.ToSLong(value); break;
                case Type.SRATIONAL: pi.Value = EXIFTypes.ToSRational(value); break;
            }
        }

        /// <summary>
        /// Set tag value.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="value">String value.</param>
        public void SetTag(Tag tag, string value)
        {
            int tadId = (int)tag;
            SetTagById(tadId, value);
        }
    }
}
