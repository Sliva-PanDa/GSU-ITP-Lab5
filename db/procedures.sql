USE PortalNauchnyhPublikatsiyDB;
GO

-- Процедура: Поиск публикаций (по автору, году, типу, журналу, ключам, проекту) с пагинацией (доп. требование)
CREATE PROCEDURE sp_SearchPublications
    @AuthorFullName NVARCHAR(255) = NULL,
    @Year INT = NULL,
    @Type NVARCHAR(50) = NULL,
    @JournalName NVARCHAR(255) = NULL,
    @Keywords NVARCHAR(500) = NULL,
    @ProjectName NVARCHAR(500) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SELECT 
        p.Id, p.Title, p.Type, p.Year, p.DOI, jc.Name AS Journal,
        t.FullName AS Author
    FROM Publications p
    INNER JOIN PublicationAuthors pa ON p.Id = pa.PublicationId
    INNER JOIN Teachers t ON pa.TeacherId = t.Id
    INNER JOIN JournalsConferences jc ON p.JournalConferenceId = jc.Id
    LEFT JOIN PublicationProjects pp ON p.Id = pp.PublicationId
    LEFT JOIN Projects pr ON pp.ProjectId = pr.Id
    WHERE (@AuthorFullName IS NULL OR t.FullName LIKE '%' + @AuthorFullName + '%')
      AND (@Year IS NULL OR p.Year = @Year)
      AND (@Type IS NULL OR p.Type = @Type)
      AND (@JournalName IS NULL OR jc.Name LIKE '%' + @JournalName + '%')
      AND (@Keywords IS NULL OR p.Title LIKE '%' + @Keywords + '%')
      AND (@ProjectName IS NULL OR pr.Name LIKE '%' + @ProjectName + '%')
    ORDER BY p.Year DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Функция: Индекс Хирша для преподавателя (доп. требование, упрощённо: MAX(rn) где rn <= COUNT публикаций по годам)
CREATE FUNCTION fn_GetHirschIndex (@TeacherId INT)
RETURNS INT
AS
BEGIN
    DECLARE @Hirsch INT = 0;
    WITH RankedPubs AS (
        SELECT ROW_NUMBER() OVER (ORDER BY p.Year DESC) AS rn
        FROM Publications p
        INNER JOIN PublicationAuthors pa ON p.Id = pa.PublicationId
        WHERE pa.TeacherId = @TeacherId
    )
    SELECT @Hirsch = MAX(rn) FROM RankedPubs WHERE rn <= (SELECT COUNT(*) FROM RankedPubs);
    RETURN ISNULL(@Hirsch, 0);
END;
GO

-- Процедура: Кол-во публикаций в Q1-Q2 для препода (доп. требование)
CREATE PROCEDURE sp_GetQ1Q2CountForTeacher
    @TeacherId INT
AS
BEGIN
    SELECT COUNT(*) AS Q1Q2Count
    FROM Publications p
    INNER JOIN PublicationAuthors pa ON p.Id = pa.PublicationId
    INNER JOIN JournalsConferences jc ON p.JournalConferenceId = jc.Id
    WHERE pa.TeacherId = @TeacherId AND jc.Rating IN ('Q1', 'Q2');
END;
GO

-- Процедура: Список публикаций препода/кафедры за год (доп. требование)
CREATE PROCEDURE sp_GetPublicationsByTeacherOrDepartmentAndYear
    @TeacherId INT = NULL,
    @DepartmentId INT = NULL,
    @Year INT
AS
BEGIN
    SELECT p.Id, p.Title, p.Type, jc.Name AS Journal
    FROM Publications p
    INNER JOIN PublicationAuthors pa ON p.Id = pa.PublicationId
    INNER JOIN Teachers t ON pa.TeacherId = t.Id
    INNER JOIN JournalsConferences jc ON p.JournalConferenceId = jc.Id
    WHERE p.Year = @Year
      AND (@TeacherId IS NULL OR t.Id = @TeacherId)
      AND (@DepartmentId IS NULL OR t.DepartmentId = @DepartmentId);
END;
GO

-- Процедура: Отчёт по участию в проектах для препода (доп. требование)
CREATE PROCEDURE sp_GetProjectReportForTeacher
    @TeacherId INT
AS
BEGIN
    SELECT pr.Id, pr.Name, pr.Number, pr.FundingOrg, pr.StartDate, pr.EndDate,
           CASE WHEN pp.TeacherId IS NOT NULL THEN 'Участник' ELSE 'Руководитель' END AS Role
    FROM Projects pr
    LEFT JOIN ProjectParticipants pp ON pr.Id = pp.ProjectId AND pp.TeacherId = @TeacherId
    WHERE pr.LeaderId = @TeacherId OR pp.TeacherId = @TeacherId;
END;
GO

-- Базовый CRUD для Publications (Create/Read/Update/Delete, с пагинацией для Read)
-- Create
CREATE PROCEDURE sp_InsertPublication
    @Title NVARCHAR(500),
    @Type NVARCHAR(50),
    @Year INT,
    @JournalConferenceId INT,
    @DOI NVARCHAR(255) = NULL,
    @FilePath NVARCHAR(500) = NULL
AS
BEGIN
    INSERT INTO Publications (Title, Type, Year, JournalConferenceId, DOI, FilePath)
    VALUES (@Title, @Type, @Year, @JournalConferenceId, @DOI, @FilePath);
    SELECT SCOPE_IDENTITY() AS NewId;
END;
GO

-- Read all with pagination
CREATE PROCEDURE sp_GetAllPublications
    @PageNumber INT = 1,
    @PageSize INT = 10
AS
BEGIN
    SELECT * FROM Publications
    ORDER BY Year DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Update
CREATE PROCEDURE sp_UpdatePublication
    @Id INT,
    @Title NVARCHAR(500),
    @Type NVARCHAR(50),
    @Year INT,
    @JournalConferenceId INT
AS
BEGIN
    UPDATE Publications SET Title = @Title, Type = @Type, Year = @Year, JournalConferenceId = @JournalConferenceId
    WHERE Id = @Id;
END;
GO

-- Delete
CREATE PROCEDURE sp_DeletePublication
    @Id INT
AS
BEGIN
    DELETE FROM Publications WHERE Id = @Id;
END;
GO