using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcSASE.Startup))]
namespace MvcSASE
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
