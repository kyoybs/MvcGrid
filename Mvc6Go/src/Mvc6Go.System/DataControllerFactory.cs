using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Http;

namespace Mvc6Go.System
{
    public class DataControllerFactory
    {
        //private IServiceProvider _serviceProvider;

        public HttpContext HttpContext {get;set;}

        public string UserName { get; set; }

        public DataControllerFactory()
        {
            //_serviceProvider = serviceProvider;
        }

        public T Create<T>() where T : AbstractDataController
        {
            T dc = Activator.CreateInstance<T>();
            dc.HttpContext = HttpContext ;
            return dc;
        }
    }
}
