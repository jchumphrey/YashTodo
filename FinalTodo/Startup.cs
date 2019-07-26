using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FinalTodo.Startup))]
namespace FinalTodo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
