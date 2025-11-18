using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalNauchnyhPublikatsiy.Application.DTO;

namespace PortalNauchnyhPublikatsiy.Application.Services
{
    public interface ITeacherService
    {
        Task<IEnumerable<TeacherDto>> GetAllTeachersAsync(); // DTO для простого списка
        Task<TeacherProfileDto?> GetTeacherProfileAsync(int id);
    }
}
