using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[Serializable]
	public class Team
	{
		public int ID { get; }
		public string Name { get; set; }
		public int Player1ID { get; set; }
		public int Player2ID { get; set; }

		public Team(int ID, string Name, int Player1ID, int Player2ID)
		{
			this.ID = ID;
			this.Name = Name;
			this.Player1ID = Player1ID;
			this.Player2ID = Player2ID;
		}

		public Team(string Name, int Player1ID, int Player2ID)
		{
			this.Name = Name;
			this.Player1ID = Player1ID;
			this.Player2ID = Player2ID;
		}
	}
}
