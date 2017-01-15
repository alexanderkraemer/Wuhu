using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;

namespace WuHu.BusinessLogic
{
	public class BLStatistic
	{
		public static void Insert(int playerId, int skill)
		{
			IDatabase database = DalFactory.CreateDatabase();
			IStatisticDao dao = DalFactory.CreateStatisticDao(database);

			dao.Insert(new Domain.Statistic(playerId, skill, DateTime.Now));
		}
	}
}
