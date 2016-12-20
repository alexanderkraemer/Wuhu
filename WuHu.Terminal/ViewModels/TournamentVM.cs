using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.Domain;
using WuHu.Terminal.Views.Player;
using WuHu.Terminal.Views.Tournaments;

namespace WuHu.Terminal.ViewModels
{
	public class TournamentVM
	{
		private const string BASE_URL = "http://localhost:42382/";
		private ICommand _saveCommad;
		private ICommand _cancelCommad;
		private Tournament currentTournament;
		private PlayerVM p1;
		private PlayerVM p2;

		private ObservableCollection<PlayerVM> playerList;
		
		public TournamentVM(Tournament t)
		{
			playerList = new ObservableCollection<PlayerVM>();
			currentTournament = new Tournament(t.ID, t.Name, t.Timestamp);
			
			ID = t.ID;
			Name = t.Name;
			Timestamp = t.Timestamp;
		}

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommad == null)
				{
					_saveCommad = new RelayCommand(param =>
					{
						Update(currentTournament);
					});
				}
				return _saveCommad;
			}
		}

		private void Update(Tournament team)
		{
			HttpClient client = new HttpClient();

			string json = JsonConvert.SerializeObject(team);

			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			client.PutAsync(BASE_URL + "api/tournaments/" + currentTournament.ID, httpContent);
			MainWindow.main.Content = new TournamentList();
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
			string json = await client.GetStringAsync(BASE_URL + "api/tournaments/" + currentTournament.ID);

			Tournament team = JsonConvert.DeserializeObject<Tournament>(json);
			currentTournament = team;
			MainWindow.main.Content = new TournamentList();
		}

		public int ID
		{
			get { return currentTournament.ID; }
			set { currentTournament.ID = value; }
		}
		public string Name
		{
			get { return currentTournament.Name; }
			set { currentTournament.Name = value; }
		}
		public DateTime Timestamp
		{
			get { return currentTournament.Timestamp; }
			set { currentTournament.Timestamp = value; }
		}
	}
}
