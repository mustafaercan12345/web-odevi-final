using System.ComponentModel.DataAnnotations;

namespace BlogNews.Models.Database.EntityModel
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(50)]
        public string Username { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        [Required, StringLength(100)]
        public string Email { get; set; }

        [Required, StringLength(2)]
        public string Language { get; set; }

        public bool NotificationEnabled { get; set; }
        public string ProfilePicturePath { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [Required]
        public int CountryId { get; set; }

        [Required]
        public int CityId { get; set; }
        public bool Deleted { get; set; }


        public Country Country { get; set; }
        public City City { get; set; }
        public ICollection<UserInterest> UserInterests { get; set; }
        public ICollection<Rate> Rates { get; set; }
    }
}
