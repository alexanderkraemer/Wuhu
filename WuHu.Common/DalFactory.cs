using System;
using System.Configuration;
using System.Reflection;


namespace WuHu.Common
{
    public static class DalFactory
    {
        private static string assemblyName;
        private static Assembly dalAssembly;

        static DalFactory()
        {
            assemblyName = ConfigurationManager.AppSettings["DalAssembly"];
            dalAssembly = Assembly.Load(assemblyName);
        }

        public static IDatabase CreateDatabase()
        {
            string connString = ConfigurationManager
                .ConnectionStrings["WuHuDBConnection"]
                .ConnectionString;
            
            return CreateDatabase(connString);
        }

        private static IDatabase CreateDatabase(string connString)
        {
            Type dbClass = dalAssembly.GetType(assemblyName + ".Database");
            return Activator.CreateInstance(dbClass, connString) as IDatabase;
        }
        public static IPlayerDao CreatePlayerDao(IDatabase database)
        {
            return CreateDao<IPlayerDao>(database, "PlayerDao");
        }

        public static IMatchDao CreateMatchDao(IDatabase database)
        {
            return CreateDao<IMatchDao>(database, "MatchDao");
        }

        public static IStatisticDao CreateStatisticDao(IDatabase database)
        {
            return CreateDao<IStatisticDao>(database, "StatisticDao");
        }

        public static ITeamDao CreateTeamDao(IDatabase database)
        {
            return CreateDao<ITeamDao>(database, "TeamDao");
        }

        private static T CreateDao<T>(IDatabase database, string typeName)
            where T : class //T has to be a ref type
        {
            Type daoType = dalAssembly.GetType(assemblyName + "." + typeName);
            return Activator.CreateInstance(daoType, database) as T;
        }
    }
}
