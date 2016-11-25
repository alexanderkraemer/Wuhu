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
        public void InsertRoles()
        {
            dao.Insert(new Role("Admin"));
            dao.Insert(new Role("Member"));

            Assert.AreEqual(dao.FindAll().Count, 2);
        }

        [TestMethod]
        public void InsertDaysException()
        {
            dao.Insert(new Role("Admin"));

            // is -1 because Montag exists already
            Assert.AreEqual(dao.Insert(new Role("Admin")), -1);
        }

        [TestMethod]
        public void CheckInsert()
        {
            Role admin = new Role("Admin");
            int ret = dao.Insert(admin);

            Assert.IsInstanceOfType(dao.FindById(ret), typeof(Role));
        }

        [TestMethod]
        public void CheckDeleteById()
        {
            InsertRoles();

            foreach (Role role in dao.FindAll())
            {
                dao.DeleteById(role.ID);
            }

            Assert.AreEqual(dao.FindAll().Count, 0);
        }

        [TestMethod]
        public void CheckDeleteAll()
        {
            InsertRoles();
            dao.DeleteAll();

            Assert.AreEqual(dao.FindAll().Count, 0);
        }

        [TestMethod]
        public void GetAllRoles()
        {
            InsertRoles();

            IList<Role> allRoles = dao.FindAll();

            Assert.AreEqual(allRoles.Count, 2);
        }

        [TestMethod]
        public void GetOneRoleByID()
        {
            int retValue = dao.Insert(new Role("Admin"));

            Role amdin = dao.FindById(retValue);

            Assert.AreEqual(amdin.Name, "Admin");
        }

        [TestMethod]
        public void GetOneRoleByName()
        {
            int retValue = dao.Insert(new Role("Admin"));

            Role amdin = dao.FindByRole("Admin");

            Assert.AreEqual(amdin.Name, "Admin");
        }

        [TestMethod]
        public void UpdateRole()
        {
            //int retValue = dao.Insert(new Role("Admin"));

            Role ret = dao.FindByRole("Admin");

            ret.Name = "Member";

            dao.Update(ret);
            

            Assert.AreEqual(dao.FindById(ret.ID).Name, "Member");
        }

        [TestMethod]
        public void GetOneDayByIDError()
        {
            Assert.IsNull(dao.FindById(1));
            Assert.IsNull(dao.FindByRole("Admin"));
        }

        [TestMethod]
        public void DeleteDaysByIdError()
        {
            Assert.AreEqual(dao.DeleteById(1), false);
        }
    }
}
