using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WuHu.Domain;
using WuHu.Terminal.Views.Player;

namespace WuHu.Terminal.ViewModels
{
	public class PlayerVM
	{
		private const string BASE_URL = "http://localhost:42382/";
		private ICommand _saveCommad;
		private ICommand _cancelCommad;
		private ICommand _createCommand;
		public Player currentPlayer;

		public PlayerVM()
		{
			currentPlayer = new Player();
		}

		public PlayerVM(Player p)
		{
			currentPlayer = p;

			ID = p.ID;
			isAdmin = p.isAdmin;
			FirstName = p.FirstName;
			LastName = p.LastName;
			Nickname = p.Nickname;
			Skills = p.Skills;


			PhotoPath = p.PhotoPath;
			Password = p.Password;
			isMonday = p.isMonday;
			isTuesday = p.isTuesday;
			isWednesday = p.isWednesday;
			isThursday = p.isThursday;
			isFriday = p.isFriday;
			isSaturday = p.isSaturday;
		}

		public Player Convert(PlayerVM p)
		{
			return new Player(p.ID, p.isAdmin, p.FirstName, p.LastName,
			p.Nickname, p.Skills, p.PhotoPath, p.Password, p.isMonday,
			p.isTuesday, p.isWednesday, p.isThursday, p.isFriday, p.isSaturday);
		}

		public bool isCurrentAdmin
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

		public ICommand SaveCommand
		{
			get
			{
				if (_saveCommad == null)
				{
					_saveCommad = new RelayCommand(param =>
					{
						Update(currentPlayer);
					});
				}
				return _saveCommad;
			}
		}

		public ICommand CreateCommand
		{
			get
			{
				if (_createCommand == null)
				{
					_createCommand = new RelayCommand(param =>
					{
						Create(currentPlayer);
					});
				}
				return _createCommand;
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
			string json = await client.GetStringAsync(BASE_URL + "api/players/" + currentPlayer.ID);
			client.DefaultRequestHeaders.Add("Authorization", Authentication.token.Token);
			Player player = JsonConvert.DeserializeObject<Player>(json);
			currentPlayer = player;
			MainWindow.main.Content = new PlayerList();
		}

		private async void Create(Player currentPlayer)
		{
			currentPlayer.PhotoPath = "img/profiles/" + currentPlayer.Nickname + ".jpg";


			isAdmin = currentPlayer.isAdmin;
			FirstName = currentPlayer.FirstName;
			LastName = currentPlayer.LastName;
			Nickname = currentPlayer.Nickname;
			Skills = currentPlayer.Skills;
			Password = currentPlayer.Password;
			PhotoPath = currentPlayer.PhotoPath;
			isMonday = currentPlayer.isMonday;
			isTuesday = currentPlayer.isTuesday;
			isWednesday = currentPlayer.isWednesday;
			isThursday = currentPlayer.isThursday;
			isFriday = currentPlayer.isFriday;
			isSaturday = currentPlayer.isSaturday;

			PlayerListVM.getInstance().Players.Add(new PlayerVM(currentPlayer));

			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Add("Authorization", Authentication.token.Token);
			string json = JsonConvert.SerializeObject(currentPlayer);

			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync(BASE_URL + "api/players/", httpContent);
			Debug.WriteLine(response.Content);
			MainWindow.main.Content = new PlayerList();
		}

		public void Update(Player player)
		{
			HttpClient client = new HttpClient();

			string json = JsonConvert.SerializeObject(player);
			client.DefaultRequestHeaders.Add("Authorization", Authentication.token.Token);
			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			client.PutAsync(BASE_URL + "api/players/" + currentPlayer.ID, httpContent);
			
			MainWindow.main.Content = new PlayerList();
		}

		// to server
		public int ID {
			get { return currentPlayer.ID; }
			set { currentPlayer.ID = value; }
		}
		public bool isAdmin {
			get { return currentPlayer.isAdmin; }
			set { currentPlayer.isAdmin = value; }
		}
		public string FirstName {
			get { return currentPlayer.FirstName; }
			set { currentPlayer.FirstName = value; }
		}
		public string LastName {
			get { return currentPlayer.LastName; }
			set { currentPlayer.LastName = value; }
		}
		public string Nickname {
			get { return currentPlayer.Nickname; }
			set { currentPlayer.Nickname = value; }
		}
		public int Skills {
			get { return currentPlayer.Skills; }
			set { currentPlayer.Skills = value; }
		}
		public string PhotoPath {
			get { return currentPlayer.PhotoPath; }
			set { currentPlayer.PhotoPath = value; }
		}
		public string Password {
			get { return currentPlayer.Password; }
			set { currentPlayer.Password = value; }
		}
		public bool isMonday {
			get { return currentPlayer.isMonday; }
			set { currentPlayer.isMonday = value; }
		}
		public bool isTuesday {
			get { return currentPlayer.isTuesday; }
			set { currentPlayer.isTuesday = value; }
		}
		public bool isWednesday {
			get { return currentPlayer.isWednesday; }
			set { currentPlayer.isWednesday = value; }
		}
		public bool isThursday {
			get { return currentPlayer.isThursday; }
			set { currentPlayer.isThursday = value; }
		}
		public bool isFriday {
			get { return currentPlayer.isFriday; }
			set { currentPlayer.isFriday = value; }
		}
		public bool isSaturday {
			get { return currentPlayer.isSaturday; }
			set { currentPlayer.isSaturday = value; }
		}
		public string FullName
		{
			get { return FirstName + " " + LastName; }
		}
	}
}
