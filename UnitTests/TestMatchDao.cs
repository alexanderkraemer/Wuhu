using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.Common;
using System.Configuration;
using WuHu.SQLServer;
using WuHu.Domain;
using System.Collections.Generic;

namespace WuHu.UnitTests
{
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

        [TestInitialize]
        public void Initialize()
        {
            dao.DeleteAll();
        }

        [TestMethod]
        public void InsertMatchs()
        {
            // delete Days, in case database was not empty
            CheckDeleteById();

            MatchDao MatchDao = new MatchDao(database);

            MatchDao.Insert(new Match(15, 16, new DateTime(2016, 11, 23)));
            
            Assert.AreEqual(MatchDao.FindAll().Count, 1);
        }

        [TestMethod]
        public void CheckInsert()
        {
            CheckDeleteAll();
            MatchDao MatchDao = new MatchDao(database);
          
            int ret = MatchDao.Insert(new Match(15, 16, new DateTime(2016, 11, 23)));

            Assert.IsInstanceOfType(MatchDao.FindById(ret), typeof(Match));
        }

        [TestMethod]
        public void CheckDeleteById()
        {
            MatchDao MatchDao = new MatchDao(database);

            foreach (Match Match in MatchDao.FindAll())
            {
                MatchDao.DeleteById(Match.ID);
            }


            Assert.AreEqual(MatchDao.FindAll().Count, 0);
        }

        [TestMethod]
        public void CheckDeleteAll()
        {
            MatchDao MatchDao = new MatchDao(database);
            MatchDao.DeleteAll();

            Assert.AreEqual(MatchDao.FindAll().Count, 0);
        }

        [TestMethod]
        public void GetAllMatchs()
        {
            InsertMatchs();

            MatchDao MatchDao = new MatchDao(database);
            IList<Match> allMatchs = MatchDao.FindAll();

            Assert.AreEqual(allMatchs.Count, 1);
        }

        [TestMethod]
        public void GetOneMatchByID()
        {
            MatchDao MatchDao = new MatchDao(database);
            MatchDao.DeleteAll();

            int retValue = MatchDao.Insert(new Match(15, 16, new DateTime(2016, 11, 23)));

            Match match = MatchDao.FindById(retValue);

            Assert.AreEqual(match.Team1ID, 15);
            Assert.AreEqual(match.Team2ID, 16);
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
