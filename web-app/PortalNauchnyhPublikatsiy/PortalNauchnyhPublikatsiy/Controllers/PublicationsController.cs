using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PortalNauchnyhPublikatsiy.Application.DTO;
using PortalNauchnyhPublikatsiy.Application.Services;

namespace PortalNauchnyhPublikatsiy.Web.Controllers
{
    public class PublicationsController : Controller
    {
        private readonly IPublicationService _publicationService;
        private readonly IJournalConferenceService _journalService;

        public PublicationsController(IPublicationService publicationService, IJournalConferenceService journalService)
        {
            _publicationService = publicationService;
            _journalService = journalService;
        }

        public async Task<IActionResult> Index()
        {
            var publications = await _publicationService.GetAllPublicationsAsync();
            return View(publications);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var publicationDto = await _publicationService.GetPublicationByIdAsync(id.Value);
            if (publicationDto == null) return NotFound();
            return View(publicationDto);
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            await PopulateJournalsDropDownList();
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePublicationDto publicationDto)
        {
            if (ModelState.IsValid)
            {
                await _publicationService.CreatePublicationAsync(publicationDto);
                return RedirectToAction(nameof(Index));
            }
            await PopulateJournalsDropDownList(publicationDto.JournalConferenceId);
            return View(publicationDto);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var publication = await _publicationService.GetPublicationByIdAsync(id.Value);
            if (publication == null) return NotFound();

            var updateDto = new UpdatePublicationDto
            {
                Id = publication.Id,
                Title = publication.Title,
                Type = publication.Type,
                Year = publication.Year,
                JournalConferenceId = publication.JournalConferenceId,
                DOI = publication.DOI,
                FilePath = "" // FilePath мы пока не храним в DTO, можно оставить пустым
            };

            await PopulateJournalsDropDownList(publication.JournalConferenceId);
            return View(updateDto);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdatePublicationDto publicationDto)
        {
            if (id != publicationDto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _publicationService.UpdatePublicationAsync(publicationDto);
                return RedirectToAction(nameof(Index));
            }
            await PopulateJournalsDropDownList(publicationDto.JournalConferenceId);
            return View(publicationDto);
        }

        // GET: Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var publicationDto = await _publicationService.GetPublicationByIdAsync(id.Value);
            if (publicationDto == null) return NotFound();
            return View(publicationDto);
        }

        // POST: Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _publicationService.DeletePublicationAsync(id);
            return RedirectToAction(nameof(Index));
        }


        private async Task PopulateJournalsDropDownList(object? selectedJournal = null)
        {
            var journals = await _journalService.GetAllAsync();
            ViewBag.JournalConferenceId = new SelectList(journals, "Id", "Name", selectedJournal);
        }
    }
}