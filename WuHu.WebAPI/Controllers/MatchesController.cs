using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WuHu.BusinessLogic;
using WuHu.Common;
using WuHu.Domain;

namespace WuHu.WebAPI.Controllers
{
	[RoutePrefix("api/matches")]
	public class MatchesController : ApiController
	{
		private IDatabase database = DalFactory.CreateDatabase();

		[HttpGet]
		[Route("")]
		public IEnumerable<Match> GetAll()
		{
			IMatchDao MatchDao = DalFactory.CreateMatchDao(database);
			return MatchDao.FindAll();
		}

		[HttpGet]
		[Route("{id}")]
		public Match FindById(int id)
		{
			IMatchDao MatchDao = DalFactory.CreateMatchDao(database);
			return MatchDao.FindById(id);
		}

		[HttpPut]
		[Route("{id}")]
		public void Update(Match match)
		{
			IMatchDao MatchDao = DalFactory.CreateMatchDao(database);
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			Player w1;
			Player w2;
			Player v1;
			Player v2;

			Match m = MatchDao.FindById(match.ID);

			if(match.ResultPointsPlayer1 != null && match.ResultPointsPlayer2 != null 
					&& (match.ResultPointsPlayer1 < 10 || match.ResultPointsPlayer2 < 10))
			{
				if (match.ResultPointsPlayer1 == 10)
				{
					w1 = PlayerDao.FindById(match.Team1Player1);
					w2 = PlayerDao.FindById(match.Team1Player2);
					v1 = PlayerDao.FindById(match.Team2Player1);
					v2 = PlayerDao.FindById(match.Team2Player2);
				}
				else
				{
					w1 = PlayerDao.FindById(match.Team2Player1);
					w2 = PlayerDao.FindById(match.Team2Player2);
					v1 = PlayerDao.FindById(match.Team1Player1);
					v2 = PlayerDao.FindById(match.Team1Player2);
				}
				
				BLPlayer.UpdateElo(w1, w2, v1, v2);

				BLStatistic.Insert(w1.ID, w1.Skills);
				BLStatistic.Insert(w2.ID, w2.Skills);
				BLStatistic.Insert(v1.ID, v1.Skills);
				BLStatistic.Insert(v2.ID, v2.Skills);

				PlayerDao.Update(w1);
				PlayerDao.Update(w2);
				PlayerDao.Update(v1);
				PlayerDao.Update(v2);
			}
		}

		[HttpPost]
		[Route("")]
		public int Insert(Match team)
		{
			IMatchDao MatchDao = DalFactory.CreateMatchDao(database);
			return MatchDao.Insert(team);
		}

		[HttpPost]
		[Route("generate")]
		public ObservableCollection<Match> GenerateMatches([FromBody]MatchGenerate MatchObj)
		{
			IMatchDao MatchDao = DalFactory.CreateMatchDao(database);
			ObservableCollection<Match> matchList;
			matchList = BLMatch.GenerateMatches(MatchObj.NumberOfMatches, MatchObj.chosenPlayers, MatchObj.tournamentId);
			if(BLMatch.insertMatches(matchList))
				return matchList;

			return null;
		}

		[HttpDelete]
		[Route("{id}")]
		public bool DeleteById(int id)
		{
			IMatchDao MatchDao = DalFactory.CreateMatchDao(database);
			return MatchDao.DeleteById(id);
		}
	}
}
