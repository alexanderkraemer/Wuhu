using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[Serializable]
	public class Match
	{
		public int ID { get; }
		public int Team1ID { get; set; }
		public int Team2ID { get; set; }
		public DateTime Timestamp { get; set; }
		public int? ResultPointsPlayer1 { get; set; }
		public int? ResultPointsPlayer2 { get; set; }

		public Match(int ID, int Team1ID, int Team2ID, DateTime Timestamp,
            int? ResultPointsPlayer1, int? ResultPointsPlayer2)
		{
			this.ID = ID;
			this.Team1ID = Team1ID;
			this.Team2ID = Team2ID;
			this.Timestamp = Timestamp;
			this.ResultPointsPlayer1 = ResultPointsPlayer1;
			this.ResultPointsPlayer2 = ResultPointsPlayer2;
		}

        public Match(int Team1ID, int Team2ID, DateTime Timestamp,
            int? ResultPointsPlayer1, int? ResultPointsPlayer2)
        {
            this.Team1ID = Team1ID;
            this.Team2ID = Team2ID;
            this.Timestamp = Timestamp;
            this.ResultPointsPlayer1 = ResultPointsPlayer1;
            this.ResultPointsPlayer2 = ResultPointsPlayer2;
        }

        public Match(int Team1ID, int Team2ID, DateTime Timestamp)
        {
            this.Team1ID = Team1ID;
            this.Team2ID = Team2ID;
            this.Timestamp = Timestamp;
            this.ResultPointsPlayer1 = null;
            this.ResultPointsPlayer2 = null;
        }

        public new string ToString()
		{
			return $"Match {this.ID}: Team {this.Team1ID} vs. Team {this.Team2ID}; Time: {this.Timestamp}; Results: ({this.ResultPointsPlayer1}:{this.ResultPointsPlayer2}) ";
		}
	}
}
