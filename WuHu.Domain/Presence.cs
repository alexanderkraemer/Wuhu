using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[Serializable]
	public class Presence
	{
		public int PlayerID { get; set; }
		public int DayID { get; set; }

		public Presence(int PlayerID, int DayID)
		{
			this.PlayerID = PlayerID;
			this.DayID = DayID;
		}

		public new string ToString()
		{
			return $"Player: {this.PlayerID}; Day: {this.DayID}";
		}
	}
}
