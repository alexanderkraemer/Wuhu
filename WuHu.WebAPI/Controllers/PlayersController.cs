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
		[Route("{playerId}")]
		public void Update([FromBody]Player player, int playerId)
		{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);
			Player p = new Player(playerId, player.isAdmin, player.FirstName, player.LastName,
				player.Nickname, player.Skills, player.PhotoPath, player.Password, player.isMonday,
				player.isTuesday, player.isWednesday, player.isThursday, player.isFriday, player.isSaturday);
			PlayerDao.Update(p);
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
