using System.ComponentModel.DataAnnotations;

namespace BlogNews.Models.Database.EntityModel
{
    public partial class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Required, StringLength(100)]
        public string CountryName { get; set; }

        [StringLength(100)]
        public string CountryNameEN { get; set; }
        public bool Deleted { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
