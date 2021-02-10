using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
    [XmlRoot(ElementName = "info")]
    public class Info
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlText]
        public string Text { get; set; }
    }
}