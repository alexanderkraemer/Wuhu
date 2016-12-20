using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[DataContract]
	public class Tournament
	{
		[DataMember]
		public int ID { get; set; }
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public DateTime Timestamp { get; set; }

		public Tournament()
		{

		}

		public Tournament(int ID, string Name, DateTime Timestamp)
		{
			this.ID = ID;
			this.Name = Name;
			this.Timestamp = Timestamp;
		}

		public Tournament(string Name, DateTime Timestamp)
		{
			this.Name = Name;
			this.Timestamp = Timestamp;
		}
	}
}
