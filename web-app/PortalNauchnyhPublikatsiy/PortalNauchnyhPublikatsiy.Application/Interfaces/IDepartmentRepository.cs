using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PortalNauchnyhPublikatsiy.Domain.Entities;

namespace PortalNauchnyhPublikatsiy.Application.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
    }
}
