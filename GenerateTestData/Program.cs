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
        private static IDatabase database;

        static void Main(string[] args)
        {
            database = DalFactory.CreateDatabase();

        }

        private void GenerateRoles()
        {

            RoleDao dao = new RoleDao(database); 

            Role admin = new Role("Admin");
            Role member = new Role("Member");
            Role editor = new Role("Editor");

            dao.Insert(admin);
            dao.Insert(member);
            dao.Insert(editor);
        }

        private void GenerateDays()
        {
            DayDao dao = new DayDao(database);

            Day montag = new Day("Montag");
            Day dienstag = new Day("Dienstag");
            Day mittwoch = new Day("Mittwoch");
            Day donnerstag = new Day("Donnerstag");
            Day freitag = new Day("Freitag");
            Day samstag = new Day("Samstag");

            dao.Insert(montag);
            dao.Insert(dienstag);
            dao.Insert(mittwoch);
            dao.Insert(donnerstag);
            dao.Insert(freitag);
            dao.Insert(samstag);
        }
    }
}
