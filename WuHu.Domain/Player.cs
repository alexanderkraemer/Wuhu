using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[Serializable]
	public class Player
	{
		public int ID { get; }
		public int RoleID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Nickname { get; set; }
		public int Skills { get; set; }
		public string PhotoPath { get; set; }
		public string Password { get; set; }


		public Player(int ID, int RoleID, string FirstName, string LastName,
			string Nickname, int Skills, string PhotoPath, string Password)
		{
			this.ID = ID;
			this.RoleID = RoleID;
			this.FirstName = FirstName;
			this.LastName = LastName;
			this.Nickname = Nickname;
			this.Skills = Skills;
			this.PhotoPath = PhotoPath;
			this.Password = Password;
		}

		public Player(int RoleID, string FirstName, string LastName,
			string Nickname, int Skills, string PhotoPath, string Password)
		{
			this.RoleID = RoleID;
			this.FirstName = FirstName;
			this.LastName = LastName;
			this.Nickname = Nickname;
			this.Skills = Skills;
			this.PhotoPath = PhotoPath;
			this.Password = Password;
		}

		public new string ToString()
		{
			return $"ID: {this.ID} Name: {this.FirstName} {this.LastName} ({this.Nickname}); Skills: {this.Skills};";
		}
	}
}
