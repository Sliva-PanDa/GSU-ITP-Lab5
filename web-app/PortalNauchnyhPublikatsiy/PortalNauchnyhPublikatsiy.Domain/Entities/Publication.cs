using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalNauchnyhPublikatsiy.Domain.Entities
{
    public class Publication
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string? DOI { get; set; }
        public string? FilePath { get; set; }
        public int JournalConferenceId { get; set; }
        public JournalsConferences JournalConference { get; set; }
    }
}
