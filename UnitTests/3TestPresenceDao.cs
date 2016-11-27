using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Common;
using System.Configuration;
using WuHu.SQLServer;
using WuHu.Domain;
using System.Collections.Generic;

namespace WuHu.UnitTests
{
	// passed
	[TestClass]
	public class TestPresenceDao
	{
		private static IDatabase database;
		private static IPresenceDao dao;

		[ClassInitialize()]
		public static void Initialize(TestContext context)
		{
			database = DalFactory.CreateDatabase();

			dao = DalFactory.CreatePresenceDao(database);

			Assert.IsNotNull(database);
		}

		[TestMethod]
		public void InsertPresence()
		{
			RoleDao rd = new RoleDao(database);
			Role re = null;
			foreach (Role r in rd.FindAll())
			{
				re = r;
				break;
			}

			Random rand = new Random();

			PlayerDao pd = new PlayerDao(database);
			Player player = new Player(re.ID, "Alexander", "Krämer", 
				"insertpresence" + rand.Next(), 100, "myPic.png", "password");
			int playerId = pd.Insert(player);

			DayDao daydao = new DayDao(database);
			Day day = null;
			foreach (Day d in daydao.FindAll())
			{
				day = d;
				break;
			}

			int first = dao.FindAll().Count;

			int ret = dao.Insert(new Presence(playerId, day.ID));

			int second = dao.FindAll().Count;
			Assert.AreNotEqual(0, ret);
			Assert.AreEqual(first, second-1);
		}

		[TestMethod]
		public void InsertPresenceException()
		{
			dao.Insert(new Presence(1, 2));
			// is 0 because player with id 1 and day with id 2 don't exist, and 0 rows are affected
			Assert.AreEqual(dao.Insert(new Presence(1, 2)), 0);
		}
		
		[TestMethod]
		public void CheckUpdate()
		{
			DayDao daydao = new DayDao(database);
			PlayerDao playerdao = new PlayerDao(database);
			RoleDao roledao = new RoleDao(database);

			Role role = null;
			foreach(Role r in roledao.FindAll())
			{
				role = r;
				break;
			}

			Random rand = new Random();

			int resDay = daydao.Insert(new Day("testupdatecomand1" + rand.Next()));
			int resNewDay = daydao.Insert(new Day("testupdatecomand2" + rand.Next()));

			Player player = new Player(role.ID, "Firstname", "Lastname", "nicknametest" + rand.Next(), 
				10, "testpath", "password");

			int resPlayer = playerdao.Insert(player);
			
			
			Assert.IsTrue(dao.Insert(new Presence(resPlayer, resDay)) == 1);

			Presence p = dao.FindById(resPlayer, resDay);

			Presence pnew = new Presence(resPlayer, resNewDay);

			dao.Update(p, pnew);
			

			Assert.AreEqual(p.PlayerID, pnew.PlayerID);
			Assert.AreNotEqual(p.DayID, pnew.DayID);
			
		}
		/*
		[TestMethod]
		public void CheckDeleteAll()
		{
			InsertPresence();
			dao.DeleteAll();

			Assert.AreEqual(dao.FindAll().Count, 0);
		}
		*/
		[TestMethod]
		public void CheckDeleteById()
		{
			PlayerDao playerdao = new PlayerDao(database);
			Player player = null;
			foreach (Player p in playerdao.FindAll())
			{
				player = p;
				break;
			}

			DayDao daydao = new DayDao(database);
			Day day = null;
			foreach (Day d in daydao.FindAll())
			{
				day = d;
				break;
			}

			int first = dao.FindAll().Count;

			dao.DeleteById(player.ID, day.ID);
			
			int second = dao.FindAll().Count;

			Assert.AreEqual(first, second+1);
		}

		[TestMethod]
		public void DeleteAll()
		{
			PresenceDao dao = new PresenceDao(database);
			dao.DeleteAll();

			Assert.AreEqual(0, dao.FindAll().Count);
		}

		[TestMethod]
		public void GetAllPresence()
		{
			PlayerDao playerdao = new PlayerDao(database);
			Player player = null;
			foreach (Player playr in playerdao.FindAll())
			{
				player = playr;
				break;
			}

			DayDao daydao = new DayDao(database);
			Day day = null;
			foreach (Day d in daydao.FindAll())
			{
				day = d;
				break;
			}

			int first = dao.FindAll().Count;
			Presence p = new Presence(player.ID, day.ID);
			dao.Insert(p);
			int second = dao.FindAll().Count;

			Assert.AreEqual(first, second - 1);
			Assert.IsTrue(dao.FindAll().Count > 0);
		}
		
		[TestMethod]
		public void GetPresencesByDay()
		{
			DayDao daydao = new DayDao(database);
			Day day = null;
			foreach(Day d in daydao.FindAll())
			{
				day = d;
				break;
			}

			IList<Presence> allPresenceOfDay = dao.FindPresenceByDay(day.ID);

			foreach (Presence p in allPresenceOfDay)
			{
				Assert.AreEqual(day.ID, p.DayID);
			}
		}

		[TestMethod]
		public void GetPresenceByPlayer()
		{
			PlayerDao playerdao = new PlayerDao(database);
			Player player = null;
			foreach(Player p in playerdao.FindAll())
			{
				player = p;
				break;
			}


			IList<Presence> allPresenceOfPlayer = dao.FindPresenceByPlayer(player.ID);

			foreach (Presence p in allPresenceOfPlayer)
			{
				Assert.AreEqual(player.ID, p.PlayerID);
			}
		}
	}
}
