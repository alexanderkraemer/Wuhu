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
		}

		[TestMethod]
		public void TestMatchContructor()
		{
			Match m = new Match(1, 2, 3, new DateTime(2016, 11, 23), 4, 5);

			Assert.AreEqual(1, m.ID);
			Assert.AreEqual(2, m.Team1ID);
			Assert.AreEqual(3, m.Team2ID);
			Assert.AreEqual(new DateTime(2016, 11, 23), m.Timestamp);
			Assert.AreEqual(4, m.ResultPointsPlayer1);
			Assert.AreEqual(5, m.ResultPointsPlayer2);
		}

		[TestMethod]
		public void InsertMatchs()
		{
			MatchDao MatchDao = new MatchDao(database);

			TeamDao teamdao = new TeamDao(database);
			Team t1 = null;
			Team t2 = null;
			int i = 0;
			foreach(Team t in teamdao.FindAll())
			{
				++i;
				switch(i)
				{
					case 1:
						t1 = t;
						break;
					case 2:
						t2 = t;
						break;
				}
			}

			int stat1 = MatchDao.FindAll().Count;
			MatchDao.Insert(new Match(t1.ID, t2.ID, new DateTime(2016, 11, 23)));
			int stat2 = MatchDao.FindAll().Count;
			Assert.IsTrue(stat1 == stat2-1);
		}

		[TestMethod]
		public void CheckDeleteById()
		{
			MatchDao MatchDao = new MatchDao(database);
			TeamDao teamdao = new TeamDao(database);
			Team t1 = null;
			Team t2 = null;
			int i = 0;
			foreach (Team t in teamdao.FindAll())
			{
				++i;
				switch (i)
				{
					case 1:
						t1 = t;
						break;
					case 2:
						t2 = t;
						break;
				}
			}

			int id = MatchDao.Insert(new Match(t1.ID, t2.ID, new DateTime(2016, 11, 27)));

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
			TeamDao teamdao = new TeamDao(database);
			Team t1 = null;
			Team t2 = null;
			int i = 0;
			foreach (Team t in teamdao.FindAll())
			{
				++i;
				switch (i)
				{
					case 1:
						t1 = t;
						break;
					case 2:
						t2 = t;
						break;
				}
			}

			int retValue = MatchDao.Insert(new Match(t1.ID, t2.ID, new DateTime(2016, 11, 23)));

			Match match = MatchDao.FindById(retValue);

			Assert.AreEqual(match.Team1ID, t1.ID);
			Assert.AreEqual(match.Team2ID, t2.ID);
			Assert.AreEqual(match.Timestamp, new DateTime(2016, 11, 23));
		}

		[TestMethod]
		public void GetOneDayByIDError()
		{
			MatchDao MatchDao = new MatchDao(database);
			MatchDao.DeleteAll();

			Assert.IsNull(MatchDao.FindById(1));
		}

		[TestMethod]
		public void DeleteDaysByIdError()
		{
			MatchDao MatchDao = new MatchDao(database);


			Assert.AreEqual(MatchDao.DeleteById(1), false);
		}
	}
}
