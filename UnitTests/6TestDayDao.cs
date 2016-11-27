using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.SQLServer;
using WuHu.Domain;
using System.Configuration;
using WuHu.Common;
using System.Collections.Generic;

namespace WuHu.UnitTests
{
	// passed
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

		[TestMethod]
		public int InsertDay(string dayname = "testday", bool WithRandom = true)
		{
			Random rand = new Random();
			int res;
			if (WithRandom)
			{
				res = dao.Insert(new Day(dayname + rand.Next()));
			}
			else
			{
				res = dao.Insert(new Day(dayname));
			}
			
			Assert.IsNotNull(dao.FindById(res));
			Assert.AreEqual(dao.FindById(res).ID, res);
			Assert.IsInstanceOfType(dao.FindById(res), typeof(Day));
			return res;
		}

		[TestMethod]
		public void InsertDaysException()
		{
			dao.Insert(new Day("testdayInsertException"));

			// is -1 because Montag exists already from last testMethod
			Assert.AreEqual(dao.Insert(new Day("testdayInsertException")), -1);
		}

		[TestMethod]
		public void CheckUpdate()
		{
			Random rand = new Random();
			int val = rand.Next();
			dao.Insert(new Day("testday5" + val));
			Day d = dao.FindByDayname("testday5" + val);

			d.Name = "testday5Updated" + val;

			dao.Update(d);

			Day d1 = dao.FindByDayname("testday5Updated" + val);

			Assert.AreEqual(d1.Name, "testday5Updated" + val);
		}

		[TestMethod]
		public void CheckDeleteById()
		{
			int ret = dao.Insert(new Day("testday 3"));
			dao.DeleteById(ret);

			Assert.IsNull(dao.FindById(ret));
		}
		/*
		[TestMethod]
		public void DeleteAll()
		{
			DayDao dao = new DayDao(database);
			dao.DeleteAll();

			Assert.AreEqual(0, dao.FindAll().Count);
		}
		*/
		[TestMethod]
		public void GetAllDays()
		{
			Random rand = new Random();
			int result1 = dao.FindAll().Count;
			dao.Insert(new Day("testda4" + rand.Next()));
			int result2 = dao.FindAll().Count;

			Assert.IsTrue(result1 == result2-1);
		}

		[TestMethod]
		public void GetOneDayByID()
		{
			Random rand = new Random();
			int val = rand.Next();
			int retValue = dao.Insert(new Day("testday6" + val));

			Day dienstag = dao.FindById(retValue);

			Assert.AreEqual(dienstag.Name, "testday6" + val);
		}


		[TestMethod]
		public void GetOneDayByIDError()
		{
			int res = dao.Insert(new Day("testday 6"));
			Assert.IsNotNull(dao.FindById(res));
			dao.DeleteById(res);
			Assert.IsNull(dao.FindById(res));
		}

		[TestMethod]
		public void DeleteDaysByIdError()
		{
			Random rand = new Random();
			int val = rand.Next();
			int res = dao.Insert(new Day("testday 7" + val));
			Assert.IsNotNull(dao.FindById(res));
			dao.DeleteById(res);

			Assert.IsFalse(dao.DeleteById(res));
		}
	}
}
