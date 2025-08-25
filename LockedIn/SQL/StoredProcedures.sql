CREATE PROCEDURE dbo.AddJob
	@employerID			INT NOT NULL,
	@jobName			NVARCHAR(55),
	@jobDescription		NVARCHAR(55),
	@jobLocation		NVARCHAR(55),
	@startDate			DATETIME,
	@endDate			DATETIME,
	@prisonersNeeded	INT,
	@completed			BIT,
	@durationPerDay		TIME(0)
AS
BEGIN
	INSERT INTO JOB(JobID, EmployerID, JobName, JobDescription,	JobLocation, StartDate,	EndDate,
	PrisonersNeeded, Completed,	DurationPerDay)
	VALUES(@jobID, @employerID, @jobName, @jobDescription, @obLocation, @startDate,	@endDate,
	@prisonersNeeded, @completed, @durationPerDay);
END


CREATE PROCEDURE dbo.DeleteJob
	@employerID			INT NOT NULL,
	@jobName			NVARCHAR(55),
	@jobDescription		NVARCHAR(55),
	@jobLocation		NVARCHAR(55),
	@startDate			DATETIME,
	@endDate			DATETIME,
	@prisonersNeeded	INT,
	@completed			BIT,
	@durationPerDay		TIME(0)
AS
BEGIN
	INSERT INTO JOB(JobID, EmployerID, JobName, JobDescription,	JobLocation, StartDate,	EndDate,
	PrisonersNeeded, Completed,	DurationPerDay)
	VALUES(@jobID, @employerID, @jobName, @jobDescription, @obLocation, @startDate,	@endDate,
	@prisonersNeeded, @completed, @durationPerDay);
END


CREATE PROCEDURE dbo.UpdateJob
	@employerID			INT NOT NULL,
	@jobName			NVARCHAR(55),
	@jobDescription		NVARCHAR(55),
	@jobLocation		NVARCHAR(55),
	@startDate			DATETIME,
	@endDate			DATETIME,
	@prisonersNeeded	INT,
	@completed			BIT,
	@durationPerDay		TIME(0)
AS
BEGIN
	INSERT INTO JOB(JobID, EmployerID, JobName, JobDescription,	JobLocation, StartDate,	EndDate,
	PrisonersNeeded, Completed,	DurationPerDay)
	VALUES(@jobID, @employerID, @jobName, @jobDescription, @obLocation, @startDate,	@endDate,
	@prisonersNeeded, @completed, @durationPerDay);
END