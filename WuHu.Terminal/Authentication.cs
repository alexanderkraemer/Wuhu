using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WuHu.Domain;
using WuHu.Terminal.ViewModels;
using WuHu.Terminal.Views;
using WuHu.WebAPI.Controllers;

namespace WuHu.Terminal
{
	public class Authentication
	{
		private static Authentication instance;
		private const string BASE_URL = "http://localhost:42382/";
		private static PlayerVM player;
		public static ResponseObject token;
		public static bool isAuthenticated; // for dev
		private static UserControl currentTab;

		public static void CheckIfLoggedIn(UserControl page)
		{
			currentTab = page;
			if (isAuthenticated)
			{
				MainWindow.main.Content = page;
			}
			else
			{
				MainWindow.main.Content = new Login();
			}
		}

		public static PlayerVM getLoggedInUser
		{
			get { return player; }
		}

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

		public static void GetPlayerData(string nickname)
		{
			foreach(Tuple<int, PlayerVM> t in HomeVM.getInstance().RankList)
			{
				if(t.Item2.Nickname == nickname)
				{
					player = t.Item2;
				}
			}
		}

		public async void Authenticate(string nickname, string password)
		{
			HttpClient client = new HttpClient();
			AuthObj obj = new AuthObj(nickname, password);
			string json = JsonConvert.SerializeObject(obj);

			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(BASE_URL + "api/players/auth", httpContent);

			if(response.IsSuccessStatusCode)
			{
				string val = await response.Content.ReadAsStringAsync();
				token = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseObject>(val);
				isAuthenticated = true;
				GetPlayerData(nickname);
				CheckIfLoggedIn(currentTab);
			}
			else
			{
				isAuthenticated = false;
			}
		}
	}
}
