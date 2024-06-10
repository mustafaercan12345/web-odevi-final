using System.Globalization;

namespace BlogNews.Models.Database.EntityModel
{
    public partial class Source
    {
        public string SourceNameMulti
        {
            get
            {
                return CultureInfo.CurrentCulture.Name.StartsWith("tr") ? SourceName : SourceNameEN;
            }
        }
    }
}
