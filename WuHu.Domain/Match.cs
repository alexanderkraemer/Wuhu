using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[DataContract]
	public class Match
	{
		[DataMember]
		public int ID { get; set; }
		[DataMember]
		public int Team1Player1 { get; set; }
		[DataMember]
		public int Team1Player2 { get; set; }
		[DataMember]
		public int Team2Player1 { get; set; }
		[DataMember]
		public int Team2Player2 { get; set; }
		[DataMember]
		public int TournamentId { get; set; }
		[DataMember]
		public int? ResultPointsPlayer1 { get; set; }
		[DataMember]
		public int? ResultPointsPlayer2 { get; set; }

		public Match()
		{

		}

		public Match(int ID, int Team1Player1, int Team1Player2, int Team2Player1, int Team2Player2, int TournamentId)
		{
			this.ID = ID;
			this.Team1Player1 = Team1Player1;
			this.Team1Player2 = Team1Player2;
			this.Team2Player1 = Team2Player1;
			this.Team2Player2 = Team2Player2;
			this.TournamentId = TournamentId;
			this.ResultPointsPlayer1 = null;
			this.ResultPointsPlayer2 = null;
		}

		public Match(int ID, int Team1Player1, int Team1Player2, int Team2Player1, int Team2Player2, int TournamentId,
				int? ResultPointsPlayer1, int? ResultPointsPlayer2)
		{
			this.ID = ID;
			this.Team1Player1 = Team1Player1;
			this.Team1Player2 = Team1Player2;
			this.Team2Player1 = Team2Player1;
			this.Team2Player2 = Team2Player2;
			this.TournamentId = TournamentId;
			this.ResultPointsPlayer1 = ResultPointsPlayer1;
			this.ResultPointsPlayer2 = ResultPointsPlayer2;
		}

		public Match(int Team1Player1, int Team1Player2, int Team2Player1, int Team2Player2, int TournamentId,
				int? ResultPointsPlayer1, int? ResultPointsPlayer2)
		{
			this.Team1Player1 = Team1Player1;
			this.Team1Player2 = Team1Player2;
			this.Team2Player1 = Team2Player1;
			this.Team2Player2 = Team2Player2;
			this.TournamentId = TournamentId;
			this.ResultPointsPlayer1 = ResultPointsPlayer1;
			this.ResultPointsPlayer2 = ResultPointsPlayer2;
		}

		public Match(int Team1Player1, int Team1Player2, int Team2Player1, int Team2Player2, int TournamentId)
		{
			this.Team1Player1 = Team1Player1;
			this.Team1Player2 = Team1Player2;
			this.Team2Player1 = Team2Player1;
			this.Team2Player2 = Team2Player2;
			this.TournamentId = TournamentId;
			this.ResultPointsPlayer1 = null;
			this.ResultPointsPlayer2 = null;
		}
	}
}
