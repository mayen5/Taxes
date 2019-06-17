using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Taxes.Clasess;
using Taxes.Migrations;
using Taxes.Models;

namespace Taxes
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TaxesContext, Configuration>());
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            this.CheckRoles();
            Utilities.CheckSuperUser();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void CheckRoles()
        {
            Utilities.CheckRole("Admin");
            Utilities.CheckRole("Employee");
            Utilities.CheckRole("Taxpayer");
        }
    }
}
