using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortalNauchnyhPublikatsiy.Application.DTO;
using PortalNauchnyhPublikatsiy.Application.Services;

namespace PortalNauchnyhPublikatsiy.Web.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly IDepartmentService _departmentService;

        public TeachersController(ITeacherService teacherService, IDepartmentService departmentService)
        {
            _teacherService = teacherService;
            _departmentService = departmentService;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            var teachers = await _teacherService.GetAllTeachersAsync();
            return View(teachers);
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var profile = await _teacherService.GetTeacherProfileAsync(id);
            if (profile == null) return NotFound();
            return View(profile);
        }

        // GET: Teachers/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDepartmentsDropDownList();
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeacherDto teacherDto)
        {
            if (ModelState.IsValid)
            {
                await _teacherService.AddTeacherAsync(teacherDto);
                return RedirectToAction(nameof(Index));
            }
            await PopulateDepartmentsDropDownList(teacherDto.DepartmentId);
            return View(teacherDto);
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _teacherService.GetTeacherProfileAsync(id); // Используем GetProfile, чтобы получить все данные
            if (teacher == null) return NotFound();

            var departments = await _departmentService.GetAllAsync();
            var departmentId = departments.FirstOrDefault(d => d.Name == teacher.DepartmentName)?.Id;

            var updateDto = new UpdateTeacherDto
            {
                Id = teacher.Id,
                FullName = teacher.FullName,
                Position = teacher.Position,
                Degree = teacher.Degree,
                DepartmentId = departmentId ?? 0
            };
            await PopulateDepartmentsDropDownList(updateDto.DepartmentId);
            return View(updateDto);
        }

        // POST: Teachers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTeacherDto teacherDto)
        {
            if (id != teacherDto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _teacherService.UpdateTeacherAsync(teacherDto);
                return RedirectToAction(nameof(Index));
            }
            await PopulateDepartmentsDropDownList(teacherDto.DepartmentId);
            return View(teacherDto);
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _teacherService.GetTeacherProfileAsync(id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _teacherService.DeleteTeacherAsync(id);
            return RedirectToAction(nameof(Index));
        }


        private async Task PopulateDepartmentsDropDownList(object? selectedDepartment = null)
        {
            var departments = await _departmentService.GetAllAsync();
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", selectedDepartment);
        }
    }
}