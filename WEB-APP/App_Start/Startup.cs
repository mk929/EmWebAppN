using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Owin;
using EmWebApp.Models;
using System.Web.ApplicationServices;

[assembly: OwinStartupAttribute(typeof(EmWebApp.Startup))]
namespace EmWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
