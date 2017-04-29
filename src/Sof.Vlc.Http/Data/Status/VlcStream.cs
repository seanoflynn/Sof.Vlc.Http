using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// A VLC stream, usually meta information about the current media or an audio, video or subtitle track.
	/// </summary>
	public class VlcStream
	{
		/// <summary>
		/// The stream name.
		/// </summary>
		[XmlAttribute("name")]
		public string Name { get; set; }

		/// <summary>
		/// A list of key-values with information about the stream.
		/// </summary>
		[XmlElement("info")]
		public VlcStreamInformation[] StreamInfomation { get; set; }
	}
}
