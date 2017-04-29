namespace Sof.Vlc.Http
{
	/// <summary>
	/// Specifies whether to include audio or video or both when adding an item to the VLC playlist.
	/// </summary>
	public enum VlcPlaylistAddMode
	{
		Default = 0,
		NoAudio = 1,
		NoVideo = 2,
	}
}
