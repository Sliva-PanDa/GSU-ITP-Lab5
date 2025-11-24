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
        IQueryable<Publication> GetAllAsQueryable(string? searchString, int? year);
        Task AddAsync(Publication publication);
        Task UpdateAsync(Publication publication);
        Task DeleteAsync(int id);
        Task<IEnumerable<Publication>> GetPublicationsByTeacherIdAsync(int teacherId);
        Task<IEnumerable<Publication>> GetPublicationsByDepartmentAndYearAsync(int departmentId, int year);
        Task<IEnumerable<Publication>> GetPublicationsByTeacherAndYearAsync(int teacherId, int year);
    }
}
