using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;
using WuHu.Domain;

namespace WuHu.SQLServer
{
	public class RoleDao: IRoleDao
	{
        private const string SQL_FIND_BY_ID = "SELECT * FROM roles WHERE id=@Id ORDER BY id";
        private const string SQL_FIND_BY_ROLE = "SELECT * FROM roles WHERE name=@name ORDER BY id";
        private const string SQL_FIND_ALL = "SELECT * FROM roles ORDER BY id";
        private const string SQL_UPDATE = "UPDATE roles SET name=@name WHERE Id=@Id";
        private const string SQL_INSERT = "INSERT INTO roles (name) OUTPUT Inserted.id VALUES (@name)";
        private const string SQL_DELETE_BY_ID = "DELETE FROM roles WHERE id=@Id";
        private const string SQL_DELETE_ALL = "DELETE FROM roles WHERE 1=1";


        private IDatabase database;

		public RoleDao(IDatabase database)
		{
			this.database = database;
		}

        protected DbCommand CreateFindByIdCommand(int id)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(command, "@Id", DbType.UInt16, id);
            return command;
        }

        protected DbCommand CreateFindByNameCommand(string name)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_ROLE);
            database.DefineParameter(command, "@name", DbType.String, name);
            return command;
        }

        protected DbCommand CreateDeleteByIdCommand(int id)
        {
            DbCommand command = database.CreateCommand(SQL_DELETE_BY_ID);
            database.DefineParameter(command, "@Id", DbType.UInt16, id);
            return command;
        }

        protected DbCommand CreateFindAllCommand()
        {
            return database.CreateCommand(SQL_FIND_ALL);
        }

        protected DbCommand CreateDeleteAllCommand()
        {
            return database.CreateCommand(SQL_DELETE_ALL);
        }

        protected DbCommand CreateUpdateCommand(int id, string name)
		{
			DbCommand command = database.CreateCommand(SQL_UPDATE);
			database.DefineParameter(command, "@Id", DbType.UInt32, id);
			database.DefineParameter(command, "@name", DbType.String, name);
			return command;
		}
		protected DbCommand CreateInsertCommand(string name)
		{
			DbCommand command = database.CreateCommand(SQL_INSERT);
			database.DefineParameter(command, "@name", DbType.String, name);
			return command;
		}

        public Role FindById(int id)
        {
            using (DbCommand command = CreateFindByIdCommand(id))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Role((int)reader["id"], (string)reader["name"]);
                }
                else
                {
                    return null;
                }
            }
        }

        public Role FindByRole(string name)
        {
            using (DbCommand command = CreateFindByNameCommand(name))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Role((int)reader["id"], (string)reader["name"]);
                }
                else
                {
                    return null;
                }
            }
        }

        public IList<Role> FindAll()
		{
			using (DbCommand command = CreateFindAllCommand())
			using (IDataReader reader = database.ExecuteReader(command))
			{
				List<Role> roles = new List<Role>();
				while (reader.Read())
                    roles.Add(new Role((int)reader["id"], (string)reader["name"]));
				return roles;
			}
		}

		public bool Update(Role role)
		{
			using (DbCommand command = CreateUpdateCommand(role.ID, role.Name))
			{
				return database.ExecuteNonQuery(command) == 1;
			}
		}

		public int Insert(Role role)
		{
			using (DbCommand command = CreateInsertCommand(role.Name))
			{
                try
                {
                    return (int)database.ExecuteScalar(command);
                }
                catch (SqlException)
                {
                    // do nothing
                    return -1;
                }
            }
		}

        public bool DeleteById(int id)
        {
            using (DbCommand command = CreateDeleteByIdCommand(id))
            {
                return database.ExecuteNonQuery(command) == 1;
            }
        }

        public bool DeleteAll()
        {
            using (DbCommand command = CreateDeleteAllCommand())
            {
                return database.ExecuteNonQuery(command) == 1;
            }
        }
    }
}
