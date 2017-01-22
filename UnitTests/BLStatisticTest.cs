using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BusinessLogic;
using WuHu.SQLServer;
using WuHu.Common;

namespace UnitTests
{
	[TestClass]
	public class BLStatisticTest
	{
		[TestMethod]
		public void Insert()
		{
			IDatabase db = DalFactory.CreateDatabase();
			IPlayerDao dao = DalFactory.CreatePlayerDao(db);
			IStatisticDao sdao = DalFactory.CreateStatisticDao(db);

			var p = dao.FindByNickname("Alex");


			int a = sdao.FindAll().Count;
			BLStatistic.Insert(p.ID, 100);
			int b = sdao.FindAll().Count;
			Assert.IsTrue(a < b);
		}
	}
}
