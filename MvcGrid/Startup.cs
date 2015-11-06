using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcGrid.Startup))]
namespace MvcGrid
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
