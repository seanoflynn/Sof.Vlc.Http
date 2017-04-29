using System;
using System.Threading.Tasks;

namespace Sof.Vlc.Http
{
	/// <summary>
	/// Specifies an interface for starting, stopping and setting a polling interval.
	/// </summary>
	public abstract class VlcPollable
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="T:Sof.Vlc.Http.VlcPollable"/> is polling.
		/// </summary>
		/// <value><c>true</c> if is polling; otherwise, <c>false</c>.</value>
		public bool IsPolling { get; private set; } = false;

		/// <summary>
		/// Gets or sets the polling interval.
		/// </summary>
		/// <value>The polling interval.</value>
		public TimeSpan PollingInterval { get; set; } = TimeSpan.FromSeconds(1);

		/// <summary>
		/// Method that is called when the polling interval has elapsed.
		/// </summary>
		/// <returns>The poll.</returns>
		protected abstract Task OnPoll();

		/// <summary>
		/// Starts the polling.
		/// </summary>
		public void StartPolling()
		{
			IsPolling = true;
			Task.Run(Poll);
		}

		/// <summary>
		/// Stops the polling.
		/// </summary>
		public void StopPolling()
		{
			IsPolling = false;
		}

		/// <summary>
		/// The internal polling loop.
		/// </summary>
		private async Task Poll()
		{
			while (IsPolling)
			{
				await OnPoll();
				await Task.Delay(PollingInterval);
			}
			IsPolling = false;
		}
    }
}
