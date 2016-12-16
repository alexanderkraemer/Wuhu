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

			Data.Clear(database);
			Data.Insert(database);

			Console.WriteLine("done.");
			Console.WriteLine("press {Enter} to quit this program...");
			Console.Read();
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

			
			bool role_id = RandomBool();
			bool isAdmin = RandomBool();
			bool isMonday = RandomBool();
			bool isTuesday = RandomBool();
			bool isWednesday = RandomBool();
			bool isThursday = RandomBool();
			bool isFriday = RandomBool();
			bool isSaturday = RandomBool();
			int skills = 1;
			string password = "myPassword";
			string photopath = "userpic.png";

			return new Player(isAdmin, firstname, lastname, nickname, skills, photopath, password, isMonday,
				isTuesday, isWednesday, isThursday, isFriday, isSaturday);
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
			Random rand = new Random();
			PlayerDao playerdao = new PlayerDao(database);
			TeamDao teamdao = new TeamDao(database);

			List<Team> teamList = new List<Team>();

			int counter = 0;
			foreach(Player p1 in playerdao.FindAll())
			{
				foreach(Player p2 in playerdao.FindAll())
				{
					if (counter < 100)
					{
						if (rand.NextDouble() > 0.6)
						{
							++counter;
							if (p1.ID != p2.ID)
							{
								teamList.Add(new Team("Team " + counter, p1.ID, p2.ID));
							}
						}
					}
					else
					{
						break;
					}
				}
			}
			return teamList;
		}

		public IList<Match> GenerateMatchs()
		{
			IList<Match> matchlist = new List<Match>();
			TeamDao teamdao = new TeamDao(database);
			IList<Team> teamlist = teamdao.FindAll();

			DateTime start = DateTime.Now.AddYears(-2);
			int range = (DateTime.Today - start).Days;
			Random rand = new Random();

			for(int i = 0; i < range; ++i)
			{
				for(int j = 0; j < 4; ++j)
				{
					int r = rand.Next(teamlist.Count);
					int r2 = rand.Next(teamlist.Count);
					while (r == r2)
					{
						r2 = rand.Next(teamlist.Count);
					}
					matchlist.Add(new Match(teamlist[r].ID, teamlist[r2].ID, start.AddDays(i)));
				}
			}
			return matchlist;
		}
	}

	public static class Data
	{
		public static void Clear(IDatabase database)
		{
			PlayerDao p = new PlayerDao(database);
			TeamDao t = new TeamDao(database);
			MatchDao m = new MatchDao(database);
			StatisticDao s = new StatisticDao(database);

			s.DeleteAll();
			Console.WriteLine("cleared statistics...");
			m.DeleteAll();
			Console.WriteLine("cleared matches...");
			t.DeleteAll();
			Console.WriteLine("cleared teams...");
			p.DeleteAll();
			Console.WriteLine("cleared player...");
		}

		public static void Insert(IDatabase database)
		{
			InsertData id = new InsertData(database);
			id.InsertPlayer(30);
			Console.WriteLine("inserted player...");
			id.InsertTeams();
			Console.WriteLine("inserted teams...");
			id.InsertMatches();
			Console.WriteLine("inserted matches...");
			id.InsertStatistics();
			Console.WriteLine("inserted statistics...");
			Console.WriteLine("done.");
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

		public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
		{
			for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(7))
				yield return day;
		}

		public void InsertStatistics()
		{
			StatisticDao dao = new StatisticDao(database);
			PlayerDao pdao = new PlayerDao(database);
			int i = 0;
			foreach (DateTime day in EachDay(new DateTime(2014, 11, 26), new DateTime(2016, 11, 26)))
			{
				++i;
				foreach(Player player in pdao.FindAll())
				{
					dao.Insert(new Statistic(player.ID, i, day));
				}
			}
		}
	}
}
