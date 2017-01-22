using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Domain;
using WuHu.Common;
using WuHu.SQLServer;
using System.Configuration;

namespace Wuhu.UnitTests
{
	// passed
	[TestClass]
	public class TestStatisticsDao
	{

		private static IDatabase database;
		private static IStatisticDao dao;

		[ClassInitialize()]
		public static void Initialize(TestContext context)
		{
			database = DalFactory.CreateDatabase();

			dao = DalFactory.CreateStatisticDao(database);

			Assert.IsNotNull(database);
		}

		[TestMethod]
		public void InsertStatistic()
		{
			PlayerDao pd = new PlayerDao(database);

			int player1 = 0;
			foreach (Player p in pd.FindAll())
			{
				player1 = p.ID;
				break;
			}
			Statistic s = new Statistic(player1, 10, new DateTime(2016, 11, 23));

			int ret = dao.Insert(s);

			Assert.IsNotNull(dao.FindById(ret));
		}

		[TestMethod]
		public void DeleteById()
		{
			PlayerDao pd = new PlayerDao(database);

			int player1 = 0;
			foreach (Player p in pd.FindAll())
			{
				player1 = p.ID;
				break;
			}

			int ret = dao.Insert(new Statistic(player1, 10, new DateTime(2016, 11, 23)));
			Assert.IsNotNull(dao.FindById(ret));

			dao.DeleteById(ret);
			Assert.IsNull(dao.FindById(ret));
		}
		/*
		[TestMethod]
		public void DeleteAll()
		{
			StatisticDao sd = new StatisticDao(database);
			sd.DeleteAll();

			Assert.AreEqual(0, sd.FindAll().Count);
		}
		*/
		[TestMethod]
		public void FindAll()
		{
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

			int stat1 = dao.FindAll().Count;

			dao.Insert(new Statistic(player1, 10, new DateTime(2016, 11, 23)));
			dao.Insert(new Statistic(player2, 12, new DateTime(2016, 11, 23)));

			int stat2 = dao.FindAll().Count;

			Assert.IsTrue(stat1 == (stat2 - 2));
		}


		[TestMethod]
		public void FindByDay()
		{
			PlayerDao pd = new PlayerDao(database);

			int player1 = 0;
			foreach (Player p in pd.FindAll())
			{
				player1 = p.ID;
				break;
			}

			dao.Insert(new Statistic(player1, 10, new DateTime(2016, 11, 23)));

			IList<Statistic> list = dao.FindByDay(new DateTime(2016, 11, 23));

			foreach(Statistic s in list)
			{
				Assert.AreEqual(s.Timestamp, new DateTime(2016, 11, 23));
			}
		}

		[TestMethod]
		public void FindById()
		{
			PlayerDao pd = new PlayerDao(database);

			int player1 = 0;
			foreach (Player p in pd.FindAll())
			{
				player1 = p.ID;
				break;
			}

			int ret = dao.Insert(new Statistic(player1, 10, new DateTime(2016, 11, 23)));
			Assert.AreEqual(new DateTime(2016, 11, 23), dao.FindById(ret).Timestamp);
		}

		[TestMethod]
		public void FindByPlayer()
		{
			PlayerDao pd = new PlayerDao(database);

			int player1 = 0;
			foreach (Player p in pd.FindAll())
			{
				player1 = p.ID;
				break;
			}
			IList<Statistic> retList = dao.FindByPlayer(player1);

			int stat1 = retList.Count;
			int ret = dao.Insert(new Statistic(player1, 10, new DateTime(2016, 11, 23)));

			retList = dao.FindByPlayer(player1);
			int stat2 = retList.Count;

			Assert.IsTrue(stat1 == stat2 - 1);

		}

		[TestMethod]
		public void Update()
		{
			PlayerDao pd = new PlayerDao(database);

			int player1 = 0;
			foreach (Player p in pd.FindAll())
			{
				player1 = p.ID;
				break;
			}
			int ret = dao.Insert(new Statistic(player1, 10, new DateTime(2016, 11, 23)));

			Statistic oldStat = dao.FindById(ret);
			oldStat.Skill = 50;
			dao.Update(oldStat);

			Assert.AreEqual(50, dao.FindById(ret).Skill);

		}
	}
}
