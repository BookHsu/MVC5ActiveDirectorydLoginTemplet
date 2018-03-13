using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(Mvc5OWINwithAD.Startup))]
namespace Mvc5OWINwithAD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}