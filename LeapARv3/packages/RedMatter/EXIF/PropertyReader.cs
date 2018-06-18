using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;

namespace RedMatter.EXIF
{
    /// <summary>
    /// Simple class for reading image metadata information.
    /// </summary>
    public class PropertyReader
    {
        /// <summary>
        /// Source image.
        /// </summary>
        private Bitmap sourceBitmap;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="image">Source image.</param>
        public PropertyReader(Bitmap image)
        {
            sourceBitmap = image;
        }

        /// <summary>
        /// Property items of image.
        /// </summary>
        public PropertyItem[] PropertyItems
        {
            get
            {
                return sourceBitmap.PropertyItems;
            }
        }

        /// <summary>
        /// Get exif tag.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <returns></returns>
        public PropertyItem GetEXIFTag(Tag tag)
        {
            return sourceBitmap.GetPropertyItem((int)tag);
        }

        /// <summary>
        /// Get Tag value.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <returns>String value.</returns>
        public string GetTagValue(Tag tag)
        {
            PropertyItem pi = GetEXIFTag(tag);
            Type type = (Type)pi.Type;
            switch (type)
            {
                case Type.ASCII: return EXIFTypes.GetASCII(pi.Value);
                case Type.BYTE: return EXIFTypes.GetByte(pi.Value).ToString();
                case Type.SHORT: return EXIFTypes.GetShort(pi.Value).ToString();
                case Type.LONG: return EXIFTypes.GetLong(pi.Value).ToString() ;
                case Type.RATIONAL: return EXIFTypes.GetRational(pi.Value).ToString();
                case Type.UNDEFINED: return EXIFTypes.GetUndefined(pi.Value).ToString();
                case Type.SLONG: return EXIFTypes.GetSLong(pi.Value).ToString();
                case Type.SRATIONAL: return EXIFTypes.GetSRational(pi.Value).ToString();
                default: return "Error!";
            }
            
        }

        /// <summary>
        /// Get list of tags.
        /// </summary>
        /// <returns></returns>
        public EXIFTag[] GetAllTags()
        {
            List<EXIFTag> tags = new List<EXIFTag>();
            foreach (PropertyItem pi in sourceBitmap.PropertyItems)
            {
                Tag tag = (Tag)pi.Id;
                string value = GetTagValue(tag);
                tags.Add(new EXIFTag(tag.ToString(), value));
            }
            return tags.ToArray();
        }

 //       public void ExportToXML(string fileName)
 //       {
 //           XmlDocument xmlDoc = new XmlDocument();
 //           string xmlString = "<EXIFInfo>";
 //           foreach (PropertyItem pi in sourceBitmap.PropertyItems)
 //           {
 //               Tag tag = (Tag)pi.Id;
 //               string value = GetTagValue(tag);

 //               XmlNode node = xmlDoc.CreateNode(XmlNodeType.Element, "Tag", "");
 //               XmlNode idNode = xmlDoc.CreateNode(XmlNodeType.Element, "ID", "");
 //               idNode.Value = pi.Id.ToString();
 //               node.AppendChild(idNode);
 //               xmlDoc.AppendChild(node);

 //               //if (value == null)
 //               //{
 //               //    value = "No information";
 //               //}
 //               //xmlString += ("<Tag><ID>" + pi.Id.ToString() + "</ID><Name>" + tag.ToString() + "</Name><Value>" + value + "</Value></Tag>");
 //           }
 //           xmlString += "</EXIFInfo>";
 /////           xmlDoc.LoadXml(xmlString);
 //           xmlDoc.Save(fileName);
 //       }
    }
}
