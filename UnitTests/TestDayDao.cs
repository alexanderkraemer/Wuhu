using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.SQLServer;
using WuHu.Domain;
using System.Configuration;
using WuHu.Common;
using System.Collections.Generic;

namespace WuHu.UnitTests
{
    [TestClass]
    public class TestDayDao
    {
        private static IDatabase database;
        private static IDayDao dao;

        [ClassInitialize()]
        public static void Initialize(TestContext context)
        {
            database = DalFactory.CreateDatabase();

            dao = DalFactory.CreateDayDao(database);

            Assert.IsNotNull(database);
        }

        [TestInitialize]
        public void Initialize()
        {
            dao.DeleteAll();
        }

        [TestMethod]
        public void InsertDays()
        {
            dao.Insert(new Day("Montag"));
            dao.Insert(new Day("Dienstag"));
            dao.Insert(new Day("Mittwoch"));
            dao.Insert(new Day("Donnerstag"));
            dao.Insert(new Day("Freitag"));
            dao.Insert(new Day("Samstag"));


            Assert.AreEqual(dao.FindAll().Count, 6);
        }

        [TestMethod]
        public void CheckIfTestDataIsValid()
        {
            InsertDays();

            Assert.AreEqual(dao.FindAll().Count, 6);
            int i = 0;
            foreach (Day day in dao.FindAll())
            {
                ++i;
                switch (i)
                {
                    case 1:
                        Assert.AreEqual(day.Name, "Montag");
                        break;
                    case 2:
                        Assert.AreEqual(day.Name, "Dienstag");
                        break;
                    case 3:
                        Assert.AreEqual(day.Name, "Mittwoch");
                        break;
                    case 4:
                        Assert.AreEqual(day.Name, "Donnerstag");
                        break;
                    case 5:
                        Assert.AreEqual(day.Name, "Freitag");
                        break;
                    case 6:
                        Assert.AreEqual(day.Name, "Samstag");
                        break;
                }
            }
        }

        [TestMethod]
        public void InsertDaysException()
        {
            dao.Insert(new Day("Montag"));

            // is -1 because Montag exists already
            Assert.AreEqual(dao.Insert(new Day("Montag")), -1);
        }

        [TestMethod]
        public void CheckInsert()
        {
            Day montag = new Day("Montag");
            int ret = dao.Insert(montag);

            Assert.IsInstanceOfType(dao.FindById(ret), typeof(Day));
        }

        [TestMethod]
        public void CheckUpdate()
        {
            CheckInsert();
            Day d = dao.FindByDayname("Montag");

            d.Name = "Dienstag";

            dao.Update(d);

            Day d1 = dao.FindByDayname("Dienstag");

            Assert.AreEqual(d1.Name, "Dienstag");
        }

        [TestMethod]
        public void CheckDeleteById()
        {
            foreach (Day day in dao.FindAll())
            {
                dao.DeleteById(day.ID);
            }


            Assert.AreEqual(dao.FindAll().Count, 0);
        }

        [TestMethod]
        public void CheckDeleteAll()
        {
            InsertDays();
            Assert.AreEqual(dao.FindAll().Count, 6);
            dao.DeleteAll();

            Assert.AreEqual(dao.FindAll().Count, 0);
        }

        [TestMethod]
        public void GetAllDays()
        {
            InsertDays();

            IList<Day> allDays = dao.FindAll();

            Assert.AreEqual(allDays.Count, 6);

        }

        [TestMethod]
        public void GetOneDayByID()
        {
            int retValue = dao.Insert(new Day("Dienstag"));

            Day dienstag = dao.FindById(retValue);

            Assert.AreEqual(dienstag.Name, "Dienstag");
        }


        [TestMethod]
        public void GetOneDayByIDError()
        {
            Assert.IsNull(dao.FindById(1));
            Assert.IsNull(dao.FindByDayname("Dienstag"));
        }

        [TestMethod]
        public void DeleteDaysByIdError()
        {
            Assert.AreEqual(dao.DeleteById(1), false);
        }
    }
}
