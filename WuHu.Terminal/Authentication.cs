using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Terminal
{
	public class Authentication
	{
		private static Authentication instance;
		private const string BASE_URL = "http://localhost:42382/";
		private Player player;
		public bool isAuthenticated;

		public static Authentication getInstance()
		{
			if(instance == null)
			{
				instance = new Authentication();
			}
			return instance;
		}

		private Authentication()
		{
			isAuthenticated = false;
		}

		public async void Authenticate(string nickname, string password)
		{
			HttpClient client = new HttpClient();
			Tuple<string, string> obj = Tuple.Create(nickname, password);
			string json = JsonConvert.SerializeObject(obj);

			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(BASE_URL + "api/players/auth", httpContent);

			if(response.IsSuccessStatusCode)
			{
				isAuthenticated = true;
			}
			isAuthenticated = false;
		}
	}
}
