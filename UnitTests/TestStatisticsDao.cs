using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Domain;
using WuHu.Common;
using WuHu.SQLServer;
using System.Configuration;

namespace Wuhu.UnitTests
{
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

        [TestInitialize]
        public void Initialize()
        {
            dao.DeleteAll();
        }

        [TestMethod]
        public void InsertTestdata()
        {
            int player_id = 1111;
            Statistic s = new Statistic(player_id, 10, new DateTime(2016, 11, 23));

            dao.Insert(s);

            Assert.AreEqual(1, dao.FindAll().Count);
        }

        [TestMethod] 
        public void TestDeleteAll()
        {
            int player_id = 1111;
            dao.Insert(new Statistic(player_id, 10, new DateTime(2016, 11, 23)));
            Assert.AreEqual(1, dao.FindAll().Count);

            dao.DeleteAll();

            Assert.AreEqual(0, dao.FindAll().Count);
        }

        [TestMethod]
        public void DeleteById()
        {
            int player_id = 1111;
            int ret = dao.Insert(new Statistic(player_id, 10, new DateTime(2016, 11, 23)));
            Assert.AreEqual(1, dao.FindAll().Count);

            dao.DeleteById(ret);
            Assert.AreEqual(0, dao.FindAll().Count);
        }

        [TestMethod]
        public void FindAll()
        {
            dao.Insert(new Statistic(1111, 10, new DateTime(2016, 11, 23)));
            dao.Insert(new Statistic(1112, 10, new DateTime(2016, 11, 23)));
            dao.Insert(new Statistic(1113, 10, new DateTime(2016, 11, 23)));

            Assert.AreEqual(3, dao.FindAll().Count);
        }


        [TestMethod]
        public void FindByDay()
        {
            dao.Insert(new Statistic(1111, 10, new DateTime(2016, 11, 23)));
            dao.Insert(new Statistic(1112, 10, new DateTime(2016, 11, 24)));
            dao.Insert(new Statistic(1113, 10, new DateTime(2016, 11, 25)));

            Assert.AreEqual(1112, dao.FindByDay(new DateTime(2016, 11, 24)).PlayerID);
        }

        [TestMethod]
        public void FindById()
        {
            int player_id = 1111;
            int ret = dao.Insert(new Statistic(player_id, 10, new DateTime(2016, 11, 23)));
            Assert.AreEqual(new DateTime(2016, 11, 23), dao.FindById(ret).Timestamp);
        }

        [TestMethod]
        public void FindByPlayer()
        {
            int player_id = 1111;
            int ret = dao.Insert(new Statistic(player_id, 10, new DateTime(2016, 11, 23)));

            Assert.AreEqual(new DateTime(2016, 11, 23), dao.FindByPlayer(player_id).Timestamp);
        }

        [TestMethod]
        public void Update()
        {
            int player_id = 1111;
            int ret = dao.Insert(new Statistic(player_id, 10, new DateTime(2016, 11, 23)));

            Statistic oldStat = dao.FindById(ret);
            oldStat.Skill = 50;
            dao.Update(oldStat);

            Assert.AreEqual(50, dao.FindById(ret).Skill);

        }
    }
}
