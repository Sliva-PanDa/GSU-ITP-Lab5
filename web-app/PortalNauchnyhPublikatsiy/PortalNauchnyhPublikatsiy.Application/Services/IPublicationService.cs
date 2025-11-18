using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalNauchnyhPublikatsiy.Application.DTO;

namespace PortalNauchnyhPublikatsiy.Application.Services
{
    public interface IPublicationService
    {
        Task<IEnumerable<PublicationDto>> GetAllPublicationsAsync(string? searchString, int? year);
        Task<PublicationDto?> GetPublicationByIdAsync(int id);
        Task CreatePublicationAsync(CreatePublicationDto publicationDto);
        Task UpdatePublicationAsync(UpdatePublicationDto publicationDto);
        Task DeletePublicationAsync(int id);
    }
}
