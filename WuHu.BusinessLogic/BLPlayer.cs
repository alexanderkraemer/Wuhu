using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Resources;
using WuHu.Common;
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
					list = playerlist.Where(player => player.isTuesday);
					break;
				case DayOfWeek.Wednesday:
					list = playerlist.Where(player => player.isWednesday);
					break;
				case DayOfWeek.Thursday:
					list = playerlist.Where(player => player.isThursday);
					break;
				case DayOfWeek.Friday:
					list = playerlist.Where(player => player.isFriday);
					break;
				case DayOfWeek.Saturday:
					list = playerlist.Where(player => player.isSaturday);
					break;
			}
			return list;
		}

		public static bool Authenticate(AuthObj obj)
		{
			IDatabase database = DalFactory.CreateDatabase();
			IPlayerDao dao = DalFactory.CreatePlayerDao(database);

			string nickname = obj.Nickname;
			string password = obj.HashedPassword;

			Player p = dao.FindByNickname(nickname);
			if(p == null)
			{
				return false;
			}

			return BLAuthentication.Verify(password, p.Password);
		}
	}
}
