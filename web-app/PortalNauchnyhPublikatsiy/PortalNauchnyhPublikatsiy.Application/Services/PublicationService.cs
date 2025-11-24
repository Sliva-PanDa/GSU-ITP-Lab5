using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalNauchnyhPublikatsiy.Application.Common;
using PortalNauchnyhPublikatsiy.Application.DTO;
using PortalNauchnyhPublikatsiy.Application.Interfaces;

namespace PortalNauchnyhPublikatsiy.Application.Services
{
    public class PublicationService : IPublicationService
    {
        private readonly IPublicationRepository _publicationRepository;

        public PublicationService(IPublicationRepository publicationRepository)
        {
            _publicationRepository = publicationRepository;
        }

        public async Task<PublicationDto?> GetPublicationByIdAsync(int id)
        {
            var publication = await _publicationRepository.GetByIdAsync(id);

            if (publication == null)
            {
                return null;
            }

            // Преобразуем (маппим) доменную модель в DTO
            return new PublicationDto
            {
                Id = publication.Id,
                Title = publication.Title,
                Type = publication.Type,
                Year = publication.Year,
                JournalName = publication.JournalConference?.Name, // Безопасно получаем имя
                JournalConferenceId = publication.JournalConferenceId,
                DOI = publication.DOI
            };
        }

        public async Task<PaginatedList<PublicationDto>> GetAllPublicationsAsync(string? searchString, int? year, int pageIndex, int pageSize)
        {
            var publicationsQuery = _publicationRepository.GetAllAsQueryable(searchString, year);

            var publicationsDtoQuery = publicationsQuery.Select(p => new PublicationDto
            {
                Id = p.Id,
                Title = p.Title,
                Type = p.Type,
                Year = p.Year,
                JournalName = p.JournalConference.Name,
                DOI = p.DOI,
                JournalConferenceId = p.JournalConferenceId
            });

            return await PaginatedList<PublicationDto>.CreateAsync(publicationsDtoQuery, pageIndex, pageSize);
        }

        public async Task CreatePublicationAsync(CreatePublicationDto publicationDto)
        {
            var publication = new Domain.Entities.Publication
            {
                Title = publicationDto.Title,
                Type = publicationDto.Type,
                Year = publicationDto.Year,
                JournalConferenceId = publicationDto.JournalConferenceId,
                DOI = publicationDto.DOI,
                FilePath = publicationDto.FilePath
            };

            await _publicationRepository.AddAsync(publication);
        }
        public async Task UpdatePublicationAsync(UpdatePublicationDto publicationDto)
        {
            // Сначала нужно найти существующую сущность в базе данных
            var publication = await _publicationRepository.GetByIdAsync(publicationDto.Id);

            if (publication != null)
            {
                publication.Title = publicationDto.Title;
                publication.Type = publicationDto.Type;
                publication.Year = publicationDto.Year;
                publication.JournalConferenceId = publicationDto.JournalConferenceId;
                publication.DOI = publicationDto.DOI;
                publication.FilePath = publicationDto.FilePath;

                await _publicationRepository.UpdateAsync(publication);
            }
        }
        public async Task DeletePublicationAsync(int id)
        {
            await _publicationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<PublicationDto>> GetPublicationsByDepartmentAndYearAsync(int departmentId, int year)
        {
            var publications = await _publicationRepository.GetPublicationsByDepartmentAndYearAsync(departmentId, year);

            return publications.Select(publication => new PublicationDto
            {
                Id = publication.Id,
                Title = publication.Title,
                Type = publication.Type,
                Year = publication.Year,
                JournalName = publication.JournalConference?.Name,
                JournalConferenceId = publication.JournalConferenceId,
                DOI = publication.DOI
            });
        }

        public async Task<IEnumerable<PublicationDto>> GetPublicationsByTeacherAndYearAsync(int teacherId, int year)
        {
            var publications = await _publicationRepository.GetPublicationsByTeacherAndYearAsync(teacherId, year);

            // Маппинг в DTO
            return publications.Select(publication => new PublicationDto
            {
                Id = publication.Id,
                Title = publication.Title,
                Type = publication.Type,
                Year = publication.Year,
                JournalName = publication.JournalConference?.Name,
                JournalConferenceId = publication.JournalConferenceId,
                DOI = publication.DOI
            });
        }
    }
}