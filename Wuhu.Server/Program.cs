using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;
using WuHu.Domain;
using WuHu.SQLServer;

namespace Wuhu.Server
{
	public class Program
	{
		private static void PrintTitle(string title, ConsoleColor color = ConsoleColor.Yellow)
		{
			ConsoleColor oldColor = Console.ForegroundColor;
			Console.ForegroundColor = color;
			Console.WriteLine("---------- {0} ----------", title);
			Console.ForegroundColor = oldColor;
		}
		static void Main(string[] args)
		{
			PrintTitle("WuHu Server");

            string connString = ConfigurationManager
                        .ConnectionStrings["DefaultConnectionString"]
                        .ConnectionString;

            IDatabase database = new Database(connString);

            Assert.IsNotNull(database);

            // delete Days, in case database was not empty
            MatchDao MatchDao = new MatchDao(database);

            Console.WriteLine(MatchDao.FindAll().Count);
            // Console.Read();
            // Assert.AreEqual(allPlayers.Count * allDays.Count, pd.FindAll().Count);


            Console.Read();
		}
	}
}
