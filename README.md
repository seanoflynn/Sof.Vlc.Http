# Sof.Vlc.Http

Control VLC media player via the HTTP interface.

## VlcClient

You can use the `VlcClient` class to control a specific VLC media player instance.

```csharp
using System;
using System.Threading.Tasks;
using Sof.Vlc.Http;

namespace tmp
{
    public class Program
    {
        public static void Main()
        {
            TestVlcClient().Wait();
        }

        public static async Task TestVlcClient()
        {
            var vlc = new VlcClient("127.0.0.1", 8080, "password");
            
            vlc.StatusUpdated += (sender, e) =>
            {
                Console.WriteLine($"Position: {vlc.Status.Time}, Volume: {vlc.Status.Volume}");
            };

            await vlc.Play();
            await Task.Delay(5000);

            await vlc.Pause();
            await Task.Delay(5000);

            await vlc.SetVolume(200);
            await Task.Delay(5000);
        }
    }
}
```

Supported actions:

- `UpdateStatus()` - Request a status update from VLC.
- `GetDirectoryContents(string location = "file://~")` - Request the contents of a specified directory on the host machine.
- `GetPlaylist(string search = null)` - Request the media player's current playlist.
- `Play(string uri, string name, VlcPlaylistAddMode options)` - Add an item to the playlist and start playback immediately.
- `Add(string uri, string name, VlcPlaylistAddMode options)` - Add an item to the playlist without starting playback immediately.
- `Play()` - Play the last active item or the first item if there is no previous item.
- `Play(int id)` - Play the specified playlist item by it's Id.
- `Pause()` - Toggle play/pause. If currently stopped, play current item or first item in the playlist.
- `Pause(int id)` - Toggle play/pause. If currently stopped, play specified item.
- `ForcePause()` - Pause the current media item.
- `ForceResume() ` - Play the current media item.
- `Stop()` - Stop the current media item.
- `Next()` - Skip to the next media item in the playlist.
- `Previous()` - Skip to the previous media item in the playlist.
- `Delete(int id)` - Delete the specified item from the playlist.
- `Clear()` - Clear the entire playlist.
- `ToggleRandom()` - Toggle whether the playlist will be randomized.
- `ToggleLoop()` - Toggle whether the playlist will loop.
- `ToggleRepeat()` - Toggle whether the current playback item will be repeated.
- `ToggleFullscreen()` - Toggle fullscreen mode.
- `TakeSnapshot()` - Take a snapshot of the currently playing video.
- `SetVolume(int val)` - Set the player's absolute volume (between 0-300).
- `SetRelativeVolume(int relVal)` - Change the player's relative volume (e.g. +5).
- `SetPercentageVolume(int val)` - Set the player's absolute volume as a percentage (50%).
- `SetRelativePercentageVolume(int relVal)` - Change the player's volume as a percentage (e.g. +10%).
- `SortPlaylist(VlcPlaylistSortKey key, VlcPlaylistSortOrder order)` - Sorts the playlist by the specified key and in the specified order.
- `SortPlaylistDescending(VlcPlaylistSortKey key)` - Sorts the playlist by the specified key in a descending order.
- `SetPosition(int secs)` - Set the playback position in seconds.
- `SetRelativePosition(int relSecs)` - Changes the playback position relative to it's current position in seconds.
- `SetPercentagePosition(int secs)` - Set the playback position as a percentage of the total length (e.g. 25%).
- `SetRelativePercentagePosition(int relSecs)` - Changes the playback position as a percentage of the total length relative to it's current position (e.g. +10%).
- `SetRate(double val)` - Set the playback rate e.g. 0.5 = half speed, 2.0 = double speed.
- `SetAudioDelay(double secs)` - Sets the audio delay in seconds.
- `SetSubDelay(double secs)` - Sets the subtitle delay in seconds.
- `SetAspectRatio(VlcAspectRatio ratio)` - Set the aspect ratio.
- `SetPreAmp(int db)` - Set the pre amp volume.
- `EnableEqualizer()` - Enable the audio equalizer.
- `DisableEqualizer()` - Disable the audio equalizer.
- `SetEqualizerPreset(string val)` - Set the equalizer preset.
- `SetEqualizerBandValue(int band, double val)` - Set the equalizer value for the specified band.
- `SetAudioTrack(int streamId)` - Set the audio track
- `SetVideoTrack(int streamId)` - Set the video track
- `SetSubtitleTrack(int streamId)` - Set the subtitle track
- `AddSubtitleTrack(string path)` - Add a subtitle track to the current media item.
- `PlayTitle(int num)` - Jump to the specified title for the current playback item.
- `PlayChapter(int num)` - Jump to the specified chapter for the current playback item.


## VlcScanner

The `VlcScanner` class can be used to scan the local network to search for machines with a running VLC media player that 
has its HTTP interface open.

```csharp
using System;
using System.Threading.Tasks;
using Sof.Vlc.Http;

namespace tmp
{
    public class Program
    {
        public static void Main()
        {
            TestVlcScanner().Wait();
        }

        public static async Task TestVlcScanner()
        {
            var scanner = new VlcScanner();
            scanner.EndPointsUpdated += (sender, endPoints) =>
            {
                Console.WriteLine("VLC media players: \n" + String.Join("\n", endPoints));
            };
            await scanner.UpdateEndPoints();
        }
    }
}
```
