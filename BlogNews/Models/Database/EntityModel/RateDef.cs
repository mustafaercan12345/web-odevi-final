using System.ComponentModel.DataAnnotations;

namespace BlogNews.Models.Database.EntityModel
{
    public class RateDef
    {
        [Key]
        public int RateDefId { get; set; }

        [Required, StringLength(255)]
        public string Value { get; set; }
        public bool Deleted { get; set; }


        public ICollection<Rate> Rates { get; set; }
    }
}
