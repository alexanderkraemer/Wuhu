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
		public void Update(Match team)
		{
			IMatchDao MatchDao = DalFactory.CreateMatchDao(database);
			MatchDao.Update(team);
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
