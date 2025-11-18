using Microsoft.AspNetCore.Mvc;
using PortalNauchnyhPublikatsiy.Application.Services;

namespace PortalNauchnyhPublikatsiy.Web.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        // Страница со списком всех преподавателей
        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return View(teachers);
        }

        // Страница-профиль преподавателя
        public async Task<IActionResult> Details(int id)
        {
            var profile = await _teacherService.GetTeacherProfileAsync(id);
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }
    }
}