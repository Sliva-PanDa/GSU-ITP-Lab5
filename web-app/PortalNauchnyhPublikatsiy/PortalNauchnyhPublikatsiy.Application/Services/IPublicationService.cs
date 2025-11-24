using PortalNauchnyhPublikatsiy.Application.DTO;
using PortalNauchnyhPublikatsiy.Application.Common;
using PortalNauchnyhPublikatsiy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalNauchnyhPublikatsiy.Application.Services
{
    public interface IPublicationService
    {
        Task<PaginatedList<PublicationDto>> GetAllPublicationsAsync(string? searchString, int? year, int pageIndex, int pageSize);
        Task<PublicationDto?> GetPublicationByIdAsync(int id);
        Task CreatePublicationAsync(CreatePublicationDto publicationDto);
        Task UpdatePublicationAsync(UpdatePublicationDto publicationDto);
        Task DeletePublicationAsync(int id);
        Task<IEnumerable<PublicationDto>> GetPublicationsByDepartmentAndYearAsync(int departmentId, int year);
        Task<IEnumerable<PublicationDto>> GetPublicationsByTeacherAndYearAsync(int teacherId, int year);
    }
}
