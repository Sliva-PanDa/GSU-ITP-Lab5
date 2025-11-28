using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SciencePortalMVC.Interfaces;
using SciencePortalMVC.Models;

namespace SciencePortalMVC.Controllers
{
    [Authorize]
    public class PublicationsController : Controller
    {
        private readonly IPublicationRepository _publicationRepository;

        public PublicationsController(IPublicationRepository publicationRepository)
        {
            _publicationRepository = publicationRepository;
        }

        // GET: Publications
        [ResponseCache(Duration = 272, Location = ResponseCacheLocation.Any)]
        public async Task<IActionResult> Index()
        {
            return View(await _publicationRepository.GetAllAsync());
        }

        // GET: Publications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var publication = await _publicationRepository.GetByIdAsync(id.Value);
            return publication == null ? NotFound() : View(publication);
        }

        // GET: Publications/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publications/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("PublicationId,Title,Type,Year")] Publication publication)
        {
            if (ModelState.IsValid)
            {
                await _publicationRepository.AddAsync(publication);
                return RedirectToAction(nameof(Index));
            }
            return View(publication);
        }

        // GET: Publications/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var publication = await _publicationRepository.GetByIdAsync(id.Value);
            return publication == null ? NotFound() : View(publication);
        }

        // POST: Publications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("PublicationId,Title,Type,Year")] Publication publication)
        {
            if (id != publication.PublicationId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _publicationRepository.UpdateAsync(publication);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _publicationRepository.ExistsAsync(publication.PublicationId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(publication);
        }

        // GET: Publications/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var publication = await _publicationRepository.GetByIdAsync(id.Value);
            return publication == null ? NotFound() : View(publication);
        }

        // POST: Publications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _publicationRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}