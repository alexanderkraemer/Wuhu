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
using System.Web;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Linq;
using System.Drawing;

namespace WuHu.WebAPI.Controllers
{
	[RoutePrefix("api/players")]
	public class PlayersController : ApiController
	{
		private IDatabase database = DalFactory.CreateDatabase();

		[HttpGet]
		[Route("")]
		public HttpResponseMessage GetAll()
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);
			return Request.CreateResponse<IList<Player>>(HttpStatusCode.OK, PlayerDao.FindAll());
			//}
			//else
			//{
			//	return Request.CreateResponse(HttpStatusCode.Forbidden);
			//}
		}

		[HttpGet]
		[Route("ranks")]
		public HttpResponseMessage GetRanks()
		{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			return Request.CreateResponse<IOrderedEnumerable<Player>>(PlayerDao.FindAll().OrderByDescending(s => s.Skills));
		}

		[Route("image/{playerId}")]
		[HttpPost]
		public HttpResponseMessage PostImage([FromBody] byte[] file, int playerId)
		{
			IPlayerDao playerDAO = DalFactory.CreatePlayerDao(database);
			Player player = playerDAO.FindById(playerId);

			try
			{
				using (MemoryStream ms = new MemoryStream(file))
				{
					Image.FromStream(ms);
				}
			}
			catch (ArgumentException)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest);
			}

			string absolutePath = ConfigurationManager.AppSettings["ImageFolder"].ToString() + "\\" + player.PhotoPath;

			try
			{
				File.WriteAllBytes(absolutePath, file);
				return Request.CreateResponse(HttpStatusCode.OK);
			}
			catch
			{
				return Request.CreateResponse(HttpStatusCode.BadGateway);
			}
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

			if (player == null || !File.Exists(absolutePath + player.PhotoPath))
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
			response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
			return response;
		}

		[HttpGet]
		[Route("byday/{day}")]
		public HttpResponseMessage GetPlayerByDay(DateTime day)
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			return Request.CreateResponse<IEnumerable<Player>>(HttpStatusCode.OK,
				BLPlayer.GetPlayerByDay(day, PlayerDao.FindAll()));
			//}
			//else
			//{
			//	return new HttpResponseMessage(HttpStatusCode.Forbidden);
			//}
		}

		[HttpGet]
		[Route("{id}")]
		public HttpResponseMessage FindById(int id)
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			return Request.CreateResponse<Player>(HttpStatusCode.OK,
				PlayerDao.FindById(id));
			//}
			//else
			//{
			//	return new HttpResponseMessage(HttpStatusCode.Forbidden);
			//}
		}

		[HttpPut]
		[Route("{playerId}")]
		public HttpResponseMessage Update([FromBody]Player player, int playerId)
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			Player pl = PlayerDao.FindById(player.ID);

			Player p = new Player(playerId, player.isAdmin, player.FirstName, player.LastName,
				player.Nickname, player.Skills, pl.PhotoPath, player.Password, player.isMonday,
				player.isTuesday, player.isWednesday, player.isThursday, player.isFriday, player.isSaturday);
			var boo = PlayerDao.Update(p);

			if (boo)
			{
				return new HttpResponseMessage(HttpStatusCode.OK);
			}
			else
			{
				return Request.CreateResponse(HttpStatusCode.Conflict);
			}

			//}
		}

		[HttpPost]
		[Route("")]
		public HttpResponseMessage Insert([FromBody]Player player)
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);
			player.Password = BLAuthentication.Hash(player.Password);

			int id = PlayerDao.Insert(player);
			if (id == -1)
			{
				return new HttpResponseMessage(HttpStatusCode.Conflict);
			}
			else
			{
				return Request.CreateResponse<int>(HttpStatusCode.OK, id);
			}
			//}
			//else
			//{
			//	return new HttpResponseMessage(HttpStatusCode.Forbidden);
			//}
		}

		[HttpPost]
		[Route("auth")]
		public HttpResponseMessage Authenticate([FromBody]AuthObj obj)
		{

			bool isAuthenticated = BLPlayer.Authenticate(obj);

			if (!isAuthenticated)
			{
				return Request.CreateResponse<bool>(HttpStatusCode.Forbidden, false);
			}
			else
			{
				IDatabase db = DalFactory.CreateDatabase();
				IPlayerDao dao = DalFactory.CreatePlayerDao(db);



				var player = dao.FindByNickname(obj.Nickname);

				var token = Authentication.getInstance().newAuthentication(obj.Nickname);

				ResponseObject r = new ResponseObject(token.Token, player);

				return Request.CreateResponse<ResponseObject>(HttpStatusCode.Created, r);
				//return new HttpResponseMessage(HttpStatusCode.OK);
			}
		}

		[HttpPost]
		[Route("photo/{nickname}")]
		public HttpResponseMessage uploadPhoto(string nickname)
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
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
			//}
			//else
			//{
			//	return new HttpResponseMessage(HttpStatusCode.Forbidden);
			//}

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
		public HttpResponseMessage FindByNickname(string nickname)
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			return Request.CreateResponse<Player>(HttpStatusCode.OK,
				PlayerDao.FindByNickname(nickname));
			//}
			//else
			//{
			//	return new HttpResponseMessage(HttpStatusCode.Forbidden);
			//}
		}

		[HttpDelete]
		[Route("{id}")]
		public HttpResponseMessage DeleteById(int id)
		{
			//if (Authentication.getInstance().isAuthenticateWithHeader(Request))
			//{
			IPlayerDao PlayerDao = DalFactory.CreatePlayerDao(database);

			return Request.CreateResponse<bool>(HttpStatusCode.OK,
				PlayerDao.DeleteById(id));
			//}
			//else
			//{
			//	return new HttpResponseMessage(HttpStatusCode.Forbidden);
			//}
		}
	}

	[DataContract]
	public class ResponseObject
	{
		public ResponseObject(string token, Player player)
		{
			Token = token;
			Player = player;
		}

		[DataMember]
		public string Token { get; set; }
		[DataMember]
		public Player Player { get; set; }
	}
}
