using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;
using WuHu.Domain;
using WuHu.GenerateTestData;
using WuHu.SQLServer;

namespace WuHu.GenerateTestData
{
	class Program
	{
		private static IDatabase database;

		static void Main(string[] args)
		{
			
			database = DalFactory.CreateDatabase();

			
			Console.WriteLine("done.");
			Console.WriteLine("press {Enter} to quit this program...");
			Console.Read();
		}
	}

}
