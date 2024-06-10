using System.ComponentModel.DataAnnotations;

namespace BlogNews.Models.Database.EntityModel
{
    public partial class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, StringLength(100)]
        public string CategoryName { get; set; }

        [StringLength(100)]
        public string CategoryNameEN { get; set; }
        public bool Deleted { get; set; }   

        public ICollection<News> NewsList { get; set; }
        public ICollection<UserInterest> UserInterests { get; set; }
    }
}
