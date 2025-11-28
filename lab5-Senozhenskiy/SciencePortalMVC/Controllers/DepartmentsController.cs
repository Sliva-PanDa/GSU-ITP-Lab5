using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SciencePortalMVC.Interfaces;

namespace SciencePortalMVC.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        // Теперь конструктор принимает ТОЛЬКО репозиторий
        public DepartmentsController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return View(departments);
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _departmentRepository.GetByIdAsync(id.Value);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        /*
            Методы Create, Edit, Delete временно закомментированы,
            так как для их работы нужно расширять репозиторий,
            а для выполнения задания это не требуется.
        */
    }
}