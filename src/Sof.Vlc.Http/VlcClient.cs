using System;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.ComponentModel;
using Sof.Vlc.Http.Data;

namespace Sof.Vlc.Http
{
    /// <summary>
    /// A client for controlling a VLC media player instance via it's HTTP interface.
    /// </summary>
    public sealed class VlcClient : VlcPollable, INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when the VLC instance cannot be reached.
        /// </summary>
        public event EventHandler<bool> ConnectionChanged;

        /// <summary>
        /// Occurs when status is updated, which can be regularly if polling is enabled.
        /// </summary>
        public event EventHandler<VlcStatus> StatusUpdated;

        /// <summary>
        /// Occurs when a browse directory response is returned.
        /// </summary>
        public event EventHandler<IEnumerable<VlcDirectoryItem>> DirectoryUpdated;

        /// <summary>
        /// Occurs when a playlist response is returned.
        /// </summary>
        public event EventHandler<IEnumerable<VlcPlaylistNode>> PlaylistUpdated;

        /// <summary>
        /// The server host address.
        /// </summary>
        public string Host { get; } = "127.0.0.1";

        /// <summary>
        /// The server port, VLC defaults to 8080.
        /// </summary>
        public int Port { get; } = 8080;

        /// <summary>
        /// Whether this instance is connected or not.
        /// </summary>
        /// <value><c>true</c> if is connected; otherwise, <c>false</c>.</value>
        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                _isConnected = value;
                ConnectionChanged?.Invoke(this, value);
            }
        }

        /// <summary>
        /// The current VLC media player and playback item's state.
        /// </summary>
        public VlcStatus Status
        {
            get => _status;
            private set
            {
                _status = value;
                StatusUpdated?.Invoke(this, value);
            }
        }

        /// <summary>
        /// Http client for interacting with the VLC media player.
        /// </summary>
        private HttpClient client;

        /// <summary>
        /// Xml serializer for status response.
        /// </summary>
        private XmlSerializer statusSerializer = new XmlSerializer(typeof(VlcStatus));

        /// <summary>
        /// Xml serializer for playlist response.
        /// </summary>
        private XmlSerializer playlistSerializer = new XmlSerializer(typeof(VlcPlaylistResponse));

        /// <summary>
        /// Xml serializer for directory response.
        /// </summary>
        private XmlSerializer directorySerializer = new XmlSerializer(typeof(VlcDirectoryResponse));

        private bool _isConnected = false;
        private VlcStatus _status = new VlcStatus();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sof.Vlc.Http.VlcClient"/> class.
        /// </summary>
        /// <param name="ipEndPoint">IP end point.</param>
        /// <param name="password">Password.</param>
        /// <param name="startPolling">If set to <c>true</c> start polling immediately.</param>
        /// <param name="pollingInterval">Polling interval, defaults to 1 second.</param>
        public VlcClient(IPEndPoint ipEndPoint, string password, bool startPolling = true,
            TimeSpan pollingInterval = default(TimeSpan))
            : this(ipEndPoint.Address.ToString(), ipEndPoint.Port, password, startPolling, pollingInterval)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Sof.Vlc.Http.VlcClient"/> class.
        /// </summary>
        /// <param name="host">Host.</param>
        /// <param name="port">Port number.</param>
        /// <param name="password">Password.</param>
        /// <param name="startPolling">If set to <c>true</c> start polling immediately.</param>
        /// <param name="pollingInterval">Polling interval.</param>
        public VlcClient(string host, int port, string password, bool startPolling = true,
            TimeSpan pollingInterval = default(TimeSpan))
        {
            Host = host;
            Port = port;

            PollingInterval = pollingInterval == default(TimeSpan) ? TimeSpan.FromSeconds(1) : pollingInterval;

            var encodedPassword = Convert.ToBase64String(Encoding.ASCII.GetBytes(":" + password));

            client = new HttpClient
            {
                BaseAddress = new Uri("http://" + host + ":" + port + "/"),
                Timeout = new TimeSpan(0, 0, 0, 0,500)
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedPassword);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));

            if (startPolling)
            {
                StartPolling();
            }
        }

        /// <summary>
        /// Called periodically when the polling interval elapses.
        /// </summary>
        protected override async Task OnPoll()
        {
            await UpdateStatus();
        }

        /// <summary>
        /// Play the last active item or the first item if there is no previous item.
        /// </summary>
        public async Task Play()
        {
            await SendCommand("command=pl_play");
        }

        /// <summary>
        /// Play the specified playlist item by it's Id.
        /// </summary>
        /// <param name="id">Playlist item Id.</param>
        public async Task Play(int id)
        {
            await SendCommand("command=pl_play&id=" + id);
        }

        /// <summary>
        /// Toggle play/pause. If currently stopped, play current item or first item in the playlist.
        /// </summary>
        public async Task Pause()
        {
            await SendCommand("command=pl_pause");
        }

        /// <summary>
        /// Toggle play/pause. If currently stopped, play specified item.
        /// </summary>
        public async Task Pause(int id)
        {
            await SendCommand("command=pl_pause&id=" + id);
        }

        /// <summary>
        /// Pause the current media item.
        /// </summary>
        public async Task ForcePause()
        {
            await SendCommand("command=pl_forcepause");
        }

        /// <summary>
        /// Play the current media item.
        /// </summary>
        public async Task ForceResume()
        {
            await SendCommand("command=pl_forceresume");
        }

        /// <summary>
        /// Stop the current media item.
        /// </summary>
        public async Task Stop()
        {
            await SendCommand("command=pl_stop");
        }

        /// <summary>
        /// Skip to the next media item in the playlist.
        /// </summary>
        public async Task Next()
        {
            await SendCommand("command=pl_next");
        }

        /// <summary>
        /// Skip to the previous media item in the playlist.
        /// </summary>
        public async Task Previous()
        {
            await SendCommand("command=pl_previous");
        }

        /// <summary>
        /// Delete the specified item from the playlist.
        /// </summary>
        /// <param name="id">Item's Id.</param>
        public async Task Delete(int id)
        {
            await SendCommand("command=pl_delete&id=" + id);
        }

        /// <summary>
        /// Clear the entire playlist.
        /// </summary>
        public async Task Clear()
        {
            await SendCommand("command=pl_empty");
        }

        /// <summary>
        /// Toggle whether the playlist will be randomized.
        /// </summary>
        public async Task ToggleRandom()
        {
            await SendCommand("command=pl_random");
        }

        /// <summary>
        /// Toggle whether the playlist will loop.
        /// </summary>
        public async Task ToggleLoop()
        {
            await SendCommand("command=pl_loop");
        }

        /// <summary>
        /// Toggle whether the current playback item will be repeated.
        /// </summary>
        public async Task ToggleRepeat()
        {
            await SendCommand("command=pl_repeat");
        }

        /// <summary>
        /// Toggle fullscreen mode.
        /// </summary>
        public async Task ToggleFullscreen()
        {
            await SendCommand("command=fullscreen");
        }

        /// <summary>
        /// Take a snapshot of the currently playing video.
        /// </summary>
        public async Task TakeSnapshot()
        {
            await SendCommand("command=snapshot");
        }

        /// <summary>
        /// Set the player's absolute volume (between 0-300)
        /// </summary>
        public async Task SetVolume(int val)
        {
            await SendCommand("command=volume&val=" + val);
        }

        /// <summary>
        /// Change the player's relative volume (the absolute volume is limited to between 0-300).
        /// </summary>
        public async Task SetRelativeVolume(int relVal)
        {
            await SendCommand("command=volume&val=" + (relVal > 0 ? "%2B" : "") + relVal);
        }

        /// <summary>
        /// Set the player's absolute volume as a percentage (between 0-100%)
        /// </summary>
        public async Task SetPercentageVolume(int val)
        {
            await SendCommand("command=volume&val=" + val + "%");
        }

        /// <summary>
        /// Change the player's volume as a percentage, as a percentage of it's current volume (the absolute volume is limited to between 0-100%).
        /// </summary>
        public async Task SetRelativePercentageVolume(int relVal)
        {
            await SendCommand("command=volume&val=" + (relVal > 0 ? "%2B" : "") + relVal + "%");
        }

        /// <summary>
        /// Sorts the playlist by the specified key and in the specified order (ascending by default).
        /// </summary>
        /// <param name="key">Key to sort by.</param>
        /// <param name="order">Sort order, ascending by default.</param>
        public async Task SortPlaylist(VlcPlaylistSortKey key,
            VlcPlaylistSortOrder order = VlcPlaylistSortOrder.Ascending)
        {
            await SendCommand("command=pl_sort&val=" + GetSortKeyString(key) + "&id=" + (int) order);
        }

        /// <summary>
        /// Sorts the playlist by the specified key in a descending order.
        /// </summary>
        /// <param name="key">Key to sort by.</param>
        public async Task SortPlaylistDescending(VlcPlaylistSortKey key)
        {
            await SendCommand("command=pl_sort&val=" + GetSortKeyString(key) + "&id=1");
        }

        /// <summary>
        /// Set the playback position in seconds.
        /// </summary>
        public async Task SetPosition(int secs)
        {
            await SendCommand("command=seek&val=" + secs);
        }

        /// <summary>
        /// Changes the playback position relative to it's current position in seconds.
        /// </summary>
        public async Task SetRelativePosition(int relSecs)
        {
            await SendCommand("command=seek&val=" + (relSecs > 0 ? "%2B" : "") + relSecs);
        }

        /// <summary>
        /// Set the playback position as a percentage of the total length.
        /// </summary>
        public async Task SetPercentagePosition(int secs)
        {
            await SendCommand("command=seek&val=" + secs + "%");
        }

        /// <summary>
        /// Changes the playback position as a percentage of the total length relative to it's current position.
        /// </summary>
        public async Task SetRelativePercentagePosition(int relSecs)
        {
            await SendCommand("command=seek&val=" + (relSecs > 0 ? "%2B" : "") + relSecs + "%");
        }

        /// <summary>
        /// Set the playback rate e.g. 0.5 = half speed, 2.0 = double speed.
        /// </summary>
        public async Task SetRate(double val)
        {
            await SendCommand("command=rate&val=" + val);
        }

        /// <summary>
        /// Sets the audio delay in seconds (can be negative).
        /// </summary>
        public async Task SetAudioDelay(double secs)
        {
            await SendCommand("command=audiodelay&val=" + secs);
        }

        /// <summary>
        /// Sets the subtitle delay in seconds (can be negative).
        /// </summary>
        public async Task SetSubDelay(double secs)
        {
            await SendCommand("command=subdelay&val=" + secs);
        }

        /// <summary>
        /// Set the aspect ratio for the current playback media.
        /// </summary>
        public async Task SetAspectRatio(VlcAspectRatio ratio)
        {
            await SendCommand("command=aspectratio&val=" + GetAspectRatioString(ratio));
        }

        /// <summary>
        /// Set the pre amp volume, in decibels (between -20 and +20).
        /// </summary>
        public async Task SetPreAmp(int db)
        {
            await SendCommand("command=preamp&val=" + Math.Max(-20, Math.Min(20, db)));
        }

        /// <summary>
        /// Enable the audio equalizer.
        /// </summary>
        public async Task EnableEqualizer()
        {
            await SendCommand("command=enableeq");
        }

        /// <summary>
        /// Disable the audio equalizer.
        /// </summary>
        public async Task DisableEqualizer()
        {
            await SendCommand("command=enableeq&val=0");
        }

        /// <summary>
        /// Set the equalizer preset, a list of which is given in the status response when the equalizer is enabled.
        /// </summary>
        public async Task SetEqualizerPreset(string val)
        {
            await SendCommand("command=setpreset&val=" + val);
        }

        /// <summary>
        /// Set the equalizer value for the specified band.
        /// </summary>
        public async Task SetEqualizerBandValue(int band, double val)
        {
            await SendCommand("command=equalizer&band=" + band + "&val=" + val);
        }

        /// <summary>
        /// Set the audio track using the stream id which can be found in the list of streams in the status response.
        /// </summary>
        public async Task SetAudioTrack(int streamId)
        {
            await SendCommand("command=audio_track&val=" + streamId);
        }

        /// <summary>
        /// Set the video track using the stream id which can be found in the list of streams in the status response.
        /// </summary>
        public async Task SetVideoTrack(int streamId)
        {
            await SendCommand("command=video_track&val=" + streamId);
        }

        /// <summary>
        /// Set the subtitle track using the stream id which can be found in the list of streams in the status response.
        /// </summary>
        public async Task SetSubtitleTrack(int streamId)
        {
            await SendCommand("command=subtitle_track&val=" + streamId);
        }

        /// <summary>
        /// Add a subtitle track to the current media item.
        /// </summary>
        public async Task AddSubtitleTrack(string path)
        {
            await SendCommand("command=addsubtitle&val=" + Uri.EscapeDataString(path));
        }

        /// <summary>
        /// Add an item to the playlist and start playback immediately.
        /// </summary>
        /// <param name="uri">The URI of the item to add.</param>
        /// <param name="name">An optional name for the new item.</param>
        /// <param name="options">Whether to use the new item's audio, video or both.</param>
        public async Task Play(string uri, string name = null, VlcPlaylistAddMode options = VlcPlaylistAddMode.Default)
        {
            await Add(uri, name, options, true);
        }

        /// <summary>
        /// Add an item to the playlist without starting playback immediately.
        /// </summary>
        /// <param name="uri">The URI of the item to add.</param>
        /// <param name="name">An optional name for the new item.</param>
        /// <param name="options">Whether to use the new item's audio, video or both.</param>
        public async Task Add(string uri, string name = null, VlcPlaylistAddMode options = VlcPlaylistAddMode.Default)
        {
            await Add(uri, name, options, false);
        }

        /// <summary>
        /// Add an item to the playlist.
        /// </summary>
        /// <param name="uri">The URI of the item to add.</param>
        /// <param name="name">An optional name for the new item.</param>
        /// <param name="options">Whether to use the new item's audio, video or both.</param>
        /// <param name="playImmediately">If set to <c>true</c> start playback immediately.</param>
        private async Task Add(string uri, string name, VlcPlaylistAddMode options, bool playImmediately)
        {
            var command = new StringBuilder();
            command.Append("command=");

            command.Append(playImmediately ? "in_play" : "in_enqueue");

            command.Append("&input=");
            command.Append(Uri.EscapeUriString(uri.Replace("/", "\\")));

            if (!string.IsNullOrEmpty(name))
            {
                command.Append("&name=" + Uri.EscapeDataString(name));
            }

            switch (options)
            {
                case VlcPlaylistAddMode.NoAudio:
                    command.Append("&option=noaudio");
                    break;
                case VlcPlaylistAddMode.NoVideo:
                    command.Append("&option=novideo");
                    break;
            }

            await SendCommand(command.ToString());
        }

        /// <summary>
        /// Jump to the specified title for the current playback item.
        /// </summary>
        public async Task PlayTitle(int num)
        {
            await SendCommand("command=title&val=" + num);
        }

        /// <summary>
        /// Jump to the specified chapter for the current playback item.
        /// </summary>
        public async Task PlayChapter(int num)
        {
            await SendCommand("command=chapter&val=" + num);
        }

        /// <summary>
        /// Request an updated status for the media player and current playback item.
        /// </summary>
        public async Task UpdateStatus()
        {
            await SendCommand();
        }

        /// <summary>
        /// Convert VlcPlaylistSortKey enum to a string.
        /// </summary>
        private string GetSortKeyString(VlcPlaylistSortKey key)
        {
            switch (key)
            {
                case VlcPlaylistSortKey.Id: return "id";
                case VlcPlaylistSortKey.Title: return "title";
                case VlcPlaylistSortKey.TitleNodesFirst: return Uri.EscapeDataString("title nodes first");
                case VlcPlaylistSortKey.Artist: return "artist";
                case VlcPlaylistSortKey.Genre: return "genre";
                case VlcPlaylistSortKey.Random: return "random";
                case VlcPlaylistSortKey.Duration: return "duration";
                case VlcPlaylistSortKey.TitleNumeric: return Uri.EscapeDataString("title numeric");
                case VlcPlaylistSortKey.Album: return "album";
            }

            throw new NotImplementedException("Sort key not implemented: " + key);
        }

        /// <summary>
        /// Convert VlcAspectRatio enum to a string.
        /// </summary>
        private string GetAspectRatioString(VlcAspectRatio ratio)
        {
            switch (ratio)
            {
                case VlcAspectRatio.Default: return "default";
                case VlcAspectRatio.Ratio_1_1: return "1:1";
                case VlcAspectRatio.Ratio_4_3: return "4:3";
                case VlcAspectRatio.Ratio_5_4: return "5:4";
                case VlcAspectRatio.Ratio_16_9: return "16:9";
                case VlcAspectRatio.Ratio_16_10: return "16:10";
                case VlcAspectRatio.Ratio_221_100: return "221:100";
                case VlcAspectRatio.Ratio_235_100: return "235:100";
                case VlcAspectRatio.Ratio_239_100: return "239:100";
            }

            throw new NotImplementedException("Aspect ratio not implemented: " + ratio);
        }

        /// <summary>
        /// Request the contents of a specified directory on the host machine.
        /// </summary>
        /// <returns>The directory contents.</returns>
        /// <param name="location">Location, default is the user's home folder file://~</param>
        public async Task<VlcDirectoryItem[]> GetDirectoryContents(string location = "file://~")
        {
            var uri = "requests/browse.xml?dir=" + Uri.EscapeDataString(location);

            var response = await client.GetAsync(uri);

            // TODO: throw exception?
            if (!response.IsSuccessStatusCode)
                return null;

            var stream = await response.Content.ReadAsStreamAsync();

            var dir = (VlcDirectoryResponse) directorySerializer.Deserialize(stream);

            DirectoryUpdated?.Invoke(this, dir.Items);

            return dir.Items;
        }

        /// <summary>
        /// Request the media player's current playlist.
        /// </summary>
        /// <returns>The playlist tree.</returns>
        /// <param name="search">An optional search string.</param>
        public async Task<VlcPlaylistNode[]> GetPlaylist(string search = null)
        {
            var uri = "requests/playlist.xml?search=";

            if (!string.IsNullOrWhiteSpace(search))
                uri += Uri.EscapeDataString(search);

            var response = await client.GetAsync(uri);

            // TODO: throw exception?
            if (!response.IsSuccessStatusCode)
                return null;

            var stream = await response.Content.ReadAsStreamAsync();

            var pl = (VlcPlaylistResponse) playlistSerializer.Deserialize(stream);

            PlaylistUpdated?.Invoke(this, pl.Items);

            return pl.Items;
        }

        /// <summary>
        /// Send a the command to the VLC media player.
        /// </summary>
        /// <param name="query">The query to send. If a null query is provided, just an updated status 
        /// will be requested.</param>
        private async Task SendCommand(string query = null)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await client.GetAsync("requests/status.xml" + (query == null ? "" : "?" + query));
            }
            catch
            {
                IsConnected = false;
                return;
            }

            IsConnected = true;

            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();

                try
                {
                    Status = (VlcStatus) statusSerializer.Deserialize(stream);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                IsConnected = false;
            }
            else
            {
                Console.WriteLine("error: " + response.StatusCode);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}