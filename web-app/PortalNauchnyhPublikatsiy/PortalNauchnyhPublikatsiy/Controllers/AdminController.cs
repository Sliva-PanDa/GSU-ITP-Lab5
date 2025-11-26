using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PortalNauchnyhPublikatsiy.Web.Models;
using PortalNauchnyhPublikatsiy.Domain.Entities.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortalNauchnyhPublikatsiy.Infrastructure.Data;

namespace PortalNauchnyhPublikatsiy.Web.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Include(u => u.Teacher).ToListAsync();

            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user), 
                    LinkedTeacherName = user.Teacher?.FullName ?? "Не привязан" 
                });
            }

            return View(userViewModels);
        }
        // GET: Admin/EditRoles/userId
        public async Task<IActionResult> EditRoles(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Не удалось найти пользователя с ID '{userId}'.");
            }

            var allRoles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new EditRolesViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                UserRoles = userRoles,
                AllRoles = allRoles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRoles(EditRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound($"Не удалось найти пользователя с ID '{model.UserId}'.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var resultRemove = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!resultRemove.Succeeded)
            {
                ModelState.AddModelError("", "Не удалось удалить существующие роли пользователя.");
                return View(model);
            }

            model.SelectedRoles ??= new List<string>();
            var resultAdd = await _userManager.AddToRolesAsync(user, model.SelectedRoles);
            if (!resultAdd.Succeeded)
            {
                ModelState.AddModelError("", "Не удалось добавить новые роли пользователю.");
                return View(model);
            }

            return RedirectToAction("Index");
        }
        // GET: Admin/LinkTeacher/userId
        public async Task<IActionResult> LinkTeacher(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Не удалось найти пользователя с ID '{userId}'.");
            }

            // Находим всех преподавателей, у которых ЕЩЕ НЕТ привязки к аккаунту
            // (т.е. ApplicationUserId - это либо null, либо пустая строка)
            var availableTeachers = await _context.Teachers
                .Where(t => string.IsNullOrEmpty(t.ApplicationUserId))
                .ToListAsync();

            var model = new LinkTeacherViewModel
            {
                UserId = user.Id,
                UserEmail = user.Email,
                AvailableTeachers = availableTeachers.Select(t => new SelectListItem
                {
                    Text = t.FullName,
                    Value = t.Id.ToString()
                }).ToList()
            };

            return View(model);
        }
        // POST: Admin/LinkTeacher
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkTeacher(LinkTeacherViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var teacher = await _context.Teachers.FindAsync(model.SelectedTeacherId);

            if (user == null || teacher == null)
            {
                return NotFound();
            }

            user.TeacherId = teacher.Id;
            teacher.ApplicationUserId = user.Id;

            await _userManager.UpdateAsync(user); 

            return RedirectToAction("Index");
        }
    }
}
