using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WuHu.Domain;

namespace WuHu.Terminal.ViewModels
{
	public class StatisticVM
	{
		private const string BASE_URL = "http://localhost:42382/";

		public StatisticVM(Statistic s)
		{
			ID = s.ID;
			PlayerID = s.PlayerID;
			Skill = s.Skill;
			Timestamp = s.Timestamp;
		}

		public int ID { get; set; }
		public int PlayerID { get; set; }
		public int Skill { get; set; }
		public DateTime Timestamp { get; set; }

	}
}
