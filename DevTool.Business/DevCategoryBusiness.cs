using DevTool.Business.Entity;
using Synvata.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTool.Business
{
    public class DevCategoryBusiness
    {
        public async static Task<List<DevCategory>> GetCategories()
        {
            using (var db = DbHelper.Create())
            {
                return (await db.QueryAsync<DevCategory>("SELECT * FROM dbo.DevCategory")).ToList();
            }
        }


    }
}
