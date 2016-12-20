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
	public class TournamentListVM
	{
		private const string BASE_URL = "http://localhost:42382/";

		public event PropertyChangedEventHandler PropertyChanged;
		private static TournamentListVM instance;
		public ObservableCollection<TournamentVM> Teams { get; private set; }
		private PlayerListVM pvm;

		public static TournamentListVM getInstance()
		{
			if (instance == null)
			{
				instance = new TournamentListVM(PlayerListVM.getInstance());
			}
			return instance;
		}

		private TournamentListVM(PlayerListVM vm)
		{
			pvm = vm;
			Teams = new ObservableCollection<TournamentVM>();
			LoadTournaments();
		}

		public async Task<ObservableCollection<Tournament>> LoadTournaments()
		{
			string json;
			HttpClient client = new HttpClient();
			json = await client.GetStringAsync(BASE_URL + "api/tournaments");

			ObservableCollection<Tournament> teams = JsonConvert.DeserializeObject<ObservableCollection<Tournament>>(json);
			Teams.Clear();
			foreach (var t in teams)
			{
				Teams.Add(new TournamentVM(t));
			}
			return teams;
		}

		private TournamentVM currentTeam;
		public TournamentVM CurrentTeam
		{
			get { return currentTeam; }
			set
			{
				if (value != currentTeam)
				{
					currentTeam = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTeam)));
				}
			}
		}

		public ObservableCollection<PlayerVM> PlayerList
		{
			get { return pvm.Players; }
		}
	}
}
