using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Common;
using System.Configuration;
using WuHu.SQLServer;
using WuHu.Domain;
using System.Collections.Generic;
using WuHu.InitDatabase;

namespace WuHu.UnitTests
{
	// passed
	[TestClass]
	public class TestMatchDao
	{
		private static IDatabase database;
		private static IMatchDao dao;

		[ClassInitialize()]
		public static void Initialize(TestContext context)
		{
			database = DalFactory.CreateDatabase();

			dao = DalFactory.CreateMatchDao(database);

			Assert.IsNotNull(database);

			//GenerateTestData.Data.Clear(database);
			//GenerateTestData.Data.Insert(database);
		}

		public void InitDB()
		{
			DatabaseReset.Clear(database);
			DatabaseReset.Insert(database);
		}

		[TestMethod]
		public void TestMatchContructor()
		{
			TestPlayerDao pdao = new TestPlayerDao();
			TournamentDao td = new TournamentDao(database);
			var list = td.FindAll();
			int tId = list[0].ID;

			PlayerDao pd = new PlayerDao(database);
			var plist = pd.FindAll();

			int p1id = plist[0].ID;
			int p2id = plist[1].ID;
			int p3id = plist[2].ID;
			int p4id = plist[3].ID;

			Match m = new Match(p1id, p2id, p3id, p4id, tId);

			Assert.AreEqual(p1id, m.Team1Player1);
			Assert.AreEqual(p2id, m.Team1Player2);
			Assert.AreEqual(p3id, m.Team2Player1);
			Assert.AreEqual(p4id, m.Team2Player2);
			Assert.AreEqual(tId, m.TournamentId);
			Assert.IsNull(m.ResultPointsPlayer1);
			Assert.IsNull(m.ResultPointsPlayer2);
		}

		[TestMethod]
		public void InsertMatchs()
		{
			MatchDao MatchDao = new MatchDao(database);

			TestPlayerDao pdao = new TestPlayerDao();
			TournamentDao td = new TournamentDao(database);
			var list = td.FindAll();
			int tId = list[0].ID;

			PlayerDao pd = new PlayerDao(database);
			var plist = pd.FindAll();

			int p1id = plist[0].ID;
			int p2id = plist[1].ID;
			int p3id = plist[2].ID;
			int p4id = plist[3].ID;

			Match m = new Match(p1id, p2id, p3id, p4id, tId);


			int stat1 = MatchDao.FindAll().Count;
			MatchDao.Insert(m);
			int stat2 = MatchDao.FindAll().Count;
			Assert.IsTrue(stat1 == stat2-1);
		}

		[TestMethod]
		public void CheckDeleteById()
		{
			MatchDao MatchDao = new MatchDao(database);
			TestPlayerDao pdao = new TestPlayerDao();
			TournamentDao td = new TournamentDao(database);
			var list = td.FindAll();
			int tId = list[0].ID;

			PlayerDao pd = new PlayerDao(database);
			var plist = pd.FindAll();

			int p1id = plist[0].ID;
			int p2id = plist[1].ID;
			int p3id = plist[2].ID;
			int p4id = plist[3].ID;

			Match m = new Match(p1id, p2id, p3id, p4id, tId);

			int id = MatchDao.Insert(m);

			int stat1 = MatchDao.FindAll().Count;
			MatchDao.DeleteById(id);
			int stat2 = MatchDao.FindAll().Count;


			Assert.AreEqual(stat1, stat2+1);
		}
		/*
		[TestMethod]
		public void DeleteAll()
		{
			MatchDao dao = new MatchDao(database);
			dao.DeleteAll();

			Assert.AreEqual(0, dao.FindAll().Count);
		}
		*/
		[TestMethod]
		public void GetAllMatchs()
		{
			MatchDao MatchDao = new MatchDao(database);

			int stat1 = MatchDao.FindAll().Count;
			InsertMatchs();
			int stat2 = MatchDao.FindAll().Count;

			Assert.AreEqual(stat1, stat2-1);
		}

		[TestMethod]
		public void GetOneMatchByID()
		{
			MatchDao MatchDao = new MatchDao(database);
			TestPlayerDao pdao = new TestPlayerDao();
			TournamentDao td = new TournamentDao(database);
			var list = td.FindAll();
			int tId = list[0].ID;

			PlayerDao pd = new PlayerDao(database);
			var plist = pd.FindAll();

			int p1id = plist[0].ID;
			int p2id = plist[1].ID;
			int p3id = plist[2].ID;
			int p4id = plist[3].ID;

			Match m = new Match(p1id, p2id, p3id, p4id, tId);

			int retValue = MatchDao.Insert(m);

			Match match = MatchDao.FindById(retValue);

			Assert.AreEqual(match.Team1Player1, p1id);
			Assert.AreEqual(match.Team1Player2, p2id);
			Assert.AreEqual(match.Team2Player1, p3id);
			Assert.AreEqual(match.Team2Player2, p4id);
			Assert.AreEqual(match.TournamentId, tId);
			Assert.IsNull(m.ResultPointsPlayer1);
			Assert.IsNull(m.ResultPointsPlayer2);
		}

		

		[TestMethod]
		public void DeleteDaysByIdError()
		{
			MatchDao MatchDao = new MatchDao(database);
			
			Assert.AreEqual(MatchDao.DeleteById(1), false);
		}

		[TestMethod]
		public void GetOneDayByIDError()
		{
			MatchDao MatchDao = new MatchDao(database);
			MatchDao.DeleteAll();

			Assert.IsNull(MatchDao.FindById(1));
		}
	}
}
