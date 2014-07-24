using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SYWeb.Startup))]
namespace SYWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
