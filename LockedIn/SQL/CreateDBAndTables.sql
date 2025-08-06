USE master;
GO

IF DB_ID('LockedIn') IS NOT NULL		-- Check if the database exists before dropping it
BEGIN
    ALTER DATABASE LockedIn SET SINGLE_USER WITH ROLLBACK IMMEDIATE; -- Set the database to single user mode to drop it
	--SINGLE_USER mode, meaning only one connection is allowed at a time
	--WITH ROLLBACK IMMEDIATE will roll back any uncommitted transactions
    DROP DATABASE LockedIn;
END
GO
--Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LockedIn;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False


--Change file paths to your local github files
CREATE DATABASE LockedIn
ON (NAME = LockedInDB, 
    FILENAME = 'C:\!Github\LockedIn\LockedIn\LockedInDB.mdf')
LOG ON (NAME = LockedInDB_log, 
    FILENAME = 'C:\!Github\LockedIn\LockedIn\LockedInDB_log.ldf');
GO

USE LockedIn
GO

CREATE TABLE Employer(
	EmployerID int IDENTITY(1,1) PRIMARY KEY,
	LastName varchar(55),
	FirstName varchar(55),
	Password varchar(50)
);

CREATE TABLE Job(
	JobID int IDENTITY(1,1) PRIMARY KEY,
	EmployerID int NOT  NULL,
	JobName varchar(55),
	JobDescription varchar(55),
	JobLocation varchar(55),
	StartDate datetime,
	EndDate datetime,
	Completed bit,
	DurationPerDay time(0),

	CONSTRAINT FK_JOB_EMPLOYER FOREIGN KEY (EmployerID) REFERENCES Employer(EmployerID)
);

CREATE TABLE Prisoner(
	PrisonerID int IDENTITY(1,1) PRIMARY KEY,
	LastName varchar(55),
	FirstName varchar(55),
	SA_ID varchar(13),
	Password varchar(25),
	ClearedToWork bit
);

CREATE TABLE Skill(
	SkillID int IDENTITY(1,1) PRIMARY KEY,
	SkillName nvarchar(30),
	SkillDescription nvarchar(50)
);

CREATE TABLE Prisoner_On_Job(
	JobID int NOT NULL,
	PrisonerID int NOT NULL,
	[Accepted/Rejeced] bit,
	JobFinished bit,
	OnPremise bit, 

	CONSTRAINT PK_PRISONER_ON_JOB PRIMARY KEY(JobID,PrisonerID),

	CONSTRAINT FK_PRISONER_ON_JOB_JOB FOREIGN KEY (JobID) REFERENCES Job(JobID),
	CONSTRAINT FK_PRISONER_ON_JOB_PRISONER FOREIGN KEY (PrisonerID) REFERENCES Prisoner(PrisonerID)
);

CREATE TABLE Skill_Of_Job(
	JobID int NOT NULL,
	SkillID int NOT NULL,

	CONSTRAINT PK_SKILL_OF_JOB PRIMARY KEY(JobID,SkillID),

	CONSTRAINT FK_SKILL_OF_JOB_JOB FOREIGN KEY (JobID) REFERENCES Job(JobID),
	CONSTRAINT FK_SKILL_OF_JOB_SKILL FOREIGN KEY (SkillID) REFERENCES Skill(SkillID)
);

CREATE TABLE Skill_Of_Prisoner(
	PrisonerID int NOT NULL,
	SkillID int NOT NULL,

	CONSTRAINT PK_SKILL_OF_PRISONER PRIMARY KEY(PrisonerID,SkillID),

	CONSTRAINT FK_SKILL_OF_PRISONER_PRISONER FOREIGN KEY (PrisonerID) REFERENCES Prisoner(PrisonerID),
	CONSTRAINT FK_SKILL_OF_PRISONER_SKILL FOREIGN KEY (SkillID) REFERENCES Skill(SkillID)
);
	