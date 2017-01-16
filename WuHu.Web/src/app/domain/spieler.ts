export class Spieler {

    id: number;
    isAdmin: boolean;
    FirstName: string;
    LastName: string;
    Nickname: string;
    Skills: number;
    Photopath: string;
    Password: string;
    isMonday: boolean;
    isTuesday: boolean;
    isWednesday: boolean;
    isThursday: boolean;
    isFriday: boolean;
    isSaturday: boolean;


    constructor(isAdmin: boolean, FirstName: string, LastName: string, Nickname: string,
                Skills: number, Photopath: string, Password: string, isMonday: boolean, isTuesday: boolean,
                isWednesday: boolean, isThursday: boolean, isFriday: boolean, isSaturday: boolean)
    {
        this.isAdmin = isAdmin;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Nickname = Nickname;
        this.Skills = Skills;
        this.Photopath = Photopath;
        this.Password = Password;
        this.isMonday = isMonday;
        this.isTuesday = isTuesday;
        this.isWednesday = isWednesday;
        this.isThursday = isThursday;
        this.isFriday = isFriday;
        this.isSaturday = isSaturday;
    }

    constructor(id: number, isAdmin: boolean, FirstName: string, LastName: string, Nickname: string,
                Skills: number, Photopath: string, Password: string, isMonday: boolean, isTuesday: boolean,
                isWednesday: boolean, isThursday: boolean, isFriday: boolean, isSaturday: boolean)
    {
        this.id = id;
        this.isAdmin = isAdmin;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.Nickname = Nickname;
        this.Skills = Skills;
        this.Photopath = Photopath;
        this.Password = Password;
        this.isMonday = isMonday;
        this.isTuesday = isTuesday;
        this.isWednesday = isWednesday;
        this.isThursday = isThursday;
        this.isFriday = isFriday;
        this.isSaturday = isSaturday;
    }
}

export class AuthObj{
    constructor(nickname: string, password: string){
        this.nickname = nickname;
        this.password = password;
    }

    nickname: string;
    password: string;
}