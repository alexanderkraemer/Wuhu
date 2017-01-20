using Newtonsoft.Json;
using OxyPlot;
using OxyPlot.Axes;
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
using System.Windows.Input;
using WuHu.Domain;
using WuHu.WebAPI.Controllers;

namespace WuHu.Terminal.ViewModels
{
	public class HomeVM : INotifyPropertyChanged
	{
		private const string BASE_URL = "http://localhost:42382/";

		public event PropertyChangedEventHandler PropertyChanged;
		private static HomeVM instance;
		public ObservableCollection<Tuple<int, PlayerVM>> RankList { get; set; }
		public ObservableCollection<MatchVM> AgendaList { get; set; }
		public IEnumerable<Serialize> StatisticList { get; set; }
		public static HomeVM getInstance()
		{
			if (instance == null)
			{
				instance = new HomeVM();
			}
			return instance;
		}

		private HomeVM()
		{
			RankList = new ObservableCollection<Tuple<int, PlayerVM>>();
			AgendaList = new ObservableCollection<MatchVM>();
			StatisticList = new List<Serialize>();
			LoadRanks();
			LoadAgenda();
			LoadStatistics();
		}

		private async void LoadAgenda()
		{
			var list = new ObservableCollection<MatchVM>();
			IEnumerable<MatchVM> listMatchIEn;
			AgendaList.Clear();

			string json;
			HttpClient client = new HttpClient();
			json = await client.GetStringAsync(BASE_URL + "api/matches/agenda");
			var val = Newtonsoft.Json.JsonConvert.SerializeObject(Authentication.token.Token.Token);
			new HttpRequestMessage().Headers.Add("Authorization", val);
			ObservableCollection<Match> matches = JsonConvert.DeserializeObject<ObservableCollection<Match>>(json);
			var tlvm = TournamentListVM.getInstance();
			await tlvm.LoadTournaments();
			foreach (var m in matches)
			{
				list.Add(new MatchVM(m));
			}

			listMatchIEn = list.Where(m =>
			{
				return m.Tournament.Timestamp >= DateTime.Today 
				&& m.ResultPointsPlayer1 == null 
				&& m.ResultPointsPlayer2 == null;
			});

			foreach(MatchVM m in listMatchIEn)
			{
				AgendaList.Add(m);
			}
			
		}

		private async void LoadRanks()
		{
			var list = new ObservableCollection<PlayerVM>();
			IEnumerable<PlayerVM> listRankIEn;
			RankList.Clear();

			string json;
			HttpClient client = new HttpClient();
			json = await client.GetStringAsync(BASE_URL + "api/players/ranks");
			
			ObservableCollection<Player> players = JsonConvert.DeserializeObject<ObservableCollection<Player>>(json);
			var tlvm = PlayerListVM.getInstance();
			await tlvm.LoadPlayer();
			

			foreach (var p in players)
			{
				list.Add(new PlayerVM(p));
			}

			listRankIEn = list.OrderByDescending(m => m.Skills);

			int count = 0;
			foreach (PlayerVM p in listRankIEn)
			{
				RankList.Add(Tuple.Create(++count, p));
			}
		}

		private async void LoadStatistics()
		{
			string json;
			HttpClient client = new HttpClient();

			json = await client.GetStringAsync(BASE_URL + "api/statistics");

			IEnumerable<Serialize> playerList = JsonConvert.DeserializeObject<List<Serialize>>(json);

			StatisticList = playerList;
			
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatisticList)));
		}

		ICommand _loadRanks;
		public ICommand LoadRanksCommand
		{
			get
			{
				if (_loadRanks == null)
				{
					_loadRanks = new RelayCommand(param =>
					{
						LoadRanks();
						PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RankList)));
					});
				}
				return _loadRanks;
			}
		}

		ICommand _loadMatches;
		public ICommand LoadMatchesCommand
		{
			get
			{
				if (_loadMatches == null)
				{
					_loadMatches = new RelayCommand(param =>
					{
						LoadAgenda();
						PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AgendaList)));
					});
				}
				return _loadMatches;
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
