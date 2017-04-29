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
