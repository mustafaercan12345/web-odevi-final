using System.ComponentModel.DataAnnotations;

namespace BlogNews.Models.Database.EntityModel
{
    public partial class News
    {
        [Key]
        public int NewsId { get; set; }

        [Required, StringLength(255)]
        public string Heading { get; set; }
        [Required, StringLength(255)]
        public string HeadingEN { get; set; }
        [StringLength(255)]
        public string PhotoPath { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public string DescriptionEN { get; set; }

        [Required]
        public int SourceId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }
        public bool Deleted { get; set; }

        public Source Source { get; set; }
        public Category Category { get; set; }
        public ICollection<Rate> Rates { get; set; }
    }
}
