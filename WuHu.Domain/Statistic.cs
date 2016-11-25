using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[Serializable]
	public class Statistic
	{
		public int ID { get; }
		public int PlayerID { get; set; }
		public int Skill { get; set; }
		public DateTime Timestamp { get; set; }


		public Statistic(int ID, int PlayerID, int Skill, DateTime Timestamp)
		{
			this.ID = ID;
			this.PlayerID = PlayerID;
			this.Skill = Skill;
			this.Timestamp = Timestamp;
		}

		public Statistic(int PlayerID, int Skill, DateTime Timestamp)
		{
			this.PlayerID = PlayerID;
			this.Skill = Skill;
			this.Timestamp = Timestamp;
		}

		public new string ToString()
		{
			return $"ID: {this.ID}; Player: {this.PlayerID}; Skill: {this.Skill}; Timestamp: {this.Timestamp};";
		}
	}
}
