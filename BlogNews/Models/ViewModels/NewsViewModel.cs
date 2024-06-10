namespace BlogNews.Models.ViewModels
{
    public class NewsViewModel
    {
        public int NewsId { get; set; }
        public string Heading { get; set; }
        public string PhotoPath { get; set; }
        public string Description { get; set; }
        public string SourceName { get; set; }
        public int SourceId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string CreatedTimeAgo { get; set; }
        public int LikeNumber { get; set; }
        public int DislikeNumber { get; set; }
        public List<NewsForYouViewModel> NewsForYouList { get; set; }
    }

    public class NewsForYouViewModel
    {
        public int NewsId { get; set; }
        public string Heading { get; set; }
        public string PhotoPath { get; set; }
        public string SourceName { get; set; }
        public int LikeNumber { get; set; }
        public int DislikeNumber { get; set; }
    }
}
