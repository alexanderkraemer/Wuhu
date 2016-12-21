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
	[RoutePrefix("api/tournaments")]
    public class TournamentsController : ApiController
    {
		private IDatabase database = DalFactory.CreateDatabase();

		[HttpGet]
		[Route("")]
		public IEnumerable<Tournament> GetAll()
		{
			ITournamentDao TournamentDao = DalFactory.CreateTournamentDao(database);
			return TournamentDao.FindAll();
		}

		[HttpGet]
		[Route("{id}")]
		public Tournament FindById(int id)
		{
			ITournamentDao TournamentDao = DalFactory.CreateTournamentDao(database);
			return TournamentDao.FindById(id);
		}

		[HttpPut]
		[Route("{id}")]
		public void Update([FromBody]Tournament team, int id)
		{
			ITournamentDao TournamentDao = DalFactory.CreateTournamentDao(database);
			Tournament t = new Tournament(id, team.Name, team.Timestamp);
			TournamentDao.Update(t);
		}

		[HttpPost]
		[Route("")]
		public HttpResponseMessage Insert([FromBody]Tournament team)
		{
			ITournamentDao TournamentDao = DalFactory.CreateTournamentDao(database);
			int id = TournamentDao.Insert(team);
			if (id == -1)
			{
				return new HttpResponseMessage(HttpStatusCode.Conflict);
			}
			else
			{
				return new HttpResponseMessage(HttpStatusCode.Created);
			}

		}
		
		[HttpDelete]
		[Route("{id}")]
		public bool DeleteById(int id)
		{
			ITournamentDao TournamentDao = DalFactory.CreateTournamentDao(database);
			return TournamentDao.DeleteById(id);
		}
	}
}
