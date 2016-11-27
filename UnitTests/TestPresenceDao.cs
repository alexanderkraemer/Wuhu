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
			PlayerDao playerdao = new PlayerDao(database);
			DayDao daydao = new DayDao(database);

			IList<Player> allPlayers = playerdao.FindAll();
			IList<Day> allDays = daydao.FindAll();

			foreach (Player p in allPlayers)
			{
				foreach (Day d in allDays)
				{
					dao.Insert(new Presence(p.ID, d.ID));
				}
			}

			Assert.AreEqual(allPlayers.Count * allDays.Count, dao.FindAll().Count);
		}

		[TestMethod]
		public void InsertPresenceException()
		{
			// is 0 because player with id 1 and day with id 2 don't exist, and 0 rows are affected
			Assert.AreEqual(dao.Insert(new Presence(1, 2)), 0);
		}
		/*
		[TestMethod]
		public void CheckUpdate()
		{
			TestDayDao testDay = new TestDayDao();
			TestPlayerDao testPlayer = new TestPlayerDao();
			
			int resDay = testDay.InsertDay();
			
			int resNewDay = testDay.InsertDay();
			
			int resPlayer = testPlayer.InsertTestPlayers();
			/*
			
			Assert.IsTrue(dao.Insert(new Presence(resPlayer, resDay)) == 1);

			Presence p = dao.FindById(resPlayer, resDay);

			Presence pnew = new Presence(resPlayer, resNewDay);

			dao.Update(p, pnew);
			

			Assert.AreEqual(p.PlayerID, pnew.PlayerID);
			Assert.AreNotEqual(p.DayID, pnew.DayID);
			
		}
		*/
		[TestMethod]
		public void CheckDeleteAll()
		{
			InsertPresence();
			dao.DeleteAll();

			Assert.AreEqual(dao.FindAll().Count, 0);
		}
		/*
		[TestMethod]
		public void CheckDeleteById()
		{
			InsertPresence();
			int first = dao.FindAll().Count;
			foreach (Presence presence in dao.FindAll())
			{
				dao.DeleteById(presence.PlayerID, presence.DayID);
				break;
			}
			int second = dao.FindAll().Count;
			Assert.AreEqual(first, second-1);
		}
		
		[TestMethod]
		public void GetAllPresence()
		{
			TestDayDao testDay = new TestDayDao();
			testDay.InsertDay();

			TestPlayerDao testPlayer = new TestPlayerDao();
			testPlayer.InsertTestPlayers();

			InsertPresence();

			IList<Presence> allPresence = dao.FindAll();

			Assert.AreEqual(allPresence.Count, 180);
		}
		*/
		[TestMethod]
		public void GetPresencesByDay()
		{
			InsertPresence();

			IList<Presence> allPresenceOfDay = dao.FindPresenceByDay(1234);

			foreach (Presence p in allPresenceOfDay)
			{
				Assert.AreEqual(1234, p.DayID);
			}
		}

		[TestMethod]
		public void GetPresenceByPlayer()
		{
			InsertPresence();

			IList<Presence> allPresenceOfPlayer = dao.FindPresenceByPlayer(1111);

			foreach (Presence p in allPresenceOfPlayer)
			{
				Assert.AreEqual(1111, p.PlayerID);
			}
		}
	}
}
