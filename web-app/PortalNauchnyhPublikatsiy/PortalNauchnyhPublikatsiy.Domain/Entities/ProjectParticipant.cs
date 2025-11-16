using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalNauchnyhPublikatsiy.Domain.Entities
{
    public class ProjectParticipant
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
