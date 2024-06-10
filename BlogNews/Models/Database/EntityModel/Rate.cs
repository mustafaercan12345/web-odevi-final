using System.ComponentModel.DataAnnotations;

namespace BlogNews.Models.Database.EntityModel
{
    public class Rate
    {
        [Key]
        public int RateId { get; set; }

        [Required]
        public int NewsId { get; set; }

        [Required]
        public int RateDefId { get; set; }

        [Required]
        public int UserId { get; set; }
        public bool Deleted { get; set; }


        public News News { get; set; }
        public RateDef RateDef { get; set; }
        public User User { get; set; }
    }
}
