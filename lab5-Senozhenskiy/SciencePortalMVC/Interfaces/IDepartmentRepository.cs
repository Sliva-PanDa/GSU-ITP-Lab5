using SciencePortalMVC.Models;

namespace SciencePortalMVC.Interfaces
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
    }
}