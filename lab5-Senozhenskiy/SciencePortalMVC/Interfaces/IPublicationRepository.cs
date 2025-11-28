using SciencePortalMVC.Models;

namespace SciencePortalMVC.Interfaces
{
    public interface IPublicationRepository
    {
        Task<IEnumerable<Publication>> GetAllAsync();
        Task<Publication?> GetByIdAsync(int id); 
        Task AddAsync(Publication publication); 
        Task UpdateAsync(Publication publication); 
        Task DeleteAsync(int id); 
        Task<bool> ExistsAsync(int id); 
    }
}