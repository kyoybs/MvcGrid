using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;
using Mvc6Go.System;
using Microsoft.AspNet.Builder;

namespace Mvc6Go.Middlewares
{
    public class RequestMiddleware:AbstractMiddleware
    {
        public RequestMiddleware(RequestDelegate next) : base(next) { }

        public async override Task Invoke(HttpContext context)
        {
            DataControllerFactory factory = context.RequestServices.GetService(typeof(DataControllerFactory)) as DataControllerFactory;
            factory.HttpContext = context;
            factory.UserName = context.User.Identity.Name;
            //var arrHost = context.Request.Host.Value.Split(':');
            //string host = arrHost[0];
            //int port = arrHost.Length > 1 ? int.Parse(arrHost[1]) : 80;
            //rc.Url = new UriBuilder(context.Request.Scheme, host, port, context.Request.Path, context.Request.QueryString.ToString());
            await this.Next.Invoke(context);
        }
    }
}
