using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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

		[HttpDelete]
		[Route("{id}")]
		public bool DeleteById(int id)
		{
			IMatchDao MatchDao = DalFactory.CreateMatchDao(database);
			return MatchDao.DeleteById(id);
		}
	}
}
