using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortalNauchnyhPublikatsiy.Application.Interfaces;
using PortalNauchnyhPublikatsiy.Domain.Entities;
using PortalNauchnyhPublikatsiy.Infrastructure.Data;

namespace PortalNauchnyhPublikatsiy.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetProjectsByTeacherIdAsync(int teacherId)
        {
            // Находим проекты, где преподаватель - руководитель
            var leaderProjects = await _context.Projects
                .Where(p => p.LeaderId == teacherId)
                .ToListAsync();

            // Находим проекты, где преподаватель - участник
            var participantProjectIds = await _context.ProjectParticipants
                .Where(pp => pp.TeacherId == teacherId)
                .Select(pp => pp.ProjectId)
                .ToListAsync();

            var participantProjects = await _context.Projects
                .Where(p => participantProjectIds.Contains(p.Id))
                .ToListAsync();

            // Объединяем списки, исключая дубликаты
            return leaderProjects.Union(participantProjects).OrderByDescending(p => p.StartDate);
        }
    }
}