using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugSmear.Startup))]
namespace BugSmear
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
