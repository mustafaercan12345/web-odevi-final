using System.Globalization;

namespace BlogNews.Models.Database.EntityModel
{
    public partial class City
    {
        public string CityNameMulti
        {
            get
            {
                return CultureInfo.CurrentCulture.Name.StartsWith("tr") ? CityName : CityNameEN;
            }
        }
    }
}
