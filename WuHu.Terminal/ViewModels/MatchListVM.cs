using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;
using WuHu.WebAPI;

namespace WuHu.Terminal.ViewModels
{
	class MatchListVM : INotifyPropertyChanged
	{
		private const string BASE_URL = "http://localhost:42382/";

		public event PropertyChangedEventHandler PropertyChanged;
		private static MatchListVM instance;
		public ObservableCollection<MatchVM> Matches { get; set; }
		
		public static MatchListVM getInstance()
		{
			if (instance == null)
			{
				instance = new MatchListVM();
			}
			return instance;
		}

		private MatchListVM()
		{
			Matches = new ObservableCollection<MatchVM>();
			LoadMatches();
		}

		private async void LoadMatches()
		{
			Matches.Clear();

			string json;
			HttpClient client = new HttpClient();
			
			TokenComb token = new TokenComb();
			token.Nickname = Authentication.getLoggedInUser.Nickname;
			token.Token = Authentication.token.Token.Token;

			var val = Newtonsoft.Json.JsonConvert.SerializeObject(token);
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			client.DefaultRequestHeaders.Add("Authorization", val);

			json = await client.GetStringAsync(BASE_URL + "api/matches");

		
			ObservableCollection<Match> matches = JsonConvert.DeserializeObject<ObservableCollection<Match>>(json);
			var tlvm = TournamentListVM.getInstance();
			await tlvm.LoadTournaments();
			foreach (var m in matches)
			{
				Matches.Add(new MatchVM(m));
			}
		}

		private MatchVM currentMatch;
		public MatchVM CurrentMatch
		{
			get { return currentMatch; }
			set
			{
				if (value != currentMatch)
				{
					currentMatch = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentMatch)));
				}
			}
		}
	}
}
