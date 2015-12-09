using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mvc6Go.System
{
    public class RequestContext
    {
        public ClaimsPrincipal LoginUser { get; set; }

        public UriBuilder Url { get; set; } 
    }
}
