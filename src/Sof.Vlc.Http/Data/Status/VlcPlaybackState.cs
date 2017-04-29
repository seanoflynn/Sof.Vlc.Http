using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{	
	/// <summary>
	/// The current playback state.
	/// </summary>
	public enum VlcPlaybackState
	{
		[XmlEnum("stopped")]
		Stopped,
		[XmlEnum("paused")]
		Paused,
		[XmlEnum("playing")]
		Playing,
	}
}
