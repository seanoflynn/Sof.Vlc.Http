using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
    [XmlRoot(ElementName = "category")]
    public class Category
    {
        [XmlElement(ElementName = "info")]
        public List<Info> Info { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }
}