using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[DataContract]
	public class Statistic
	{
		[DataMember]
		public int ID { get; set; }
		[DataMember]
		public int PlayerID { get; set; }
		[DataMember]
		public int Skill { get; set; }
		[DataMember]
		public DateTime Timestamp { get; set; }

		public Statistic()
		{

		}

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
	}
}
