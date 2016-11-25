using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using WuHu.Common;

namespace WuHu.Generic
{
    public class Database : IDatabase
    {
        private static readonly Regex paramNameRegex = new Regex(@"@\w+");

        private string connectionString;
        private DbProviderFactory providerFactory;

        public Database(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected DbProviderFactory ProviderFactory
        {
            get
            {
                if (providerFactory == null)
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["DefaultConnectionString"];
                    providerFactory = DbProviderFactories.GetFactory(connectionString.ProviderName);
                }

                return providerFactory;
            }
        }

        public DbCommand CreateCommand(string genericCommandText)
        {
            DbCommand command = ProviderFactory.CreateCommand();
            command.CommandText = TranslateCommand(genericCommandText);
            return command;
        }

        public int DeclareParameter(DbCommand command, string name, DbType type)
        {
            DbParameter parameter = null;
            if (!command.Parameters.Contains(name))
            {
                parameter = ProviderFactory.CreateParameter();
                parameter.ParameterName = name;
                parameter.DbType = type;
                return command.Parameters.Add(parameter);
            }
            else
                throw new ArgumentException(string.Format("Parameter {0} already exists.", name));
        }

        public void DefineParameter(DbCommand command, string name, DbType type, object value)
        {
            int parameterIndex = DeclareParameter(command, name, type);
            command.Parameters[parameterIndex].Value = value;
        }

        public void SetParameter(DbCommand command, string name, object value)
        {
            if (command.Parameters.Contains(name))
                ((DbParameter)command.Parameters[name]).Value = value;
            else
                throw new ArgumentException(string.Format("Parameter {0} is not declared.", name));
        }

        public IDataReader ExecuteReader(DbCommand command)
        {
            DbConnection connection = null;
            try
            {
                connection = GetOpenConnection();
                command.Connection = connection;

                var behavior = Transaction.Current == null ?
                     CommandBehavior.CloseConnection :
                     CommandBehavior.Default;
                return command.ExecuteReader(behavior);
            }
            catch // catch any exception
            {
                ReleaseConnection(connection); // close the connection
                throw;                         // and rethrow the exception
            }
        }

        public int ExecuteNonQuery(DbCommand command)
        {
            DbConnection connection = null;
            try
            {
                connection = GetOpenConnection();
                command.Connection = connection;
                return command.ExecuteNonQuery();
            }
            finally
            {
                ReleaseConnection(connection);
            }
        }

        protected string TranslateCommand(string genericCommand)
        {
            string connName = ProviderFactory.GetType().Name;
            switch (connName)
            {
                case "OleDbFactory":
                    return paramNameRegex.Replace(genericCommand, "?");
                case "OracleClientFactory":
                    return genericCommand.Replace("@", ":");
                default:
                    // generic format == sql server format
                    return genericCommand;
            }
        }

        [ThreadStatic]
        private static DbConnection sharedConnection;

        private DbConnection CreateOpenConnection()
        {
            DbConnection connection = ProviderFactory.CreateConnection();
            connection.ConnectionString = connectionString;
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
                    currentTransaction.TransactionCompleted += (s, e) =>
                    {
                        sharedConnection.Close();
                        sharedConnection = null;
                    };
                }

                return sharedConnection;
            }
        }

        private void ReleaseConnection(DbConnection connection)
        {
            // close connection if no transaction is active
            if (connection != null && Transaction.Current == null)
                connection.Close();
        }
    }
}
