using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalNauchnyhPublikatsiy.Application.DTO;

namespace PortalNauchnyhPublikatsiy.Application.Services
{
    public interface IJournalConferenceService
    {
        Task<IEnumerable<JournalConferenceDto>> GetAllAsync();
    }
}
