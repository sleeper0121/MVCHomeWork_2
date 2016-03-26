using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyMVCTest04.Startup))]
namespace MyMVCTest04
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
