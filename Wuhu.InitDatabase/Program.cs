using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;
using WuHu.InitDatabase;

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
			//PrintTitle("WuHu - nothing to do here...");
			
			PrintTitle("WuHu Server - Create Database");

			IDatabase database = DalFactory.CreateDatabase();
			Assert.IsNotNull(database);
			Console.WriteLine("create database...");
			CreateDatabase(database);
			Console.WriteLine("done.");
			Console.WriteLine("clear database...");
			DatabaseReset.Clear(database);
			Console.WriteLine("done.");
			Console.WriteLine("insert data to database.");
			DatabaseReset.Insert(database);
			Console.WriteLine("done.");
			Console.WriteLine("press {Enter} to quit this program...");
			Console.Read();
			
		}

		public static void CreateDatabase(IDatabase database)
		{
			// hier den Absoluten Pfad zur Dateil "create_database.sql" eingeben:
			string sqlPath = "C:\\Users\\Alexander\\Documents\\CloudStation\\FH\\5. Semester\\SWK\\Uebung\\Projekt\\Ausbaustufe 2\\Wuhu\\create_database.sql";
			string script = File.ReadAllText(sqlPath);
			DbCommand cmd = database.CreateCommand(script);
			database.ExecuteNonQuery(cmd);
		}
	}
}
