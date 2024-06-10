using static BlogNews.Models.ViewModels.HomeViewModel;

namespace BlogNews.Models.ViewModels
{
    public class UserInterestSettingsViewModel
    {
        public List<CategoryUserInterestViewModel> Categories { get; set; }
        public List<int> SelectedCategoryIds { get; set; }

        public UserInterestSettingsViewModel()
        {
            Categories = new List<CategoryUserInterestViewModel>();
            SelectedCategoryIds = new List<int>();
        }
    }
}
