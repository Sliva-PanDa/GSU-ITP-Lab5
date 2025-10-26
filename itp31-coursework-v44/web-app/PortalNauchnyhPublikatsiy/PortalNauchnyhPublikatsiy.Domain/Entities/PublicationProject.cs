using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalNauchnyhPublikatsiy.Domain.Entities
{
    public class PublicationProject
    {
        public int PublicationId { get; set; }
        public Publication Publication { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
