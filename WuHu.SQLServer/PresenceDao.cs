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
	public class PresenceDao : IPresenceDao
	{
        private const string SQL_FIND_BY_IDs = "SELECT * FROM presence WHERE player_id=@player_id AND day_id=@day_id";
        private const string SQL_FIND_ALL = "SELECT * FROM presence";
        private const string SQL_UPDATE = "UPDATE presence SET player_id=@playerId, day_id=@dayId WHERE player_id=@old_playerId AND day_id=@old_dayId";
        private const string SQL_INSERT = "INSERT INTO presence (player_id, day_id) VALUES (@player_id, @day_id)";
        private const string SQL_DELETE_BY_IDs = "DELETE FROM presence WHERE player_id=@player_id AND day_id=@day_id";
        private const string SQL_DELETE_ALL = "DELETE FROM presence WHERE 1=1";
        private const string SQL_FIND_BY_DAY = "SELECT * FROM presence WHERE day_id=@day_id";
        private const string SQL_FIND_BY_PLAYER = "SELECT * FROM presence WHERE player_id=@player_id";

        private IDatabase database;

		public PresenceDao(IDatabase database)
		{
			this.database = database;
		}

        protected DbCommand CreateFindByIdCommand(int player_id, int day_id)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_IDs);
            database.DefineParameter(command, "@player_id", DbType.UInt32, player_id);
            database.DefineParameter(command, "@day_id", DbType.UInt32, day_id);
            return command;
        }

        protected DbCommand CreateFindByDay(int day_id)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_DAY);
            database.DefineParameter(command, "@day_id", DbType.UInt32, day_id);
            return command;
        }

        protected DbCommand CreateFindByPlayer(int player_id)
        {
            DbCommand command = database.CreateCommand(SQL_FIND_BY_PLAYER);
            database.DefineParameter(command, "@player_id", DbType.UInt32, player_id);
            return command;
        }

        protected DbCommand CreateDeleteByIdCommand(int player_id, int day_id)
        {
            DbCommand command = database.CreateCommand(SQL_DELETE_BY_IDs);
            database.DefineParameter(command, "@player_id", DbType.UInt32, player_id);
            database.DefineParameter(command, "@day_id", DbType.UInt32, day_id);
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

        protected DbCommand CreateUpdateCommand(int old_player_id, int old_day_id, int player_id, int day_id)
		{
			DbCommand command = database.CreateCommand(SQL_UPDATE);
			database.DefineParameter(command, "@playerId", DbType.UInt16, player_id);
			database.DefineParameter(command, "@dayId", DbType.UInt16, day_id);
			database.DefineParameter(command, "@old_playerId", DbType.UInt16, old_player_id);
			database.DefineParameter(command, "@old_dayId", DbType.UInt16, old_day_id);
			return command;
		}
		protected DbCommand CreateInsertCommand(int player_id, int day_id)
		{
			DbCommand command = database.CreateCommand(SQL_INSERT);
            database.DefineParameter(command, "@player_id", DbType.UInt32, player_id);
            database.DefineParameter(command, "@day_id", DbType.UInt32, day_id);
            return command;
		}

		public Presence FindById(int player_id, int day_id)
		{
			using (DbCommand command = CreateFindByIdCommand(player_id, day_id))
			using (IDataReader reader = database.ExecuteReader(command))
			{
				if (reader.Read())
				{
					return new Presence((int)reader["player_id"], (int)reader["day_id"]);
				}
				else
				{
					return null;
				}
			}
		}

		public IList<Presence> FindAll()
		{
			using (DbCommand command = CreateFindAllCommand())
			using (IDataReader reader = database.ExecuteReader(command))
			{
				List<Presence> presences = new List<Presence>();
				while (reader.Read())
					presences.Add(new Presence((int)reader["player_id"], (int)reader["day_id"]));
				return presences;
			}
		}

		public int Insert(Presence presence)
		{
			using (DbCommand command = CreateInsertCommand(presence.PlayerID, presence.DayID))
			{
                try
                {
                    return database.ExecuteNonQuery(command);
                }
                catch (SqlException)
                {
                    // do nothing
                    return 0;
                }
            }
		}

		public bool Update(Presence oldDay, Presence newDay)
		{
			using (DbCommand command = CreateUpdateCommand(oldDay.PlayerID, oldDay.DayID, newDay.PlayerID, newDay.DayID))
			{
				return database.ExecuteNonQuery(command) == 1;
			}
		}

        public bool DeleteById(int player_id, int day_id)
        {
            using (DbCommand command = CreateDeleteByIdCommand(player_id, day_id))
            {
                return database.ExecuteNonQuery(command) == 1;
            }
        }

        public IList<Presence> FindPresenceByDay(int day_id)
        {
            using (DbCommand command = CreateFindByDay(day_id))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                List<Presence> presences = new List<Presence>();
                while (reader.Read())
                    presences.Add(new Presence((int)reader["player_id"], (int)reader["day_id"]));
                return presences;
            }
        }
        public IList<Presence> FindPresenceByPlayer(int player_id)
        {
            using (DbCommand command = CreateFindByPlayer(player_id))
            using (IDataReader reader = database.ExecuteReader(command))
            {
                List<Presence> presences = new List<Presence>();
                while (reader.Read())
                    presences.Add(new Presence((int)reader["player_id"], (int)reader["day_id"]));
                return presences;
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
