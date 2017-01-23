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
	public class TournamentDao : ITournamentDao
	{
		private const string SQL_FIND_BY_ID = "SELECT * FROM tournaments WHERE id=@Id ORDER BY id";
		private const string SQL_FIND_BY_DAY = "SELECT * FROM tournaments WHERE timestamp=@Timestamp ORDER BY id";
		private const string SQL_FIND_ALL = "SELECT * FROM tournaments ORDER BY id";
		private const string SQL_UPDATE = "UPDATE tournaments SET name=@name, timestamp=@timestamp WHERE Id=@Id";
		private const string SQL_INSERT = "INSERT INTO tournaments (name, timestamp) OUTPUT Inserted.Id VALUES (@name, @timestamp)";
		private const string SQL_DELETE_BY_ID = "DELETE FROM tournaments WHERE id=@Id";
		private const string SQL_DELETE_ALL = "DELETE FROM tournaments WHERE 1=1";


		private IDatabase database;

		public TournamentDao(IDatabase database)
		{
			this.database = database;
		}

		protected DbCommand CreateFindByIdCommand(int id)
		{
			DbCommand findByIdCommand = database.CreateCommand(SQL_FIND_BY_ID);
			database.DefineParameter(findByIdCommand, "@Id", DbType.UInt32, id);
			return findByIdCommand;
		}

		protected DbCommand CreateFindByDayCommand(DateTime day)
		{
			DbCommand findByIdCommand = database.CreateCommand(SQL_FIND_BY_DAY);
			database.DefineParameter(findByIdCommand, "@Timestamp", DbType.DateTime, day);
			return findByIdCommand;
		}

		protected DbCommand CreateFindAllCommand()
		{
			return database.CreateCommand(SQL_FIND_ALL);
		}

		protected DbCommand CreateUpdateCommand(int id, string name, DateTime timestamp)
		{
			DbCommand updateCommand = database.CreateCommand(SQL_UPDATE);
			database.DefineParameter(updateCommand, "@Id", DbType.UInt32, id);
			database.DefineParameter(updateCommand, "@timestamp", DbType.DateTime, timestamp);
			database.DefineParameter(updateCommand, "@name", DbType.String, name);
			return updateCommand;
		}
		protected DbCommand CreateInsertCommand(string name, DateTime timestamp)
		{
			DbCommand updateCommand = database.CreateCommand(SQL_INSERT);
			database.DefineParameter(updateCommand, "@timestamp", DbType.DateTime, timestamp);
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


		public Tournament FindById(int id)
		{
			using (DbCommand command = CreateFindByIdCommand(id))
			using (IDataReader reader = database.ExecuteReader(command))
			{
				if (reader.Read())
				{
					return new Tournament((int)reader["id"], (string)reader["name"], (DateTime)reader["timestamp"]);
				}
				else
				{
					return null;
				}
			}
		}

		public IList<Tournament> FindByDay(DateTime day)
		{
			using (DbCommand command = CreateFindByDayCommand(day))
			using (IDataReader reader = database.ExecuteReader(command))
			{
				List<Tournament> list = new List<Tournament>();
				while(reader.Read())
					list.Add(new Tournament((int)reader["id"], (string)reader["name"], (DateTime)reader["timestamp"]));

				return list;
			}
		}

		public IList<Tournament> FindAll()
		{
			using (DbCommand command = CreateFindAllCommand())
			using (IDataReader reader = database.ExecuteReader(command))
			{
				List<Tournament> teams = new List<Tournament>();
				while (reader.Read())
					teams.Add(new Tournament((int)reader["id"], (string)reader["name"], (DateTime)reader["timestamp"]));

				return teams;
			}
		}

		public bool Update(Tournament tournament)
		{
			using (DbCommand command = CreateUpdateCommand(tournament.ID, tournament.Name, tournament.Timestamp))
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

		public int Insert(Tournament tournament)
		{
			DateTime myDateTime = DateTime.Now;
			DateTime d = new DateTime(myDateTime.Year, myDateTime.Month, myDateTime.Day);

			using (DbCommand command = CreateInsertCommand(tournament.Name, d))
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
