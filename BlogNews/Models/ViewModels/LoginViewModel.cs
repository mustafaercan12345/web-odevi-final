using System.ComponentModel.DataAnnotations;

namespace BlogNews.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string LoginEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string LoginPassword { get; set; }
    }
}
