using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
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
		[Route("img/{nickname}")]
		public Image GetImage(string nickname)
		{
			return BLPlayer.GetImageByNickname(nickname);
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
		public HttpResponseMessage Insert([FromBody]Player player)
		{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);
			player.Password = BLAuthentication.Hash(player.Password);

			int id = PlayerDao.Insert(player);
			if (id == -1)
			{
				return new HttpResponseMessage(HttpStatusCode.Conflict);
			}
			else
			{
				return new HttpResponseMessage(HttpStatusCode.Created);
			}

		}

		[HttpPost]
		[Route("auth")]
		public HttpResponseMessage Authenticate([FromBody]AuthObj obj)
		{
			bool isAuthenticated = BLPlayer.Authenticate(obj);

			if (!isAuthenticated)
			{
				return new HttpResponseMessage(HttpStatusCode.Conflict);
			}
			else
			{
				return new HttpResponseMessage(HttpStatusCode.OK);
			}

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
