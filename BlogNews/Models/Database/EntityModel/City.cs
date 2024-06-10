using System.ComponentModel.DataAnnotations;

namespace BlogNews.Models.Database.EntityModel
{
    public partial class City
    {
        [Key]
        public int CityId { get; set; }

        [Required, StringLength(100)]
        public string CityName { get; set; }

        [StringLength(100)]
        public string CityNameEN { get; set; }

        [Required]
        public int CountryId { get; set; }
        public bool Deleted { get; set; }

        public Country Country { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
