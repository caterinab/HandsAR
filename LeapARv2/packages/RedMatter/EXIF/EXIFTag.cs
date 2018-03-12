using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedMatter.EXIF
{
    /// <summary>
    /// Class for exif tag data.
    /// </summary>
    public class EXIFTag
    {
        private string tagName;

        public string TagName
        {
            get { return tagName; }
            set { tagName = value; }
        }
        private string tagValue;

        public string TagValue
        {
            get { return tagValue; }
            set { tagValue = value; }
        }

        public EXIFTag(string tag, string value)
        {
            this.tagName = tag;
            this.tagValue = value;
        }
    }
}
