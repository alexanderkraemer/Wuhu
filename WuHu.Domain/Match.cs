﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Domain
{
	[DataContract]
	public class MatchGenerate
	{
		public MatchGenerate(ObservableCollection<Player> chosenPlayers, int NumberOfMatches, int tournamentId)
		{
			this.chosenPlayers = chosenPlayers;
			this.NumberOfMatches = NumberOfMatches;
			this.tournamentId = tournamentId;
		}

		[DataMember]
		public ObservableCollection<Player> chosenPlayers { get; set; }
		[DataMember]
		public int NumberOfMatches { get; set; }
		[DataMember]
		public int tournamentId { get; set; }
	}

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
		[DataMember]
		public bool Finished { get; set; }

		public Match()
		{

		}

		public Match(int ID, int Team1Player1, int Team1Player2, int Team2Player1, int Team2Player2, int TournamentId, bool Finished)
		{
			this.ID = ID;
			this.Team1Player1 = Team1Player1;
			this.Team1Player2 = Team1Player2;
			this.Team2Player1 = Team2Player1;
			this.Team2Player2 = Team2Player2;
			this.TournamentId = TournamentId;
			this.ResultPointsPlayer1 = null;
			this.ResultPointsPlayer2 = null;
			this.Finished = Finished;
		}

		public Match(int ID, int Team1Player1, int Team1Player2, int Team2Player1, int Team2Player2, int TournamentId,
				int? ResultPointsPlayer1, int? ResultPointsPlayer2, bool Finished)
		{
			this.ID = ID;
			this.Team1Player1 = Team1Player1;
			this.Team1Player2 = Team1Player2;
			this.Team2Player1 = Team2Player1;
			this.Team2Player2 = Team2Player2;
			this.TournamentId = TournamentId;
			this.ResultPointsPlayer1 = ResultPointsPlayer1;
			this.ResultPointsPlayer2 = ResultPointsPlayer2;
			this.Finished = Finished;
		}

		public Match(int Team1Player1, int Team1Player2, int Team2Player1, int Team2Player2, int TournamentId,
				int? ResultPointsPlayer1, int? ResultPointsPlayer2, bool Finished)
		{
			this.Team1Player1 = Team1Player1;
			this.Team1Player2 = Team1Player2;
			this.Team2Player1 = Team2Player1;
			this.Team2Player2 = Team2Player2;
			this.TournamentId = TournamentId;
			this.ResultPointsPlayer1 = ResultPointsPlayer1;
			this.ResultPointsPlayer2 = ResultPointsPlayer2;
			this.Finished = Finished;
		}

		public Match(int Team1Player1, int Team1Player2, int Team2Player1, int Team2Player2, int TournamentId, bool Finished)
		{
			this.Team1Player1 = Team1Player1;
			this.Team1Player2 = Team1Player2;
			this.Team2Player1 = Team2Player1;
			this.Team2Player2 = Team2Player2;
			this.TournamentId = TournamentId;
			this.ResultPointsPlayer1 = null;
			this.ResultPointsPlayer2 = null;
			this.Finished = Finished;
		}
	}
}
