using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WuHu.Common;

namespace WuHu.SQLServer
{
	public class Database: IDatabase
	{
		private readonly string connectionString;
		public Database(string connectionString)
		{
			this.connectionString = connectionString;
		}
		public DbCommand CreateCommand(string commandTxt)
		{
			return new SqlCommand(commandTxt);
		}

		public int DeclareParameter(DbCommand command, string name, DbType type)
		{
			if (!command.Parameters.Contains(name))
			{
				return command.Parameters.Add(new SqlParameter(name, type));
			}
			else
			{
				throw new ArgumentException($"Parameter {name} already exists.");
			}
		}
		public void SetParameter(DbCommand command, string name, object value)
		{
			if (command.Parameters.Contains(name))
			{
				command.Parameters[name].Value = value;
			}
			else
			{
				throw new ArgumentException($"Parameter {name} not declared");
			}
		}

		public void DefineParameter(DbCommand command, string name, DbType type, object value)
		{
			int index = DeclareParameter(command, name, type); // throws exeception if param already exists
			command.Parameters[index].Value = value;
		}


		public int ExecuteNonQuery(DbCommand command)
		{
			DbConnection conn = null;
			try
			{
				conn = GetOpenConnection();
				command.Connection = conn;

				return command.ExecuteNonQuery();
			}
			finally
			{
				ReleaseConnection(conn);
			}
		}

		public object ExecuteScalar(DbCommand command)
		{
			DbConnection conn = null;
			try
			{
				conn = GetOpenConnection();
				command.Connection = conn;

				return command.ExecuteScalar();
			}
			finally
			{
				ReleaseConnection(conn);
			}
		}

		public IDataReader ExecuteReader(DbCommand command)
		{
			DbConnection conn = null;
			try
			{
				conn = GetOpenConnection();
				command.Connection = conn;

				// behavior for data reader, if there is no transaction, release connection
				var behavior = Transaction.Current == null ?
					CommandBehavior.CloseConnection : CommandBehavior.Default;

				return command.ExecuteReader(behavior);
			}
			catch // catch any exception
			{
				// close connection only when exception occurs!
				// DataReader needs open connection to walk results
				ReleaseConnection(conn);
				throw; //rethrow occured exception
			}
		}

		[ThreadStatic] // that means there is an instance of this member for EACH thread
		private static DbConnection sharedConnection;

		private DbConnection CreateOpenConnection()
		{
			SqlConnection connection = new SqlConnection(this.connectionString);
			connection.Open();
			return connection;
		}
		private DbConnection GetOpenConnection()
		{
			Transaction currentTransaction = Transaction.Current;

			if (currentTransaction == null)
			{
				return CreateOpenConnection();
			}
			else
			{
				if (sharedConnection == null)
				{
					sharedConnection = CreateOpenConnection();
					currentTransaction.TransactionCompleted += (sender, args) =>
					{
						if (sharedConnection != null)
						{
							sharedConnection?.Close();
							sharedConnection = null;
						}
					};
				}
				return sharedConnection;
			}
		}

		private void ReleaseConnection(DbConnection connection)
		{
			if (connection != null && Transaction.Current == null)
			{
				connection.Close();
			}
		}
	}
}
