using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcSASE2.Startup))]
namespace MvcSASE2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
