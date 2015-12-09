using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc6Go.Middlewares
{
    public abstract class AbstractMiddleware
    {
        protected RequestDelegate Next { get; set; }

        protected AbstractMiddleware(RequestDelegate next)
        {
            this.Next = next;
        }

        public abstract Task Invoke(HttpContext context);
    }
}
