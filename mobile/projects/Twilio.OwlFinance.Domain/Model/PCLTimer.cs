// http://stackoverflow.com/questions/21151143/pcl-net-4-5-timer
using System;
using System.Threading.Tasks;

namespace Twilio.OwlFinance.Domain
{
	public class PCLTimer
	{
		private bool timerRunning;
		private TimeSpan interval;
		private Action tick;
		private bool runOnce;

		public PCLTimer(TimeSpan interval, Action tick, bool runOnce = false)
		{
			this.interval = interval;
			this.tick = tick;
			this.runOnce = runOnce;
		}

		public PCLTimer Start()
		{
			if (!timerRunning)
			{
				timerRunning = true;
				RunTimer();
			}

			return this;
		}

		public void Stop()
		{
			timerRunning = false;
		}

		private async Task RunTimer()
		{
			while (timerRunning)
			{
				await Task.Delay(interval);

				if (timerRunning)
				{
					tick();

					if (runOnce)
					{
						Stop();
					}
				}
			}
		}
	}
}