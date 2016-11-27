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
	public class MatchDao : IMatchDao
	{
        private const string SQL_FIND_BY_ID = "SELECT * FROM matches WHERE id=@Id ORDER BY id";
        private const string SQL_FIND_ALL = "SELECT * FROM matches";
        private const string SQL_UPDATE = "UPDATE matches SET team_id1=@team_id1, team_id2=@team_id2, timestamp=@timestamp, results_points_p1=@results_points_p1, results_points_p2=@results_points_p2 WHERE Id=@Id";
        private const string SQL_UPDATE_NULL = "UPDATE matches SET team_id1=@team_id1, team_id2=@team_id2, timestamp=@timestamp WHERE Id=@Id";
        private const string SQL_INSERT_NULL = "INSERT INTO matches (team_id1, team_id2, timestamp) OUTPUT Inserted.id VALUES (@team_id1, @team_id2, @timestamp)";
        private const string SQL_INSERT = "INSERT INTO matches (team_id1, team_id2, timestamp, results_points_p1, results_points_p2) OUTPUT Inserted.Id VALUES (@team_id1, @team_id2, @timestamp, @results_points_p1, @results_points_p2)";
        private const string SQL_DELETE_BY_ID = "DELETE FROM matches WHERE id=@Id";
        private const string SQL_DELETE_ALL = "DELETE FROM matches WHERE 1=1";

        private IDatabase database;

		public MatchDao(IDatabase database)
		{
			this.database = database;
		}

        protected DbCommand CreateFindByIdCommand(int id)
        {
            DbCommand findByIdCommand = database.CreateCommand(SQL_FIND_BY_ID);
            database.DefineParameter(findByIdCommand, "@Id", DbType.UInt16, id);
            return findByIdCommand;
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

        protected DbCommand CreateUpdateCommand(int id, int team_id1, int team_id2, DateTime timestamp, 
			int? results_points_p1, int? results_points_p2)
		{
            DbCommand command;
            if (results_points_p1 == null && results_points_p2 == null)
            {
                command = database.CreateCommand(SQL_UPDATE_NULL);
            }
            else
            {
                command = database.CreateCommand(SQL_UPDATE);
                database.DefineParameter(command, "@results_points_p1", DbType.String, results_points_p1);
                database.DefineParameter(command, "@results_points_p2", DbType.String, results_points_p2);
            }
            database.DefineParameter(command, "@Id", DbType.String, id);
			database.DefineParameter(command, "@team_id1", DbType.String, team_id1);
			database.DefineParameter(command, "@team_id2", DbType.String, team_id2);
			database.DefineParameter(command, "@timestamp", DbType.String, timestamp);
			
			return command;
		}
		protected DbCommand CreateInsertCommand(int team_id1, int team_id2, DateTime timestamp,
			int? results_points_p1, int? results_points_p2)
		{
            DbCommand command;
            if (results_points_p1 == null && results_points_p2 == null)
            {
                command = database.CreateCommand(SQL_INSERT_NULL);
            }
            else
            {
                command = database.CreateCommand(SQL_UPDATE);
                database.DefineParameter(command, "@results_points_p1", DbType.String, results_points_p1);
                database.DefineParameter(command, "@results_points_p2", DbType.String, results_points_p2);
            }
                database.DefineParameter(command, "@team_id1", DbType.UInt32, team_id1);
                database.DefineParameter(command, "@team_id2", DbType.UInt32, team_id2);
                database.DefineParameter(command, "@timestamp", DbType.DateTime, timestamp);
                return command;
            
		}
		public Match FindById(int id)
		{
			using (DbCommand command = CreateFindByIdCommand(id))
			using (IDataReader reader = database.ExecuteReader(command))
			{
				if (reader.Read())
				{
                    int? res1 = reader.IsDBNull(reader.GetOrdinal("results_points_p1")) ? null : (int?)reader["results_points_p1"];
                    int? res2 = reader.IsDBNull(reader.GetOrdinal("results_points_p2")) ? null : (int?)reader["results_points_p2"];

                    return new Match((int)reader["id"], (int)reader["team_id1"], (int)reader["team_id2"],
                         (DateTime)reader["timestamp"], res1, res2);
                }
				else
				{
					return null;
				}
			}
		}

		public IList<Match> FindAll()
		{
			
			using (DbCommand command = CreateFindAllCommand())
			using (IDataReader reader = database.ExecuteReader(command))
			{
                List<Match> matchs = new List<Match>();
				while (reader.Read())
                {
                    int? res1 = reader.IsDBNull(reader.GetOrdinal("results_points_p1")) ? null : (int?)reader["results_points_p1"];
                    int? res2 = reader.IsDBNull(reader.GetOrdinal("results_points_p2")) ? null : (int?)reader["results_points_p2"];

                    matchs.Add(new Match((int)reader["id"], (int)reader["team_id1"], (int)reader["team_id2"],
                         (DateTime)reader["timestamp"], res1, res2));
                }
                    
				return matchs;
			}
		}

        public bool Update(Match match)
        {
            using (DbCommand command = CreateUpdateCommand(match.ID, match.Team1ID, match.Team2ID, 
                match.Timestamp, match.ResultPointsPlayer1, match.ResultPointsPlayer2))
            {
                return database.ExecuteNonQuery(command) == 1;
            }
        }

        public int Insert(Match match)
        {
            using (DbCommand command = CreateInsertCommand(match.Team1ID, match.Team2ID,
                match.Timestamp, match.ResultPointsPlayer1, match.ResultPointsPlayer2))
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


        public bool DeleteAll()
        {
            using (DbCommand command = CreateDeleteAllCommand())
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
    }
}
