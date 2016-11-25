
DROP TABLE IF EXISTS [statistic];
DROP TABLE IF EXISTS [matches];
DROP TABLE IF EXISTS [teams];
DROP TABLE IF EXISTS [presence];
DROP TABLE IF EXISTS [player];
DROP TABLE IF EXISTS [roles];
DROP TABLE IF EXISTS [days];





CREATE TABLE [dbo].[roles] (
    [id]   INT          IDENTITY (1, 1) NOT NULL,
    [name] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    UNIQUE NONCLUSTERED ([name] ASC)
);



CREATE TABLE [dbo].[days] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [name] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([name] ASC)
);



CREATE TABLE [dbo].[player] (
    [id]         INT           IDENTITY (1, 1) NOT NULL,
    [role_id]    INT           NOT NULL,
    [skills]     INT           NOT NULL,
    [first_name] VARCHAR (255) NOT NULL,
    [last_name]  VARCHAR (255) NOT NULL,
    [nickname]   VARCHAR (255) NOT NULL,
    [photopath]  VARCHAR (255) NOT NULL,
    [password]   VARCHAR (64)  NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    UNIQUE NONCLUSTERED ([nickname] ASC),
    CONSTRAINT [FK_player_ToRole] FOREIGN KEY ([role_id]) REFERENCES [dbo].[roles] ([id]),
    CONSTRAINT [CK_player_skillOverZero] CHECK ([skills]>(0))
);



CREATE TABLE [dbo].[statistic] (
    [id]        INT      IDENTITY (1, 1) NOT NULL,
    [player_id] INT      NOT NULL,
    [skill]     INT      NOT NULL,
    [timestamp] DATETIME NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [player_idFK] FOREIGN KEY ([player_id]) REFERENCES [dbo].[player] ([id])
);



CREATE TABLE [dbo].[teams] (
    [Id]         INT          IDENTITY (1, 1) NOT NULL,
    [name]       VARCHAR (50) NOT NULL,
    [player_id1] INT          NOT NULL,
    [player_id2] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([name] ASC),
    CONSTRAINT [FK_teams_Toplayer1] FOREIGN KEY ([player_id1]) REFERENCES [dbo].[player] ([id]),
    CONSTRAINT [FK_teams_Toplayer2] FOREIGN KEY ([player_id2]) REFERENCES [dbo].[player] ([id]),
    CONSTRAINT [CK_teams_p1NotEqualToP2] CHECK ([player_id1]<>[player_id2])
);


CREATE TABLE [dbo].[matches] (
    [id]                INT      IDENTITY (1, 1) NOT NULL,
    [team_id1]          INT      NOT NULL,
    [team_id2]          INT      NOT NULL,
    [timestamp]         DATETIME NOT NULL,
    [results_points_t1] INT      NULL,
    [results_points_t2] INT      NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_matches_ToTeam1] FOREIGN KEY ([team_id1]) REFERENCES [dbo].[teams] ([Id]),
    CONSTRAINT [FK_matches_ToTeam2] FOREIGN KEY ([team_id2]) REFERENCES [dbo].[teams] ([Id]),
    CONSTRAINT [CK_team1Andteam2NotEqual] CHECK ([team_id1]<>[team_id2])
);


CREATE TABLE [dbo].[presence] (
    [player_id] INT NOT NULL,
    [day_id]    INT NOT NULL,
    PRIMARY KEY CLUSTERED ([player_id] ASC, [day_id] ASC),
    CONSTRAINT [FK_presence_ToDay] FOREIGN KEY ([day_id]) REFERENCES [dbo].[days] ([Id]),
    CONSTRAINT [FK_presence_ToPlayer] FOREIGN KEY ([player_id]) REFERENCES [dbo].[player] ([id])
);





