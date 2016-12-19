using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
		private Player currentPlayer;

		public Player ConvertToPlayer()
		{
			return new Player(this.ID, this.isAdmin, this.FirstName, this.LastName,
												this.Nickname, this.Skills, this.PhotoPath, this.Password, this.isMonday,
												this.isTuesday, this.isWednesday, this.isThursday, this.isFriday, this.isSaturday);
		}

		public PlayerVM(Player p)
		{
			currentPlayer = new Player(p.ID, p.isAdmin, p.FirstName, p.LastName,
												p.Nickname, p.Skills, p.PhotoPath, p.Password, p.isMonday,
												p.isTuesday, p.isWednesday, p.isThursday, p.isFriday, p.isSaturday);

			ID = p.ID;
			isAdmin = p.isAdmin;
			FirstName = p.FirstName;
			LastName = p.LastName;
			Nickname = p.Nickname;
			Skills = p.Skills;
			Password = p.Password;
			isMonday = isMonday;
			isTuesday = isTuesday;
			isWednesday = isWednesday;
			isThursday = isThursday;
			isFriday = isFriday;
			isSaturday = isSaturday;
		}


		public ICommand SaveCommand
		{
			get
			{
				if(_saveCommad == null)
				{
					_saveCommad = new RelayCommand(param =>
					{
						Update(currentPlayer);
					}); 
				}
				return _saveCommad;
			}
		}

		public void Update(Player player)
		{
			HttpClient client = new HttpClient();

			string json = JsonConvert.SerializeObject(player);

			var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
			client.PutAsync(BASE_URL + "api/players/" + httpContent, httpContent);
			Debug.WriteLine("updatecommmand");
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
