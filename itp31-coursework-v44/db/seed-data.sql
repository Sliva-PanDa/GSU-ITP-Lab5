USE PortalNauchnyhPublikatsiyDB;
GO

-- Полностью отключаем проверку внешних ключей
EXEC sp_MSforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all";
GO

-- Очистка ВСЕХ таблиц
DELETE FROM PublicationProjects;
DELETE FROM ProjectParticipants;
DELETE FROM PublicationAuthors;
DELETE FROM Projects;
DELETE FROM Publications;
DELETE FROM ScientificDirections;
DELETE FROM JournalsConferences;
DELETE FROM Teachers;
DELETE FROM Departments;
GO

-- Включаем проверку внешних ключей обратно
EXEC sp_MSforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all";
GO

-- СБРОС идентификаторов (чтобы ID начинались с 1)
DBCC CHECKIDENT ('Departments', RESEED, 0);
DBCC CHECKIDENT ('Teachers', RESEED, 0);
DBCC CHECKIDENT ('JournalsConferences', RESEED, 0);
DBCC CHECKIDENT ('Publications', RESEED, 0);
DBCC CHECKIDENT ('Projects', RESEED, 0);
DBCC CHECKIDENT ('ScientificDirections', RESEED, 0);
GO

-- Теперь вставка в правильном порядке:

-- 1. Кафедры (3 записи)
INSERT INTO Departments (Name, Profile) VALUES
(N'Информатика', N'Программная инженерия и ИИ'),
(N'Математика', N'Прикладная математика и алгоритмы'),
(N'Физика', N'Квантовая физика и моделирование');
GO

-- 2. Преподаватели (4 записи)
INSERT INTO Teachers (FullName, Position, Degree, DepartmentId) VALUES
(N'Иванов Иван Иванович', N'Доцент', N'К.т.н.', 1),
(N'Петров Петр Петрович', N'Профессор', N'Д.ф.м.н.', 2),
(N'Сидорова Светлана Сергеевна', N'Ст. преподаватель', N'К.ф.н.', 1),
(N'Кузнецов Константин Константинович', N'Ассистент', N'М.ф.н.', 3);
GO

-- 3. Журналы/Конференции (3 записи)
INSERT INTO JournalsConferences (Name, Rating, Publisher, ISSNISBN) VALUES
(N'IEEE Transactions on AI', N'Q1', N'IEEE', N'0098-5589'),
(N'Journal of Applied Math', N'Q2', N'Springer', N'1110-757X'),
(N'International Conference on Physics', N'Q3', N'ACM', N'978-1-4503-0000-0');
GO

-- 4. Научные направления (2 записи)
INSERT INTO ScientificDirections (Name, Description) VALUES
(N'ИИ и ML', N'Искусственный интеллект и машинное обучение'),
(N'Алгоритмы', N'Оптимизация и сложность алгоритмов');
GO

-- 5. Публикации (5 записей)
INSERT INTO Publications (Title, Type, Year, JournalConferenceId, DOI, FilePath) VALUES
(N'Методы ИИ в образовании', N'Статья', 2024, 1, N'10.1109/TSE.2024.123', N'/files/pub1.pdf'),
(N'Алгоритмы графа', N'Монография', 2023, 2, N'10.1007/978-3-031-10000-0', N'/files/pub2.pdf'),
(N'Тезисы по ML', N'Тезисы', 2022, 3, NULL, N'/files/pub3.pdf'),
(N'Квантовая модель', N'Статья', 2024, 1, N'10.1109/QP.2024.456', N'/files/pub4.pdf'),
(N'Оптимизация', N'Статья', 2023, 2, N'10.1007/978-3-031-12000-0', N'/files/pub5.pdf');
GO

-- 6. PublicationAuthors (6 записей)
INSERT INTO PublicationAuthors (PublicationId, TeacherId) VALUES
(1, 1), (1, 3), -- Иванов и Сидорова для pub1
(2, 2), -- Петров для pub2
(3, 1), -- Иванов для pub3
(4, 4), -- Кузнецов для pub4
(5, 2); -- Петров для pub5
GO

-- 7. Проекты (3 записи)
INSERT INTO Projects (Name, Number, FundingOrg, StartDate, EndDate, LeaderId) VALUES
(N'Грант по ИИ', N'GR-2024-001', N'РФФИ', '2024-01-01', '2025-12-31', 1),
(N'Оптимизация алгоритмов', N'GR-2023-002', N'Минобр', '2023-06-01', NULL, 2),
(N'Квантовая модель', N'GR-2024-003', N'РФФИ', '2024-03-01', '2025-06-30', 4);
GO

-- 8. ProjectParticipants (3 записи)
INSERT INTO ProjectParticipants (ProjectId, TeacherId) VALUES
(1, 3), -- Сидорова в проекте 1
(2, 1), -- Иванов в проекте 2
(3, 2); -- Петров в проекте 3
GO

-- 9. PublicationProjects (3 записей)
INSERT INTO PublicationProjects (PublicationId, ProjectId) VALUES
(1, 1), -- pub1 из проекта 1
(2, 2), -- pub2 из проекта 2
(4, 3); -- pub4 из проекта 3
GO

-- Проверка что все вставилось
SELECT 'Departments' as TableName, COUNT(*) as Count FROM Departments
UNION ALL SELECT 'Teachers', COUNT(*) FROM Teachers
UNION ALL SELECT 'JournalsConferences', COUNT(*) FROM JournalsConferences
UNION ALL SELECT 'Publications', COUNT(*) FROM Publications
UNION ALL SELECT 'PublicationAuthors', COUNT(*) FROM PublicationAuthors
UNION ALL SELECT 'Projects', COUNT(*) FROM Projects
UNION ALL SELECT 'ProjectParticipants', COUNT(*) FROM ProjectParticipants
UNION ALL SELECT 'PublicationProjects', COUNT(*) FROM PublicationProjects;