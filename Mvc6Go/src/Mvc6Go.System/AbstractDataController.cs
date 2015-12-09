using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mvc6Go.System
{
    public class AbstractDataController : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; }

        public ClaimsPrincipal LoginUser
        {
            get { return HttpContext.User; }
        }
    }
}
