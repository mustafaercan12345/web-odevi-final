using System.Globalization;

namespace BlogNews.Models.Database.EntityModel
{
    public partial class News
    {
        public string HeadingMulti
        {
            get
            {
                return CultureInfo.CurrentCulture.Name.StartsWith("tr") ? Heading : HeadingEN;
            }
        }
        public string DescriptionMulti
        {
            get
            {
                return CultureInfo.CurrentCulture.Name.StartsWith("tr") ? Description : DescriptionEN;
            }
        }


    }
}
