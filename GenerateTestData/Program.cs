using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;
using WuHu.Domain;
using WuHu.SQLServer;

namespace WuHu.GenerateTestData
{
    class Program
    {
        private IDatabase database;

        static void Main(string[] args)
        {
        }

        private void GenerateRoles()
        {
            database = DalFactory.CreateDatabase();

            RoleDao dao = new RoleDao(database); 

            Role role = new Role("Admin");

        }
    }
}
