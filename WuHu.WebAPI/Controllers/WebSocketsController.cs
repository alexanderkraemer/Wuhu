using Microsoft.Web.WebSockets;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WuHu.BusinessLogic;
using WuHu.Domain;

namespace WuHu.WebAPI.Controllers
{
	[RoutePrefix("api/websocket")]
	public class WebSocketsController : ApiController
	{
		[Route("")]
		public HttpResponseMessage Get()
		{
			HttpContext.Current.AcceptWebSocketRequest(new MyAppWebSocketHandler());
			return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
		}
	 }

	class MyAppWebSocketHandler : WebSocketHandler
	{
		public static WebSocketCollection _connectedChatClients;
		private string _userId = string.Empty;

		static MyAppWebSocketHandler()
		{
			_connectedChatClients = new WebSocketCollection();
		}

		public override void OnOpen()
		{
			base.OnOpen();
			_userId = new Random().Next(1, 10000).ToString(); //Random userId
			_connectedChatClients.Add(this);
			this.Send(_userId);
		}

		public override void OnClose()
		{
			_connectedChatClients.Remove(this);
			base.OnClose();
		}
		public override void OnMessage(string message)
		{
			SocketObj obj = Newtonsoft.Json.JsonConvert.DeserializeObject<SocketObj>(message);
			_connectedChatClients.Broadcast(_userId + " : " + message);

			BLMatch.IncrementValue(obj);
			//base.OnMessage(message);
		}

	}
}
