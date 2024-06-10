using System.ComponentModel.DataAnnotations;

namespace BlogNews.Models.Database.EntityModel
{
    public class UserInterest
    {
        [Key]
        public int UserInterestId { get; set; }
        [Required]
        public int UserId { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public bool Deleted { get; set; }

        public User User { get; set; }
        public Category Category { get; set; }
    }
}
