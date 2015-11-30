using Synvata.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTool.Business
{
    public static class DbHelper
    {
        public static DbAgent Create()
        {
            return DbAgent.Create("DevDB");
        }
    }
}
