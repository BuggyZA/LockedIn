--File for the SQL stored procedures
-- AddJob, UpdateJob and DeleteJob
CREATE PROCEDURE dbo.AddJob
    @employerID        INT NOT NULL,
    @jobName           NVARCHAR(55),
    @jobDescription    NVARCHAR(55),
    @jobLocation       NVARCHAR(55),
    @startDate         DATETIME,
    @endDate           DATETIME,
    @prisonersNeeded   INT,
    @completed         BIT,
    @durationPerDay    TIME(0)
AS
BEGIN
    INSERT INTO JOB (EmployerID, JobName, JobDescription, JobLocation, StartDate, EndDate,
                     PrisonersNeeded, Completed, DurationPerDay)
    VALUES (@employerID, @jobName, @jobDescription, @jobLocation, @startDate, @endDate,
            @prisonersNeeded, @completed, @durationPerDay);
END

--Update Job

CREATE PROCEDURE dbo.UpdateJob
    @jobID             INT,
    @employerID        INT,
    @jobName           NVARCHAR(55),
    @jobDescription    NVARCHAR(55),
    @jobLocation       NVARCHAR(55),
    @startDate         DATETIME,
    @endDate           DATETIME,
    @prisonersNeeded   INT,
    @completed         BIT,
    @durationPerDay    TIME(0)
AS
BEGIN
    UPDATE JOB
    SET EmployerID      = @employerID,
        JobName         = @jobName,
        JobDescription  = @jobDescription,
        JobLocation     = @jobLocation,
        StartDate       = @startDate,
        EndDate         = @endDate,
        PrisonersNeeded = @prisonersNeeded,
        Completed       = @completed,
        DurationPerDay  = @durationPerDay
    WHERE JobID = @jobID;
END

--Delete job

CREATE PROCEDURE dbo.DeleteJob
    @jobID INT
AS
BEGIN
    DELETE FROM JOB
    WHERE JobID = @jobID;
END
