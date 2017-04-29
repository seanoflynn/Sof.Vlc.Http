using System.Xml.Serialization;

namespace Sof.Vlc.Http.Data
{
	/// <summary>
	/// Statistics about the media playback such as read bytes or lost audio/video data.
	/// </summary>
	public class VlcStatistics
	{
		[XmlElement("playedabuffers")]
		public int PlayedAudioBuffers { get; set; }
		[XmlElement("lostabuffers")]
		public int LostAudioBuffers { get; set; }

		[XmlElement("displayedpictures")]
		public int DisplayedPictures { get; set; }
		[XmlElement("lostpictures")]
		public int LostPictures { get; set; }

		[XmlElement("readpackets")]
		public int ReadPackets { get; set; }
		[XmlElement("demuxreadpackets")]
		public int DemuxReadPackets { get; set; }
		[XmlElement("demuxreadbytes")]
		public int DemuxReadBytes { get; set; }

		[XmlElement("demuxbitrate")]
		public double DemuxBitrate { get; set; }
		[XmlElement("averagedemuxbitrate")]
		public double AverageDemuxBitrate { get; set; }

		[XmlElement("demuxcorrupted")]
		public int DemuxCorrupted { get; set; }
		[XmlElement("demuxdiscontinuity")]
		public int DemuxDiscontinuity { get; set; }

		[XmlElement("sendbitrate")]
		public double SendBitrate { get; set; }
		[XmlElement("sentbytes")]
		public int SentBytes { get; set; }
		[XmlElement("readbytes")]
		public int ReadBytes { get; set; }

		[XmlElement("sentpackets")]
		public int SentPackets { get; set; }

		[XmlElement("averageinputbitrate")]
		public double AverageInputBitrate { get; set; }
		[XmlElement("inputbitrate")]
		public double InputBitrate { get; set; }

		[XmlElement("decodedvideo")]
		public int DecodedVideo { get; set; }
		[XmlElement("decodedaudio")]
		public int DecodedAudio { get; set; }
	}
}
