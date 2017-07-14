using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DaGym.Startup))]
namespace DaGym
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
