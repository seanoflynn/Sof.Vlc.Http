using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// The playback video's width to height (aspect) ratio.
	/// </summary>
	public enum VlcAspectRatio
	{
		[XmlEnum("default")]
		Default,
		[XmlEnum("1:1")]
		Ratio_1_1,
		[XmlEnum("4:3")]
		Ratio_4_3,
		[XmlEnum("5:4")]
		Ratio_5_4,
		[XmlEnum("16:9")]
		Ratio_16_9,
		[XmlEnum("16:10")]
		Ratio_16_10,
		[XmlEnum("221:100")]
		Ratio_221_100,
		[XmlEnum("235:100")]
		Ratio_235_100,
		[XmlEnum("239:100")]
		Ratio_239_100,
	}
}
