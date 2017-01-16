export class Tournier {
    id: number;
    Name: string;
    Timestamp: Date;

    constructor(id: number, Name: string, Timestamp: Date)
    {
        this.id = id;
        this.Name = Name;
        this.Timestamp = Timestamp;
    }

    constructor(Name: string, Timestamp: Date)
    {
        this.Name = Name;
        this.Timestamp = Timestamp;
    }
}
