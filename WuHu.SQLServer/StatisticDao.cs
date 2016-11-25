using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using WuHu.Common;
using WuHu.Domain;

namespace WuHu.SQLServer
{
	public class StatisticDao : IStatisticDao
	{
        private const string SQL_FIND_BY_ID = "SELECT * FROM statistic WHERE id=@Id ORDER BY timestamp";
        private const string SQL_FIND_BY_Player = "SELECT * FROM statistic WHERE player_id=@player_id ORDER BY timestamp";
        private const string SQL_FIND_BY_Day = "SELECT * FROM statistic WHERE timestamp=@timestamp ORDER BY id";
        private const string SQL_FIND_ALL = "SELECT * FROM statistic ORDER BY id";
        private const string SQL_UPDATE = "UPDATE statistic SET player_id=@player_id, skill=@skill, timestamp=@timestamp WHERE Id=@Id";
        private const string SQL_INSERT = "INSERT INTO statistic (player_id, skill, timestamp) OUTPUT Inserted.id VALUES (@player_id, @skill, @timestamp)";
        private const string SQL_DELETE_BY_ID = "DELETE FROM statistic WHERE id=@Id";
        private const string SQL_DELETE_ALL = "DELETE FROM statistic WHERE 1=1";


        private IDatabase database;

		public StatisticDao(IDatabase database)
		{
			this.database = database;
		}

        protected DbCommand CreateFindByIdCommand(int id)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(command, "@Id", DbType.UInt16, id);
            return command;
        }
        protected DbCommand CreateFindByPlayerCommand(int player_id)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_Player);
            database.DefineParameter(command, "@player_id", DbType.UInt16, player_id);
            return command;
        }
        protected DbCommand CreateFindByDayCommand(DateTime timestamp)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_Day);
            database.DefineParameter(command, "@timestamp", DbType.DateTime, timestamp);
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

        protected DbCommand CreateUpdateCommand(int id, int player_id, int skill, DateTime timestamp)
		{
			DbCommand command = database.CreateCommand(SQL_UPDATE);
			database.DefineParameter(command, "@Id", DbType.UInt16, id);
			database.DefineParameter(command, "@player_id", DbType.UInt16, player_id);
			database.DefineParameter(command, "@skill", DbType.UInt16, skill);
			database.DefineParameter(command, "@timestamp", DbType.String, timestamp);
			return command;
		}
		protected DbCommand CreateInsertCommand(int player_id, int skill, DateTime timestamp)
		{
			DbCommand command = database.CreateCommand(SQL_INSERT);
			database.DefineParameter(command, "@player_id", DbType.UInt16, player_id);
			database.DefineParameter(command, "@skill", DbType.UInt16, skill);
			database.DefineParameter(command, "@timestamp", DbType.String, timestamp);
			return command;
		}

        public Statistic FindById(int id)
        {
            using (DbCommand command = CreateFindByIdCommand(id))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Statistic((int)reader["id"], (int)reader["player_id"], (int)reader["skill"], (DateTime)reader["timestamp"]);
                }
                else
                {
                    return null;
                }
            }
        }

        public Statistic FindByDay(DateTime timestamp)
        {
            using (DbCommand command = CreateFindByDayCommand(timestamp))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Statistic((int)reader["id"], (int)reader["player_id"], (int)reader["skill"], (DateTime)reader["timestamp"]);
                }
                else
                {
                    return null;
                }
            }
        }

        public Statistic FindByPlayer(int player_id)
        {
            using (DbCommand command = CreateFindByPlayerCommand(player_id))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    return new Statistic((int)reader["id"], (int)reader["player_id"], (int)reader["skill"], (DateTime)reader["timestamp"]);
                }
                else
                {
                    return null;
                }
            }
        }

        public IList<Statistic> FindAll()
		{
			using (DbCommand command = CreateFindAllCommand())
			using (IDataReader reader = database.ExecuteReader(command))
			{
				List<Statistic> statistics = new List<Statistic>();
				while (reader.Read())
					statistics.Add(new Statistic((int)reader["id"], (int)reader["player_id"], (int)reader["skill"], (DateTime)reader["timestamp"]));
				return statistics;
			}
		}

		public bool Update(Statistic statistic)
		{
			using (DbCommand command = CreateUpdateCommand(statistic.ID, statistic.PlayerID, statistic.Skill, statistic.Timestamp))
			{
				return database.ExecuteNonQuery(command) == 1;
			}
		}

		public int Insert(Statistic statistic)
		{
			using (DbCommand command = CreateInsertCommand(statistic.PlayerID, statistic.Skill, statistic.Timestamp))
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
            };
        }
    }
}
