using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
	public class PlayerVM
	{
		public PlayerVM(Player p)
		{
			ID = p.ID;
			RoleID = p.RoleID;
			FirstName = p.FirstName;
			LastName = p.LastName;
			Nickname = p.Nickname;
			Skills = p.Skills;
			Password = p.Password;
		}

		public int ID { get; }
		public int RoleID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Nickname { get; set; }
		public int Skills { get; set; }
		public string PhotoPath { get; set; }
		public string Password { get; set; }


		public string FullName
		{
			get { return FirstName + " " + LastName; }
		}
	}
}
