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

        private Player GeneratePlayer()
        {
            Random rFirst;
            Random rLast;

            rFirst = new Random();
            rLast = new Random(rFirst.Next());
            string nickname;

            string[] _firstNames = new string[] { "Adam", "Alex", "Aaron", "Ben", "Carl", "Dan",
                "David", "Edward", "Fred", "Frank", "George", "Hal", "Hank", "Ike", "John", "Jack",
                "Joe", "Larry", "Monte", "Matthew", "Mark", "Nathan", "Otto", "Paul", "Peter", "Roger",
                "Roger", "Steve", "Thomas", "Tim", "Ty", "Victor", "Walter" };

            string[] _lastNames = new string[] { "Anderson", "Ashwoon", "Aikin", "Bateman", "Bongard",
                "Bowers", "Boyd", "Cannon", "Cast", "Deitz", "Dewalt", "Ebner", "Frick", "Hancock", "Haworth",
                "Hesch", "Hoffman", "Kassing", "Knutson", "Lawless", "Lawicki", "Mccord", "McCormack", "Miller",
                "Myers", "Nugent", "Ortiz", "Orwig", "Ory", "Paiser", "Pak", "Pettigrew", "Quinn", "Quizoz",
                "Ramachandran", "Resnick", "Sagar", "Schickowski", "Schiebel", "Sellon", "Severson", "Shaffer",
                "Solberg", "Soloman", "Sonderling", "Soukup", "Soulis", "Stahl", "Sweeney", "Tandy", "Trebil",
                "Trusela", "Trussel", "Turco", "Uddin", "Uflan", "Ulrich", "Upson", "Vader", "Vail", "Valente",
                "Van Zandt", "Vanderpoel", "Ventotla", "Vogal", "Wagle", "Wagner", "Wakefield", "Weinstein", "Weiss",
                "Woo", "Yang", "Yates", "Yocum", "Zeaser", "Zeller", "Ziegler", "Bauer", "Baxster", "Casal", "Cataldi",
                "Caswell", "Celedon", "Chambers", "Chapman", "Christensen", "Darnell", "Davidson", "Davis", "DeLorenzo",
                "Dinkins", "Doran", "Dugelman", "Dugan", "Duffman", "Eastman", "Ferro", "Ferry", "Fletcher", "Fietzer",
                "Hylan", "Hydinger", "Illingsworth", "Ingram", "Irwin", "Jagtap", "Jenson", "Johnson", "Johnsen", "Jones",
                "Jurgenson", "Kalleg", "Kaskel", "Keller", "Leisinger", "LePage", "Lewis", "Linde", "Lulloff", "Maki",
                "Martin", "McGinnis", "Mills", "Moody", "Moore", "Napier", "Nelson", "Norquist", "Nuttle", "Olson",
                "Ostrander", "Reamer", "Reardon", "Reyes", "Rice", "Ripka", "Roberts", "Rogers", "Root", "Sandstrom",
                "Sawyer", "Schlicht", "Schmitt", "Schwager", "Schutz", "Schuster", "Tapia", "Thompson", "Tiernan", "Tisler" };

            string firstname = _firstNames[rFirst.Next(_firstNames.Length)];
            string lastname = _lastNames[rLast.Next(_lastNames.Length)];

            StringBuilder sb = new StringBuilder();
            sb.Append(firstname[0]);
            sb.Append(firstname[1]);
            sb.Append(lastname[0]);
            sb.Append(lastname[1]);
            nickname = sb.ToString();

            PlayerDao pd = new PlayerDao(database);

            while (pd.FindByNickname(nickname) != null)
            {
                rFirst = new Random();
                rLast = new Random(rFirst.Next());

                firstname = _firstNames[rFirst.Next(_firstNames.Length)];
                lastname = _lastNames[rLast.Next(_lastNames.Length)];

                sb = new StringBuilder();
                sb.Append(firstname[0]);
                sb.Append(firstname[1]);
                sb.Append(lastname[0]);
                sb.Append(lastname[1]);

                nickname = sb.ToString();
            }


            int role_id = 1051;
            int skills = 1;
            string password = "myPassword";
            string photopath = "userpic.png";

            return new Player(role_id, firstname, lastname, nickname, skills, photopath, password);
        }
    }
}
