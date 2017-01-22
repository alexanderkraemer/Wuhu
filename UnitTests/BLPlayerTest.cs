using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BusinessLogic;
using WuHu.Domain;
using WuHu.SQLServer;
using WuHu.Common;
using System.Collections;
using System.Collections.Generic;

namespace UnitTests
{
	[TestClass]
	public class BLPlayerTest
	{
		[TestMethod]
		public void Authenticate()
		{
			Assert.IsTrue(BLPlayer.Authenticate(new AuthObj("Alex", "passalex")));
		}

		[TestMethod]
		public void Decay()
		{
			IDatabase database = DalFactory.CreateDatabase();
			IPlayerDao dao = DalFactory.CreatePlayerDao(database);

			Player p = dao.FindByNickname("Alex");

			BLPlayer.Decay();

			Player p1 = dao.FindByNickname("Alex");
			
			Assert.IsFalse(p1.Skills < p.Skills);
		}

		[TestMethod]
		public void GetPlayerByDay()
		{
			IDatabase database = DalFactory.CreateDatabase();
			IPlayerDao dao = DalFactory.CreatePlayerDao(database);



			for (int i = 0; i < 6; i++)
			{
				IEnumerable<Player> p = BLPlayer.GetPlayerByDay(DateTime.Now.Date.AddDays(i), dao.FindAll());

				foreach (Player pl in p)
				{
					switch (DateTime.Now.DayOfWeek)
					{
						case DayOfWeek.Monday:
							Assert.IsTrue(pl.isMonday);
							break;
						case DayOfWeek.Tuesday:
							Assert.IsTrue(pl.isTuesday);
							break;
						case DayOfWeek.Wednesday:
							Assert.IsTrue(pl.isWednesday);
							break;
						case DayOfWeek.Thursday:
							Assert.IsTrue(pl.isThursday);
							break;
						case DayOfWeek.Friday:
							Assert.IsTrue(pl.isFriday);
							break;
						case DayOfWeek.Saturday:
							Assert.IsTrue(pl.isSaturday);
							break;
					}
				}
			}
		}
		[TestMethod]
		public void UpdateElo()
		{
			IDatabase database = DalFactory.CreateDatabase();
			IPlayerDao dao = DalFactory.CreatePlayerDao(database);

			var plist = dao.FindAll();
			Player p1 = new Player();
			Player p2 = new Player();
			Player p3 = new Player();
			Player p4 = new Player();
			int i = 0;

			Assert.IsTrue(plist.Count >= 4);
			foreach(Player pl in plist)
			{
				switch(i)
				{
					case 1:
					p1 = pl;
						break;
					case 2:
						p2 = pl;
						break;
					case 3:
						p3 = pl;
						break;
					case 4:
						p4 = pl;
						break;
				}
				i++;
			}

			int p1S = p1.Skills;
			int p2S = p2.Skills;
			int p3S = p3.Skills;
			int p4S = p4.Skills;

			BLPlayer.UpdateElo(p1, p2, p3, p4);

			Assert.IsTrue(p1.Skills > p1S);
			Assert.IsTrue(p2.Skills > p2S);
			Assert.IsTrue(p3.Skills < p3S);
			Assert.IsTrue(p4.Skills < p4S);
		}

	}
}
