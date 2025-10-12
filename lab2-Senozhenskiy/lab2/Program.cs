using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using lab2.Data;
using lab2.Models;

namespace lab2 
{
    class Program
    {
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");


            var optionsBuilder = new DbContextOptionsBuilder<SciencePortalDbContext>();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;


            using (var db = new SciencePortalDbContext(options))
            {
                Console.WriteLine("Подключение к базе данных успешно.");
                Console.WriteLine("Нажмите любую клавишу для выполнения LINQ-запросов...");
                Console.ReadKey();

                // 2.1: Выборка всех данных из таблицы на стороне «один» (Departments)
                Console.WriteLine("\n--- 2.1: Все кафедры (первые 5) ---");
                var allDepartments = db.Departments.Take(5).ToList();
                foreach (var dept in allDepartments)
                {
                    Console.WriteLine($"ID: {dept.DepartmentId}, Название: {dept.Name}");
                }

                // 2.2: Выборка данных из таблицы «один» с фильтром (Teachers)
                Console.WriteLine("\n--- 2.2: Преподаватели со степенью 'Доктор наук' (первые 5) ---");
                var doctors = db.Teachers.Where(t => t.Degree == "Доктор наук").Take(5).ToList();
                if (doctors.Any())
                {
                    foreach (var teacher in doctors)
                    {
                        Console.WriteLine($"ID: {teacher.TeacherId}, ФИО: {teacher.FullName}, Степень: {teacher.Degree}");
                    }
                }
                else
                {
                    Console.WriteLine("Преподаватели с такой степенью не найдены.");
                }

                // 2.3: Выборка данных с группировкой из таблицы «многие» (Publications)
                Console.WriteLine("\n--- 2.3: Количество публикаций по годам ---");
                var publicationsByYear = db.Publications
                    .GroupBy(p => p.Year.Year)
                    .Select(g => new { Year = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Year)
                    .Take(5)
                    .ToList();
                foreach (var item in publicationsByYear)
                {
                    Console.WriteLine($"Год: {item.Year}, Количество публикаций: {item.Count}");
                }

                // 2.4: Выборка из двух таблиц (Projects и Teachers)
                Console.WriteLine("\n--- 2.4: Проекты и их руководители (первые 5) ---");
                var projectsWithLeaders = db.Projects
                    .Include(p => p.Leader)
                    .Select(p => new
                    {
                        ProjectName = p.Name,
                        LeaderName = p.Leader != null ? p.Leader.FullName : "Не назначен"
                    })
                    .Take(5)
                    .ToList();

                foreach (var item in projectsWithLeaders)
                {
                    Console.WriteLine($"Проект: '{item.ProjectName}', Руководитель: {item.LeaderName}");
                }

                // 2.5: Выборка из двух таблиц с фильтром (Projects)
                Console.WriteLine("\n--- 2.5: Проекты, начатые после 1 января 2023 года ---");
                var dateFilter = new DateOnly(2023, 1, 1);
                var recentProjects = db.Projects
                    .Where(p => p.StartDate > dateFilter)
                    .Include(p => p.Leader)
                    .Select(p => new
                    {
                        ProjectName = p.Name,
                        LeaderName = p.Leader != null ? p.Leader.FullName : "Не назначен"
                    })
                    .Take(5)
                    .ToList();

                if (recentProjects.Any())
                {
                    foreach (var item in recentProjects)
                    {
                        Console.WriteLine($"Проект: '{item.ProjectName}', Руководитель: {item.LeaderName}");
                    }
                }
                else
                {
                    Console.WriteLine("Проекты, начатые после 2023 года, не найдены.");
                }

                Console.WriteLine("\nНажмите любую клавишу для выполнения операций вставки, обновления и удаления...");
                Console.ReadKey();

                // 2.6: Вставка данных в таблицу «один» (ScientificDirections)
                Console.WriteLine("\n--- 2.6: Вставка нового научного направления ---");
                var newDirection = new ScientificDirection
                {
                    Name = "Квантовые вычисления",
                    Description = "Исследование вычислений с использованием квантовых эффектов."
                };
                db.ScientificDirections.Add(newDirection);
                db.SaveChanges();
                Console.WriteLine($"Направление '{newDirection.Name}' добавлено с ID: {newDirection.DirectionId}");

                // 2.7: Вставка данных в таблицу «многие» (Publications)
                Console.WriteLine("\n--- 2.7: Вставка новой публикации в это направление ---");
                var newPublication = new Publication
                {
                    Title = "Основы квантовых алгоритмов",
                    Type = "статья",
                    Year = new DateTime(DateTime.Now.Year, 1, 1),
                    DirectionId = newDirection.DirectionId
                };
                db.Publications.Add(newPublication);
                db.SaveChanges();
                Console.WriteLine($"Публикация '{newPublication.Title}' добавлена с ID: {newPublication.PublicationId}");

                // 2.10: Обновление данных (меняем год у добавленной публикации)
                Console.WriteLine("\n--- 2.10: Обновление года у новой публикации ---");
                var publicationToUpdate = db.Publications.Find(newPublication.PublicationId);
                if (publicationToUpdate != null)
                {
                    publicationToUpdate.Year = new DateTime(DateTime.Now.Year - 1, 1, 1);
                    db.SaveChanges();
                    Console.WriteLine($"Год публикации с ID {publicationToUpdate.PublicationId} обновлен на {publicationToUpdate.Year.Year}.");
                }

                // 2.9: Удаление данных из таблицы «многие» (удаляем созданную публикацию)
                Console.WriteLine("\n--- 2.9: Удаление созданной публикации ---");
                var publicationToDelete = db.Publications.Find(newPublication.PublicationId);
                if (publicationToDelete != null)
                {
                    db.Publications.Remove(publicationToDelete);
                    db.SaveChanges();
                    Console.WriteLine($"Публикация с ID {newPublication.PublicationId} удалена.");
                }

                // 2.8: Удаление данных из таблицы «один» (удаляем созданное направление)
                Console.WriteLine("\n--- 2.8: Удаление созданного научного направления ---");
                var directionToDelete = db.ScientificDirections.Find(newDirection.DirectionId);
                if (directionToDelete != null)
                {
                    db.ScientificDirections.Remove(directionToDelete);
                    db.SaveChanges();
                    Console.WriteLine($"Направление с ID {directionToDelete.DirectionId} удалено.");
                }

                Console.WriteLine("\nРабота программы завершена. Нажмите любую клавишу для выхода.");
                Console.ReadKey();
            }
        }
    }
}