using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using WuHu.BusinessLogic;
using WuHu.Common;
using WuHu.Domain;

namespace WuHu.WebAPI.Controllers
{
	[DataContract]
	public class MatchPaginateClass
	{
		public MatchPaginateClass(int nr, IEnumerable<Match> m)
		{
			this.numberOfMatches = nr;
			this.matchList = m;
		}

		[DataMember]
		private int numberOfMatches;
		[DataMember]
		private IEnumerable<Match> matchList;
	}

	[RoutePrefix("api/matches")]
	public class MatchesController : ApiController
	{
		private IDatabase database = DalFactory.CreateDatabase();

		[HttpGet]
		[Route("")]
		public MatchPaginateClass GetAll()
		{
			IMatchDao MatchDao = DalFactory.CreateMatchDao(database);
			var list = MatchDao.FindAll();
			
			MatchPaginateClass mpc = new MatchPaginateClass(list.Count, list);
			return mpc;
		}

		[HttpGet]
		[Route("page/{page}/{numberPerPage}")]
		public MatchPaginateClass GetAllByPage(int page, int numberPerPage)
		{
			IMatchDao MatchDao = DalFactory.CreateMatchDao(database);
			var list = MatchDao.FindAll();
			var remain = list.Count - (page - 1) * numberPerPage;
			var count = remain >= numberPerPage ? numberPerPage : remain;

			var newlist = list.ToList().GetRange(((page - 1) * numberPerPage), count);
			MatchPaginateClass mpc = new MatchPaginateClass(list.Count, newlist);
			return mpc;
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


			if (match.Finished)
			{
				Player w1;
				Player w2;
				Player v1;
				Player v2;

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
			MatchDao.Update(match);
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
			if (BLMatch.insertMatches(matchList))
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
