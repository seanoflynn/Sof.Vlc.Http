using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// Vlc stream information.
	/// </summary>
	public class VlcStreamInformation
	{
		/// <summary>
		/// The key.
		/// </summary>
		[XmlAttribute("name")]
		public string Name { get; set; }

		/// <summary>
		/// The value.
		/// </summary>
		[XmlText()]
		public string Value { get; set; }
	}
}
