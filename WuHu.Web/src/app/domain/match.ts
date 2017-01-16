export class Match {

    id: number;
    Team1Player1: number;
    Team1Player2: number;
    Team2Player1: number;
    Team2Player2: number;
    ResultPointsPlayer1: number;
    ResultPointsPlayer2: number;

    constructor(id: number, Team1Player1: number, Team1Player2: number, Team2Player1: number,
                Team2Player2: number, ResultPointsPlayer1: number, ResultPointsPlayer2: number)
    {
        this.id = id;
        this.Team1Player1 = Team1Player1;
        this.Team1Player2 = Team1Player2;
        this.Team2Player1 = Team2Player1;
        this.Team2Player2 = Team2Player2;
        this.ResultPointsPlayer1 = ResultPointsPlayer1;
        this.ResultPointsPlayer2 = ResultPointsPlayer2;
    }

    constructor(Team1Player1: number, Team1Player2: number, Team2Player1: number,
                Team2Player2: number, ResultPointsPlayer1: number, ResultPointsPlayer2: number)
    {
        this.Team1Player1 = Team1Player1;
        this.Team1Player2 = Team1Player2;
        this.Team2Player1 = Team2Player1;
        this.Team2Player2 = Team2Player2;
        this.ResultPointsPlayer1 = ResultPointsPlayer1;
        this.ResultPointsPlayer2 = ResultPointsPlayer2;
    }
}
