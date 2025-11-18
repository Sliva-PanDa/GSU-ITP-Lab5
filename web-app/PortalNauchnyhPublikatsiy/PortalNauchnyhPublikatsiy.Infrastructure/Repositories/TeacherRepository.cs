using Microsoft.EntityFrameworkCore;
using PortalNauchnyhPublikatsiy.Application.Interfaces;
using PortalNauchnyhPublikatsiy.Domain.Entities;
using PortalNauchnyhPublikatsiy.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortalNauchnyhPublikatsiy.Infrastructure.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _context;

        public TeacherRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Teacher>> GetAllAsync()
        {
            return await _context.Teachers
                .Include(t => t.Department) 
                .ToListAsync();
        }

        public async Task<Teacher?> GetByIdAsync(int id)
        {
            return await _context.Teachers
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id);
        }
        public async Task<int> GetHirschIndexAsync(int teacherId)
        {
            var sql = $"SELECT dbo.fn_GetHirschIndex({teacherId})";
            var result = await _context.Database.ExecuteSqlRawAsync(sql);


            var hIndex = _context.Database.SqlQuery<int>($"SELECT dbo.fn_GetHirschIndex({teacherId})").AsEnumerable().FirstOrDefault();
            return hIndex;
        }

        public async Task<int> GetQ1Q2CountAsync(int teacherId)
        {
            // Вызов хранимой процедуры, которая возвращает одно значение
            var q1q2Count = _context.Database.SqlQuery<int>($"EXEC sp_GetQ1Q2CountForTeacher @TeacherId={teacherId}").AsEnumerable().FirstOrDefault();
            return q1q2Count;
        }
    }
}