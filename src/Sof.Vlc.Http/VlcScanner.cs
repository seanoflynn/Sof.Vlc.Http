using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Sof.Vlc.Http
{
    /// <summary>
    ///     Used to scan the network for VLC media player instances with open HTTP interfaces.
    /// </summary>
    public class VlcScanner
    {
        private const string Mask = "192.168.1.{0}";

        private CancellationTokenSource CancellationToken { get; set; }

        private bool IsLoopRunning { get; set; }

        private Regex MaskRegex { get; } = new Regex(@"{\d+}");

        private List<string> PossibleIps { get; } = new List<string>();

        public VlcScanner()
        {
            GenerateIps();
            Start();
        }

        /// <summary>
        /// Occurs when a valid vlc host is found, passes the IP.
        /// </summary>
        public event EventHandler<string> VlcHostFound;

        /// <summary>
        ///     Test an IP address to see whether a VLC media player instance has an open HTTP interface
        ///     running on the specified port.
        /// </summary>
        /// <returns><c>true</c> if an running HTTP interface is found.</returns>
        /// <param name="ip">IP address.</param>
        /// <param name="port">Port number.</param>
        private async Task<bool> CheckHostForVlc(string ip, int port)
        {
            var requestUri = $"http://{ip}:{port}/requests/status.xml";

            try
            {
                using (var client = new HttpClient {Timeout = new TimeSpan(0, 0, 0, 0, 100)})
                {
                    var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);

                    if (response?.Headers?.WwwAuthenticate?.FirstOrDefault()?.Parameter == "realm=\"VLC stream\"")
                        return true;

                    return false;
                }
            }
            catch (TaskCanceledException)
            {
                //Console.WriteLine("Timeout: " + requestUri);
                return false;
            }
            catch (HttpRequestException)
            {
                //Console.WriteLine("Couldn't reach server: " + requestUri);
                return false;
            }
            catch (OperationCanceledException)
            {
                //Console.WriteLine("Operation cancelled: " + requestUri);
                return false;
            }
            catch (AggregateException e)
            {
                //Console.WriteLine("Inner exception, usually timeout: " + requestUri);
                if (e.InnerException.GetType() != typeof(TaskCanceledException))
                    throw;
            }

            //Console.WriteLine("Failed for unknown reason: " + requestUri);
            return false;
        }

        /// <summary>
        ///     Fills the possible Ips list, based on the mask
        /// </summary>
        public void GenerateIps()
        {
            var matches = MaskRegex.Matches(Mask);
            PossibleIps.Clear();

            for (var j = 0; j < 255; j++)
                if (matches.Count > 1)
                    for (var i = 0; i < 255; i++)
                        if (matches.Count > 2)
                            for (var k = 0; k < 255; k++)
                                PossibleIps.Add(string.Format(Mask, j, i, k));
                        else
                            PossibleIps.Add(string.Format(Mask, j, i));
                else
                    PossibleIps.Add(string.Format(Mask, j));
        }

        protected virtual void OnVlcHostFound(string e)
        {
            VlcHostFound?.Invoke(this, e);
        }

        /// <summary>
        ///     Starts to look for vlc hosts
        /// </summary>
        public void Start()
        {
            CancellationToken = new CancellationTokenSource();
            IsLoopRunning = true;
            Task.Factory.StartNew(() =>
            {
                while (IsLoopRunning)
                    Parallel.ForEach(PossibleIps, item =>
                    {
                        if (CheckHostForVlc(item, 8080).Result)
                            OnVlcHostFound(item);
                    });
            }, CancellationToken.Token);
        }

        /// <summary>
        ///     Stops the IPs scan
        /// </summary>
        public void Stop()
        {
            IsLoopRunning = false;
            CancellationToken.Cancel();
        }
    }
}