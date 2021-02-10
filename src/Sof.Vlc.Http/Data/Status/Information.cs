using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
    [XmlRoot(ElementName = "information")]
    public class Information
    {
        [XmlElement(ElementName = "category")]
        public List<Category> Category { get; set; }
    }
}