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
	[RoutePrefix("api/statistics")]
    public class StatisticsController : ApiController
    {
		private IDatabase database = DalFactory.CreateDatabase();

		[HttpGet]
		[Route("")]
		public HttpResponseMessage GetAll()
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
			IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			var retList = new List<Serialize>();

			foreach (Player p in PlayerDao.FindAll())
			{

				var list = StatisticDao.FindAll().Where(s => { return s.PlayerID == p.ID; }).ToList();

				retList.Add(new Serialize(p, list));
			}

			return Request.CreateResponse<List<Serialize>>(HttpStatusCode.OK, retList);
			//}
			//else
			//{
			//	return Request.CreateResponse(HttpStatusCode.Forbidden);
			//}
		}

		[HttpGet]
		[Route("{id}")]
		public HttpResponseMessage FindById(int id)
		{
			if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			{
				IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
				return Request.CreateResponse<Statistic>(HttpStatusCode.OK, StatisticDao.FindById(id));
			}
			else
			{
				return Request.CreateResponse(HttpStatusCode.Forbidden);
			}
		}

		[HttpGet]
		[Route("player/{player_id}")]
		public HttpResponseMessage FindByPlayer(int player_id)
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
				IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
				return Request.CreateResponse<IList<Statistic>>(HttpStatusCode.OK, StatisticDao.FindByPlayer(player_id));
			//}
			//else
			//{
			//	return Request.CreateResponse(HttpStatusCode.Forbidden);
			//}
		}

		[HttpGet]
		[Route("day/{timestamp}")]
		public HttpResponseMessage FindByTimestamp(DateTime timestamp)
		{
			if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			{
				IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
				return Request.CreateResponse<IList<Statistic>>(HttpStatusCode.OK, StatisticDao.FindByDay(timestamp));
			}
			else
			{
				return Request.CreateResponse(HttpStatusCode.Forbidden);
			}
		}

		[HttpPut]
		[Route("{id}")]
		public void Update(Statistic team)
		{
			if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			{
				IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
				StatisticDao.Update(team);
			}
		}

		[HttpPost]
		[Route("")]
		public HttpResponseMessage Insert(Statistic team)
		{
			if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			{
				IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
				return Request.CreateResponse<int>(HttpStatusCode.OK, StatisticDao.Insert(team));
			}
			else
			{
				return Request.CreateResponse(HttpStatusCode.Forbidden);
			}
		}

		[HttpDelete]
		[Route("{id}")]
		public HttpResponseMessage DeleteById(int id)
		{
			if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			{
				IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
				return Request.CreateResponse<bool>(HttpStatusCode.OK, StatisticDao.DeleteById(id));
			}
			else
			{
				return Request.CreateResponse(HttpStatusCode.Forbidden);
			}
		}
	}

	public class Serialize
	{
		public Serialize(Player p, List<Statistic> l)
		{
			player = p;
			statList = l;
		}

		public Player player { get; set; }
		public List<Statistic> statList { get; set; }
	}
}
