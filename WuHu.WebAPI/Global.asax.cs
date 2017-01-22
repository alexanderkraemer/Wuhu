using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using WuHu.WebAPI.Jobs;

namespace WuHu.WebAPI
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			GlobalConfiguration.Configure(WebApiConfig.Register);

			Registry r = new Registry();
			r.Schedule<DecayJob>().ToRunNow().AndEvery(7).Days();

			JobManager.Initialize(r);
		}
	}
}
