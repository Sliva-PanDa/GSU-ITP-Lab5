using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalNauchnyhPublikatsiy.Application.DTO
{
    public class PublicationDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Year { get; set; }
        public string? JournalName { get; set; } // Название журнала, а не его Id
        public string? DOI { get; set; }
        public int JournalConferenceId { get; set; }
    }
}