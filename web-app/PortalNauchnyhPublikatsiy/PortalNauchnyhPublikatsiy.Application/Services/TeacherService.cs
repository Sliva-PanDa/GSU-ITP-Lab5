using PortalNauchnyhPublikatsiy.Application.DTO;
using PortalNauchnyhPublikatsiy.Application.Interfaces;
using PortalNauchnyhPublikatsiy.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalNauchnyhPublikatsiy.Application.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IPublicationRepository _publicationRepository;
        private readonly IProjectRepository _projectRepository; 

        public TeacherService(
            ITeacherRepository teacherRepository,
            IPublicationRepository publicationRepository,
            IProjectRepository projectRepository)
        {
            _teacherRepository = teacherRepository;
            _publicationRepository = publicationRepository;
            _projectRepository = projectRepository; 
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
            var projects = await _projectRepository.GetProjectsByTeacherIdAsync(id);
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
                }),
                Projects = projects.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Number = p.Number,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Role = p.LeaderId == id ? "Руководитель" : "Участник"
                })
            };
        }

        public async Task AddTeacherAsync(CreateTeacherDto dto)
        {
            var teacher = new Teacher // Используем using ...Domain.Entities;
            {
                FullName = dto.FullName,
                Position = dto.Position,
                Degree = dto.Degree,
                DepartmentId = dto.DepartmentId
            };
            await _teacherRepository.AddAsync(teacher);
        }

        public async Task UpdateTeacherAsync(UpdateTeacherDto dto)
        {
            var teacher = await _teacherRepository.GetByIdAsync(dto.Id);
            if (teacher != null)
            {
                teacher.FullName = dto.FullName;
                teacher.Position = dto.Position;
                teacher.Degree = dto.Degree;
                teacher.DepartmentId = dto.DepartmentId;
                await _teacherRepository.UpdateAsync(teacher);
            }
        }

        public async Task DeleteTeacherAsync(int id)
        {
            await _teacherRepository.DeleteAsync(id);
        }
    }
}