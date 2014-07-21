﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcSASE
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            var exception = context.Server.GetLastError();
            if (exception is HttpRequestValidationException)
            {
                context.Server.ClearError();
                Response.Clear();
                Response.StatusCode = 200;
                Response.Write(@"<html><head></head><body>Nope.  Go back and try again.  <hr>Invalid input data detected.</body></html>");
                Response.End();
                return;
            }
        }
    }
}
