using Microsoft.AspNetCore.Mvc.Rendering;

namespace PortalNauchnyhPublikatsiy.Web.Models
{
    public class EditRolesViewModel
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public IList<string> UserRoles { get; set; } 
        public List<SelectListItem> AllRoles { get; set; } 
        public List<string> SelectedRoles { get; set; } 
    }
}
