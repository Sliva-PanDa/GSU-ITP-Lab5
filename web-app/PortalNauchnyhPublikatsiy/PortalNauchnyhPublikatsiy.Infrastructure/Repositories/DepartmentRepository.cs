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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context) => _context = context;

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments.OrderBy(d => d.Name).ToListAsync();
        }
    }
}
