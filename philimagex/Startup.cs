using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(philimagex.Startup))]
namespace philimagex
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
