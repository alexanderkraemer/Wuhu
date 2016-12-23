using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
	public class HomeVM : INotifyPropertyChanged
	{
		private const string BASE_URL = "http://localhost:42382/";

		public event PropertyChangedEventHandler PropertyChanged;
		private static HomeVM instance;
		public ObservableCollection<Tuple<int, PlayerVM>> RankList { get; set; }
		public ObservableCollection<MatchVM> AgendaList { get; set; }
		public ObservableCollection<StatisticVM> StatisticList { get; set; }
		public new ObservableCollection<StatisticVM> list;
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
			StatisticList = new ObservableCollection<StatisticVM>();
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
			json = await client.GetStringAsync(BASE_URL + "api/matches");

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
			AgendaList.Clear();

			string json;
			HttpClient client = new HttpClient();
			json = await client.GetStringAsync(BASE_URL + "api/players");

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
			var list = new ObservableCollection<StatisticVM>();

			string json;
			HttpClient client = new HttpClient();
			 
			json = await client.GetStringAsync(BASE_URL + "api/statistics/player/101");

			ObservableCollection<Statistic> statistics = JsonConvert.DeserializeObject<ObservableCollection<Statistic>>(json);

			foreach (var s in statistics)
			{
				list.Add(new StatisticVM(s));
			}

			//StatisticList = list.Where(m => { return m.Timestamp > DateTime.Today.AddDays(-30); });
			foreach(StatisticVM s in list)
			{
				StatisticList.Add(s);
			}

			//StatisticList = new ObservableCollection<ObservableCollection<StatisticVM>>();
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StatisticList)));
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
