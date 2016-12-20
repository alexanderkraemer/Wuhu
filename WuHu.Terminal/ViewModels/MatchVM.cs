using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.Domain;
using WuHu.Terminal.Views.Matches;

namespace WuHu.Terminal.ViewModels
{
	class MatchVM
	{
		private const string BASE_URL = "http://localhost:42382/";
		private ICommand _saveCommad;
		private ICommand _cancelCommad;
		public Match currentMatch;
		private PlayerVM team1Player1;
		private PlayerVM team1Player2;
		private PlayerVM team2Player1;
		private PlayerVM team2Player2;

		public MatchVM(Match m)
		{
			currentMatch = new Match(m.ID, m.Team1Player1, m.Team1Player2, m.Team2Player1, m.Team2Player2, m.TournamentId);
			getTeam(m);

			ID = m.ID;
			Team1Player1ID = m.Team1Player1;
			Team1Player2ID = m.Team1Player2;
			Team2Player1ID = m.Team2Player1;
			Team2Player2ID = m.Team2Player2;
			TournamentId = m.TournamentId;
			ResultPointsPlayer1 = m.ResultPointsPlayer1;
			ResultPointsPlayer2 = m.ResultPointsPlayer2;
		}
		
		private async void getTeam(Match m)
		{
			PlayerListVM plvm = PlayerListVM.getInstance();

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
		
		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommad == null)
				{
					_saveCommad = new RelayCommand(param =>
					{
						Update(currentMatch);
					});
				}
				return _saveCommad;
			}
		}

		public ICommand CancelCommand
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

			Match match = JsonConvert.DeserializeObject<Match>(json);
			currentMatch = match;
			MainWindow.main.Content = new MatchList();
		}



		public void Update(Match match)
		{
			HttpClient client = new HttpClient();

			string json = JsonConvert.SerializeObject(match);

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

		public int Team1Player1ID
		{
			get { return currentMatch.Team1Player1; }
			set { currentMatch.Team1Player1 = value; }
		}

		public int Team1Player2ID
		{
			get { return currentMatch.Team1Player2; }
			set { currentMatch.Team1Player2 = value; }
		}

		public int Team2Player1ID
		{
			get { return currentMatch.Team2Player1; }
			set { currentMatch.Team2Player1 = value; }
		}

		public int Team2Player2ID
		{
			get { return currentMatch.Team2Player2; }
			set { currentMatch.Team2Player2 = value; }
		}

		public int TournamentId
		{
			get { return currentMatch.TournamentId; }
			set { currentMatch.TournamentId = value; }
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

		public PlayerVM Team1Player1
		{
			get { return team1Player1; }
		}

		public PlayerVM Team1Player2
		{
			get { return team1Player2; }
		}

		public PlayerVM Team2Player1
		{
			get { return team2Player1; }
		}

		public PlayerVM Team2Player2
		{
			get { return team2Player2; }
		}
	}
}
