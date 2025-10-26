using SciencePortalWebApp.Models;
using System;
using System.Linq;

namespace SciencePortalWebApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SciencePortalDbContext context)
        {
            context.Database.EnsureCreated();

            // Проверяем, есть ли уже данные
            if (context.Departments.Any())
            {
                return;   // БД уже заполнена
            }

            // Добавляем кафедры
            for (int i = 1; i <= 10; i++)
            {
                context.Departments.Add(new Department { Name = $"Кафедра {i}", Profile = $"Профиль {i}" });
            }
            context.SaveChanges();

            // Добавляем преподавателей
            for (int i = 1; i <= 500; i++)
            {
                context.Teachers.Add(new Teacher { FullName = $"Преподаватель {i}", Position = "Должность", Degree = "Степень", DepartmentId = (i % 10) + 1 });
            }
            context.SaveChanges();

            // Добавляем проекты
            for (int i = 1; i <= 500; i++)
            {
                context.Projects.Add(new Project { Name = $"Проект {i}", Number = $"P-{i}", FundingOrg = "Фонд", StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-i * 5)), LeaderId = (i % 500) + 1 });
            }
            context.SaveChanges();
            
            // Добавляем журналы
            for (int i = 1; i <= 500; i++)
            {
                context.JournalsConferences.Add(new JournalsConference { Name = $"Журнал {i}" });
            }
            context.SaveChanges();

            // Добавляем направления
            for (int i = 1; i <= 500; i++)
            {
                context.ScientificDirections.Add(new ScientificDirection { Name = $"Направление {i}" });
            }
            context.SaveChanges();
        }
    }
}