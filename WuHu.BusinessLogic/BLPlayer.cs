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
		public static void Decay()
		{
			IDatabase database = DalFactory.CreateDatabase();
			IPlayerDao pDao = DalFactory.CreatePlayerDao(database);
			IStatisticDao sDao = DalFactory.CreateStatisticDao(database);

			List<Player> pList = pDao.FindAll().Where(p =>
			{
				DateTime maxdate = sDao.FindByPlayer(p.ID).Max(s => s.Timestamp);

				return DateTime.Now.AddDays(-60) < maxdate;
			}).ToList();

			foreach (Player p in pList)
			{
				p.Skills = (int)(p.Skills *  0.98);
				pDao.Update(p);
			}
		}

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

		public static void UpdateElo(Player winner1, Player winner2, Player loser1, Player loser2)
		{
			// compute enemy rating as average of enemy
			double winnerEnemies = (loser1.Skills + loser2.Skills) / 2.0;
			double loserEnemies = (winner1.Skills + winner2.Skills) / 2.0;

			// compute expected outcome for each player based on enemy rating
			double winner1Exp = 1.0 / (1.0 + Math.Pow(10, (winnerEnemies - winner1.Skills) / 400.0));
			double winner2Exp = 1.0 / (1.0 + Math.Pow(10, (winnerEnemies - winner2.Skills) / 400.0));
			double loser1Exp = 1.0 / (1.0 + Math.Pow(10, (loserEnemies - loser1.Skills) / 400.0));
			double loser2Exp = 1.0 / (1.0 + Math.Pow(10, (loserEnemies - loser2.Skills) / 400.0));

			// compute new rating based on expected outcome
			winner1.Skills = (int)Math.Round(winner1.Skills + 10 * (1 - winner1Exp));
			winner2.Skills = (int)Math.Round(winner2.Skills + 10 * (1 - winner2Exp));
			loser1.Skills = (int)Math.Round(loser1.Skills + 10 * (0 - loser1Exp));
			loser2.Skills = (int)Math.Round(loser2.Skills + 10 * (0 - loser2Exp));
		}

		public static bool Authenticate(AuthObj obj)
		{
			IDatabase database = DalFactory.CreateDatabase();
			IPlayerDao dao = DalFactory.CreatePlayerDao(database);

			string nickname = obj.Nickname;
			string password = obj.HashedPassword;

			Player p = dao.FindByNickname(nickname);
			if (p == null)
			{
				return false;
			}

			return BLAuthentication.Verify(password, p.Password);
		}
	}
}
