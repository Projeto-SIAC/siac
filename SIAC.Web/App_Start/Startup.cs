using Microsoft.Owin;
using Owin;
using SIAC.Web;

namespace SIAC.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}