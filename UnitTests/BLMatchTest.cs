using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BusinessLogic;
using WuHu.SQLServer;
using WuHu.Common;
using WuHu.Domain;
using WuHu.UnitTests;
using System.Collections.ObjectModel;

namespace UnitTests
{
	[TestClass]
	public class BLMatchTest
	{
		private static IDatabase database;
		private static IMatchDao dao;
		[ClassInitialize()]
		public static void Initialize(TestContext context)
		{
			database = DalFactory.CreateDatabase();

			dao = DalFactory.CreateMatchDao(database);

			Assert.IsNotNull(database);

			//GenerateTestData.Data.Clear(database);
			//GenerateTestData.Data.Insert(database);
		}
		[TestMethod]
		public void InsertMatchs()
		{
			TournamentDao tdao = new TournamentDao(database);

			int t = tdao.Insert(new Tournament("name", DateTime.Now));

			PlayerDao pd = new PlayerDao(database);
			var plist = pd.FindAll();

			int p1id = plist[0].ID;
			int p2id = plist[1].ID;
			int p3id = plist[2].ID;
			int p4id = plist[3].ID;

			Match m = new Match(p1id, p2id, p3id, p4id, t, false);


			int stat1 = dao.FindAll().Count;
			dao.Insert(m);
			int stat2 = dao.FindAll().Count;
			Assert.IsTrue(stat1 == stat2 - 1);
		}

		[TestMethod]
		public void IncrementValue()
		{
			TournamentDao td = new TournamentDao(database);
			var list = td.FindAll();
			int tId = list[0].ID;
			Assert.IsNotNull(tId);
			
			PlayerDao pd = new PlayerDao(database);
			var plist = pd.FindAll();

			int p1id = plist[0].ID;
			int p2id = plist[1].ID;
			int p3id = plist[2].ID;
			int p4id = plist[3].ID;

			Match m = new Match(p1id, p2id, p3id, p4id, tId, 1, 1, false);
			
			int i = dao.Insert(m);
			

			SocketObj obj = new SocketObj();
			obj.match = i;
			obj.team = 1;

			int? i1 = m.ResultPointsPlayer1;
			
			BLMatch.IncrementValue(obj);

			Match m1 = dao.FindById(i);
			int? i2 = m1.ResultPointsPlayer1;
			
			Assert.AreEqual(i1, i2-1);
			
		}

		[TestMethod]
		public void GenerateMatches()
		{
			var player = DalFactory.CreatePlayerDao(DalFactory.CreateDatabase()).FindAll();

			var PlayerList = new ObservableCollection<Player>();
			foreach(Player p in player)
			{
				PlayerList.Add(p);
			}
			var matches = BLMatch.GenerateMatches(3, PlayerList, 2);
			Assert.AreEqual(3, matches.Count);

			var l = BLMatch.insertMatches(matches);
			Assert.IsFalse(l);
		}
	}
}
