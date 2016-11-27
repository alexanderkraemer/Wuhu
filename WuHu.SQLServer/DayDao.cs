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
    public class DayDao : IDayDao
    {
        private const string SQL_FIND_BY_ID = "SELECT * FROM days WHERE id=@Id ORDER BY id";
        private const string SQL_FIND_BY_DAYNAME = "SELECT * FROM days WHERE name=@name ORDER BY id";
        private const string SQL_FIND_ALL = "SELECT * FROM days ORDER BY id";
        private const string SQL_UPDATE = "UPDATE days SET name=@name WHERE Id=@Id";
        private const string SQL_INSERT = "INSERT INTO days (name) OUTPUT Inserted.id VALUES (@name)";
        private const string SQL_DELETE_BY_ID = "DELETE FROM days WHERE id=@Id";
        private const string SQL_DELETE_ALL = "DELETE FROM days WHERE 1=1";

        private IDatabase database;

        public DayDao(IDatabase database)
        {
            this.database = database;
        }

        protected DbCommand CreateFindByIdCommand(int id)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(command, "@Id", DbType.UInt16, id);
            return command;
        }
        protected DbCommand CreateFindByDaynameCommand(string name)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_DAYNAME);
            database.DefineParameter(command, "@name", DbType.String, name);
            return command;
        }

        protected DbCommand CreateDelteByIdCommand(int id)
        {
            DbCommand findByIdCommand = database.CreateCommand(SQL_DELETE_BY_ID);
            database.DefineParameter(findByIdCommand, "@Id", DbType.UInt16, id);
            return findByIdCommand;
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
            DbCommand updateCommand = database.CreateCommand(SQL_UPDATE);
            database.DefineParameter(updateCommand, "@Id", DbType.UInt32, id);
            database.DefineParameter(updateCommand, "@Name", DbType.String, name);
            return updateCommand;
        }
        protected DbCommand CreateInsertCommand(string name)
        {
            DbCommand command = database.CreateCommand(SQL_INSERT);
            database.DefineParameter(command, "@name", DbType.String, name);
            return command;
        }


        /*
         *  here is the start of the public methods
         */

        public Day FindById(int id)
        {
            using (DbCommand command = CreateFindByIdCommand(id))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Day((int)reader["id"], (string)reader["name"]);
                }
                else
                {
                    return null;
                }
            }
        }

        public Day FindByDayname(string name)
        {
            using (DbCommand command = CreateFindByDaynameCommand(name))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Day((int)reader["id"], (string)reader["name"]);
                }
                else
                {
                    return null;
                }
            }
        }

        public IList<Day> FindAll()
        {
            using (DbCommand command = CreateFindAllCommand())
            using (IDataReader reader = database.ExecuteReader(command))
            {
                List<Day> days = new List<Day>();
                while (reader.Read())
                    days.Add(new Day((int)reader["id"], (string)reader["name"]));
                return days;
            }
        }

        public int Insert(Day day)
        {
            using (DbCommand command = CreateInsertCommand(day.Name))
            {
                try
                {
                    return (int)database.ExecuteScalar(command);
                }
                catch(SqlException)
                {
                    // do nothing
                    return -1;
                }
            }
        }

        public bool Update(Day day)
        {
            using (DbCommand command = CreateUpdateCommand(day.ID, day.Name))
            {
                return database.ExecuteNonQuery(command) == 1;
            }
        }

        public bool DeleteById(int id)
        {
            using (DbCommand command = CreateDelteByIdCommand(id))
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
