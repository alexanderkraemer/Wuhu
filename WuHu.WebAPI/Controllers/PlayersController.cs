using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using WuHu.BusinessLogic;
using WuHu.Common;
using WuHu.Domain;
using WuHu.SQLServer;

namespace WuHu.WebAPI.Controllers
{
	[RoutePrefix("api/players")]
	public class PlayersController : ApiController
	{
		private IDatabase database = DalFactory.CreateDatabase();

		[HttpGet]
		[Route("")]
		public IEnumerable<Player> GetAll()
		{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			return PlayerDao.FindAll();
		}

		[HttpGet]
		[Route("byday/{day}")]
		public IEnumerable<Player> GetPlayerByDay (DateTime day)
		{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			return BLPlayer.GetPlayerByDay(day, PlayerDao.FindAll());
		}

		[HttpGet]
		[Route("{id}")]
		public Player FindById(int id)
		{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			return PlayerDao.FindById(id);
		}

		[HttpPut]
		[Route("{player}")]
		public HttpResponseMessage Update([FromBody]Player player)
		{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			PlayerDao.Update(player);

			var response = Request.CreateResponse(HttpStatusCode.Created);

			return response;

		}

		[HttpPost]
		[Route("")]
		public int Insert(Player player)
		{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			return PlayerDao.Insert(player);
		}
		
		[HttpGet]
		[Route("nickname/{nickname}")]
		public Player FindByNickname(string nickname)
		{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			return PlayerDao.FindByNickname(nickname);
		}

		[HttpDelete]
		[Route("{id}")]
		public bool DeleteById(int id)
		{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			return PlayerDao.DeleteById(id);
		}
	}
}
