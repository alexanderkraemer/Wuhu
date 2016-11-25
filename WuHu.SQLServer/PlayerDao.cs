using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using WuHu.Common;
using WuHu.Domain;

namespace WuHu.SQLServer
{
	public class PlayerDao : IPlayerDao
	{
		private const string SQL_FIND_BY_ID = "SELECT * FROM player WHERE id=@Id ORDER BY id";
		private const string SQL_FIND_ALL = "SELECT * FROM player ORDER BY id";
		private const string SQL_UPDATE = "UPDATE player SET role_id=@roleID, skills=@skills, first_name=@firstname, last_name=@lastname, nickname=@nickname, photopath=@photopath, password=@password WHERE Id=@Id";
		private const string SQL_INSERT = "INSERT INTO player (role_id, skills, first_name, last_name, nickname, photopath, password) OUTPUT Inserted.id VALUES (@roleID, @skills, @firstname, @lastname, @nickname, @photopath, @password)";
		private const string SQL_FIND_BY_NICKNAME = "SELECT * FROM player WHERE nickname=@nickname ORDER BY id";
		private const string SQL_DELETE_ALL = "DELETE FROM player WHERE 1 = 1";
        private const string SQL_DELETE_BY_ID = "DELETE FROM player WHERE id=@Id";


        private IDatabase database;

		public PlayerDao(IDatabase database)
		{
			this.database = database;
		}


		protected DbCommand CreateFindByIdCommand(int id)
		{
			DbCommand findByIdCommand = database.CreateCommand(SQL_FIND_BY_ID);
			database.DefineParameter(findByIdCommand, "@Id", DbType.UInt32, id);
			return findByIdCommand;
		}

		protected DbCommand CreateFindByNickname(string nickname)
		{
			DbCommand findByNicknameCommand = database.CreateCommand(SQL_FIND_BY_NICKNAME);
			database.DefineParameter(findByNicknameCommand, "@nickname", DbType.String, nickname);
			return findByNicknameCommand;
		}

        protected DbCommand CreateDeleteByIdCommand(int id)
        {
            DbCommand findByIdCommand = database.CreateCommand(SQL_DELETE_BY_ID);
            database.DefineParameter(findByIdCommand, "@Id", DbType.UInt32, id);
            return findByIdCommand;
        }

        protected DbCommand CreateFindAllCommand()
		{
			return this.database.CreateCommand(SQL_FIND_ALL);
		}

		protected DbCommand CreateDeleteAllCommand()
		{
			return database.CreateCommand(SQL_DELETE_ALL);
		}

		protected DbCommand CreateUpdateCommand(int id, int role_id, int skills, string firstname, string lastname, string nickname, string photopath, string password)
		{
			DbCommand updateCommand = database.CreateCommand(SQL_UPDATE);
			database.DefineParameter(updateCommand, "@Id", DbType.UInt16, id);
			database.DefineParameter(updateCommand, "@roleID", DbType.UInt16, role_id);
			database.DefineParameter(updateCommand, "@skills", DbType.UInt16, skills);
			database.DefineParameter(updateCommand, "@firstname", DbType.String, firstname);
			database.DefineParameter(updateCommand, "@lastname", DbType.String, lastname);
			database.DefineParameter(updateCommand, "@nickname", DbType.String, nickname);
			database.DefineParameter(updateCommand, "@photopath", DbType.String, photopath);
			database.DefineParameter(updateCommand, "@password", DbType.String, password);
			return updateCommand;
		}
		protected DbCommand CreateInsertCommand(int role_id, int skills, string firstname, string lastname, string nickname, string photopath, string password)
		{
			DbCommand insertCommand = database.CreateCommand(SQL_INSERT);
			database.DefineParameter(insertCommand, "@roleID", DbType.UInt16, role_id);
			database.DefineParameter(insertCommand, "@skills", DbType.UInt16, skills);
			database.DefineParameter(insertCommand, "@firstname", DbType.String, firstname);
			database.DefineParameter(insertCommand, "@lastname", DbType.String, lastname);
			database.DefineParameter(insertCommand, "@nickname", DbType.String, nickname);
			database.DefineParameter(insertCommand, "@photopath", DbType.String, photopath);
			database.DefineParameter(insertCommand, "@password", DbType.String, password);
			return insertCommand;
		}

		public Player FindById(int playerId)
		{
			using (DbCommand command = CreateFindByIdCommand(playerId))
			using (IDataReader reader = database.ExecuteReader(command))
			{
				if (reader.Read())
				{
					return new Player((int)reader["id"], (int)reader["role_id"], (string)reader["first_name"], (string)reader["last_name"],
						 (string)reader["nickname"], (int)reader["skills"], (string)reader["photopath"], (string)reader["password"]);
				}
				else
				{
					return null;
				}
			}
		}

		public Player FindByNickname (string nickname)
		{
			using (DbCommand command = CreateFindByNickname(nickname))
			using (IDataReader reader = database.ExecuteReader(command))
			{
				if (reader.Read())
				{
					return new Player((int)reader["id"], (int)reader["role_id"], (string)reader["first_name"], (string)reader["last_name"],
						 (string)reader["nickname"], (int)reader["skills"], (string)reader["photopath"], (string)reader["password"]);
				}
				else
				{
					return null;
				}
			}
		}

		public IList<Player> FindAll()
		{
			using (DbCommand command = CreateFindAllCommand())
			using (IDataReader reader = database.ExecuteReader(command))
			{
				List<Player> players = new List<Player>();
				while (reader.Read())
					players.Add(new Player((int)reader["id"], (int)reader["role_id"], (string)reader["first_name"], (string)reader["last_name"],
						 (string)reader["nickname"], (int)reader["skills"], (string)reader["photopath"], (string)reader["password"]));
				return players;
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

		public bool Update(Player player)
		{
			using (DbCommand command = CreateUpdateCommand(player.ID, player.RoleID, player.Skills,
				 player.FirstName, player.LastName, player.Nickname, player.PhotoPath, player.Password))
			{
				return database.ExecuteNonQuery(command) == 1;
			}
		}

		public int Insert(Player player)
		{
			using (DbCommand command = CreateInsertCommand(player.RoleID, player.Skills,
				 player.FirstName, player.LastName, player.Nickname, player.PhotoPath, player.Password))
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