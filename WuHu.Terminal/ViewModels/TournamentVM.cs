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
using WuHu.Terminal.Views.Player;
using WuHu.Terminal.Views.Tournaments;

namespace WuHu.Terminal.ViewModels
{
	public class TournamentVM : INotifyPropertyChanged
	{
		private const string BASE_URL = "http://localhost:42382/";
		private ICommand _createCommad;
		private ICommand _cancelCommad;
		private string _notification;
		private Tournament currentTournament;

		public event PropertyChangedEventHandler PropertyChanged;

		public TournamentVM()
		{
			currentTournament = new Tournament();
		}

		public TournamentVM(Tournament t)
		{
			currentTournament = new Tournament(t.ID, t.Name, t.Timestamp);
			
			ID = t.ID;
			Name = t.Name;
			Timestamp = t.Timestamp;
		}

		public ICommand SaveCommand
		{
			get
			{
				if (_createCommad == null)
				{
					_createCommad = new RelayCommand(param =>
					{
						int list = TournamentListVM.getInstance().Tournaments.Count(p =>
						{
							return p.Timestamp == currentTournament.Timestamp;
						});
						if (list > 0)
						{
							Notification = "Es gibt bereits ein Tournament an diesem Tag!";
						}
						else
						{
							Create(currentTournament);
						}
					});
				}
				return _createCommad;
			}
		}

		private async void Create(Tournament currentTournament)
		{
			TournamentListVM.getInstance().Tournaments.Add(new TournamentVM(currentTournament));

			HttpClient client = new HttpClient();
			string json = JsonConvert.SerializeObject(currentTournament);
			client.DefaultRequestHeaders.Add("Authorization", Authentication.token.Token);
			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(BASE_URL + "api/tournaments/", httpContent);
		
			Debug.WriteLine(response.Content);

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
			client.DefaultRequestHeaders.Add("Authorization", Authentication.token.Token);
			string json = await client.GetStringAsync(BASE_URL + "api/tournaments/" + currentTournament.ID);

			Tournament tournament = JsonConvert.DeserializeObject<Tournament>(json);
			currentTournament = tournament;
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

		public string Notification
		{
			get { return _notification; }
			set
			{
				if (value != _notification)
				{
					_notification = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Notification)));
				}
			}
		}
	}
}
