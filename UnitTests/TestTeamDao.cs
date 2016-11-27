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
	public class TestTeamDao
	{
		private static IDatabase database;
		private static ITeamDao dao;

		[ClassInitialize()]
		public static void Initialize(TestContext context)
		{
			database = DalFactory.CreateDatabase();

			dao = DalFactory.CreateTeamDao(database);

			Assert.IsNotNull(database);
		}

		[TestMethod]
		public int InsertTeams(string teamname = "Testteam")
		{
			PlayerDao pd = new PlayerDao(database);

			int i = 0;
			int player1 = 0;
			int player2 = 0;
			foreach(Player p in pd.FindAll())
			{
				++i;
				switch (i)
				{
					case 1:
						player1 = p.ID;
						break;
					case 2:
						player2 = p.ID;
						break;
				}
			}
			Assert.AreNotEqual(0, player1);
			Assert.AreNotEqual(0, player2);

			Random rand = new Random();

			int res = dao.Insert(new Team(teamname + rand.Next() , player1, player2));

			Assert.AreEqual(dao.FindById(res).ID, res);

			return res;
		}

		[TestMethod]
		public void GetOneTeamByID()
		{
			int teamId = InsertTeams("getOneTeamByIdTest");

			Team team = dao.FindById(teamId);

			Assert.IsNotNull(team);
			Assert.IsInstanceOfType(team, typeof(Team));
		}

		[TestMethod]
		public void InsertTeamsException()
		{
			int randVal;
			Random rand = new Random();
			randVal = rand.Next();

			PlayerDao pd = new PlayerDao(database);

			int i = 0;
			int player1 = 0;
			int player2 = 0;
			foreach (Player p in pd.FindAll())
			{
				++i;
				switch (i)
				{
					case 1:
						player1 = p.ID;
						break;
					case 2:
						player2 = p.ID;
						break;
				}
			}
			Assert.AreNotEqual(0, player1);
			Assert.AreNotEqual(0, player2);

			dao.Insert(new Team("Team" + randVal, player1, player2));

			// is -1 because Montag exists already
			Assert.AreEqual(dao.Insert(new Team("Team" + randVal, player1, player2)), -1);
		}

		[TestMethod]
		public void CheckInsert()
		{
			int firstState = dao.FindAll().Count;

			InsertTeams();

			int secondState = dao.FindAll().Count;

			Assert.IsTrue(firstState == (secondState - 1));
		}

		[TestMethod]
		public void CheckUpdate()
		{
			PlayerDao pd = new PlayerDao(database);

			int i = 0;
			int player1 = 0;
			int player2 = 0;
			int player3 = 0;
			foreach (Player p in pd.FindAll())
			{
				++i;
				switch (i)
				{
					case 1:
						player1 = p.ID;
						break;
					case 2:
						player2 = p.ID;
						break;
					case 3:
						player3 = p.ID;
						break;
				}
			}
			Random rand = new Random();
			int randVal = rand.Next();
			int res = dao.Insert(new Team("teamname" + randVal, player1, player2));
			Team t = dao.FindById(res);
			t.Player2ID = player3;
			dao.Update(t);

			Team t2 = dao.FindById(res);
			Assert.IsTrue(t2.Player1ID == t.Player1ID);
			Assert.IsTrue(t2.Player2ID == t.Player2ID);
		}

		[TestMethod]
		public void GetAllTeams()
		{
			TeamDao teamdao = new TeamDao(database);

			int stat1 = teamdao.FindAll().Count;
			InsertTeams();
			int stat2 = teamdao.FindAll().Count;

			Assert.AreEqual(stat1, stat2-1);
		}

		[TestMethod]
		public void CheckDeleteById()
		{
			int ret = InsertTeams("checkdeleteTest");

			Assert.IsNotNull(dao.FindById(ret));
			dao.DeleteById(ret);
			Assert.IsNull(dao.FindById(ret));
		}
	}
}
