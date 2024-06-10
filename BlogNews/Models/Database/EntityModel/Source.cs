using System.ComponentModel.DataAnnotations;

namespace BlogNews.Models.Database.EntityModel
{
    public partial class Source
    {
        [Key]
        public int SourceId { get; set; }
        [Required, StringLength(255)]
        public string SourceName { get; set; }
        [StringLength(255)]
        public string SourceNameEN { get; set; }
        public bool Deleted { get; set; }
        public ICollection<News> NewsList { get; set; }
    }
}
