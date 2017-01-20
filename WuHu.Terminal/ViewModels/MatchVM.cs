using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.Domain;
using WuHu.Terminal.Views.Matches;

namespace WuHu.Terminal.ViewModels
{
	public class MatchVM
	{
		private const string BASE_URL = "http://localhost:42382/";
		private ICommand _updateCommad;
		private ICommand _cancelCommad;
		public Match currentMatch;
		private PlayerVM team1Player1;
		private PlayerVM team1Player2;
		private PlayerVM team2Player1;
		private PlayerVM team2Player2;
		private TournamentVM tournament;

		private ObservableCollection<PlayerVM> playerList;

		private ObservableCollection<TournamentVM> tournamentList;

		public bool isAdmin
		{
			get
			{
				if (Authentication.isAuthenticated)
				{
					return Authentication.getLoggedInUser.isAdmin;
				}
				return false;
			}
		}
		public ObservableCollection<TournamentVM> Tournaments
		{
			get { return tournamentList; }
		}

		public ObservableCollection<PlayerVM> Players
		{
			get { return playerList; }
		}

		public MatchVM(Match m)
		{
			playerList = new ObservableCollection<PlayerVM>();
			tournamentList = new ObservableCollection<TournamentVM>();


			foreach (PlayerVM p in PlayerListVM.getInstance().Players)
			{
				playerList.Add(p);
			}
			var tlvm = TournamentListVM.getInstance();

			tournamentList = new ObservableCollection<TournamentVM>(tlvm.
				Tournaments.Where(tourn => tourn.Timestamp >= DateTime.Today || tourn.ID == m.TournamentId));
			
			currentMatch = m;
			getData(m);

			ID = m.ID;
			Team1Player1.ID = m.Team1Player1;
			Team1Player2.ID = m.Team1Player2;
			Team2Player1.ID = m.Team2Player1;
			Team2Player2.ID = m.Team2Player2;
			Finished = m.Finished;
			Tournament.ID = m.TournamentId;
			ResultPointsPlayer1 = m.ResultPointsPlayer1;
			ResultPointsPlayer2 = m.ResultPointsPlayer2;
		}

		private async void getData(Match m)
		{
			PlayerListVM plvm = PlayerListVM.getInstance();
			var tlvm = TournamentListVM.getInstance();


			tournament = tlvm.Tournaments.Where(t =>
			{
				return t.ID == m.TournamentId;
			}).Single();

			team1Player1 = plvm.Players.Where(t =>
			{
				return t.ID == m.Team1Player1;
			}).Single();

			team1Player2 = plvm.Players.Where(t =>
			{
				return t.ID == m.Team1Player2;
			}).Single();

			team2Player1 = plvm.Players.Where(t =>
			{
				return t.ID == m.Team2Player1;
			}).Single();

			team2Player2 = plvm.Players.Where(t =>
			{
				return t.ID == m.Team2Player2;
			}).Single();
		}
		
		public ICommand UpdateMatchCommand
		{
			get
			{
				if (_updateCommad == null)
				{
					_updateCommad = new RelayCommand(param =>
					{
						Update(currentMatch);
					});
				}
				return _updateCommad;
			}
		}

		public ICommand CancelMatchCommand
		{
			get
			{
				if (_cancelCommad == null)
				{
					_cancelCommad = new RelayCommand(param =>
					{
						Cancel();
					});
				}
				return _cancelCommad;
			}
		}

		private async void Cancel()
		{
			HttpClient client = new HttpClient();
			string json = await client.GetStringAsync(BASE_URL + "api/matches/" + currentMatch.ID);
			client.DefaultRequestHeaders.Add("Authorization", Authentication.token.Token);
			Match match = JsonConvert.DeserializeObject<Match>(json);
			currentMatch = match;
			MainWindow.main.Content = new MatchList();
		}



		public void Update(Match match)
		{
			HttpClient client = new HttpClient();

			string json = JsonConvert.SerializeObject(match);
			client.DefaultRequestHeaders.Add("Authorization", Authentication.token.Token);
			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			client.PutAsync(BASE_URL + "api/matches/" + currentMatch.ID, httpContent);
			MainWindow.main.Content = new MatchList();
		}

		// to server
		public int ID
		{
			get { return currentMatch.ID; }
			set { currentMatch.ID = value; }
		}

		public int? ResultPointsPlayer1
		{
			get { return currentMatch.ResultPointsPlayer1; }
			set { currentMatch.ResultPointsPlayer1 = value; }
		}

		public int? ResultPointsPlayer2
		{
			get { return currentMatch.ResultPointsPlayer2; }
			set { currentMatch.ResultPointsPlayer2 = value; }
		}

		public bool Finished
		{
			get { return currentMatch.Finished; }
			set { currentMatch.Finished = value; }
		}

		public PlayerVM Team1Player1
		{
			get { return team1Player1; }
			set
			{
				currentMatch.Team1Player1 = value.ID;
				team1Player1 = value;
			}
		}

		public PlayerVM Team1Player2
		{
			get { return team1Player2; }
			set
			{
				currentMatch.Team1Player2 = value.ID;
				team1Player2 = value;
			}
		}

		public PlayerVM Team2Player1
		{
			get { return team2Player1; }
			set
			{
				currentMatch.Team2Player1 = value.ID;
				team2Player1 = value;
			}
		}

		public PlayerVM Team2Player2
		{
			get { return team2Player2; }
			set
			{
				currentMatch.Team2Player2 = value.ID;
				team2Player2 = value;
			}
		}

		public TournamentVM Tournament
		{
			get { return tournament; }
			set
			{
				currentMatch.TournamentId = value.ID;
				tournament = value;
			}
		}
	}
}
