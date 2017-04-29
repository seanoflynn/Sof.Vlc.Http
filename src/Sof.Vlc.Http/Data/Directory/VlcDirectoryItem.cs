using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// An item in the specified directory.
	/// </summary>
	public class VlcDirectoryItem
	{
		[XmlAttribute("uid")]
		public int Id { get; set; }

		// TODO: what is this?
		[XmlAttribute("gid")]
		public int GId { get; set; }

		[XmlAttribute("type")]
		public VlcDirectoryItemType Type { get; set; }

		/// <summary>
		/// The local file path.
		/// </summary>
		[XmlAttribute("path")]
		public string Path { get; set; }

		/// <summary>
		/// The URI path, used for adding items to the VLC playlist.
		/// </summary>
		[XmlAttribute("uri")]
		public string Uri { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		/// <summary>
		/// The file size, in bytes.
		/// </summary>
		[XmlAttribute("size")]
		public int Size { get; set; }

		[XmlAttribute("access_time")]
		public int DateLastOpened { get; set; }

		[XmlAttribute("creation_time")]
		public string DateCreated { get; set; }

		[XmlAttribute("modification_time")]
		public string DateLastModified { get; set; }

		// TODO: what is this?
		[XmlAttribute("mode")]
		public int Mode { get; set; }
	}
}
