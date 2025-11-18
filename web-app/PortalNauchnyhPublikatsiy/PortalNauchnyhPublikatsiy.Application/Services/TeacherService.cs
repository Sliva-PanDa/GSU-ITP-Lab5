using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalNauchnyhPublikatsiy.Application.DTO;
using PortalNauchnyhPublikatsiy.Application.Interfaces;

namespace PortalNauchnyhPublikatsiy.Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IPublicationRepository _publicationRepository;

        public TeacherService(ITeacherRepository teacherRepository, IPublicationRepository publicationRepository)
        {
            _teacherRepository = teacherRepository;
            _publicationRepository = publicationRepository;
        }

        public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync()
        {
            var teachers = await _teacherRepository.GetAllAsync();
            return teachers.Select(t => new TeacherDto
            {
                Id = t.Id,
                FullName = t.FullName,
                DepartmentName = t.Department?.Name ?? "N/A"
            });
        }

        public async Task<TeacherProfileDto?> GetTeacherProfileAsync(int id)
        {
            var teacher = await _teacherRepository.GetByIdAsync(id);
            if (teacher == null) return null;

            var publications = await _publicationRepository.GetPublicationsByTeacherIdAsync(id);
            var hIndex = await _teacherRepository.GetHirschIndexAsync(id);
            var q1q2Count = await _teacherRepository.GetQ1Q2CountAsync(id);

            return new TeacherProfileDto
            {
                Id = teacher.Id,
                FullName = teacher.FullName,
                Position = teacher.Position,
                Degree = teacher.Degree,
                DepartmentName = teacher.Department?.Name ?? "N/A",
                HirschIndex = hIndex,
                Q1Q2Count = q1q2Count,
                Publications = publications.Select(p => new PublicationDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Type = p.Type,
                    Year = p.Year,
                    JournalName = p.JournalConference?.Name,
                    DOI = p.DOI,
                    JournalConferenceId = p.JournalConferenceId
                })
            };
        }
    }
}