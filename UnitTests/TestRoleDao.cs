using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using WuHu.SQLServer;
using WuHu.Common;
using WuHu.Domain;
using System.Text;
using System.Collections.Generic;

namespace WuHu.UnitTests
{
	// passed
	[TestClass]
	public class TestRoleDao
	{
		private static IDatabase database;
		private static IRoleDao dao;

		[ClassInitialize()]
		public static void Initialize(TestContext context)
		{
			database = DalFactory.CreateDatabase();

			dao = DalFactory.CreateRoleDao(database);

			Assert.IsNotNull(database);
		}

		[TestInitialize]
		public void Initialize()
		{
			// dao.DeleteAll();
		}

		[TestMethod]
		public int InsertRoles(string rolename = "TestRole", bool WithRand = true)
		{
			Random rand = new Random();
			int res1 = dao.FindAll().Count;
			Role role;
			if (WithRand)
			{
				role = new Role(rolename + rand.Next());
			}
			else
			{
				role = new Role(rolename);
			}


			int ret = dao.Insert(role);
			int res2 = dao.FindAll().Count;
			
			Assert.IsTrue(res1 == res2-1);

			return ret;
		}

		[TestMethod]
		public void CheckInsert()
		{
			Role admin = new Role("Testrole 1");
			int ret = dao.Insert(admin);

			Assert.IsInstanceOfType(dao.FindById(ret), typeof(Role));
		}

		[TestMethod]
		public void CheckDeleteById()
		{
			int res = InsertRoles();
			dao.DeleteById(res);
			
			Assert.IsNull(dao.FindById(res));
		}

		[TestMethod]
		public void GetAllRoles()
		{
			int first = dao.FindAll().Count;
			InsertRoles();
			int second = dao.FindAll().Count;

			Assert.IsTrue(first == second-1);
		}

		[TestMethod]
		public void GetOneRoleByID()
		{
			int retValue = dao.Insert(new Role("TestGetRoleById"));

			Role amdin = dao.FindById(retValue);

			Assert.AreEqual(amdin.Name, "TestGetRoleById");
		}

		[TestMethod]
		public void GetOneRoleByName()
		{
			int retValue = dao.Insert(new Role("TestGetRoleByName"));

			Role role = dao.FindByRole("TestGetRoleByName");

			Assert.AreEqual(retValue, role.ID);
		}

		[TestMethod]
		public void UpdateRole()
		{
			int retValue = dao.Insert(new Role("TestUpdateRole"));

			Role ret = dao.FindById(retValue);

			ret.Name = "TestUpdateRoleUpdate";

			dao.Update(ret);


			Assert.AreEqual(dao.FindById(ret.ID).Name, "TestUpdateRoleUpdate");
		}

		[TestMethod]
		public void GetOneDayByIDError()
		{
			int retValue = dao.Insert(new Role("TestFindByIDError"));

			Assert.IsNotNull(dao.FindById(retValue));
			dao.DeleteById(retValue);
			Assert.IsNull(dao.FindById(retValue));
		}
	}
}
