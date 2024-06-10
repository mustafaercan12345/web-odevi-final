using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BlogNews.Models.ViewModels
{
    public class ProfileSettingsViewModel
    {
        public int UserId { get; set; }
        [Required, StringLength(2)]
        public string Language { get; set; }
        public bool NotificationEnabled { get; set; }
        [Required]
        public int CountryId { get; set; }

        [Required]
        public int CityId { get; set; }

        public List<SelectListItem> Countries { get; set; }
        public List<SelectListItem> Cities { get; set; }
        public string ProfilePicturePath { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public List<SelectListItem> Languages { get; set; }
        public List<SelectListItem> NotificationOptions { get; set; }
    }
}
