using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SASE.Startup))]
namespace SASE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
