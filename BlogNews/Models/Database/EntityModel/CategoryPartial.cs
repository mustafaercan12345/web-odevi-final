using System.Globalization;

namespace BlogNews.Models.Database.EntityModel
{
    public partial class Category
    {
        public string CategoryNameMulti
        {
            get
            {
                return CultureInfo.CurrentCulture.Name.StartsWith("tr") ? CategoryName : CategoryNameEN;
            }
        }
    }
}
