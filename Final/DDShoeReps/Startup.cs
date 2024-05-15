using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DDShoeReps.Startup))]
namespace DDShoeReps
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
