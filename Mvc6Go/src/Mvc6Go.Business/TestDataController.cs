using Mvc6Go.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc6Go.Business
{
    public class TestDataController:AbstractDataController
    {
        public string GetUserName()
        {
            return this.LoginUser.Identity.Name;
        }
    }
}
