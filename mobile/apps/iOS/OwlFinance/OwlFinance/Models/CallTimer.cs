using System;
using System.Timers;

namespace OwlFinance.Models
{
	public class CallTimer
	{
		public string Time { get; private set; }
		public event Action OnUpdate;

		private int hours;
		private int minutes;
		private int seconds;
		private Timer timer;

		public void Start()
		{
			timer = new Timer(1000);
			timer.Elapsed += (sender, e) => UpdateTime();
			timer.Start();
		}

		public void Stop()
		{
			timer?.Stop();
			timer = null;
			Reset();
		}

		private void Reset()
		{
			seconds = 0;
			minutes = 0;
			hours = 0;
		}

		private void UpdateTime()
		{
			seconds++;
			if (seconds == 60)
			{
				minutes++;
				seconds = 0;
			}
			if (minutes == 60)
			{
				hours++;
				minutes = 0;
			}

			var callHours = hours.ToString();
			var callMinutes = minutes.ToString();
			var callSeconds = seconds.ToString();

			if (seconds < 10)
			{
				callSeconds = "0" + seconds;
			}
			if (minutes < 10)
			{
				callMinutes = "0" + minutes;
			}
			if (hours < 10)
			{
				callHours = "0" + hours;
			}

			Time = $"{callMinutes}:{callSeconds}";
			if (hours > 0) Time = Time.Insert(0, callHours);

			OnUpdate?.Invoke();
		}
	}
}