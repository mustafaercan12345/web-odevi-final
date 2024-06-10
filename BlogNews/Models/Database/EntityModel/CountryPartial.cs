using System.Globalization;

namespace BlogNews.Models.Database.EntityModel
{
    public partial class Country
    {
        public string CountryNameMulti
        {
            get
            {
                return CultureInfo.CurrentCulture.Name.StartsWith("tr") ? CountryName : CountryNameEN;
            }
        }
    }
}
