
namespace BlogNews.Models.ViewModels
{
    public class NewsSearchViewModel
    {
        public int NewsId { get; set; }
        public int CategoryId { get; set; }
        public string Heading { get; set; }
        public string PhotoPath { get; set; }
        public string SourceName { get; set; }
        public string CreatedTimeAgo { get; set; }
        public bool NotRelatedToCategory { get; set; }


    }
    public class NewsSearchListViewModel
    {
        public string SearchedText { get; set; }
        public int SearchedCategoryId { get; set; }

        public List<NewsSearchViewModel> NewsSearchList { get; set;}
        public List<CategorySearchViewModel> Categories { get; set; }
        public class CategorySearchViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
