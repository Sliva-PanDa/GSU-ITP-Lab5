using Microsoft.EntityFrameworkCore;
using PortalNauchnyhPublikatsiy.Application.Interfaces;
using PortalNauchnyhPublikatsiy.Domain.Entities;
using PortalNauchnyhPublikatsiy.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalNauchnyhPublikatsiy.Infrastructure.Repositories
{
    public class PublicationRepository : IPublicationRepository
    {
        private readonly ApplicationDbContext _context;

        public PublicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Publication?> GetByIdAsync(int id)
        {
            return await _context.Publications
                .Include(p => p.JournalConference) 
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Publication>> GetAllAsync(string? searchString, int? year)
        {
            var query = _context.Publications.Include(p => p.JournalConference).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Title.Contains(searchString));
            }

            if (year.HasValue)
            {
                query = query.Where(p => p.Year == year.Value);
            }

            return await query.ToListAsync();
        }

        public async Task AddAsync(Publication publication)
        {
            await _context.Publications.AddAsync(publication);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Publication publication)
        {
            _context.Publications.Update(publication);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var publication = await _context.Publications.FindAsync(id);
            if (publication != null)
            {
                _context.Publications.Remove(publication);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Publication>> GetPublicationsByTeacherIdAsync(int teacherId)
        {
            return await _context.Publications
                .Include(p => p.JournalConference)
                .Where(p => _context.PublicationAuthors.Any(pa => pa.PublicationId == p.Id && pa.TeacherId == teacherId))
                .OrderByDescending(p => p.Year)
                .ToListAsync();
        }

        public async Task<IEnumerable<Publication>> GetPublicationsByDepartmentAndYearAsync(int departmentId, int year)
        {
            // 1. Находим всех преподавателей указанной кафедры
            var teacherIds = await _context.Teachers
                .Where(t => t.DepartmentId == departmentId)
                .Select(t => t.Id)
                .ToListAsync();

            // 2. Находим все публикации этих преподавателей за указанный год
            return await _context.Publications
                .Include(p => p.JournalConference)
                .Where(p => p.Year == year &&
                            _context.PublicationAuthors.Any(pa => pa.PublicationId == p.Id && teacherIds.Contains(pa.TeacherId)))
                .OrderBy(p => p.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<Publication>> GetPublicationsByTeacherAndYearAsync(int teacherId, int year)
        {
            return await _context.Publications
                .Include(p => p.JournalConference)
                .Where(p => p.Year == year &&
                            _context.PublicationAuthors.Any(pa => pa.PublicationId == p.Id && pa.TeacherId == teacherId))
                .OrderBy(p => p.Title)
                .ToListAsync();
        }
    }
}
