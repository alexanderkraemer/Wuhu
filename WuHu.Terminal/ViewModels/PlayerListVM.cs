using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WuHu.Domain;
using WuHu.Terminal.Views.Player;

namespace WuHu.Terminal.ViewModels
{
	public class PlayerListVM : INotifyPropertyChanged
	{
		private const string BASE_URL = "http://localhost:42382/";

		public event PropertyChangedEventHandler PropertyChanged;
		private static PlayerListVM instance;
		public ObservableCollection<PlayerVM> Players { get; set; }

		public static PlayerListVM getInstance()
		{
			if(instance == null)
			{
				instance = new PlayerListVM();
			}
			return instance;
		}

		private PlayerListVM()
		{
			Players = new ObservableCollection<PlayerVM>();
			LoadPlayer();
		}

		public async Task<ObservableCollection<Player>> LoadPlayer()
		{
			string json;
			HttpClient client = new HttpClient();
			json = await client.GetStringAsync(BASE_URL + "api/players");
			
			ObservableCollection<Player> players = JsonConvert.DeserializeObject<ObservableCollection<Player>>(json);
			this.Players.Clear();
			foreach (var p in players)
			{
				Players.Add(new PlayerVM(p));
			}
			return players;
		}
	
		private PlayerVM currentPlayer;
		public PlayerVM CurrentPlayer
		{
			get { return currentPlayer; }
			set
			{
				if (value != currentPlayer)
				{
					currentPlayer = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPlayer)));
				}
			}
		}
	}
}
