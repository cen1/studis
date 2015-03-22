using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(studis.Startup))]
namespace studis
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
