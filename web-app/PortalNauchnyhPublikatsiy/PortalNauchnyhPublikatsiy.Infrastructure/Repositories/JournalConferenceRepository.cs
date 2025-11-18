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
    public class JournalConferenceRepository : IJournalConferenceRepository
    {
        private readonly ApplicationDbContext _context;

        public JournalConferenceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JournalsConferences>> GetAllAsync()
        {
            return await _context.JournalsConferences.OrderBy(j => j.Name).ToListAsync();
        }
    }
}
