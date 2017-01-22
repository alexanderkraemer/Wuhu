

DROP TABLE IF EXISTS [statistic];
DROP TABLE IF EXISTS [matches];
DROP TABLE IF EXISTS [player];
DROP TABLE IF EXISTS [tournaments];

CREATE TABLE [dbo].[player] (
    [id]          INT           IDENTITY (1, 1) NOT NULL,
    [is_Admin]    BIT           NOT NULL,
    [skills]      INT           NOT NULL,
    [first_name]  VARCHAR (255) NOT NULL,
    [last_name]   VARCHAR (255) NOT NULL,
    [nickname]    VARCHAR (255) NOT NULL,
    [photopath]   VARCHAR (255) NULL,
    [password]    VARCHAR (255) NOT NULL,
    [isMonday]    BIT           NOT NULL,
    [isTuesday]   BIT           NOT NULL,
    [isWednesday] BIT           NOT NULL,
    [isThursday]  BIT           NOT NULL,
    [isFriday]    BIT           NOT NULL,
    [isSaturday]  BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    UNIQUE NONCLUSTERED ([nickname] ASC),
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





CREATE TABLE [dbo].[tournaments] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [name]      VARCHAR (50) NOT NULL,
    [timestamp] DATETIME     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([timestamp] ASC)
);




CREATE TABLE [dbo].[matches] (
    [id]                INT IDENTITY (1, 1) NOT NULL,
    [team1_p1ID]        INT NOT NULL,
    [team1_p2ID]        INT NOT NULL,
    [team2_p1ID]        INT NOT NULL,
    [team2_p2ID]        INT NOT NULL,
    [tournamentId]      INT NOT NULL,
    [results_points_p1] INT NULL,
    [results_points_p2] INT NULL,
	[finished]      BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_matches_ToTeam11] FOREIGN KEY ([team1_p1ID]) REFERENCES [dbo].[player] ([id]),
    CONSTRAINT [FK_matches_ToTeam12] FOREIGN KEY ([team1_p2ID]) REFERENCES [dbo].[player] ([id]),
    CONSTRAINT [FK_matches_ToTeam21] FOREIGN KEY ([team2_p1ID]) REFERENCES [dbo].[player] ([id]),
    CONSTRAINT [FK_matches_ToTeam22] FOREIGN KEY ([team2_p2ID]) REFERENCES [dbo].[player] ([id]),
    CONSTRAINT [FK_matches_TfdoTeam11] FOREIGN KEY ([tournamentId]) REFERENCES [dbo].[tournaments] ([Id]),
    CONSTRAINT [CK_team1Andteam21NotEqual] CHECK ([team1_p1ID]<>[team1_p2ID]),
    CONSTRAINT [CK_team1Andteam2NotE2qual] CHECK ([team1_p1ID]<>[team2_p1ID]),
    CONSTRAINT [CK_team1Andteam2No3tEqual] CHECK ([team1_p1ID]<>[team2_p2ID]),
    CONSTRAINT [CK_team1Andt4eam2NotEqual] CHECK ([team1_p2ID]<>[team2_p1ID]),
    CONSTRAINT [CK_team1Andteam2Not5Equal] CHECK ([team1_p2ID]<>[team2_p2ID])
);




