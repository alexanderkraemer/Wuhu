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
	[RoutePrefix("api/teams")]
    public class TeamsController : ApiController
    {
		private IDatabase database = DalFactory.CreateDatabase();

		[HttpGet]
		[Route("")]
		public IEnumerable<Team> GetAll()
		{
			ITeamDao TeamDao = DalFactory.CreateTeamDao(database);
			return TeamDao.FindAll();
		}

		[HttpGet]
		[Route("{id}")]
		public Team FindById(int id)
		{
			ITeamDao TeamDao = DalFactory.CreateTeamDao(database);
			return TeamDao.FindById(id);
		}

		[HttpPut]
		[Route("{id}")]
		public void Update(Team team)
		{
			ITeamDao TeamDao = DalFactory.CreateTeamDao(database);
			TeamDao.Update(team);
		}

		[HttpPost]
		[Route("")]
		public int Insert(Team team)
		{
			ITeamDao TeamDao = DalFactory.CreateTeamDao(database);
			return TeamDao.Insert(team);
		}
		
		[HttpDelete]
		[Route("{id}")]
		public bool DeleteById(int id)
		{
			ITeamDao TeamDao = DalFactory.CreateTeamDao(database);
			return TeamDao.DeleteById(id);
		}
	}
}
