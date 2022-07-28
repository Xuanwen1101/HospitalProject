using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HospitalProject_Group3.Startup))]
namespace HospitalProject_Group3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
