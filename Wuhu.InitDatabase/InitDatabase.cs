using System;
using System.Collections.Generic;
using System.Text;
using WuHu.Common;
using WuHu.Domain;
using WuHu.SQLServer;

namespace WuHu.InitDatabase
{

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
			rLast = new Random();
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
			if(firstname.Length > 2)
			{
				sb.Append(firstname[2]);
			}
			sb.Append(lastname[0]);
			sb.Append(lastname[1]);
			if (lastname.Length > 2)
			{
				sb.Append(lastname[2]);
			}
			nickname = sb.ToString();

			PlayerDao pd = new PlayerDao(database);

			while (pd.FindByNickname(nickname) != null)
			{
				rFirst = new Random();
				rLast = new Random();

				firstname = _firstNames[rFirst.Next(_firstNames.Length)];
				lastname = _lastNames[rLast.Next(_lastNames.Length)];

				sb = new StringBuilder();
				sb.Append(firstname[0]);
				sb.Append(firstname[1]);
				sb.Append(firstname[2]);
				sb.Append(lastname[0]);
				sb.Append(lastname[1]);
				sb.Append(lastname[2]);

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
			int skills = 1000;
			string password = "$2y$10$10000$6MkE05hvO5ktC+/1Z1P+kdBFWN80p/mG6d8T9LqDmAjYrupL";
			string photopath = nickname + ".png";

			return new Player(isAdmin, firstname, lastname, nickname, skills, photopath, password, isMonday,
				isTuesday, isWednesday, isThursday, isFriday, isSaturday);
		}
		public IList<Player> GeneratePlayers(int number = 30)
		{
			List<Player> playerlist = new List<Player>();
			for (int i = 0; i < number; i++)
			{
				playerlist.Add(GeneratePlayer());
			}

			Player alex = new Player(true, "Alexander", "Krämer", "Alex", 1790, "alex.png",
				"$2y$10$10000$6MkE05hvO5ktC+/1Z1P+kdBFWN80p/mG6d8T9LqDmAjYrupL", true, true, true, false, false, false);
			playerlist.Add(alex);

			return playerlist;
		}


		public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
		{
			for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
				yield return day;
		}

		public IList<Tournament> GenerateTournaments()
		{
			Random rand = new Random();
			PlayerDao playerdao = new PlayerDao(database);
			TournamentDao tournamentDao = new TournamentDao(database);

			List<Tournament> tournamentList = new List<Tournament>();

			int counter = 0;

			foreach (DateTime day in EachDay(DateTime.Today.AddYears(-1), DateTime.Today))
			{
				++counter;
				tournamentList.Add(new Tournament("Tournament " + counter, day));
			}
			return tournamentList;
		}

		private int GenerateRandom(int start, int end)
		{
			Random rand = new Random();
			return rand.Next(start, end);
		}

		private IList<E> ShuffleList<E>(IList<E> inputList)
		{
			IList<E> randomList = new List<E>();
			IList<E> inputCopy = new List<E>();
			foreach (var o in inputList)
			{
				inputCopy.Add(o);
			}
			Random r = new Random();
			int randomIndex = 0;
			while (inputCopy.Count > 0)
			{
				randomIndex = r.Next(0, inputCopy.Count); //Choose a random object in the list
				randomList.Add(inputCopy[randomIndex]); //add it to the new, random list

				inputCopy.RemoveAt(randomIndex); //remove to avoid duplicates
			}

			return randomList; //return the new random list
		}

		public IList<Match> GenerateMatchs()
		{
			IList<Match> matchlist = new List<Match>();
			TournamentDao tournamentDao = new TournamentDao(database);
			PlayerDao PlayerDao = new PlayerDao(database);

			IList<Tournament> tournamentList = tournamentDao.FindAll();
			IList<Player> playerList = PlayerDao.FindAll();


			foreach (Tournament t in tournamentList)
			{
				IList<Player> newList = ShuffleList<Player>(playerList);
				int i = 0;
				Random rand = new Random();

				matchlist.Add(new Match(newList[i++].ID, newList[i++].ID, newList[i++].ID, newList[i++].ID, t.ID, rand.Next(0, 10), rand.Next(0, 10), RandomBool()));
				matchlist.Add(new Match(newList[i++].ID, newList[i++].ID, newList[i++].ID, newList[i++].ID, t.ID, rand.Next(0, 10), rand.Next(0, 10), RandomBool()));
				matchlist.Add(new Match(newList[i++].ID, newList[i++].ID, newList[i++].ID, newList[i++].ID, t.ID, rand.Next(0, 10), rand.Next(0, 10), RandomBool()));
				matchlist.Add(new Match(newList[i++].ID, newList[i++].ID, newList[i++].ID, newList[i++].ID, t.ID, rand.Next(0, 10), rand.Next(0, 10), RandomBool()));
			}

			return matchlist;
		}
	}

	public static class DatabaseReset
	{
		public static void Clear(IDatabase database)
		{
			PlayerDao p = new PlayerDao(database);
			TournamentDao t = new TournamentDao(database);
			MatchDao m = new MatchDao(database);
			StatisticDao s = new StatisticDao(database);

			s.DeleteAll();
			Console.WriteLine("cleared statistics...");
			m.DeleteAll();
			Console.WriteLine("cleared matches...");
			t.DeleteAll();
			Console.WriteLine("cleared Tournaments...");
			p.DeleteAll();
			Console.WriteLine("cleared player...");
		}

		public static void Insert(IDatabase database)
		{
			InsertData id = new InsertData(database);
			id.InsertPlayer(100);
			Console.WriteLine("inserted player...");
			id.InsertTournaments();
			Console.WriteLine("inserted tournaments...");
			id.InsertMatches();
			Console.WriteLine("inserted matches...");
			id.InsertStatistics();
			Console.WriteLine("inserted statistics...");
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

			foreach (Player p in dataSource.GeneratePlayers(number))
			{
				dao.Insert(p);
			}
		}
		public void InsertTournaments()
		{
			TournamentDao dao = new TournamentDao(database);

			foreach (Tournament t in dataSource.GenerateTournaments())
			{
				dao.Insert(t);
			}
		}
		public void InsertMatches()
		{
			MatchDao dao = new MatchDao(database);
			foreach (Match m in dataSource.GenerateMatchs())
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
				foreach (Player player in pdao.FindAll())
				{
					Random rand = new Random();
					dao.Insert(new Statistic(player.ID, rand.Next(700, 1500), day));
				}
			}
		}
	}

}
