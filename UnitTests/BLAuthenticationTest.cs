using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WuHu.BusinessLogic;

namespace UnitTests
{
	[TestClass]
	public class BLAuthenticationTest
	{
		[TestMethod]
		public void Hash()
		{
			string ret = BLAuthentication.Hash("myPassword");
			Assert.AreNotSame(ret, "myPassword");

			this.Verify(ret);
		}

		public void Verify(string hash)
		{
			Assert.IsTrue(BLAuthentication.Verify("myPassword", hash));
		}
	}
}
