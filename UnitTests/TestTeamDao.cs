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

        [TestInitialize]
        public void Initialize()
        {
            dao.DeleteAll();
        }

        [TestMethod]
        public void InsertTeams()
        {
            dao.Insert(new Team("Team 1", 1111, 1112));
            dao.Insert(new Team("Team 2", 1113, 1114));

            Assert.AreEqual(dao.FindAll().Count, 2);
        }

        [TestMethod]
        public void InsertTeamsException()
        {
            dao.Insert(new Team("Team 1", 1111, 1112));
            dao.Insert(new Team("Team 2", 1113, 1114));

            // is -1 because Montag exists already
            Assert.AreEqual(dao.Insert(new Team("Team 1", 1115, 1116)), -1);
        }

        [TestMethod]
        public void CheckInsert()
        {
            int ret = dao.Insert(new Team("Team 1", 1111, 1112));
            
            Assert.IsInstanceOfType(dao.FindById(ret), typeof(Team));
        }

        [TestMethod]
        public void CheckDeleteById()
        {
            foreach (Team team in dao.FindAll())
            {
                dao.DeleteById(team.ID);
            }

            Assert.AreEqual(dao.FindAll().Count, 0);
        }

        [TestMethod]
        public void CheckDeleteAll()
        {
            dao.Insert(new Team("Team 1", 1111, 1112));
            Assert.AreEqual(dao.FindAll().Count, 1);

            dao.DeleteAll();
            Assert.AreEqual(dao.FindAll().Count, 0);
        }

        [TestMethod]
        public void GetAllTeams()
        {
            InsertTeams();

            IList<Team> allTeams = dao.FindAll();

            Assert.AreEqual(allTeams.Count, 2);
        }

        [TestMethod]
        public void GetOneTeamByID()
        {
            int retValue = dao.Insert(new Team("Team", 1111, 1112));

            Team amdin = dao.FindById(retValue);

            Assert.AreEqual(amdin.Name, "Team");
        }
    }
}
