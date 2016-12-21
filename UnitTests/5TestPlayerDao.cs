using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using WuHu.SQLServer;
using WuHu.Common;
using WuHu.Domain;
using System.Text;

namespace WuHu.UnitTests
{
	// passed
	[TestClass]
	public class TestPlayerDao
	{
		private static IDatabase database;
		private static IPlayerDao dao;

		[ClassInitialize()]
		public static void Initialize(TestContext context)
		{
			database = DalFactory.CreateDatabase();

			dao = DalFactory.CreatePlayerDao(database);

			Assert.IsNotNull(database);
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
			sb.Append(lastname[0]);
			sb.Append(lastname[1]);
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

		[TestMethod]
		public int InsertTestPlayers()
		{
			PlayerDao pd = new PlayerDao(database);

			Player player = GeneratePlayer();
			int stat1 = pd.FindAll().Count;
			int ret = pd.Insert(player);
			int stat2 = pd.FindAll().Count;
			Assert.IsTrue(stat1 == stat2-1);
			return ret;
		}

		

		[TestMethod]
		public void CheckUpdate()
		{
			int id = InsertTestPlayers();
			PlayerDao pd = new PlayerDao(database);
			Player player = pd.FindById(id);
			Random rand = new Random();
			int randVal = rand.Next();
			player.Nickname = "Hannes" + randVal;
			pd.Update(player);

			Player newPlayer = pd.FindByNickname("Hannes" + randVal);

			Assert.AreEqual(newPlayer.Nickname, "Hannes" + randVal);
		}

		[TestMethod]
		public void CheckDeleteById()
		{
			PlayerDao pd = new PlayerDao(database);
			Player player = new Player(true, "Alexander", "Krämer", "checkDeleteById", 234, "asdf", "pw", true,
				false, true, false, true, false);

			int lastInsertedId = pd.Insert(player);

			Assert.AreNotEqual(0, lastInsertedId);

			pd.DeleteById(lastInsertedId);

			Assert.IsNull(pd.FindById(lastInsertedId));
		}
		/*
		[TestMethod]
		public void DeleteAll()
		{
			PlayerDao dao = new PlayerDao(database);
			dao.DeleteAll();

			Assert.AreEqual(0, dao.FindAll().Count);
		}
		*/
	}
}
