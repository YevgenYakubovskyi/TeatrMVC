using Ninject;
using Ninject.Modules;
using Ninject.Web.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Theatr.BLL.Infrastructure;

namespace Theatr.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DependencyResolver.SetResolver(new BLL.Infrastructure.NinjectDependencyResolver("DbConnection"));
        }
        protected void Application_Error()
        {
            Exception ex = Server.GetLastError();
            Server.ClearError();
            Response.Redirect("~/Home/Index");
        }
    }
}
