using System;
using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// A wrapper class to map to the root element of a VLC playlist response.
	/// </summary>
	[XmlRoot("node")]
	public class VlcPlaylistResponse
	{
		[XmlElement("node")]
		public VlcPlaylistNode[] Items { get; set; }
	}
}
