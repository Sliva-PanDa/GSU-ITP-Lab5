USE PortalNauchnyhPublikatsiyDB;
GO

-- 1. Departments (Кафедры)
CREATE TABLE Departments (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL UNIQUE,
    Profile NVARCHAR(MAX)
);
GO

-- 2. Teachers (Преподаватели)
CREATE TABLE Teachers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(255) NOT NULL,
    Position NVARCHAR(100) NOT NULL,
    Degree NVARCHAR(50),
    DepartmentId INT FOREIGN KEY REFERENCES Departments(Id)
);
GO

-- 3. JournalsConferences (Журналы/Конференции)
CREATE TABLE JournalsConferences (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Rating NVARCHAR(10) CHECK (Rating IN ('Q1', 'Q2', 'Q3', 'Q4')),
    Publisher NVARCHAR(255),
    ISSNISBN NVARCHAR(20) UNIQUE
);
GO

-- 4. ScientificDirections (Научные направления)
CREATE TABLE ScientificDirections (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL UNIQUE,
    Description NVARCHAR(MAX)
);
GO

-- 5. Publications (Публикации)
CREATE TABLE Publications (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(500) NOT NULL,
    Type NVARCHAR(50) NOT NULL CHECK (Type IN ('Статья', 'Тезисы', 'Монография')),
    Year INT NOT NULL CHECK (Year >= 1900 AND Year <= 2100),
    DOI NVARCHAR(255) UNIQUE,
    FilePath NVARCHAR(500),
    JournalConferenceId INT FOREIGN KEY REFERENCES JournalsConferences(Id)
);
GO

-- 6. PublicationAuthors (Junction M:N)
CREATE TABLE PublicationAuthors (
    PublicationId INT FOREIGN KEY REFERENCES Publications(Id) ON DELETE CASCADE,
    TeacherId INT FOREIGN KEY REFERENCES Teachers(Id) ON DELETE CASCADE,
    PRIMARY KEY (PublicationId, TeacherId)
);
GO

-- 7. Projects (Проекты/Гранты)
CREATE TABLE Projects (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(500) NOT NULL,
    Number NVARCHAR(100) UNIQUE,
    FundingOrg NVARCHAR(255) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE,
    LeaderId INT FOREIGN KEY REFERENCES Teachers(Id),
    CONSTRAINT CHK_EndDate CHECK (EndDate >= StartDate OR EndDate IS NULL)
);
GO

-- 8. ProjectParticipants (Junction M:N)
CREATE TABLE ProjectParticipants (
    ProjectId INT FOREIGN KEY REFERENCES Projects(Id) ON DELETE CASCADE,
    TeacherId INT FOREIGN KEY REFERENCES Teachers(Id) ON DELETE CASCADE,
    PRIMARY KEY (ProjectId, TeacherId)
);
GO

-- 9. PublicationProjects (Junction M:N)
CREATE TABLE PublicationProjects (
    PublicationId INT FOREIGN KEY REFERENCES Publications(Id) ON DELETE CASCADE,
    ProjectId INT FOREIGN KEY REFERENCES Projects(Id) ON DELETE CASCADE,
    PRIMARY KEY (PublicationId, ProjectId)
);
GO