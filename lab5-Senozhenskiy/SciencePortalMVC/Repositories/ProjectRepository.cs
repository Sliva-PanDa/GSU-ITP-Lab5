using Microsoft.EntityFrameworkCore;
using SciencePortalMVC.Data;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;

namespace SciencePortalMVC.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly SciencePortalDbContext _context;

        public ProjectRepository(SciencePortalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects.Include(p => p.Leader).ToListAsync();
        }
        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects.Include(p => p.Leader).FirstOrDefaultAsync(p => p.ProjectId == id);
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Projects.AnyAsync(e => e.ProjectId == id);
        }
        public async Task<IEnumerable<Teacher>> GetAllTeachersAsync()
        {
            return await _context.Teachers.ToListAsync();
        }
    }
}