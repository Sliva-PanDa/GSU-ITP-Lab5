using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortalNauchnyhPublikatsiy.Application.Services;

namespace PortalNauchnyhPublikatsiy.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IPublicationService _publicationService;
        private readonly IDepartmentService _departmentService;

        public ReportsController(IPublicationService publicationService, IDepartmentService departmentService)
        {
            _publicationService = publicationService;
            _departmentService = departmentService;
        }

        // GET-метод для отображения формы и результатов
        public async Task<IActionResult> PublicationsByDepartment(int? departmentId, int? year)
        {
            // Загружаем список кафедр для выпадающего списка
            var departments = await _departmentService.GetAllAsync();
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", departmentId);

            // Если параметры фильтра переданы, выполняем поиск
            if (departmentId.HasValue && year.HasValue)
            {
                var publications = await _publicationService.GetPublicationsByDepartmentAndYearAsync(departmentId.Value, year.Value);
                return View(publications);
            }

            // Если параметры не переданы, возвращаем пустой список
            return View(new List<PortalNauchnyhPublikatsiy.Application.DTO.PublicationDto>());
        }
    }
}