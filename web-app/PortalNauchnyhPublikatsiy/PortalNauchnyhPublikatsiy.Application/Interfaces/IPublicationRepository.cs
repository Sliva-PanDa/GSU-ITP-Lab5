using PortalNauchnyhPublikatsiy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalNauchnyhPublikatsiy.Application.Interfaces
{
    public interface IPublicationRepository
    {
        Task<Publication?> GetByIdAsync(int id);
        Task<IEnumerable<Publication>> GetAllAsync(string? searchString, int? year);
        Task AddAsync(Publication publication);
        Task UpdateAsync(Publication publication);
        Task DeleteAsync(int id);
        Task<IEnumerable<Publication>> GetPublicationsByTeacherIdAsync(int teacherId);
    }
}
