using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WuHu.BusinessLogic;
using WuHu.Common;
using WuHu.Domain;
using System.Web.Http.Cors;
using System.Web;
using System.Threading.Tasks;
using System.Diagnostics;

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


		[Route("image/{playerId}")]
		[HttpGet]
		public HttpResponseMessage GetImage(int playerId)
		{
			IPlayerDao playerDAO = DalFactory.CreatePlayerDao(database);
			Player player = playerDAO.FindById(playerId);


			string absolutePath = ConfigurationManager.AppSettings["ImageFolder"].ToString() + "\\";

			HttpResponseMessage response = new HttpResponseMessage();
			Byte[] b;

			if (!File.Exists(absolutePath + player.PhotoPath) || player == null)
			{
				b = (File.ReadAllBytes(absolutePath + "default.png")); 
			}
			else
			{
				absolutePath = absolutePath + player.PhotoPath;
				 b = (File.ReadAllBytes(absolutePath));
			}
			
			response.Content = new ByteArrayContent(b);
			response.Content.LoadIntoBufferAsync(b.Length).Wait();
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
			return response;
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

		[HttpPost]
		[Route("photo/{nickname}")]
		public HttpResponseMessage uploadPhoto(string nickname)
		{
			var httpRequest = HttpContext.Current.Request;
			if (httpRequest.Files.Count < 1)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest);
			}

			foreach (string file in httpRequest.Files)
			{
				var postedFile = httpRequest.Files[file];
				string absolutePath = ConfigurationManager.AppSettings["ImageFolder"].ToString() + "\\";

				var filePath = HttpContext.Current.Server.MapPath(absolutePath + nickname);
				postedFile.SaveAs(filePath);
				// NOTE: To store in memory use postedFile.InputStream
			}

			return Request.CreateResponse(HttpStatusCode.Created);
		}

/*
		[HttpPost]
		[Route("photo/{nickname}")]
		public Task Upload(IFormFile file)
		{
			Debug.WriteLine(file);
			if (file == null) throw new Exception("File is null");
			if (file.Length == 0) throw new Exception("File is empty");

			using (Stream stream = file.OpenReadStream())
			{
				using (var binaryReader = new BinaryReader(stream))
				{
					var fileContent = binaryReader.ReadBytes((int)file.Length);
					await _uploadService.AddFile(fileContent, file.FileName, file.ContentType);
				}
			}
		
		}
*/
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
