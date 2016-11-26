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
	}

	public class GenerateData
	{

		private Random gen = new Random();
		private IDatabase database;

		public GenerateData(IDatabase database)
		{
			this.database = database;
		}

		private Player GeneratePlayer()
		{
			Random rFirst;
			Random rLast;

			rFirst = new Random();
			rLast = new Random(rFirst.Next());
			string nickname;

			// 33 firstnames
			string[] _firstNames = new string[] { "Adam", "Alex", "Aaron", "Ben", "Carl", "Dan",
					 "David", "Edward", "Fred", "Frank", "George", "Hal", "Hank", "Ike", "John", "Jack",
					 "Joe", "Larry", "Monte", "Matthew", "Mark", "Nathan", "Otto", "Paul", "Peter", "Roger",
					 "Roger", "Steve", "Thomas", "Tim", "Ty", "Victor", "Walter" };

			// 150 lastnames
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

			Console.WriteLine(_lastNames.Length);

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
		public IList<Player> GeneratePlayers(int number = 30)
		{
			List<Player> playerlist = new List<Player>();
			for(int i = 0; i < number; ++i)
			{
				playerlist.Add(GeneratePlayer());
			}

			return playerlist;
		}

		public IList<Team> GenerateTeams()
		{
			PlayerDao playerdao = new PlayerDao(database);
			TeamDao teamdao = new TeamDao(database);

			List<Team> teamList = new List<Team>();

			int counter = 0;
			foreach(Player p1 in playerdao.FindAll())
			{
				foreach(Player p2 in playerdao.FindAll())
				{
					++counter;
					if(p1.ID != p2.ID)
					{
						teamList.Add(new Team("Team " + counter, p1.ID, p2.ID));
					}			
				}
			}
			return teamList;
		}

		private DateTime RandomDay()
		{
			DateTime start = new DateTime(1995, 1, 1);
			int range = (DateTime.Today - start).Days;
			return start.AddDays(gen.Next(range));
		}
		public IList<Match> GenerateMatchs()
		{
			IList<Match> matchlist = new List<Match>();
			TeamDao teamdao = new TeamDao(database);

			foreach(Team team1 in teamdao.FindAll())
			{
				foreach(Team team2 in teamdao.FindAll())
				{
					if(team1.ID != team2.ID)
					{
						matchlist.Add(new Match(team1.ID, team2.ID, RandomDay()));
					}
				}
			}
			return matchlist;
		}
	}

	public static class Data
	{
		public static void Clear(IDatabase database)
		{
			DayDao d = new DayDao(database);
			RoleDao r = new RoleDao(database);
			PlayerDao p = new PlayerDao(database);
			TeamDao t = new TeamDao(database);
			PresenceDao pr = new PresenceDao(database);
			MatchDao m = new MatchDao(database);
			StatisticDao s = new StatisticDao(database);

			d.DeleteAll();
			r.DeleteAll();
			p.DeleteAll();
			t.DeleteAll();
			pr.DeleteAll();
			m.DeleteAll();
			s.DeleteAll();
		}

		public static void Insert(IDatabase database)
		{
			InsertData id = new InsertData(database);
			id.InsertDays();
			id.InsertRoles();
			id.InsertPlayer(30);
			id.InsertTeams();
			id.InsertPresence();
			id.InsertMatches();
			id.InsertStatistics();
		}
	}

	public class InsertData
	{
		private GenerateData dataSource;
		private IDatabase database;
		public InsertData(IDatabase database)
		{
			this.database = database;
			this.dataSource = new GenerateData(database);
		}

		private bool RandomBool()
		{
			Random rand = new Random();
			double randNumb = rand.NextDouble();
			if (randNumb > 0.5)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public void InsertRoles()
		{

			RoleDao dao = new RoleDao(database);

			Role admin = new Role("Admin");
			Role member = new Role("Member");
			Role editor = new Role("Editor");

			dao.Insert(admin);
			dao.Insert(member);
			dao.Insert(editor);
		}
		public void InsertDays()
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
		public void InsertPlayer(int number = 30)
		{
			PlayerDao dao = new PlayerDao(database);		

			foreach(Player p in dataSource.GeneratePlayers(number))
			{
				dao.Insert(p);
			}
		}
		public void InsertTeams()
		{
			TeamDao dao = new TeamDao(database);

			foreach(Team t in dataSource.GenerateTeams())
			{
				dao.Insert(t);
			}
		}
		public void InsertMatches()
		{
			MatchDao dao = new MatchDao(database);
			foreach(Match m in dataSource.GenerateMatchs())
			{
				dao.Insert(m);
			}
		}
		public void InsertPresence()
		{
			PlayerDao pd = new PlayerDao(database);
			DayDao dd = new DayDao(database);
			PresenceDao pcd = new PresenceDao(database);

			foreach (Player p in pd.FindAll())
			{
				foreach(Day d in dd.FindAll())
				{
					if(RandomBool())
					{
						pcd.Insert(new Presence(p.ID, d.ID));
					}
				}
			}
		}

		

		public void InsertStatistics()
		{

		}
	}
}
