using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VNASTWebsite.Startup))]
namespace VNASTWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
