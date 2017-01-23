using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;
using WuHu.Domain;
using WuHu.SQLServer;

namespace WuHu.BusinessLogic
{
	public class BLMatch
	{
		private static Random rand = new Random();
		
		public static ObservableCollection<Match> GenerateMatches(int numberOfMatchesToGenerate, ObservableCollection<Player> playerList, int tournamentId)
		{
			var retList = new ObservableCollection<Match>();
			var potentialPlayerList = CalculatePotentialPlayer(playerList);

			for (int i = 0; i < numberOfMatchesToGenerate; ++i)
			{
				retList.Add(GenerateMatch(potentialPlayerList, tournamentId));
			}
			return retList;
		}

		public static bool insertMatches(ObservableCollection<Match> matchList)
		{
			var database = DalFactory.CreateDatabase();
			var MatchDao = DalFactory.CreateMatchDao(database);
			List<int> intList = new List<int>();
			foreach(Match m in matchList)
			{
				var numb = MatchDao.Insert(m);
				intList.Add(numb); 
			}

			if(intList.Count(p => p == -1) > 0)
			{
				return false;
			}
			return true;
		}

		private static ObservableCollection<Player> CalculatePotentialPlayer(ObservableCollection<Player> playerList)
		{
			var list = playerList.OrderBy(r => r.Skills);

			var randomIndex = rand.Next(0, list.Count() - 1);
			var skillMid = list.ElementAt(randomIndex).Skills;

			/*
			var potentialPlayers = new ObservableCollection<Player>();

			while (potentialPlayers.Count() < 4)
			{
				skillMid = list.ElementAt(randomIndex).Skills;
				randomIndex = rand.Next(0, list.Count() - 1);
				var potentialPlayersList = list.Where(param =>
				{
					return param.Skills <= skillMid + 100 && param.Skills >= skillMid - 100;
				});

				foreach(Player p in potentialPlayersList)
				{
					potentialPlayers.Add(p);
				}
			}
			*/
			return playerList;
		}

		private static Match GenerateMatch(ObservableCollection<Player> potentialPlayerListOfRange, int tournamentId)
		{
			var list = potentialPlayerListOfRange.OrderBy(p => p.Skills);

			int maxIndex = list.Count()-2;
			
			int randomIndex = rand.Next(0, maxIndex);
			Player p1;
			Player p2;
			Player p3;
			Player p4;

			p1 = list.ElementAt(randomIndex);

			var orderedByDistancelist = list.OrderBy(p => Math.Abs(p.Skills - p1.Skills)); 

			p2 = orderedByDistancelist.ElementAt(NextIndex(rand.NextDouble(), maxIndex));
			while (p2 == p1)
			{
				p2 = orderedByDistancelist.ElementAt(NextIndex(rand.NextDouble(), maxIndex));
			}
			p3 = orderedByDistancelist.ElementAt(NextIndex(rand.NextDouble(), maxIndex));
			while (p3 == p1 || p3 == p2)
			{
				p3 = orderedByDistancelist.ElementAt(NextIndex(rand.NextDouble(), maxIndex));
			}
			p4 = orderedByDistancelist.ElementAt(NextIndex(rand.NextDouble(), maxIndex));
			while (p4 == p1 || p4 == p2 || p4 == p3)
			{
				p4 = orderedByDistancelist.ElementAt(NextIndex(rand.NextDouble(), maxIndex));
			}
			return new Match(p1.ID, p2.ID, p3.ID, p4.ID, tournamentId, false);
		}

		public static void IncrementValue(SocketObj obj)
		{
			var database = DalFactory.CreateDatabase();
			var dao = DalFactory.CreateMatchDao(database);

			Match match = dao.FindById(obj.match);
			if (obj.team == 1)
			{
				if(match.ResultPointsPlayer1 == null)
				{
					match.ResultPointsPlayer1 = 0;
					if (match.ResultPointsPlayer2 == null)
					{
						match.ResultPointsPlayer2 = 0;
					}
				}
				match.ResultPointsPlayer1++;
			}
			else if (obj.team == 2)
			{
				if (match.ResultPointsPlayer2 == null)
				{
					match.ResultPointsPlayer2 = 0;
					if (match.ResultPointsPlayer1 == null)
					{
						match.ResultPointsPlayer1 = 0;
					}
				}
				match.ResultPointsPlayer2++;
			}

			dao.Update(match);
		}

		private static int NextIndex(double randDouble, int maxIndex)
		{
			int randomIndex = 0;
			int i = 0;
			while (randDouble > 0.4)
			{
				if(randomIndex > maxIndex)
				{
					randomIndex = 0;
				}
				i++;
				randomIndex++;
				randDouble = rand.NextDouble();
			}
			return randomIndex;
		}
	}

	public class SocketObj
	{
		public int match;
		public int team;
		public int number;
	}
}
