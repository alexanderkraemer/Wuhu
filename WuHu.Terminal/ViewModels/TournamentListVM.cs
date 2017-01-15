using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
	public class TournamentListVM : INotifyPropertyChanged
	{
		private const string BASE_URL = "http://localhost:42382/";

		public event PropertyChangedEventHandler PropertyChanged;
		private static TournamentListVM instance;
		public ObservableCollection<TournamentVM> Tournaments { get; private set; }
		public ObservableCollection<PlayerVM> Players { get; private set; }
		private bool isLocked = false;
		private bool isEditing = false;
		public ObservableCollection<PlayerVM> originalPlayers { get; private set; }
		public ObservableCollection<PlayerVM> chosenPlayers { get; private set; }
		private PlayerListVM pvm;
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

		public bool IsLocked
		{
			get { return isLocked; }
			set
			{
				isLocked = value;
			}
		}

		public bool IsEditing
		{
			get { return isEditing; }
			set { isEditing = value; }
		}

		private ICommand _addPlayerCommand;
		private ICommand _activePlayerClickedCommand;
		private ICommand _generateMatchesCommand;

		public static TournamentListVM getInstance()
		{
			if (instance == null)
			{
				instance = new TournamentListVM();
			}
			return instance;
		}

		private TournamentListVM()
		{
			
			pvm = PlayerListVM.getInstance();
			Tournaments = new ObservableCollection<TournamentVM>();
			Players = new ObservableCollection<PlayerVM>();
			originalPlayers = new ObservableCollection<PlayerVM>();
			
			LoadTournaments();
			chosenPlayers = new ObservableCollection<PlayerVM>();
			Task<ObservableCollection<Player>> task = pvm.LoadPlayer();
			
			App.Current.Dispatcher.Invoke(() =>
			{
				task.ContinueWith(param =>
				{
					foreach (PlayerVM p in pvm.Players)
					{
						Players.Add(p);
						originalPlayers.Add(p);
					}
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Players)));
				});
			});
			
		}
	
		public async Task<ObservableCollection<Tournament>> LoadTournaments()
		{
			string json;
			HttpClient client = new HttpClient();
			json = await client.GetStringAsync(BASE_URL + "api/tournaments");

			ObservableCollection<Tournament> tournaments = JsonConvert.DeserializeObject<ObservableCollection<Tournament>>(json);
			Tournaments.Clear();
			foreach (var t in tournaments)
			{
				Tournaments.Add(new TournamentVM(t));
			}
			return tournaments;
		}

		private TournamentVM currentTournament;
		public TournamentVM CurrentTournament
		{
			get { return currentTournament; }
			set
			{
				if (value != currentTournament)
				{
					currentTournament = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentTournament)));
					Players.Clear();
					foreach(PlayerVM p in originalPlayers)
					{
						Players.Add(p);
					}
					IEnumerable<PlayerVM> playList = new List<PlayerVM>();
					if (currentTournament != null)
					{
						playList = Players.Where(p =>
						{
							switch (currentTournament.Timestamp.DayOfWeek)
							{
								case DayOfWeek.Monday:
									return p.isMonday;
								case DayOfWeek.Tuesday:
									return p.isTuesday;
								case DayOfWeek.Wednesday:
									return p.isWednesday;
								case DayOfWeek.Thursday:
									return p.isThursday;
								case DayOfWeek.Friday:
									return p.isFriday;
								case DayOfWeek.Saturday:
									return p.isSaturday;
								default:
									return false;
							}
						});
					}
					
					chosenPlayers.Clear();
					foreach (PlayerVM p in playList)
					{
						chosenPlayers.Add(p);
					}

					foreach(PlayerVM p in chosenPlayers)
					{
						Players.Remove(p);
					}
				}
				
			}
		}

		private PlayerVM currentPlayer;

		public PlayerVM CurrentPlayer
		{
			get { return currentPlayer; }
			set
			{
				if(value != currentPlayer)
				{
					currentPlayer = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPlayer)));
				}
			}
		}

		private string numberOfMatches;
		public string NumberOfMatches
		{
			get { return numberOfMatches; }
			set
			{
				if (value != numberOfMatches)
				{
					numberOfMatches = value;
				}
			}
		}

		public ObservableCollection<PlayerVM> PlayerList
		{
			get { return pvm.Players; }
		}

		public ICommand AddPlayerCommand
		{
			get
			{
				if (_addPlayerCommand == null)
				{
					_addPlayerCommand = new RelayCommand(param =>
					{
						chosenPlayers.Add(CurrentPlayer);
						Players.Remove(CurrentPlayer);
					}, a =>
					{
						return !IsLocked || isEditing;
					});
				}
				return _addPlayerCommand;
			}
		}

		public ICommand activePlayerSelectedCommand
		{
			get
			{
				if (_activePlayerClickedCommand == null)
				{
					_activePlayerClickedCommand = new RelayCommand(param =>
					{
						chosenPlayers.Remove((PlayerVM)param);
						Players.Add((PlayerVM)param);
					}, a =>
					{
						return !IsLocked || isEditing;
					});
				}
				return _activePlayerClickedCommand;
			}
		}

		public ICommand GenerateMatchesButton
		{
			get
			{
				if (_generateMatchesCommand == null)
				{
					_generateMatchesCommand = new RelayCommand(param =>
					{
						var list = new ObservableCollection<Player>();
						foreach(PlayerVM p in chosenPlayers)
						{
							list.Add(p.Convert(p));
						}
						int a;
						int.TryParse(NumberOfMatches, out a);
						MatchGenerate m = new MatchGenerate(list, a, CurrentTournament.ID);
						CreateMatches(m);
						// do shit nao!!!!
						// chosenPlayers
						// NumberOfPlayers
						// currentTournament
						Debug.WriteLine("sent matches to server");
						//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof()));
					}, a =>
					{
						return !IsLocked || isEditing;
					});
				}
				return _generateMatchesCommand;
			}
		}

		public async void LockTournament()
		{
			HttpClient client = new HttpClient();

			string json = JsonConvert.SerializeObject(true);

			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(BASE_URL + "api/tournaments/lock", httpContent);
			if (response.IsSuccessStatusCode)
			{
				IsLocked = true;
				isEditing = true;
			}
			else
			{
				IsLocked = true;
				isEditing = false;
			}
		}

		public async void UnlockTournament()
		{
			HttpClient client = new HttpClient();

			string json = JsonConvert.SerializeObject(false);

			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(BASE_URL + "api/tournaments/unlock", httpContent);
			if (response.IsSuccessStatusCode)
			{
				IsLocked = false;
				isEditing = false;
			}
			else
			{
				IsLocked = true;
				isEditing = false;
			}
		}

		private async void CreateMatches(MatchGenerate matchObj)
		{
			HttpClient client = new HttpClient();
			string json = JsonConvert.SerializeObject(matchObj);

			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(BASE_URL + "api/matches/generate", httpContent);

			Debug.WriteLine(response.Content);
		}
	}
}
