namespace BlogNews.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }
        public List<NewsSliderHomeViewModel> NewsSliderHomeList { get; set; }
        public  List<NewsForYouViewModel> NewsForYouHomeList { get; set; }
        public string CityName {  get; set; }


        public class CategoryViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        public class NewsSliderHomeViewModel
        {
            public int NewsId { get; set; }
            public string Heading { get; set; }
            public string PhotoPath { get; set; }
            public string SourceName { get; set; }
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
}
