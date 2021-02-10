using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// A media item.
	/// </summary>
	public class VlcPlaylistLeaf
	{
		[XmlAttribute("id")]
		public string Id { get; set; }

		[XmlAttribute("name")]
		public string Name { get; set; }

		/// <summary>
		/// The media's URI path, used for adding items to the VLC playlist.
		/// </summary>
		[XmlAttribute("uri")]
		public string Uri { get; set; }

		/// <summary>
		/// The media's duration in seconds.
		/// </summary>
		[XmlAttribute("duration")]
		public int Duration { get; set; }

	    /// <summary>
	    /// Equals to current when this is the current playlist item
	    /// </summary>
	    [XmlAttribute("current")]
        public string Current { get; set; }
	}
}
