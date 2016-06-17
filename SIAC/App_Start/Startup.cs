using Microsoft.Owin;
using Owin;
using SIAC;

namespace SIAC
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}