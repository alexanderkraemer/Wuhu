using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WuHu.Common;
using WuHu.Domain;

namespace WuHu.SQLServer
{
	public class MatchDao : IMatchDao
	{
        private const string SQL_FIND_BY_ID = "SELECT * FROM matches WHERE id=@Id ORDER BY id";
        private const string SQL_FIND_ALL = "SELECT * FROM matches";
        private const string SQL_UPDATE = "UPDATE matches SET team1_p1ID=@team1_p1ID, team1_p2ID=@team1_p2ID, team2_p1ID=@team2_p1ID, team2_p2ID=@team2_p2ID, tournamentId=@tournamentId, results_points_p1=@results_points_p1, results_points_p2=@results_points_p2, finished=@finished WHERE Id=@Id";
        private const string SQL_UPDATE_NULL = "UPDATE matches SET team1_p1ID=@team1_p1ID, team1_p2ID=@team1_p2ID, team2_p1ID=@team2_p1ID, team2_p2ID=@team2_p2ID, tournamentId=@tournamentId, finished=@finished WHERE Id=@Id";
        private const string SQL_INSERT_NULL = "INSERT INTO matches (team1_p1ID, team1_p2ID, team2_p1ID, team2_p2ID, tournamentId, finished) OUTPUT Inserted.id VALUES (@team1_p1ID, @team1_p2ID, @team2_p1ID, @team2_p2ID, @tournamentId, @finished)";
        private const string SQL_INSERT = "INSERT INTO matches (team1_p1ID, team1_p2ID, team2_p1ID, team2_p2ID, tournamentId, results_points_p1, results_points_p2, finished) OUTPUT Inserted.Id VALUES (@team1_p1ID, @team1_p2ID, @team2_p1ID, @team2_p2ID, @tournamentId, @results_points_p1, @results_points_p2, @finished)";
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

        protected DbCommand CreateUpdateCommand(int id, int team1_p1ID, int team2_p1ID, int team1_p2ID, int team2_p2ID, int tournamentId, 
			int? results_points_p1, int? results_points_p2, bool finished)
		{
            DbCommand command;
            if (results_points_p1 == null && results_points_p2 == null)
            {
                command = database.CreateCommand(SQL_UPDATE_NULL);
            }
            else
            {
                command = database.CreateCommand(SQL_UPDATE);
                database.DefineParameter(command, "@results_points_p1", DbType.Int32, results_points_p1);
                database.DefineParameter(command, "@results_points_p2", DbType.Int32, results_points_p2);
            }
            database.DefineParameter(command, "@Id", DbType.Int32, id);

			database.DefineParameter(command, "@team1_p1ID", DbType.Int32, team1_p1ID);
			database.DefineParameter(command, "@team2_p1ID", DbType.Int32, team2_p1ID);
			database.DefineParameter(command, "@team1_p2ID", DbType.Int32, team1_p2ID);
			database.DefineParameter(command, "@team2_p2ID", DbType.Int32, team2_p2ID);
			database.DefineParameter(command, "@tournamentId", DbType.Int32, tournamentId);
			database.DefineParameter(command, "@finished", DbType.Boolean, finished);

			return command;
		}
		protected DbCommand CreateInsertCommand(int team1_p1ID, int team2_p1ID, int team1_p2ID, int team2_p2ID, int tournamentId,
			int? results_points_p1, int? results_points_p2, bool finished)
		{
            DbCommand command;
            if (results_points_p1 == null && results_points_p2 == null)
            {
                command = database.CreateCommand(SQL_INSERT_NULL);
            }
            else
            {
                command = database.CreateCommand(SQL_UPDATE);
                database.DefineParameter(command, "@results_points_p1", DbType.Int32, results_points_p1);
                database.DefineParameter(command, "@results_points_p2", DbType.Int32, results_points_p2);
            }
			database.DefineParameter(command, "@team1_p1ID", DbType.Int32, team1_p1ID);
			database.DefineParameter(command, "@team2_p1ID", DbType.Int32, team2_p1ID);
			database.DefineParameter(command, "@team1_p2ID", DbType.Int32, team1_p2ID);
			database.DefineParameter(command, "@team2_p2ID", DbType.Int32, team2_p2ID);
			database.DefineParameter(command, "@tournamentId", DbType.Int32, tournamentId);
			database.DefineParameter(command, "@finished", DbType.Boolean, finished);

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

                    return new Match((int)reader["id"], (int)reader["team1_p1ID"], (int)reader["team2_p1ID"], (int)reader["team1_p2ID"], (int)reader["team2_p2ID"],
                         (int)reader["tournamentId"], res1, res2, (bool)reader["finished"]);
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

                    matchs.Add(new Match((int)reader["id"], (int)reader["team1_p1ID"], (int)reader["team2_p1ID"], (int)reader["team1_p2ID"], (int)reader["team2_p2ID"],
								 (int)reader["tournamentId"], res1, res2, (bool)reader["finished"]));
                }
				return matchs;
			}
		}

        public bool Update(Match match)
        {
            using (DbCommand command = CreateUpdateCommand(match.ID, match.Team1Player1, match.Team1Player2, match.Team2Player1, match.Team2Player2,
					 match.TournamentId, match.ResultPointsPlayer1, match.ResultPointsPlayer2, match.Finished))
            {
                return database.ExecuteNonQuery(command) == 1;
            }
        }

        public int Insert(Match match)
        {
            using (DbCommand command = CreateInsertCommand(match.Team1Player1, match.Team1Player2, match.Team2Player1, match.Team2Player2,
					 match.TournamentId, match.ResultPointsPlayer1, match.ResultPointsPlayer2, match.Finished))
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
