using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using WuHu.SQLServer;
using WuHu.Common;
using WuHu.Domain;
using System.Text;

namespace WuHu.UnitTests
{
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

        [TestInitialize]
        public void Initialize()
        {
            dao.DeleteAll();
        }

        public Player GeneratePlayer()
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

		[TestMethod]
		public void InsertTestPlayers()
		{
			CheckDeleteAll();
			PlayerDao pd = new PlayerDao(database);
			for (int i = 0; i < 30; ++i)
			{
				Player player = this.GeneratePlayer();

				int ret = pd.Insert(player);
			}
			Assert.AreEqual(pd.FindAll().Count, 30);
		  }

		[TestMethod]
		public void CheckIfTestDataIsValid()
		{
			InsertTestPlayers();
			PlayerDao pd = new PlayerDao(database);

			foreach (Player p in pd.FindAll())
			{
				Assert.AreEqual(p.Password, "myPassword");
				Assert.AreEqual(p.PhotoPath, "userpic.png");
				Assert.AreEqual(p.RoleID, 1);
				Assert.AreEqual(p.Skills, 1);

				string firstname = p.FirstName;
				string lastname = p.LastName;

				StringBuilder sb = new StringBuilder();
				sb.Append(firstname[0]);
				sb.Append(firstname[1]);
				sb.Append(lastname[0]);
				sb.Append(lastname[1]);
				string nickname = sb.ToString();

				Assert.AreEqual(p.Nickname, nickname);
			}
		}

		[TestMethod]
		public void CheckInsert()
		{
			this.CheckDeleteAll();
			PlayerDao pd = new PlayerDao(database);
			Player player = new Player(1, "Alexander", "Krämer", "Alex", 100, "myPic.png", "password");

			int ret = pd.Insert(player);
			Assert.IsInstanceOfType(pd.FindById(ret), typeof(Player));
		}

		[TestMethod]
		public void CheckUpdate()
		{
			this.CheckInsert();
			PlayerDao pd = new PlayerDao(database);
			Player player = pd.FindByNickname("Alex");
			player.Nickname = "Hannes";
			pd.Update(player);

			Player newPlayer = pd.FindByNickname("Hannes");

			Assert.AreEqual(newPlayer.Nickname, "Hannes");
		}

		[TestMethod]
		public void CheckDeleteById()
		{
			this.CheckDeleteAll();
			PlayerDao pd = new PlayerDao(database);
			Player player = new Player(1, "Alexander", "Krämer", "Alex", 100, "myPic.png", "password");
			int lastInsertedId = pd.Insert(player);

			Assert.AreNotEqual(0, lastInsertedId);

			pd.DeleteById(lastInsertedId);

			Assert.AreEqual(0, pd.FindAll().Count);
		}

		[TestMethod]
		public void CheckDeleteAll()
		{
			PlayerDao pd = new PlayerDao(database);
			pd.DeleteAll();

			Assert.AreEqual(pd.FindAll().Count, 0);
		}
	}
}
