using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// Current values for the VLC player's video effects.
	/// </summary>
	public class VlcVideoEffects
	{
		/// <summary>
		/// The video hue, between 0 and 360.
		/// </summary>
		[XmlElement("hue")]
		public int Hue { get; set; }

		/// <summary>
		/// The video contrast, between 0 and 1.
		/// </summary>
		[XmlElement("contrast")]
		public double Contrast { get; set; }

		/// <summary>
		/// The video brightness, between 0 and 1.
		/// </summary>
		[XmlElement("brightness")]
		public double Brightness { get; set; }

		/// <summary>
		/// The video saturation, between 0 and 1.
		/// </summary>
		[XmlElement("saturation")]
		public double Saturation { get; set; }

		/// <summary>
		/// The video gamma, between 0 and 1.
		/// </summary>
		[XmlElement("gamma")]
		public double Gamma { get; set; }
	}
}
