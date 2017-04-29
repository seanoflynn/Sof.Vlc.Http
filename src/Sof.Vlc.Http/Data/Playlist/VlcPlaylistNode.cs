using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// A node in the playlist tree.
	/// </summary>
	public class VlcPlaylistNode
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		/// <summary>
		/// A list of media items under this node.
		/// </summary>
		[XmlElement("leaf")]
		public VlcPlaylistLeaf[] Leaves { get; set; }

		/// <summary>
		/// A list of further playlist nodes under this node.
		/// </summary>
		[XmlElement("node")]
		public VlcPlaylistNode[] Nodes { get; set; }
	}
}
