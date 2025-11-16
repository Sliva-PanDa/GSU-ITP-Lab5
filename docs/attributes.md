# Атрибуты сущностей (логическая модель БД, вариант 44)

## Кафедры (Departments)
| Название | Описание | Тип БД | Тип C# | Ограничения |
|----------|----------|--------|--------|-------------|
| Id | Первичный ключ | INT PK IDENTITY | int | >0, auto-increment |
| Name | Название кафедры | NVARCHAR(255) NOT NULL | string | NOT NULL, UNIQUE, Length <=255 |
| Profile | Профиль кафедры | NVARCHAR(MAX) | string |  |

## Преподаватели (Teachers)
| Название | Описание | Тип БД | Тип C# | Ограничения |
|----------|----------|--------|--------|-------------|
| Id | Первичный ключ | INT PK IDENTITY | int | >0 |
| FullName | ФИО | NVARCHAR(255) NOT NULL | string | NOT NULL, Length <=255 |
| Position | Должность | NVARCHAR(100) NOT NULL | string | NOT NULL, Length <=100 |
| Degree | Ученая степень | NVARCHAR(50) | string | Length <=50 |
| DepartmentId | Кафедра (FK) | INT FK REFERENCES Departments(Id) | int |  |

## Журналы/Конференции (JournalsConferences)
| Название | Описание | Тип БД | Тип C# | Ограничения |
|----------|----------|--------|--------|-------------|
| Id | Первичный ключ | INT PK IDENTITY | int | >0 |
| Name | Название | NVARCHAR(255) NOT NULL | string | NOT NULL, Length <=255 |
| Rating | Рейтинг (Q1-Q4) | NVARCHAR(10) | string | CHECK (Rating IN ('Q1', 'Q2', 'Q3', 'Q4')) |
| Publisher | Издатель | NVARCHAR(255) | string | Length <=255 |
| ISSNISBN | ISSN/ISBN | NVARCHAR(20) UNIQUE | string | UNIQUE, Length <=20 |

## Научные направления (ScientificDirections)
| Название | Описание | Тип БД | Тип C# | Ограничения |
|----------|----------|--------|--------|-------------|
| Id | Первичный ключ | INT PK IDENTITY | int | >0 |
| Name | Название направления | NVARCHAR(255) NOT NULL | string | NOT NULL, UNIQUE |
| Description | Описание | NVARCHAR(MAX) | string |  |

## Публикации (Publications)
| Название | Описание | Тип БД | Тип C# | Ограничения |
|----------|----------|--------|--------|-------------|
| Id | Первичный ключ | INT PK IDENTITY | int | >0 |
| Title | Название публикации | NVARCHAR(500) NOT NULL | string | NOT NULL, Length <=500 |
| Type | Тип (статья/тезисы/монография) | NVARCHAR(50) NOT NULL | string | NOT NULL, CHECK (Type IN ('Статья', 'Тезисы', 'Монография')) |
| Year | Год | INT NOT NULL | int | NOT NULL, CHECK (Year >=1900 AND Year <=2100) |
| DOI | DOI/ссылка | NVARCHAR(255) UNIQUE | string | UNIQUE, Length <=255 |
| FilePath | Путь к файлу | NVARCHAR(500) | string | Length <=500 |
| JournalConferenceId | Журнал/конференция (FK) | INT FK REFERENCES JournalsConferences(Id) | int |  |

## PublicationAuthors (junction M:N, Авторы)
| Название | Описание | Тип БД | Тип C# | Ограничения |
|----------|----------|--------|--------|-------------|
| PublicationId | FK Публикация | INT FK REFERENCES Publications(Id) ON DELETE CASCADE | int |  |
| TeacherId | FK Автор | INT FK REFERENCES Teachers(Id) ON DELETE CASCADE | int |  |
| (Composite PK) | PublicationId + TeacherId | PRIMARY KEY (PublicationId, TeacherId) |  | UNIQUE pair |

## Проекты/Гранты (Projects)
| Название | Описание | Тип БД | Тип C# | Ограничения |
|----------|----------|--------|--------|-------------|
| Id | Первичный ключ | INT PK IDENTITY | int | >0 |
| Name | Название проекта | NVARCHAR(500) NOT NULL | string | NOT NULL, Length <=500 |
| Number | Номер | NVARCHAR(100) UNIQUE | string | UNIQUE, Length <=100 |
| FundingOrg | Финансирующая организация | NVARCHAR(255) NOT NULL | string | NOT NULL, Length <=255 |
| StartDate | Дата начала | DATE NOT NULL | DateTime | NOT NULL |
| EndDate | Дата окончания | DATE | DateTime | CHECK (EndDate >= StartDate OR EndDate IS NULL) |
| LeaderId | Руководитель (FK) | INT FK REFERENCES Teachers(Id) | int |  |

## ProjectParticipants (junction M:N, Участники)
| Название | Описание | Тип БД | Тип C# | Ограничения |
|----------|----------|--------|--------|-------------|
| ProjectId | FK Проект | INT FK REFERENCES Projects(Id) ON DELETE CASCADE | int |  |
| TeacherId | FK Участник | INT FK REFERENCES Teachers(Id) ON DELETE CASCADE | int |  |
| (Composite PK) | ProjectId + TeacherId | PRIMARY KEY (ProjectId, TeacherId) |  | UNIQUE pair |

## PublicationProjects (junction M:N, Связи публикаций с проектами)
| Название | Описание | Тип БД | Тип C# | Ограничения |
|----------|----------|--------|--------|-------------|
| PublicationId | FK Публикация | INT FK REFERENCES Publications(Id) ON DELETE CASCADE | int |  |
| ProjectId | FK Проект | INT FK REFERENCES Projects(Id) ON DELETE CASCADE | int |  |
| (Composite PK) | PublicationId + ProjectId | PRIMARY KEY (PublicationId, ProjectId) |  | UNIQUE pair |

**Связи между сущностями:** 1:N (Departments 1--N Teachers; JournalsConferences 1--N Publications; Projects 1--N LeaderId). M:N (Teachers M--N Publications via PublicationAuthors; Teachers M--N Projects via ProjectParticipants; Publications M--N Projects via PublicationProjects). Нормализация: 3NF (FK/junction вместо дубликатов, нет транзитивных зависимостей).