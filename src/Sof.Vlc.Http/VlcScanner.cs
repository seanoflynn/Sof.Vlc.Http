using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Sof.Vlc.Http
{
	/// <summary>
	/// Used to scan the network for VLC media player instances with open HTTP interfaces.
	/// </summary>
	public class VlcScanner : VlcPollable
	{
		/// <summary>
		/// Occurs when the list of available end points are updated.
		/// </summary>
		public event EventHandler<IEnumerable<IPEndPoint>> EndPointsUpdated;

		/// <summary>
		/// The list of most recently found available end points.
		/// </summary>
		public List<IPEndPoint> EndPoints { get; private set; } = new List<IPEndPoint>();

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Sof.Vlc.Http.VlcScanner"/> class.
		/// </summary>
		/// <param name="pollingInterval">Polling interval.</param>
		public VlcScanner(TimeSpan pollingInterval = default(TimeSpan))
		{
			if (pollingInterval == default(TimeSpan))
				PollingInterval = TimeSpan.FromSeconds(10);
			else
				PollingInterval = pollingInterval;
		}

		/// <summary>
		/// Called periodically when the polling interval elapses.
		/// </summary>
		protected override async Task OnPoll()
		{
			await UpdateEndPoints();
		}

		/// <summary>
		/// Scans the local network 192.168.*.* for VLC media player instances with open HTTP interfaces.
		/// </summary>
		/// <returns>A list of end points.</returns>
		/// <param name="port">Port number to check.</param>
		public async Task<IEnumerable<IPEndPoint>> UpdateEndPoints(int port = 8080)
		{
			int z = -1;

			var tasks = new Task[256];
			var endPoints = new List<IPEndPoint>();

			for (int i = 0; i < 256; i++)
			{
				tasks[i] = Task.Run(async () =>
				{
					int r = Interlocked.Increment(ref z);

					if (await Ping($"192.168.{r}.255"))
					{
						var ips = await GetEndPointsWithVlcInRange(r, port);
						endPoints.AddRange(ips);
					}
				});
			}
			await Task.WhenAll(tasks);

			EndPoints = endPoints;
			EndPointsUpdated?.Invoke(this, endPoints);

			return endPoints;
		}

		/// <summary>
		/// Gets the end points with VLC in the specified IP range.
		/// </summary>
		/// <returns>The end points with a VLC instance and open HTTP interface.</returns>
		/// <param name="range">The IP range - 192.168.{r}.*.</param>
		/// <param name="port">Port number.</param>
		private async Task<IEnumerable<IPEndPoint>> GetEndPointsWithVlcInRange(int range, int port)
		{	
			int z = -1;

			var tasks = new Task[255];
			var endPoints = new List<IPEndPoint>();

			for (int i = 0; i < 255; i++)
			{
				tasks[i] = Task.Run(async () =>
				{
					int x = Interlocked.Increment(ref z);
					string ip = $"192.168.{range}.{x}";

					if (await Ping(ip) && await CheckHostForVlc(ip, port))
					{
						endPoints.Add(new IPEndPoint(IPAddress.Parse(ip), 8080));
					}
				});
			}

			await Task.WhenAll(tasks);

			return endPoints;
		}

		/// <summary>
		/// Test an IP address to see whether a VLC media player instance has an open HTTP interface 
		/// running on the specified port.
		/// </summary>
		/// <returns><c>true</c> if an running HTTP interface is found.</returns>
		/// <param name="ip">IP address.</param>
		/// <param name="port">Port number.</param>
		private async Task<bool> CheckHostForVlc(string ip, int port)
		{
			string requestUri = $"http://{ip}:{port}/requests/status.xml";

			try
			{
				using (HttpClient client = new HttpClient())
				{					
					client.Timeout = TimeSpan.FromSeconds(5);
					var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);

					if (response?.Headers?.WwwAuthenticate?.FirstOrDefault()?.Parameter == "realm=\"VLC stream\"")
					{
						return true;
					}

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
				{
					throw;
				}
			}

			//Console.WriteLine("Failed for unknown reason: " + requestUri);
			return false;
		}

		/// <summary>
		/// Ping the specified IP address.
		/// </summary>
		/// <returns><c>true</c> if the IP is active.</returns>
		/// <param name="ip">The IP address to ping.</param>
		private async Task<bool> Ping(string ip)
		{
			var ping = new Ping();
			var rep = await ping.SendPingAsync(ip, 500);

			return rep.Status == IPStatus.Success;
		}
	}
}