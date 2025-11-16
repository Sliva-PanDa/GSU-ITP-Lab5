using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortalNauchnyhPublikatsiy.Application.DTO;
using PortalNauchnyhPublikatsiy.Application.Interfaces;

namespace PortalNauchnyhPublikatsiy.Application.Services
{
    public class JournalConferenceService : IJournalConferenceService
    {
        private readonly IJournalConferenceRepository _repository;

        public JournalConferenceService(IJournalConferenceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<JournalConferenceDto>> GetAllAsync()
        {
            var journals = await _repository.GetAllAsync();
            return journals.Select(j => new JournalConferenceDto
            {
                Id = j.Id,
                Name = j.Name
            });
        }
    }
}
