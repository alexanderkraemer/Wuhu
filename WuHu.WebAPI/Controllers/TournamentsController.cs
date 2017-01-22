using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WuHu.Common;
using WuHu.Domain;

namespace WuHu.WebAPI.Controllers
{
	[EnableCors("*", "*", "*")]
	[RoutePrefix("api/tournaments")]
    public class TournamentsController : ApiController
    {
		private IDatabase database = DalFactory.CreateDatabase();

		[HttpGet]
		[Route("")]
		public HttpResponseMessage GetAll()
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
				ITournamentDao TournamentDao = DalFactory.CreateTournamentDao(database);
				return Request.CreateResponse<IEnumerable<Tournament>>(HttpStatusCode.OK, TournamentDao.FindAll());
			//}
			//else
			//{
				//return new HttpResponseMessage(HttpStatusCode.Forbidden);
			//}
		}
		
		[HttpGet]
		[Route("{id}")]
		public HttpResponseMessage FindById(int id)
		{
			// if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			// {
				ITournamentDao TournamentDao = DalFactory.CreateTournamentDao(database);
				return Request.CreateResponse<Tournament>(TournamentDao.FindById(id));
			//}
			//else
			//{
				//return new HttpResponseMessage(HttpStatusCode.Forbidden);
			//}
		}

		[HttpGet]
		[Route("day/{day}")]
		public HttpResponseMessage FindByDay(DateTime day)
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
				//var time = UnixTimeStampToDateTime(day);
				ITournamentDao TournamentDao = DalFactory.CreateTournamentDao(database);
				return Request.CreateResponse<IList<Tournament>>(TournamentDao.FindByDay(day));
			//}
			//else
			//{
			//	return new HttpResponseMessage(HttpStatusCode.Forbidden);
			//}
		}

		
		[HttpPut]
		[Route("{id}")]
		public HttpResponseMessage Update([FromBody]Tournament team, int id)
		{
			if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			{
				ITournamentDao TournamentDao = DalFactory.CreateTournamentDao(database);
				Tournament t = new Tournament(id, team.Name, team.Timestamp);
				var tx = TournamentDao.FindByDay(t.Timestamp);
				if (tx.Count > 1 && tx.Any(x => x.ID == t.ID))
				{
					return Request.CreateResponse<bool>(HttpStatusCode.Conflict, false);
				}
				else
				{
					TournamentDao.Update(t);
					return Request.CreateResponse<bool>(HttpStatusCode.OK, true);
				}
			}
			return new HttpResponseMessage(HttpStatusCode.Forbidden);
		}

		[HttpPost]
		[Route("")]
		public HttpResponseMessage Insert([FromBody]Tournament team)
		{
			if (Authentication.getInstance().isAuthenticateWithHeader(Request))
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
			else
			{
				return new HttpResponseMessage(HttpStatusCode.Forbidden);
			}
		}
		
		[HttpDelete]
		[Route("{id}")]
		public HttpResponseMessage DeleteById(int id)
		{
			if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			{
				ITournamentDao TournamentDao = DalFactory.CreateTournamentDao(database);
				return Request.CreateResponse<bool>(TournamentDao.DeleteById(id));
			}
			else
			{
				return new HttpResponseMessage(HttpStatusCode.Forbidden);
			}
		}

		[HttpPost]
		[Route("lock")]
		public HttpResponseMessage Lock([FromBody]bool state)
		{
			if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			{
				if (BusinessLogic.BLTournaments.IsLocked)
				{
					return new HttpResponseMessage(HttpStatusCode.Conflict);
				}
				else
				{
					BusinessLogic.BLTournaments.IsLocked = true;
					return new HttpResponseMessage(HttpStatusCode.OK);
				}
			}
			else
			{
				return new HttpResponseMessage(HttpStatusCode.Forbidden);
			}
		}

		[HttpPost]
		[Route("unlock")]
		public HttpResponseMessage Unlock([FromBody]bool state)
		{
			if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			{
				if (BusinessLogic.BLTournaments.IsLocked)
				{
					BusinessLogic.BLTournaments.IsLocked = false;
				}
				return new HttpResponseMessage(HttpStatusCode.OK);
			}
			else
			{
				return new HttpResponseMessage(HttpStatusCode.Forbidden);
			}
		}
	}
}
