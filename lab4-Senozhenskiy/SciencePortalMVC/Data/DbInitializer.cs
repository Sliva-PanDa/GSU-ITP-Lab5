using SciencePortalMVC.Models;
using System;
using System.Linq;

namespace SciencePortalMVC.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SciencePortalDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Departments.Any())
            {
                return; 
            }

            var departments = new Department[]
            {
                new Department { Name = "Информационные технологии", Profile = "ИТ" },
                new Department { Name = "Автоматизированный электропривод", Profile = "АЭП" },
                new Department { Name = "Промышленная электроника", Profile = "ПЭ" }
            };
            context.Departments.AddRange(departments);
            context.SaveChanges();

            var teachers = new Teacher[]
            {
                new Teacher { FullName = "Асенчик О.Д.", Position = "Доцент", Degree = "к.т.н.", DepartmentId = 1 },
                new Teacher { FullName = "Иванов И.И.", Position = "Профессор", Degree = "д.т.н.", DepartmentId = 2 },
                new Teacher { FullName = "Петров П.П.", Position = "Ассистент", Degree = "", DepartmentId = 1 },
                new Teacher { FullName = "Сидоров С.С.", Position = "Ст. преподаватель", Degree = "к.ф.-м.н", DepartmentId = 3 }
            };
            context.Teachers.AddRange(teachers);
            context.SaveChanges();
        }
    }
}