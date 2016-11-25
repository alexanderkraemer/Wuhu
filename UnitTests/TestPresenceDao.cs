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

        [TestInitialize]
        public void Initialize()
        {
            dao.DeleteAll();
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

        [TestMethod]
        public void CheckInsert()
        {
            TestDayDao testDay = new TestDayDao();
            testDay.InsertDays();

            TestPlayerDao testPlayer = new TestPlayerDao();
            testPlayer.InsertTestPlayers();


            DayDao dayDao = new DayDao(database);
            int dayId = dayDao.Insert(new Day("Montag"));


            PlayerDao playerDao = new PlayerDao(database);

            int playerId = 0;
            foreach(Player player in playerDao.FindAll())
            {
                playerId = player.ID;
                break;
            }

            Assert.AreNotEqual(0, playerId);

            Presence p = new Presence(playerId, dayId);
            int ret = dao.Insert(p);

            Assert.IsInstanceOfType(dao.FindById(playerId, dayId), typeof(Presence));
        }

        [TestMethod]
        public void CheckUpdate()
        {
            TestDayDao testDay = new TestDayDao();
            testDay.InsertDays();

            TestPlayerDao testPlayer = new TestPlayerDao();
            testPlayer.InsertTestPlayers();


            DayDao dayDao = new DayDao(database);
            int dayId = dayDao.FindByDayname("Montag").ID;

            PlayerDao playerDao = new PlayerDao(database);

            int playerId = 0;
            int newPlayerId = 0;
            int i = 0;
            foreach (Player player in playerDao.FindAll())
            {
                ++i;
                if(i == 1)
                {
                    playerId = player.ID;
                }
                else
                {
                    newPlayerId = player.ID;
                    break;
                }
            }

            Assert.AreNotEqual(0, playerId);
            Assert.AreNotEqual(0, newPlayerId);

            Presence p = dao.FindById(playerId, dayId);

            Presence p_new = new Presence(newPlayerId, dayId);

            dao.Update(p, p_new);

            Presence d1 = dao.FindById(newPlayerId, dayId);

            Assert.AreEqual(d1.PlayerID, 1112);
            Assert.AreEqual(dao.FindAll().Count, 1);
        }

        [TestMethod]
        public void CheckDeleteAll()
        {
            InsertPresence();
            dao.DeleteAll();

            Assert.AreEqual(dao.FindAll().Count, 0);
        }

        [TestMethod]
        public void CheckDeleteById()
        {
            InsertPresence();
            foreach (Presence presence in dao.FindAll())
            {
                dao.DeleteById(presence.PlayerID, presence.DayID);
            }
            Assert.AreEqual(dao.FindAll().Count, 0);
        }

        [TestMethod]
        public void GetAllPresence()
        {
            TestDayDao testDay = new TestDayDao();
            testDay.InsertDays();

            TestPlayerDao testPlayer = new TestPlayerDao();
            testPlayer.InsertTestPlayers();

            InsertPresence();

            IList<Presence> allPresence = dao.FindAll();

            Assert.AreEqual(allPresence.Count, 180);
        }

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
