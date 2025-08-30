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

--Change file paths to your local github files
CREATE DATABASE LockedIn
ON (NAME = LockedInDB, 
    FILENAME = 'C:\!Github\LockedIn\LockedIn\LockedInDB.mdf')
LOG ON (NAME = LockedInDB_log, 
    FILENAME = 'C:\!Github\LockedIn\LockedIn\LockedInDB_log.ldf');
GO

USE LockedIn
GO

CREATE TABLE EMPLOYER(
	EmployerID int IDENTITY(1,1) PRIMARY KEY,
	LastName varchar(55),
	FirstName varchar(55),
	Password varchar(50)
);

CREATE TABLE JOB(
	JobID int IDENTITY(1,1) PRIMARY KEY,
	EmployerID int NOT  NULL,
	JobName varchar(55),
	JobDescription varchar(55),
	JobLocation varchar(55),
	StartDate datetime,
	EndDate datetime,
	PrisonersNeeded int,
	Completed bit,
	DurationPerDay int,  -- Changed from time(0) to int

	CONSTRAINT FK_JOB_EMPLOYER FOREIGN KEY (EmployerID) REFERENCES EMPLOYER(EmployerID)
);

CREATE TABLE PRISONER(
	PrisonerID int IDENTITY(1,1) PRIMARY KEY,
	PrisonerNum char(10),			
	LastName varchar(55),
	FirstName varchar(55),
	SA_ID varchar(13),
	Password varchar(25),
	ClearedToWork bit
);

CREATE TABLE SKILL(
	SkillID int IDENTITY(1,1) PRIMARY KEY,
	SkillName nvarchar(30),
	SkillDescription nvarchar(50)
);

CREATE TABLE PRISONER_ON_JOB(
	JobID int NOT NULL,
	PrisonerID int NOT NULL,
	[Accepted/Rejeced] bit,
	JobFinished bit,
	OnPremise bit, 

	CONSTRAINT PK_PRISONER_ON_JOB PRIMARY KEY(JobID,PrisonerID),

	CONSTRAINT FK_PRISONER_ON_JOB_JOB FOREIGN KEY (JobID) REFERENCES JOB(JobID),
	CONSTRAINT FK_PRISONER_ON_JOB_PRISONER FOREIGN KEY (PrisonerID) REFERENCES PRISONER(PrisonerID)
);

CREATE TABLE SKILL_OF_JOB(
	JobID int NOT NULL,
	SkillID int NOT NULL,

	CONSTRAINT PK_SKILL_OF_JOB PRIMARY KEY(JobID,SkillID),

	CONSTRAINT FK_SKILL_OF_JOB_JOB FOREIGN KEY (JobID) REFERENCES JOB(JobID),
	CONSTRAINT FK_SKILL_OF_JOB_SKILL FOREIGN KEY (SkillID) REFERENCES SKILL(SkillID)
);

CREATE TABLE SKILL_OF_PRISONER(
	PrisonerID int NOT NULL,
	SkillID int NOT NULL,

	CONSTRAINT PK_SKILL_OF_PRISONER PRIMARY KEY(PrisonerID,SkillID),

	CONSTRAINT FK_SKILL_OF_PRISONER_PRISONER FOREIGN KEY (PrisonerID) REFERENCES PRISONER(PrisonerID),
	CONSTRAINT FK_SKILL_OF_PRISONER_SKILL FOREIGN KEY (SkillID) REFERENCES SKILL(SkillID)
);

-- Insert test data for EMPLOYER table - Community Organizations
INSERT INTO EMPLOYER (LastName, FirstName, Password) VALUES
('Van Der Merwe', 'Pieter', 'parks123'),      -- Parks Department
('Nkomo', 'Thabo', 'health456'),              -- Health Department  
('Coetzee', 'Susan', 'roads789'),             -- Roads & Infrastructure
('Molefe', 'Grace', 'education321'),          -- Education Department
('Du Plessis', 'Johan', 'community999'),      -- Community Development
('Mahlangu', 'Patricia', 'municipal555'),     -- Municipal Services
('Steyn', 'Andre', 'social777'),              -- Social Services
('Dlamini', 'Nomsa', 'housing888');           -- Housing Department

-- Insert test data for SKILL table
INSERT INTO SKILL (SkillName, SkillDescription) VALUES
('Carpentry', 'Woodworking and furniture construction'),
('Welding', 'Metal joining and fabrication skills'),
('Cooking', 'Food preparation and kitchen management'),
('Cleaning', 'Janitorial and maintenance services'),
('Landscaping', 'Gardening and grounds maintenance'),
('Data Entry', 'Computer and administrative tasks'),
('Painting', 'Interior and exterior painting'),
('Plumbing', 'Pipe installation and repair'),
('Electrical', 'Basic electrical maintenance'),
('Laundry', 'Commercial laundry operations'),
('Warehouse', 'Inventory and storage management'),
('Construction', 'General construction work');

-- Insert test data for PRISONER table
INSERT INTO PRISONER (PrisonerNum, LastName, FirstName, SA_ID, Password, ClearedToWork) VALUES
('P001234567', 'Anderson', 'James', '8501015800083', 'pass001', 1),
('P002345678', 'Thompson', 'Mark', '7812123456789', 'pass002', 1),
('P003456789', 'Garcia', 'Carlos', '9203045678901', 'pass003', 1),
('P004567890', 'Martinez', 'Luis', '8706078901234', 'pass004', 0),
('P005678901', 'Robinson', 'Kevin', '9410112345678', 'pass005', 1),
('P006789012', 'Lewis', 'Thomas', '8205156789012', 'pass006', 1),
('P007890123', 'Walker', 'Daniel', '9109203456789', 'pass007', 1),
('P008901234', 'Hall', 'Christopher', '8503078901234', 'pass008', 0),
('P009012345', 'Allen', 'Matthew', '9207125678901', 'pass009', 1),
('P010123456', 'Young', 'Anthony', '8811089012345', 'pass010', 1),
('P011234567', 'King', 'Donald', '7904156789012', 'pass011', 1),
('P012345678', 'Wright', 'Steven', '9008203456789', 'pass012', 1);

-- Insert test data for JOB table - Community Service Jobs (DurationPerDay converted to hours as INT)
INSERT INTO JOB (EmployerID, JobName, JobDescription, JobLocation, StartDate, EndDate, PrisonersNeeded, Completed, DurationPerDay) VALUES
(1, 'Park Cleanup', 'Cleaning and maintenance of Mandela Park', 'Mandela Park, Potchefstroom', '2025-09-01', '2025-11-30', 6, 0, 6),     -- was '06:00:00'
(2, 'Hospital Construction', 'Assisting with new community hospital build', 'New Hospital Site, CBD', '2025-08-15', '2025-03-15', 12, 0, 8),   -- was '08:00:00'
(3, 'Road Maintenance', 'Pothole repairs and road cleaning', 'Various Streets, Potchefstroom', '2025-09-10', '2025-12-10', 8, 0, 7),       -- was '07:00:00'
(4, 'School Renovation', 'Painting and repairs at local primary school', 'Mooiriver Primary School', '2025-10-01', '2025-12-15', 5, 0, 8),    -- was '07:30:00' -> rounded to 8
(5, 'Community Garden', 'Establishing vegetable gardens for soup kitchens', 'Ikageng Community Center', '2025-08-01', '2025-10-31', 4, 0, 6),  -- was '05:30:00' -> rounded to 6
(6, 'Library Restoration', 'Renovation of old municipal library', 'Central Library, Church Street', '2025-07-15', '2025-09-30', 6, 1, 7),     -- was '07:00:00'
(7, 'Elderly Home Support', 'Maintenance and landscaping services', 'Sunset Retirement Home', '2025-08-20', '2025-11-20', 3, 0, 6),           -- was '06:00:00'
(8, 'Bridge Construction', 'Assisting with new pedestrian bridge', 'Mooi River Crossing', '2025-06-01', '2025-08-31', 10, 1, 8),              -- was '08:00:00'
(1, 'Town Square Beautification', 'Landscaping and maintenance of town square', 'Central Town Square', '2025-09-15', '2025-12-31', 4, 0, 7),  -- was '06:30:00' -> rounded to 7
(3, 'Homeless Shelter Repairs', 'Building repairs and maintenance', 'Hope Haven Shelter', '2025-10-05', '2025-01-05', 7, 0, 8);               -- was '07:30:00' -> rounded to 8

-- Insert test data for PRISONER_ON_JOB table (community service assignments)
INSERT INTO PRISONER_ON_JOB (JobID, PrisonerID, [Accepted/Rejeced], JobFinished, OnPremise) VALUES
-- Park Cleanup (Job 1)
(1, 1, 1, 0, 0),
(1, 3, 1, 0, 0),
(1, 5, 1, 0, 0),
(1, 9, 1, 0, 0),
(1, 10, 1, 0, 0),
(1, 12, 0, 0, 1),

-- Hospital Construction (Job 2)
(2, 2, 1, 0, 0),
(2, 6, 1, 0, 0),
(2, 7, 1, 0, 0),
(2, 10, 1, 0, 0),
(2, 11, 1, 0, 0),
(2, 12, 1, 0, 0),

-- Road Maintenance (Job 3)
(3, 1, 1, 0, 0),
(3, 5, 1, 0, 0),
(3, 7, 1, 0, 0),
(3, 9, 1, 0, 0),

-- School Renovation (Job 4)
(4, 3, 1, 0, 0),
(4, 6, 1, 0, 0),
(4, 11, 1, 0, 0),
(4, 12, 0, 0, 1),

-- Community Garden (Job 5)
(5, 1, 1, 0, 0),
(5, 5, 1, 0, 0),
(5, 9, 1, 0, 0),

-- Library Restoration (Job 6) - Completed
(6, 2, 1, 1, 1),
(6, 3, 1, 1, 1),
(6, 7, 1, 1, 1),
(6, 10, 1, 1, 1),

-- Elderly Home Support (Job 7)
(7, 5, 1, 0, 0),
(7, 9, 1, 0, 0),
(7, 11, 1, 0, 0),

-- Bridge Construction (Job 8) - Completed
(8, 2, 1, 1, 1),
(8, 6, 1, 1, 1),
(8, 7, 1, 1, 1),
(8, 12, 1, 1, 1),

-- Town Square Beautification (Job 9)
(9, 1, 1, 0, 0),
(9, 5, 1, 0, 0),
(9, 10, 0, 0, 1),

-- Homeless Shelter Repairs (Job 10)
(10, 3, 1, 0, 0),
(10, 6, 1, 0, 0),
(10, 11, 1, 0, 0);

-- Insert test data for SKILL_OF_JOB table (skills required for community jobs)
INSERT INTO SKILL_OF_JOB (JobID, SkillID) VALUES
-- Park Cleanup needs Cleaning and Landscaping
(1, 4), (1, 5),
-- Hospital Construction needs Construction, Carpentry, Welding
(2, 12), (2, 1), (2, 2),
-- Road Maintenance needs Construction and general labor
(3, 12), (3, 4),
-- School Renovation needs Painting, Carpentry, Electrical
(4, 7), (4, 1), (4, 9),
-- Community Garden needs Landscaping
(5, 5),
-- Library Restoration needs Carpentry, Painting, Electrical
(6, 1), (6, 7), (6, 9),
-- Elderly Home Support needs Landscaping, Cleaning, Plumbing
(7, 5), (7, 4), (7, 8),
-- Bridge Construction needs Construction, Welding, Carpentry
(8, 12), (8, 2), (8, 1),
-- Town Square Beautification needs Landscaping, Cleaning
(9, 5), (9, 4),
-- Homeless Shelter Repairs needs Construction, Plumbing, Electrical
(10, 12), (10, 8), (10, 9);

-- Insert test data for SKILL_OF_PRISONER table (skills each prisoner has)
INSERT INTO SKILL_OF_PRISONER (PrisonerID, SkillID) VALUES
-- Prisoner 1 (Anderson) - Kitchen and maintenance skills
(1, 3), (1, 4), (1, 5),
-- Prisoner 2 (Thompson) - Construction and warehouse
(2, 1), (2, 6), (2, 11), (2, 12),
-- Prisoner 3 (Garcia) - Cooking and painting
(3, 3), (3, 7), (3, 10),
-- Prisoner 4 (Martinez) - Multiple trade skills (not cleared to work)
(4, 2), (4, 8), (4, 9),
-- Prisoner 5 (Robinson) - Landscaping and warehouse
(5, 5), (5, 11),
-- Prisoner 6 (Lewis) - Carpentry and laundry
(6, 1), (6, 10), (6, 12),
-- Prisoner 7 (Walker) - Maintenance skills
(7, 4), (7, 7), (7, 8),
-- Prisoner 8 (Hall) - Electrical and plumbing (not cleared to work)
(8, 8), (8, 9),
-- Prisoner 9 (Allen) - Landscaping and painting
(9, 5), (9, 7),
-- Prisoner 10 (Young) - Multiple skills
(10, 1), (10, 10), (10, 11),
-- Prisoner 11 (King) - Construction and data entry
(11, 6), (11, 12),
-- Prisoner 12 (Wright) - General maintenance
(12, 4), (12, 11);

-- Verification queries to check the data
SELECT 'EMPLOYER Records' as TableName, COUNT(*) as RecordCount FROM EMPLOYER
UNION ALL
SELECT 'SKILL Records', COUNT(*) FROM SKILL
UNION ALL
SELECT 'PRISONER Records', COUNT(*) FROM PRISONER
UNION ALL
SELECT 'JOB Records', COUNT(*) FROM JOB
UNION ALL
SELECT 'PRISONER_ON_JOB Records', COUNT(*) FROM PRISONER_ON_JOB
UNION ALL
SELECT 'SKILL_OF_JOB Records', COUNT(*) FROM SKILL_OF_JOB
UNION ALL
SELECT 'SKILL_OF_PRISONER Records', COUNT(*) FROM SKILL_OF_PRISONER;