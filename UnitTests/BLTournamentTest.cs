using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BusinessLogic;

namespace UnitTests
{
	[TestClass]
	public class BLTournamentTest
	{
		[TestMethod]
		public void getLocked()
		{

			BLTournaments.IsLocked = true;
			Assert.IsTrue(BLTournaments.IsLocked);


			BLTournaments.IsLocked = false;
			Assert.IsFalse(BLTournaments.IsLocked);

		}
	}
}
