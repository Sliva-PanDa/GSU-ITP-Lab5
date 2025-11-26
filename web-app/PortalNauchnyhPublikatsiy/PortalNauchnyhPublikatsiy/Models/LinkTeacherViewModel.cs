using Microsoft.AspNetCore.Mvc.Rendering;

namespace PortalNauchnyhPublikatsiy.Web.Models
{
    public class LinkTeacherViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }

        public int SelectedTeacherId { get; set; }

        public List<SelectListItem> AvailableTeachers { get; set; }
    }
}