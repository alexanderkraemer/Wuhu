using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.BusinessLogic
{
	public class BLMatch
	{
		private static int randomIndex = 0;
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

		private static ObservableCollection<Player> CalculatePotentialPlayer(ObservableCollection<Player> playerList)
		{
			Random rand = new Random();

			var list = playerList.OrderBy(r => r.Skills);

			var randomIndex = rand.Next(0, list.Count() - 1);
			var skillMid = list.ElementAt(randomIndex).Skills;

			
			var potentialPlayers = new ObservableCollection<Player>();

			while (potentialPlayers.Count() < 4)
			{
				skillMid = list.ElementAt(randomIndex).Skills;
				randomIndex = rand.Next(0, list.Count() - 1);
				potentialPlayers = (ObservableCollection<Player>)list.Where(param =>
				{
					return param.Skills <= skillMid + 100 && param.Skills >= skillMid - 100;
				});
			}
			return potentialPlayers;
		}

		private static Match GenerateMatch(ObservableCollection<Player> potentialPlayerListOfRange, int tournamentId)
		{
			var list = potentialPlayerListOfRange.OrderBy(p => p.Skills);

			Random rand = new Random();
			int maxIndex = list.Count()-1;
			
			randomIndex = rand.Next(0, maxIndex);
			Player p1 = list.ElementAt(randomIndex);

			double randDouble = rand.NextDouble();
			

			Player p2 = list.ElementAt(NextIndex(randDouble, maxIndex));
			Player p3 = list.ElementAt(NextIndex(randDouble, maxIndex));
			Player p4 = list.ElementAt(NextIndex(randDouble, maxIndex));

			return new Match(p1.ID, p2.ID, p3.ID, p4.ID, tournamentId);
		}

		private static int NextIndex(double randDouble, int maxIndex)
		{
			Random rand = new Random();
			while (randDouble > 0.4 && randomIndex++ < maxIndex)
			{
				randDouble = rand.NextDouble();
			}
			return randomIndex;
		}
	}
}
