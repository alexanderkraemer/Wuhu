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
	 [RoutePrefix("api/statistics")]
    public class StatisticsController : ApiController
    {

		private IDatabase database = DalFactory.CreateDatabase();

		[HttpGet]
		[Route("")]
		public IEnumerable<Statistic> GetAll()
		{
			IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
			return StatisticDao.FindAll();
		}

		[HttpGet]
		[Route("{id}")]
		public Statistic FindById(int id)
		{
			IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
			return StatisticDao.FindById(id);
		}

		[HttpGet]
		[Route("player/{player_id}")]
		public IEnumerable<Statistic> FindByPlayer(int player_id)
		{
			IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
			return StatisticDao.FindByPlayer(player_id);
		}

		[HttpGet]
		[Route("day/{timestamp}")]
		public IEnumerable<Statistic> FindByTimestamp(DateTime timestamp)
		{
			IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
			return StatisticDao.FindByDay(timestamp);
		}

		[HttpPut]
		[Route("{id}")]
		public void Update(Statistic team)
		{
			IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
			StatisticDao.Update(team);
		}

		[HttpPost]
		[Route("")]
		public int Insert(Statistic team)
		{
			IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
			return StatisticDao.Insert(team);
		}

		[HttpDelete]
		[Route("{id}")]
		public bool DeleteById(int id)
		{
			IStatisticDao StatisticDao = DalFactory.CreateStatisticDao(database);
			return StatisticDao.DeleteById(id);
		}
	}
}
