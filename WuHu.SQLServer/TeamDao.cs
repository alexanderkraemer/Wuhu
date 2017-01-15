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
	public class TeamDao : ITeamDao
	{
		private const string SQL_FIND_BY_ID = "SELECT * FROM teams WHERE id=@Id ORDER BY id";
		private const string SQL_FIND_ALL = "SELECT * FROM teams ORDER BY id";
		private const string SQL_UPDATE = "UPDATE teams SET name=@name, player_id1=@player_id1, player_id2=@player_id2 WHERE Id=@Id";
		private const string SQL_INSERT = "INSERT INTO teams (name, player_id1, player_id2) OUTPUT Inserted.id VALUES (@name, @player_id1, @player_id2)";
        private const string SQL_DELETE_BY_ID = "DELETE FROM teams WHERE id=@Id";
        private const string SQL_DELETE_ALL = "DELETE FROM teams WHERE 1=1";


        private IDatabase database;

		public TeamDao(IDatabase database)
		{
			this.database = database;
		}

		protected DbCommand CreateFindByIdCommand(int id)
		{
			DbCommand findByIdCommand = database.CreateCommand(SQL_FIND_BY_ID);
			database.DefineParameter(findByIdCommand, "@Id", DbType.UInt16, id);
			return findByIdCommand;
		}

		protected DbCommand CreateFindAllCommand()
		{
			return database.CreateCommand(SQL_FIND_ALL);
		}

		protected DbCommand CreateUpdateCommand(int id, string name, int player_id1, int player_id2)
		{
			DbCommand updateCommand = database.CreateCommand(SQL_UPDATE);
			database.DefineParameter(updateCommand, "@Id", DbType.UInt16, id);
			database.DefineParameter(updateCommand, "@player_id1", DbType.UInt16, player_id1);
			database.DefineParameter(updateCommand, "@player_id2", DbType.UInt16, player_id2);
			database.DefineParameter(updateCommand, "@name", DbType.String, name);
			return updateCommand;
		}
		protected DbCommand CreateInsertCommand(string name, int player_id1, int player_id2)
		{
			DbCommand updateCommand = database.CreateCommand(SQL_INSERT);
			database.DefineParameter(updateCommand, "@player_id1", DbType.UInt16, player_id1);
			database.DefineParameter(updateCommand, "@player_id2", DbType.UInt16, player_id2);
			database.DefineParameter(updateCommand, "@name", DbType.String, name);
			return updateCommand;
		}

        private DbCommand CreateDeleteAllCommand()
        {
            return this.database.CreateCommand(SQL_DELETE_ALL);
        }

        private DbCommand CreateDeleteByIdCommand(int id)
        {
            DbCommand findByIdCommand = database.CreateCommand(SQL_DELETE_BY_ID);
            database.DefineParameter(findByIdCommand, "@Id", DbType.UInt32, id);
            return findByIdCommand;
        }


        public Team FindById(int id)
		{
			using (DbCommand command = CreateFindByIdCommand(id))
			using (IDataReader reader = database.ExecuteReader(command))
			{
				if (reader.Read())
				{
					return new Team((int)reader["id"], (string)reader["name"], (int)reader["player_id1"], (int)reader["player_id2"]);
				}
				else
				{
					return null;
				}
			}
		}

		public IList<Team> FindAll()
		{
			using (DbCommand command = CreateFindAllCommand())
			using (IDataReader reader = database.ExecuteReader(command))
			{
				List<Team> teams = new List<Team>();
				while (reader.Read())
					teams.Add(new Team((int)reader["id"], (string)reader["name"], (int)reader["player_id1"], (int)reader["player_id2"]));
				return teams;
			}
		}

		public bool Update(Team team)
		{
			using (DbCommand command = CreateUpdateCommand(team.ID, team.Name, team.Player1ID, team.Player2ID))
			{
				return database.ExecuteNonQuery(command) == 1;
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

        public int Insert(Team team)
        {
            using (DbCommand command = CreateInsertCommand(team.Name, team.Player1ID, team.Player2ID))
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
    }
}
