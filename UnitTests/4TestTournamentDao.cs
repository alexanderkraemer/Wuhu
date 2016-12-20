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
	public class TestTournamentDao
	{
		private static IDatabase database;
		private static ITournamentDao dao;

		[ClassInitialize()]
		public static void Initialize(TestContext context)
		{
			database = DalFactory.CreateDatabase();

			dao = DalFactory.CreateTournamentDao(database);

			Assert.IsNotNull(database);
		}

		[TestMethod]
		public int InsertTournaments(string teamname = "Testteam")
		{
			Random rand = new Random();

			int res = dao.Insert(new Tournament(teamname + rand.Next() , RandomDay()));

			Assert.AreEqual(dao.FindById(res).ID, res);

			return res;
		}

		DateTime RandomDay()
		{
			Random gen = new Random();
			DateTime start = new DateTime(1995, 1, 1);
			int range = (DateTime.Today - start).Days;
			return start.AddDays(gen.Next(range));
		}

		[TestMethod]
		public void GetOneTeamByID()
		{
			int teamId = InsertTournaments("getOneTeamByIdTest");

			Tournament team = dao.FindById(teamId);

			Assert.IsNotNull(team);
			Assert.IsInstanceOfType(team, typeof(Tournament));
		}

		[TestMethod]
		public void InsertTournamentException()
		{
			Random rand = new Random();
			DateTime day = RandomDay();
			double val = rand.Next();
			int res = dao.Insert(new Tournament("Team" + val, day));

			// is -1 because Montag exists already
			Assert.AreEqual(dao.Insert(new Tournament("Team" + val, day)), -1);
		}

		[TestMethod]
		public void CheckInsert()
		{
			int firstState = dao.FindAll().Count;

			InsertTournaments();

			int secondState = dao.FindAll().Count;

			Assert.IsTrue(firstState == (secondState - 1));
		}

		[TestMethod]
		public void CheckUpdate()
		{
			Random rand = new Random();
			int randVal = rand.Next();
			DateTime day = RandomDay();
			int res = dao.Insert(new Tournament("teamname" + randVal, day));
			Tournament t = dao.FindById(res);
			t.Name = "new test name";
			dao.Update(t);

			Tournament t2 = dao.FindById(res);
			Assert.IsTrue(t2.Name == "new test name");
			Assert.IsTrue(t2.Timestamp == day);
		}

		[TestMethod]
		public void GetAllTeams()
		{
			int stat1 = dao.FindAll().Count;
			InsertTournaments();
			int stat2 = dao.FindAll().Count;

			Assert.AreEqual(stat1, stat2-1);
		}

		[TestMethod]
		public void CheckDeleteById()
		{
			int ret = InsertTournaments("checkdeleteTest");

			Assert.IsNotNull(dao.FindById(ret));
			dao.DeleteById(ret);
			Assert.IsNull(dao.FindById(ret));
		}
		/*
		[TestMethod]
		public void DeleteAll()
		{
			TeamDao dao = new TeamDao(database);
			dao.DeleteAll();

			Assert.AreEqual(0, dao.FindAll().Count);
		}
		*/
	}
}
