using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(SEPAMIAR_SGC.Startup))]

namespace SEPAMIAR_SGC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

		public void ConfigureAuth(IAppBuilder app)
		{
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = "ApplicationCookie",
				LoginPath = new PathString("/Account/Login")
			});
		}
	}
}
