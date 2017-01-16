export class Statistik {
    id: number;
    PlayerId: number;
    Skill: number;
    Timestamp: date;


    constructor(id: number, PlayerId: number, Skill: number, Timestamp: date) {
        this.id = id;
        this.PlayerId = PlayerId;
        this.Skill = Skill;
        this.Timestamp = Timestamp;
    }

    constructor(PlayerId: number, Skill: number, Timestamp: date) {
        this.PlayerId = PlayerId;
        this.Skill = Skill;
        this.Timestamp = Timestamp;
    }
}
