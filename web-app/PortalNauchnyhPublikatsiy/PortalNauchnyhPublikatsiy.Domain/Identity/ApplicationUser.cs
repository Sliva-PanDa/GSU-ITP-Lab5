using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalNauchnyhPublikatsiy.Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace PortalNauchnyhPublikatsiy.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public int? TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}
