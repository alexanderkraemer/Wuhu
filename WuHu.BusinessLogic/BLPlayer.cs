using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BusinessLogic
{
	public class BLPlayer
	{
		public static IEnumerable<Player> GetPlayerByDay(DateTime date, IEnumerable<Player> playerlist)
		{
			IEnumerable<Player> list = new List<Player>();

			switch (date.DayOfWeek)
			{
				case DayOfWeek.Monday:
					list = playerlist.Where(player => player.isMonday);
					break;
				case DayOfWeek.Tuesday:
					list = playerlist.Where(player => player.isMonday);
					break;
				case DayOfWeek.Wednesday:
					list = playerlist.Where(player => player.isMonday);
					break;
				case DayOfWeek.Thursday:
					list = playerlist.Where(player => player.isMonday);
					break;
				case DayOfWeek.Friday:
					list = playerlist.Where(player => player.isMonday);
					break;
				case DayOfWeek.Saturday:
					list = playerlist.Where(player => player.isMonday);
					break;
			}
			return list;
		}
	}
}
