using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[DataContract]
	public class Player
	{
		[DataMember]
		public int ID { get; set; }
		[DataMember]
		public bool isAdmin { get; set; }
		[DataMember]
		public string FirstName { get; set; }
		[DataMember]
		public string LastName { get; set; }
		[DataMember]
		public string Nickname { get; set; }
		[DataMember]
		public int Skills { get; set; }
		[DataMember]
		public string PhotoPath { get; set; }
		[DataMember]
		public string Password { get; set; }
		[DataMember]
		public bool isMonday { get; set; }
		[DataMember]
		public bool isTuesday { get; set; }
		[DataMember]
		public bool isWednesday { get; set; }
		[DataMember]
		public bool isThursday { get; set; }
		[DataMember]
		public bool isFriday { get; set; }
		[DataMember]
		public bool isSaturday { get; set; }

		public Player(int ID, bool isAdmin, string FirstName, string LastName,
			string Nickname, int Skills, string PhotoPath, string Password, bool isMonday, 
			bool isTuesday, bool isWednesday, bool isThursday, bool isFriday, bool isSaturday)
		{
			this.ID = ID;
			this.isAdmin = isAdmin;
			this.FirstName = FirstName;
			this.LastName = LastName;
			this.Nickname = Nickname;
			this.Skills = Skills;
			this.PhotoPath = PhotoPath;
			this.Password = Password;
			this.isMonday = isMonday;
			this.isTuesday = isTuesday;
			this.isWednesday = isWednesday;
			this.isThursday = isThursday;
			this.isFriday = isFriday;
			this.isSaturday = isSaturday;
		}

		//public Player(bool isAdmin, string FirstName, string LastName,
		//	string Nickname, int Skills, string PhotoPath, string Password, bool isMonday,
		//	bool isTuesday, bool isWednesday, bool isThursday, bool isFriday, bool isSaturday)
		//{
		//	this.isAdmin = isAdmin;
		//	this.FirstName = FirstName;
		//	this.LastName = LastName;
		//	this.Nickname = Nickname;
		//	this.Skills = Skills;
		//	this.PhotoPath = PhotoPath;
		//	this.Password = Password;
		//	this.isMonday = isMonday;
		//	this.isTuesday = isTuesday;
		//	this.isWednesday = isWednesday;
		//	this.isThursday = isThursday;
		//	this.isFriday = isFriday;
		//	this.isSaturday = isSaturday;
		//}
	}
}
