using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using WuHu.BusinessLogic;

namespace WuHu.WebAPI.Jobs
{
	public class DecayJob : IJob, IRegisteredObject
	{
		private readonly object _lock = new object();

		private bool _shuttingDown;

		public DecayJob()
		{
			// Register this job with the hosting environment.
			// Allows for a more graceful stop of the job, in the case of IIS shutting down.
			HostingEnvironment.RegisterObject(this);
		}

		public void Execute()
		{
			lock (_lock)
			{
				if (_shuttingDown)
					return;

				BLPlayer.Decay();
			}
		}

		public void Stop(bool immediate)
		{
			// Locking here will wait for the lock in Execute to be released until this code can continue.
			lock (_lock)
			{
				_shuttingDown = true;
			}

			HostingEnvironment.UnregisterObject(this);
		}
	}
}