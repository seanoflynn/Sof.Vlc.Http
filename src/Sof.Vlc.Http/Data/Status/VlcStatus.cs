using System;
using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// Information about the VLC playlist and current playback media state.
	/// </summary>
	[XmlRoot("root")]
	public class VlcStatus
	{
		/// <summary>
		/// VLC player version.
		/// </summary>
		[XmlElement("version")]
		public string Version { get; set; }

		/// <summary>
		/// The VLC API version.
		/// </summary>
		[XmlElement("apiversion")]
		public int ApiVersion { get; set; }

		/// <summary>
		/// The current playlist item.
		/// </summary>
		[XmlElement("currentplid")]
		public int CurrentPlaylistId { get; set; }

		/// <summary>
		/// The current playback position in seconds.
		/// </summary>
		[XmlElement("time")]
		public int Time { get; set; }

		/// <summary>
		/// The length of the current media in seconds.
		/// </summary>
		[XmlElement("length")]
		public int Length { get; set; }

		/// <summary>
		/// The aspect ratio of the current media.
		/// </summary>
		/// <value>The aspect ratio.</value>
		[XmlElement("aspectratio")]
		public VlcAspectRatio AspectRatio { get; set; }

		/// <summary>
		/// <c>true</c> if the VLC media player is in fullscreen mode.
		/// </summary>
		[XmlElement("fullscreen")]
		public bool IsFullScreen { get; set; }

		/// <summary>
		/// The current playback volume (0-300).
		/// </summary>
		[XmlElement("volume")]
		public int Volume { get; set; }

		/// <summary>
		/// The current playback state.
		/// </summary>
		[XmlElement("state")]
		public VlcPlaybackState State { get; set; }

		/// <summary>
		/// The current playback rate.
		/// </summary>
		[XmlElement("rate")]
		public double Rate { get; set; }

		/// <summary>
		/// The current playback position, as a fraction of the total length.
		/// </summary>
		[XmlElement("position")]
		public double Position { get; set; }

		/// <summary>
		/// <c>true</c> if the playlist will loop.
		/// </summary>
		[XmlElement("loop")]
		public bool IsLooping { get; set; }

		/// <summary>
		/// <c>true</c> if the current playlist item will loop.
		/// </summary>
		[XmlElement("repeat")]
		public bool IsRepeating { get; set; }

		/// <summary>
		/// <c>true</c> if the playlist is randomized.
		/// </summary>
		[XmlElement("random")]
		public bool IsRandom { get; set; }

		/// <summary>
		/// The audio delay in seconds.
		/// </summary>
		[XmlElement("audiodelay")]
		public double AudioDelay { get; set; }

		/// <summary>
		/// The subtitle delay in seconds.
		/// </summary>
		[XmlElement("subtitledelay")]
		public double SubtitleDelay { get; set; }

		/// <summary>
		/// The video effects, such as hue and brightness.
		/// </summary>
		[XmlElement("videoeffects")]
		public VlcVideoEffects VideoEffects { get; set; } = new VlcVideoEffects();

		/// <summary>
		/// Technical statistics about playback.
		/// </summary>
		[XmlElement("stats")]
		public VlcStatistics Statistics { get; set; }

	    [XmlElement(ElementName = "information")]
	    public Information Information { get; set; }

        // TODO: audiofilters
        // TODO: equalizer
    }
}
