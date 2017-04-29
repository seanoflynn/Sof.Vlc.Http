using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// A wrapper class to map to the root element of a VLC directory response.
	/// </summary>
	[XmlRoot("root")]
	public class VlcDirectoryResponse
	{		
		[XmlElement("element")]
		public VlcDirectoryItem[] Items { get; set; }
	}
}
