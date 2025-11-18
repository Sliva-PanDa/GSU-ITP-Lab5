using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalNauchnyhPublikatsiy.Application.DTO
{
    public class TeacherProfileDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;

        public int HirschIndex { get; set; }
        public int Q1Q2Count { get; set; }

        public IEnumerable<PublicationDto> Publications { get; set; } = new List<PublicationDto>();
    }
}
