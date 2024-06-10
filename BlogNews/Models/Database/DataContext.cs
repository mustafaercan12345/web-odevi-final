using BlogNews.Models.Database.EntityModel;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BlogNews.Models.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<UserInterest> UserInterests { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<RateDef> RateDefs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Country)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CountryId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.City)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CityId);

            modelBuilder.Entity<News>()
                .HasOne(n => n.Source)
                .WithMany(s => s.NewsList)
                .HasForeignKey(n => n.SourceId);

            modelBuilder.Entity<News>()
                .HasOne(n => n.Category)
                .WithMany(c => c.NewsList)
                .HasForeignKey(n => n.CategoryId);

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.News)
                .WithMany(n => n.Rates)
                .HasForeignKey(r => r.NewsId);

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.RateDef)
                .WithMany(rd => rd.Rates)
                .HasForeignKey(r => r.RateDefId);

            modelBuilder.Entity<Rate>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rates)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<UserInterest>()
                 .HasKey(ui => ui.UserInterestId);

            modelBuilder.Entity<UserInterest>()
                .HasOne(ui => ui.User)
                .WithMany(u => u.UserInterests)
                .HasForeignKey(ui => ui.UserId);

            modelBuilder.Entity<UserInterest>()
                .HasOne(ui => ui.Category)
                .WithMany(c => c.UserInterests)
                .HasForeignKey(ui => ui.CategoryId);


            //// SEED DATA
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Spor", CategoryNameEN = "Sports" },
                new Category { CategoryId = 2, CategoryName = "Teknoloji", CategoryNameEN = "Technology" },
                new Category { CategoryId = 3, CategoryName = "Sanat", CategoryNameEN = "Art" },
                new Category { CategoryId = 4, CategoryName = "Eğitim", CategoryNameEN = "Education" },
                new Category { CategoryId = 5, CategoryName = "Sağlık", CategoryNameEN = "Health" }
            );


            modelBuilder.Entity<Country>().HasData(
                new Country { CountryId = 1, CountryName = "Türkiye", CountryNameEN = "Turkiye" },
                new Country { CountryId = 2, CountryName = "Almanya", CountryNameEN = "Germany" },
                new Country { CountryId = 3, CountryName = "Fransa", CountryNameEN = "France" },
                new Country { CountryId = 4, CountryName = "Japonya", CountryNameEN = "Japan" },
                new Country { CountryId = 5, CountryName = "Brezilya", CountryNameEN = "Brazil" }
            );

            modelBuilder.Entity<City>().HasData(
                new City { CityId = 1, CityName = "Adana", CityNameEN = "Adana", CountryId = 1 },
                new City { CityId = 2, CityName = "Adıyaman", CityNameEN = "Adiyaman", CountryId = 1 },
                new City { CityId = 3, CityName = "Afyonkarahisar", CityNameEN = "Afyonkarahisar", CountryId = 1 },
                new City { CityId = 4, CityName = "Ağrı", CityNameEN = "Agri", CountryId = 1 },
                new City { CityId = 5, CityName = "Amasya", CityNameEN = "Amasya", CountryId = 1 },
                new City { CityId = 6, CityName = "Ankara", CityNameEN = "Ankara", CountryId = 1 },
                new City { CityId = 7, CityName = "Antalya", CityNameEN = "Antalya", CountryId = 1 },
                new City { CityId = 8, CityName = "Artvin", CityNameEN = "Artvin", CountryId = 1 },
                new City { CityId = 9, CityName = "Aydın", CityNameEN = "Aydin", CountryId = 1 },
                new City { CityId = 10, CityName = "Balıkesir", CityNameEN = "Balikesir", CountryId = 1 },
                new City { CityId = 11, CityName = "Bilecik", CityNameEN = "Bilecik", CountryId = 1 },
                new City { CityId = 12, CityName = "Bingöl", CityNameEN = "Bingol", CountryId = 1 },
                new City { CityId = 13, CityName = "Bitlis", CityNameEN = "Bitlis", CountryId = 1 },
                new City { CityId = 14, CityName = "Bolu", CityNameEN = "Bolu", CountryId = 1 },
                new City { CityId = 15, CityName = "Burdur", CityNameEN = "Burdur", CountryId = 1 },
                new City { CityId = 16, CityName = "Bursa", CityNameEN = "Bursa", CountryId = 1 },
                new City { CityId = 17, CityName = "Çanakkale", CityNameEN = "Canakkale", CountryId = 1 },
                new City { CityId = 18, CityName = "Çankırı", CityNameEN = "Cankiri", CountryId = 1 },
                new City { CityId = 19, CityName = "Çorum", CityNameEN = "Corum", CountryId = 1 },
                new City { CityId = 20, CityName = "Denizli", CityNameEN = "Denizli", CountryId = 1 },
                new City { CityId = 21, CityName = "Diyarbakır", CityNameEN = "Diyarbakir", CountryId = 1 },
                new City { CityId = 22, CityName = "Edirne", CityNameEN = "Edirne", CountryId = 1 },
                new City { CityId = 23, CityName = "Elazığ", CityNameEN = "Elazig", CountryId = 1 },
                new City { CityId = 24, CityName = "Erzincan", CityNameEN = "Erzincan", CountryId = 1 },
                new City { CityId = 25, CityName = "Erzurum", CityNameEN = "Erzurum", CountryId = 1 },
                new City { CityId = 26, CityName = "Eskişehir", CityNameEN = "Eskisehir", CountryId = 1 },
                new City { CityId = 27, CityName = "Gaziantep", CityNameEN = "Gaziantep", CountryId = 1 },
                new City { CityId = 28, CityName = "Giresun", CityNameEN = "Giresun", CountryId = 1 },
                new City { CityId = 29, CityName = "Gümüşhane", CityNameEN = "Gumushane", CountryId = 1 },
                new City { CityId = 30, CityName = "Hakkari", CityNameEN = "Hakkari", CountryId = 1 },
                new City { CityId = 31, CityName = "Hatay", CityNameEN = "Hatay", CountryId = 1 },
                new City { CityId = 32, CityName = "Isparta", CityNameEN = "Isparta", CountryId = 1 },
                new City { CityId = 33, CityName = "Mersin", CityNameEN = "Mersin", CountryId = 1 },
                new City { CityId = 34, CityName = "İstanbul", CityNameEN = "Istanbul", CountryId = 1 },
                new City { CityId = 35, CityName = "İzmir", CityNameEN = "Izmir", CountryId = 1 },
                new City { CityId = 36, CityName = "Kars", CityNameEN = "Kars", CountryId = 1 },
                new City { CityId = 37, CityName = "Kastamonu", CityNameEN = "Kastamonu", CountryId = 1 },
                new City { CityId = 38, CityName = "Kayseri", CityNameEN = "Kayseri", CountryId = 1 },
                new City { CityId = 39, CityName = "Kırklareli", CityNameEN = "Kirklareli", CountryId = 1 },
                new City { CityId = 40, CityName = "Kırşehir", CityNameEN = "Kirsehir", CountryId = 1 },
                new City { CityId = 41, CityName = "Kocaeli", CityNameEN = "Kocaeli", CountryId = 1 },
                new City { CityId = 42, CityName = "Konya", CityNameEN = "Konya", CountryId = 1 },
                new City { CityId = 43, CityName = "Kütahya", CityNameEN = "Kutahya", CountryId = 1 },
                new City { CityId = 44, CityName = "Malatya", CityNameEN = "Malatya", CountryId = 1 },
                new City { CityId = 45, CityName = "Manisa", CityNameEN = "Manisa", CountryId = 1 },
                new City { CityId = 46, CityName = "Kahramanmaraş", CityNameEN = "Kahramanmaras", CountryId = 1 },
                new City { CityId = 47, CityName = "Mardin", CityNameEN = "Mardin", CountryId = 1 },
                new City { CityId = 48, CityName = "Muğla", CityNameEN = "Mugla", CountryId = 1 },
                new City { CityId = 49, CityName = "Muş", CityNameEN = "Mus", CountryId = 1 },
                new City { CityId = 50, CityName = "Nevşehir", CityNameEN = "Nevsehir", CountryId = 1 },
                new City { CityId = 51, CityName = "Niğde", CityNameEN = "Nigde", CountryId = 1 },
                new City { CityId = 52, CityName = "Ordu", CityNameEN = "Ordu", CountryId = 1 },
                new City { CityId = 53, CityName = "Rize", CityNameEN = "Rize", CountryId = 1 },
                new City { CityId = 54, CityName = "Sakarya", CityNameEN = "Sakarya", CountryId = 1 },
                new City { CityId = 55, CityName = "Samsun", CityNameEN = "Samsun", CountryId = 1 },
                new City { CityId = 56, CityName = "Siirt", CityNameEN = "Siirt", CountryId = 1 },
                new City { CityId = 57, CityName = "Sinop", CityNameEN = "Sinop", CountryId = 1 },
                new City { CityId = 58, CityName = "Sivas", CityNameEN = "Sivas", CountryId = 1 },
                new City { CityId = 59, CityName = "Tekirdağ", CityNameEN = "Tekirdag", CountryId = 1 },
                new City { CityId = 60, CityName = "Tokat", CityNameEN = "Tokat", CountryId = 1 },
                new City { CityId = 61, CityName = "Trabzon", CityNameEN = "Trabzon", CountryId = 1 },
                new City { CityId = 62, CityName = "Tunceli", CityNameEN = "Tunceli", CountryId = 1 },
                new City { CityId = 63, CityName = "Şanlıurfa", CityNameEN = "Sanliurfa", CountryId = 1 },
                new City { CityId = 64, CityName = "Uşak", CityNameEN = "Usak", CountryId = 1 },
                new City { CityId = 65, CityName = "Van", CityNameEN = "Van", CountryId = 1 },
                new City { CityId = 66, CityName = "Yozgat", CityNameEN = "Yozgat", CountryId = 1 },
                new City { CityId = 67, CityName = "Zonguldak", CityNameEN = "Zonguldak", CountryId = 1 },
                new City { CityId = 68, CityName = "Aksaray", CityNameEN = "Aksaray", CountryId = 1 },
                new City { CityId = 69, CityName = "Bayburt", CityNameEN = "Bayburt", CountryId = 1 },
                new City { CityId = 70, CityName = "Karaman", CityNameEN = "Karaman", CountryId = 1 },
                new City { CityId = 71, CityName = "Kırıkkale", CityNameEN = "Kirikkale", CountryId = 1 },
                new City { CityId = 72, CityName = "Batman", CityNameEN = "Batman", CountryId = 1 },
                new City { CityId = 73, CityName = "Şırnak", CityNameEN = "Sirnak", CountryId = 1 },
                new City { CityId = 74, CityName = "Bartın", CityNameEN = "Bartin", CountryId = 1 },
                new City { CityId = 75, CityName = "Ardahan", CityNameEN = "Ardahan", CountryId = 1 },
                new City { CityId = 76, CityName = "Iğdır", CityNameEN = "Igdir", CountryId = 1 },
                new City { CityId = 77, CityName = "Yalova", CityNameEN = "Yalova", CountryId = 1 },
                new City { CityId = 78, CityName = "Karabük", CityNameEN = "Karabuk", CountryId = 1 },
                new City { CityId = 79, CityName = "Kilis", CityNameEN = "Kilis", CountryId = 1 },
                new City { CityId = 80, CityName = "Osmaniye", CityNameEN = "Osmaniye", CountryId = 1 },
                new City { CityId = 81, CityName = "Düzce", CityNameEN = "Duzce", CountryId = 1 },
                new City { CityId = 82, CityName = "Berlin", CityNameEN = "Berlin", CountryId = 2 },
                new City { CityId = 83, CityName = "Münih", CityNameEN = "Munich", CountryId = 2 },
                new City { CityId = 84, CityName = "Hamburg", CityNameEN = "Hamburg", CountryId = 2 },
                new City { CityId = 85, CityName = "Köln", CityNameEN = "Cologne", CountryId = 2 },
                new City { CityId = 86, CityName = "Frankfurt", CityNameEN = "Frankfurt", CountryId = 2 },
                new City { CityId = 87, CityName = "Paris", CityNameEN = "Paris", CountryId = 3 },
                new City { CityId = 88, CityName = "Marsilya", CityNameEN = "Marseille", CountryId = 3 },
                new City { CityId = 89, CityName = "Lyon", CityNameEN = "Lyon", CountryId = 3 },
                new City { CityId = 90, CityName = "Toulouse", CityNameEN = "Toulouse", CountryId = 3 },
                new City { CityId = 91, CityName = "Nice", CityNameEN = "Nice", CountryId = 3 },
                new City { CityId = 92, CityName = "Tokyo", CityNameEN = "Tokyo", CountryId = 4 },
                new City { CityId = 93, CityName = "Osaka", CityNameEN = "Osaka", CountryId = 4 },
                new City { CityId = 94, CityName = "Kyoto", CityNameEN = "Kyoto", CountryId = 4 },
                new City { CityId = 95, CityName = "Yokohama", CityNameEN = "Yokohama", CountryId = 4 },
                new City { CityId = 96, CityName = "Nagoya", CityNameEN = "Nagoya", CountryId = 4 },
                new City { CityId = 97, CityName = "Rio de Janeiro", CityNameEN = "Rio de Janeiro", CountryId = 5 },
                new City { CityId = 98, CityName = "São Paulo", CityNameEN = "Sao Paulo", CountryId = 5 },
                new City { CityId = 99, CityName = "Brasília", CityNameEN = "Brasilia", CountryId = 5 },
                new City { CityId = 100, CityName = "Salvador", CityNameEN = "Salvador", CountryId = 5 },
                new City { CityId = 101, CityName = "Fortaleza", CityNameEN = "Fortaleza", CountryId = 5 }
            );

            modelBuilder.Entity<RateDef>().HasData(
                new RateDef { RateDefId=1,Value ="Like"},
                new RateDef { RateDefId=2,Value ="Dislike"}
                );

            modelBuilder.Entity<Source>().HasData(
                new Source { SourceId = 1, SourceName = "Anadolu Ajansı", SourceNameEN = "Anadolu Agency", Deleted = false },
                new Source { SourceId = 2, SourceName = "BBC Türkçe", SourceNameEN = "BBC Turkish", Deleted = false },
                new Source { SourceId = 3, SourceName = "Hürriyet", SourceNameEN = "Hurriyet", Deleted = false },
                new Source { SourceId = 4, SourceName = "CNN Türk", SourceNameEN = "CNN Turk", Deleted = false },
                new Source { SourceId = 5, SourceName = "NTV", SourceNameEN = "NTV", Deleted = false }
            );

            modelBuilder.Entity<News>().HasData(
                new News
                {
                    NewsId = 1,
                    Heading = "Futbol Sezonu Başladı",
                    HeadingEN = "Football Season Started",
                    PhotoPath = "img/sp1.jpg",
                    Description = "Yeni futbol sezonu büyük bir coşkuyla başladı. Takımlar hazırlıklarını tamamladı ve ilk maçlar oynandı. Sezonun açılış maçında büyük bir heyecan vardı ve taraftarlar tribünleri doldurdu. Teknik direktörler ve oyuncular, sezon boyunca başarılı olmak için ellerinden geleni yapacaklarını belirtti. İlk haftada sürpriz sonuçlar da yaşandı ve bu sonuçlar sezonun ne kadar çekişmeli geçeceğinin bir göstergesi oldu. Futbolseverler, sezon boyunca takımlarını desteklemek için sabırsızlanıyor.",
                    DescriptionEN = "The new football season has started with great enthusiasm. Teams have completed their preparations and the first matches were played. There was great excitement in the opening match of the season, and the fans filled the stands. Coaches and players stated that they will do their best to be successful throughout the season. There were also surprise results in the first week, which indicated how competitive the season will be. Football fans are eagerly looking forward to supporting their teams throughout the season.",
                    SourceId = 1, // Anadolu Ajansı
                    CategoryId = 1, // Spor
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 2,
                    Heading = "Basketbol Ligi Şampiyonu Belli Oldu",
                    HeadingEN = "Basketball League Champion Announced",
                    PhotoPath = "img/sp2.jpg",
                    Description = "Basketbol ligi şampiyonu büyük bir mücadele sonucunda belli oldu. Final maçında heyecan doruktaydı. Takımlar, sezon boyunca gösterdikleri performanslarla taraftarlarını mutlu etti. Şampiyon takımın koçu, oyuncularının sezon boyunca gösterdikleri fedakarlıklardan ötürü gurur duyduğunu belirtti. Taraftarlar, şampiyonluğu kutlamak için sokaklara döküldü ve büyük bir coşku yaşandı. Bu sezonun en dikkat çeken oyuncuları, gelecek sezon için de umut vaat ediyor.",
                    DescriptionEN = "The basketball league champion was announced after a fierce competition. The excitement peaked during the final match. Teams made their fans happy with their performances throughout the season. The coach of the champion team stated that he is proud of his players for their sacrifices throughout the season. Fans took to the streets to celebrate the championship, and there was great enthusiasm. The most notable players of this season also promise hope for the next season.",
                    SourceId = 2, // BBC Türkçe
                    CategoryId = 1, // Spor
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 3,
                    Heading = "Olimpiyatlara Hazırlık Tamamlandı",
                    HeadingEN = "Preparations for the Olympics Completed",
                    PhotoPath = "img/sp3.jpg",
                    Description = "Sporcular, yaklaşan Olimpiyatlar için hazırlıklarını tamamladı. Tüm gözler büyük etkinlikte. Sporcular, antrenmanlarda yoğun bir tempo ile çalıştı ve performanslarını en üst düzeye çıkarmak için ellerinden geleni yaptılar. Olimpiyat köyünde son hazırlıklar da tamamlanmış durumda. Uluslararası sporcular, ülkelerini en iyi şekilde temsil etmek için sabırsızlanıyor. Olimpiyat komitesi, bu yılki etkinliğin önceki yıllardan daha büyük ve daha görkemli olacağını belirtti. Sporcuların hikayeleri ve başarıları, milyonlarca insan tarafından ilgiyle takip edilecek.",
                    DescriptionEN = "Athletes have completed their preparations for the upcoming Olympics. All eyes are on the big event. Athletes worked intensively in training and did their best to maximize their performance. The final preparations in the Olympic village are also complete. International athletes are eager to represent their countries in the best possible way. The Olympic committee stated that this year's event will be bigger and more magnificent than previous years. The stories and achievements of the athletes will be followed with great interest by millions of people.",
                    SourceId = 3, // Hürriyet
                    CategoryId = 1, // Spor
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 4,
                    Heading = "Tenis Turnuvası Sona Erdi",
                    HeadingEN = "Tennis Tournament Concluded",
                    PhotoPath = "img/sp4.jpg",
                    Description = "Büyük tenis turnuvası sona erdi ve kazananlar ödüllerini aldı. Turnuva boyunca birçok sürpriz yaşandı. Tenisçiler, kortlarda büyük bir mücadele sergiledi ve izleyicilere unutulmaz anlar yaşattı. Şampiyon, final maçında gösterdiği üstün performansla büyük beğeni topladı. Turnuva organizatörleri, bu yılki etkinliğin önceki yıllardan daha başarılı geçtiğini belirtti. Tenis tutkunları, turnuva boyunca favori oyuncularını desteklemek için kortları doldurdu. Şampiyonanın ardından tenisçiler, bir sonraki büyük etkinlik için hazırlıklara başladı.",
                    DescriptionEN = "The big tennis tournament has concluded and the winners received their awards. Many surprises occurred throughout the tournament. Tennis players displayed great effort on the courts and gave the audience unforgettable moments. The champion received great acclaim for his outstanding performance in the final match. Tournament organizers stated that this year's event was more successful than previous years. Tennis enthusiasts filled the courts to support their favorite players throughout the tournament. After the championship, tennis players started preparations for the next big event.",
                    SourceId = 4, // CNN Türk
                    CategoryId = 1, // Spor
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 5,
                    Heading = "Yüzme Yarışları Nefes Kesti",
                    HeadingEN = "Swimming Races Took Breath Away",
                    PhotoPath = "img/sp5.jpg",
                    Description = "Bu yılki yüzme yarışları nefes kesen performanslara sahne oldu. Sporcular rekor üstüne rekor kırdı. Yarışlar boyunca izleyiciler büyük bir heyecan yaşadı. Su sporlarına olan ilgi her geçen yıl artarken, bu yılki etkinlikte de katılım oldukça yoğundu. Şampiyon yüzücüler, kazandıkları madalyalarla büyük bir mutluluk yaşadı. Yarışların ardından yapılan basın toplantısında, sporcular yarışların çok zorlu geçtiğini ancak elde ettikleri başarıdan ötürü gurur duyduklarını ifade etti. Su sporları tutkunları, gelecek yılki yarışlar için sabırsızlanıyor.",
                    DescriptionEN = "This year's swimming races featured breathtaking performances. Athletes broke record after record. The audience experienced great excitement throughout the races. Interest in water sports is increasing every year, and participation in this year's event was also very high. Champion swimmers were very happy with the medals they won. At the press conference held after the races, the athletes stated that the races were very challenging but they were proud of their achievements. Water sports enthusiasts are eagerly looking forward to next year's races.",
                    SourceId = 5, // NTV
                    CategoryId = 1, // Spor
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                 {
                     NewsId = 6,
                     Heading = "Yeni Akıllı Telefon Tanıtıldı",
                     HeadingEN = "New Smartphone Introduced",
                     PhotoPath = "img/te1.jpg",
                     Description = "Yeni akıllı telefon modeli büyük bir etkinlikte tanıtıldı. Gelişmiş özellikleri ve şık tasarımıyla dikkat çeken telefon, teknoloji meraklıları tarafından büyük ilgi gördü. Tanıtım sırasında yapılan sunumda, telefonun yüksek çözünürlüklü kamerası ve hızlı işlemcisi ön plana çıkarıldı. Ayrıca, batarya ömrü ve suya dayanıklılık gibi özellikler de vurgulandı. Yeni modelin piyasaya sürülmesiyle birlikte teknoloji dünyasında büyük bir hareketlilik yaşanması bekleniyor.",
                     DescriptionEN = "The new smartphone model was introduced at a grand event. The phone, which stands out with its advanced features and sleek design, attracted great interest from technology enthusiasts. During the presentation, the high-resolution camera and fast processor of the phone were highlighted. Additionally, features such as battery life and water resistance were emphasized. With the release of the new model, great activity is expected in the tech world.",
                     SourceId = 1, // Anadolu Ajansı
                     CategoryId = 2, // Teknoloji
                     CreatedTime = DateTime.UtcNow,
                     Deleted = false
                 },
                new News
                {
                    NewsId = 7,
                    Heading = "Yapay Zeka Alanında Yeni Gelişmeler",
                    HeadingEN = "New Developments in Artificial Intelligence",
                    PhotoPath = "img/te2.jpg",
                    Description = "Yapay zeka alanında kaydedilen yeni gelişmeler bilim dünyasında büyük heyecan yarattı. Araştırmacılar, yapay zekanın insan benzeri düşünme yeteneklerini daha da geliştirdiğini belirtti. Yapay zeka algoritmalarının çeşitli alanlarda kullanımının yaygınlaşması bekleniyor. Sağlık, finans ve eğitim gibi sektörlerde yapay zekanın etkinliği artarken, bu alandaki çalışmalar da hız kesmeden devam ediyor. Yapay zeka teknolojisinin gelecekte neler getireceği merakla bekleniyor.",
                    DescriptionEN = "New developments in the field of artificial intelligence have created great excitement in the scientific community. Researchers have stated that artificial intelligence has further developed human-like thinking abilities. The use of artificial intelligence algorithms in various fields is expected to become widespread. While the effectiveness of artificial intelligence in sectors such as health, finance, and education is increasing, studies in this area continue at full speed. The future implications of artificial intelligence technology are eagerly anticipated.",
                    SourceId = 2, // BBC Türkçe
                    CategoryId = 2, // Teknoloji
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 8,
                    Heading = "Yeni Nesil Oyun Konsolu Piyasaya Sürüldü",
                    HeadingEN = "Next-Gen Game Console Released",
                    PhotoPath = "img/te3.jpg",
                    Description = "Oyun dünyasının merakla beklediği yeni nesil oyun konsolu nihayet piyasaya sürüldü. Konsol, yüksek grafik kalitesi ve hızlı işlemcisi ile oyunculara benzersiz bir deneyim sunuyor. Tanıtım etkinliğinde yapılan gösterimlerde, yeni konsolun oyun performansı büyük beğeni topladı. Oyun geliştiricileri de yeni konsol için özel olarak tasarlanan oyunlarını tanıttı. Konsolun piyasaya sürülmesiyle birlikte oyun sektöründe büyük bir hareketlilik yaşanması bekleniyor.",
                    DescriptionEN = "The next-generation game console, eagerly awaited by the gaming world, has finally been released. The console offers a unique experience to players with its high graphics quality and fast processor. During the launch event, the gaming performance of the new console received great acclaim. Game developers also introduced their games designed specifically for the new console. With the release of the console, significant activity is expected in the gaming industry.",
                    SourceId = 3, // Hürriyet
                    CategoryId = 2, // Teknoloji
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 9,
                    Heading = "Sanal Gerçeklik Teknolojisinde Büyük Adım",
                    HeadingEN = "A Big Step in Virtual Reality Technology",
                    PhotoPath = "img/te4.jpg",
                    Description = "Sanal gerçeklik teknolojisinde kaydedilen büyük adımlar, bu alandaki yenilikleri beraberinde getiriyor. Yeni geliştirilen VR cihazları, kullanıcılara daha gerçekçi ve etkileyici deneyimler sunuyor. Özellikle eğitim ve sağlık alanlarında sanal gerçeklik uygulamalarının etkinliği artıyor. Uzmanlar, sanal gerçekliğin gelecekte daha geniş bir kullanım alanına sahip olacağını belirtiyor. Bu teknoloji, farklı sektörlerde devrim niteliğinde değişikliklere yol açabilir.",
                    DescriptionEN = "Significant strides in virtual reality technology are bringing about innovations in this field. Newly developed VR devices offer users more realistic and immersive experiences. The effectiveness of virtual reality applications is increasing, especially in the fields of education and health. Experts state that virtual reality will have a broader range of applications in the future. This technology could lead to revolutionary changes in various sectors.",
                    SourceId = 4, // CNN Türk
                    CategoryId = 2, // Teknoloji
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 10,
                    Heading = "Blockchain Teknolojisi ve Kripto Paralar",
                    HeadingEN = "Blockchain Technology and Cryptocurrencies",
                    PhotoPath = "img/te5.jpg",
                    Description = "Blockchain teknolojisi ve kripto paralar, son yıllarda büyük ilgi görüyor. Bu teknolojinin güvenlik ve şeffaflık gibi avantajları, finans sektöründe devrim niteliğinde değişikliklere yol açıyor. Kripto paraların değeri, piyasa dalgalanmalarına bağlı olarak hızla değişiyor. Uzmanlar, blockchain teknolojisinin sadece finans alanında değil, farklı sektörlerde de kullanılabileceğini belirtiyor. Gelecekte, bu teknolojinin daha geniş bir kullanım alanına sahip olması bekleniyor.",
                    DescriptionEN = "Blockchain technology and cryptocurrencies have garnered great interest in recent years. The advantages of this technology, such as security and transparency, are leading to revolutionary changes in the financial sector. The value of cryptocurrencies fluctuates rapidly based on market changes. Experts state that blockchain technology can be used not only in finance but also in different sectors. In the future, this technology is expected to have a broader range of applications.",
                    SourceId = 5, // NTV
                    CategoryId = 2, // Teknoloji
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                    {
                        NewsId = 11,
                        Heading = "Yeni Sanat Sergisi Açıldı",
                        HeadingEN = "New Art Exhibition Opened",
                        PhotoPath = "img/ar1.jpg",
                        Description = "Şehirde merakla beklenen yeni sanat sergisi büyük bir katılımla açıldı. Sergide, yerli ve yabancı sanatçıların eserleri yer alıyor. Modern sanatın en güzel örneklerinin bulunduğu sergi, sanatseverlerin ilgisini çekiyor. Açılış töreninde sanatçılar ve sanat eleştirmenleri bir araya geldi. Sergi boyunca çeşitli atölye çalışmaları ve söyleşiler de düzenlenecek. Sanatseverler, bu eşsiz sergiyi görmek için akın ediyor.",
                        DescriptionEN = "The eagerly awaited new art exhibition has opened with a large attendance in the city. The exhibition features works by local and international artists. The exhibition, which includes the finest examples of modern art, attracts the attention of art lovers. At the opening ceremony, artists and art critics came together. Various workshops and talks will also be organized throughout the exhibition. Art lovers are flocking to see this unique exhibition.",
                        SourceId = 1, // Anadolu Ajansı
                        CategoryId = 3, // Sanat
                        CreatedTime = DateTime.UtcNow,
                        Deleted = false
                    },
                new News
                {
                    NewsId = 12,
                    Heading = "Sanat Festivali Büyük İlgi Gördü",
                    HeadingEN = "Art Festival Attracted Great Interest",
                    PhotoPath = "img/ar2.jpg",
                    Description = "Şehirde düzenlenen sanat festivali, büyük bir ilgiyle karşılandı. Festivalde, resimden heykele, müzikten tiyatroya kadar birçok sanat dalı yer aldı. Sanatçılar, eserlerini sergileme ve performanslarını sergileme fırsatı buldu. Festival süresince birçok etkinlik düzenlendi ve sanatseverler, dolu dolu bir program yaşadı. Festival, sanatın toplum üzerindeki etkisini bir kez daha gözler önüne serdi.",
                    DescriptionEN = "The art festival held in the city was met with great interest. The festival featured many art forms, from painting to sculpture, music to theater. Artists had the opportunity to showcase their works and perform their acts. Many events were organized throughout the festival, and art lovers experienced a full program. The festival once again highlighted the impact of art on society.",
                    SourceId = 2, // BBC Türkçe
                    CategoryId = 3, // Sanat
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 13,
                    Heading = "Ünlü Ressamdan Yeni Eserler",
                    HeadingEN = "New Works by Famous Painter",
                    PhotoPath = "img/ar3.jpg",
                    Description = "Ünlü ressamın yeni eserleri sanat galerilerinde sergilenmeye başladı. Ressam, son dönem çalışmalarında farklı teknikler kullanarak dikkat çekici eserler ortaya koydu. Eserler, sanat eleştirmenleri tarafından büyük beğeni topladı. Sanatseverler, yeni eserleri görmek için galerilere akın ediyor. Sergi süresince ressam, eserleri hakkında bilgi vererek izleyicilerle buluşacak. Bu sergi, sanat dünyasında büyük yankı uyandırdı.",
                    DescriptionEN = "The new works of the famous painter have started to be exhibited in art galleries. The painter has created remarkable works using different techniques in his recent works. The works have received great acclaim from art critics. Art lovers are flocking to galleries to see the new works. During the exhibition, the painter will meet with the audience, providing information about his works. This exhibition has created a great stir in the art world.",
                    SourceId = 3, // Hürriyet
                    CategoryId = 3, // Sanat
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 14,
                    Heading = "Tiyatro Sezonu Açıldı",
                    HeadingEN = "Theater Season Opened",
                    PhotoPath = "img/ar4.jpg",
                    Description = "Tiyatro sezonu, görkemli bir açılış töreniyle başladı. Yeni sezonda sahnelenecek oyunlar, büyük bir merakla bekleniyor. Tiyatroseverler, sezon boyunca birbirinden ilginç ve etkileyici oyunları izleme fırsatı bulacak. Açılış gecesinde sahnelenen oyun, izleyicilerden tam not aldı. Tiyatro sanatçıları, yeni sezon için uzun süredir hazırlık yapıyordu. Bu sezon, tiyatro dünyasında unutulmaz anlara sahne olacak.",
                    DescriptionEN = "The theater season has opened with a grand opening ceremony. The plays to be staged in the new season are eagerly awaited. Theater enthusiasts will have the opportunity to watch many interesting and impressive plays throughout the season. The play staged on the opening night received full marks from the audience. Theater artists have been preparing for the new season for a long time. This season will feature unforgettable moments in the theater world.",
                    SourceId = 4, // CNN Türk
                    CategoryId = 3, // Sanat
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 15,
                    Heading = "Müze Ziyaretçi Rekoru Kırdı",
                    HeadingEN = "Museum Broke Visitor Record",
                    PhotoPath = "img/ar5.jpg",
                    Description = "Şehirdeki müze, ziyaretçi rekoru kırdı. Özellikle yeni açılan sergi, büyük ilgi gördü. Müze yetkilileri, bu yoğun ilginin kendilerini çok mutlu ettiğini belirtti. Sergide, tarihi eserlerden modern sanat eserlerine kadar birçok farklı eser yer alıyor. Ziyaretçiler, müze turu sırasında birçok bilgi edinme fırsatı buldu. Müze, yıl boyunca çeşitli etkinlikler ve sergiler düzenlemeye devam edecek.",
                    DescriptionEN = "The museum in the city broke the visitor record. The newly opened exhibition, in particular, attracted great interest. Museum officials stated that they were very pleased with this intense interest. The exhibition features many different works, from historical artifacts to modern art pieces. Visitors had the opportunity to learn a lot of information during the museum tour. The museum will continue to organize various events and exhibitions throughout the year.",
                    SourceId = 5, // NTV
                    CategoryId = 3, // Sanat
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                  {
                      NewsId = 16,
                      Heading = "Yeni Eğitim Öğretim Yılı Başladı",
                      HeadingEN = "New Academic Year Started",
                      PhotoPath = "img/ed1.jpg",
                      Description = "Yeni eğitim öğretim yılı büyük bir heyecanla başladı. Öğrenciler ve öğretmenler, uzun bir yaz tatilinin ardından okullara döndü. Okullar, pandemi koşullarına uygun olarak düzenlemeler yaptı ve hijyen önlemlerini arttırdı. Bu yıl, müfredatta önemli değişiklikler yapıldı ve öğrencilere daha geniş bir öğrenme deneyimi sunulması hedeflendi. Eğitim yılı boyunca, öğrencilerin sosyal ve akademik gelişimlerini desteklemek için çeşitli etkinlikler düzenlenecek. Veliler, çocuklarının eğitimine aktif olarak katılmaları için teşvik ediliyor.",
                      DescriptionEN = "The new academic year has started with great excitement. Students and teachers have returned to schools after a long summer break. Schools have made arrangements in accordance with pandemic conditions and increased hygiene measures. Significant changes have been made to the curriculum this year, aiming to provide students with a broader learning experience. Throughout the academic year, various activities will be organized to support the social and academic development of students. Parents are encouraged to actively participate in their children's education.",
                      SourceId = 1, // Anadolu Ajansı
                      CategoryId = 4, // Eğitim
                      CreatedTime = DateTime.UtcNow,
                      Deleted = false
                  },
                new News
                {
                    NewsId = 17,
                    Heading = "Online Eğitimde Yeni Dönem",
                    HeadingEN = "New Era in Online Education",
                    PhotoPath = "img/ed2.jpg",
                    Description = "Pandemi sürecinde yaygınlaşan online eğitim, yeni dönemde de devam ediyor. Eğitim kurumları, online platformlar üzerinden derslerini sürdürmeye devam ediyor. Öğrenciler, evlerinden katıldıkları derslerde, interaktif bir öğrenme deneyimi yaşıyor. Eğitim teknolojilerindeki gelişmeler, online eğitimin kalitesini arttırdı. Öğretmenler, online eğitim sürecinde öğrencilere birebir destek sağlamak için çeşitli yöntemler geliştiriyor. Bu yeni eğitim modeli, gelecekte de etkisini sürdürecek gibi görünüyor.",
                    DescriptionEN = "Online education, which became widespread during the pandemic, continues in the new era. Educational institutions continue their classes through online platforms. Students experience an interactive learning environment in the classes they attend from their homes. Developments in educational technologies have increased the quality of online education. Teachers are developing various methods to provide one-on-one support to students during the online education process. This new education model seems to continue its impact in the future.",
                    SourceId = 2, // BBC Türkçe
                    CategoryId = 4, // Eğitim
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 18,
                    Heading = "Üniversite Giriş Sınavında Değişiklikler",
                    HeadingEN = "Changes in University Entrance Exam",
                    PhotoPath = "img/ed3.jpg",
                    Description = "Üniversite giriş sınavında yapılan değişiklikler, öğrenciler ve veliler tarafından yakından takip ediliyor. Bu yıl, sınav formatında bazı yenilikler yapıldı ve öğrencilere daha esnek bir sınav deneyimi sunulması hedeflendi. Uzmanlar, bu değişikliklerin öğrencilerin sınav stresini azaltacağını ve başarı oranını artıracağını belirtiyor. Yeni sınav sistemi hakkında detaylı bilgiler, eğitim kurumları tarafından öğrencilere ve velilere aktarılmaya başlandı. Sınav hazırlık sürecinde, öğrencilerin motivasyonunu artırmak için çeşitli destek programları da devreye sokuldu.",
                    DescriptionEN = "Changes made in the university entrance exam are being closely followed by students and parents. This year, some innovations have been made in the exam format, aiming to provide students with a more flexible exam experience. Experts state that these changes will reduce students' exam stress and increase success rates. Detailed information about the new exam system has started to be communicated to students and parents by educational institutions. Various support programs have also been implemented to increase students' motivation during the exam preparation process.",
                    SourceId = 3, // Hürriyet
                    CategoryId = 4, // Eğitim
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 19,
                    Heading = "Eğitimde Dijital Dönüşüm",
                    HeadingEN = "Digital Transformation in Education",
                    PhotoPath = "img/ed4.jpg",
                    Description = "Eğitimde dijital dönüşüm hız kesmeden devam ediyor. Okullar ve üniversiteler, dijital öğrenme araçları ve platformları kullanarak eğitimde yenilikçi yaklaşımlar benimsiyor. Dijital ders içerikleri, öğrencilere daha zengin ve etkileşimli bir öğrenme deneyimi sunuyor. Eğitimde kullanılan yapay zeka ve veri analitiği, öğrencilerin bireysel öğrenme ihtiyaçlarına daha uygun çözümler sunulmasını sağlıyor. Bu dönüşüm, eğitimde kaliteyi ve erişilebilirliği artırarak, öğrencilerin geleceğe daha iyi hazırlanmasını hedefliyor.",
                    DescriptionEN = "Digital transformation in education continues at full speed. Schools and universities are adopting innovative approaches in education by using digital learning tools and platforms. Digital course materials provide students with a richer and more interactive learning experience. The use of artificial intelligence and data analytics in education allows for more suitable solutions for students' individual learning needs. This transformation aims to improve quality and accessibility in education, better preparing students for the future.",
                    SourceId = 4, // CNN Türk
                    CategoryId = 4, // Eğitim
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 20,
                    Heading = "Öğretmenler İçin Mesleki Gelişim Programları",
                    HeadingEN = "Professional Development Programs for Teachers",
                    PhotoPath = "img/ed5.jpg",
                    Description = "Öğretmenler için düzenlenen mesleki gelişim programları, eğitim kalitesini artırmayı hedefliyor. Bu programlar, öğretmenlerin yeni eğitim teknikleri ve teknolojileri konusunda bilgi sahibi olmalarını sağlıyor. Mesleki gelişim programları, öğretmenlerin sınıf içi uygulamalarını geliştirerek, öğrencilere daha etkili bir eğitim sunmalarına yardımcı oluyor. Eğitim kurumları, öğretmenlerin sürekli olarak kendilerini geliştirmeleri için çeşitli eğitim ve atölye çalışmaları düzenliyor. Bu programlar, öğretmenlerin motivasyonunu artırarak, eğitimde başarının anahtarı olarak görülüyor.",
                    DescriptionEN = "Professional development programs organized for teachers aim to improve the quality of education. These programs provide teachers with knowledge about new educational techniques and technologies. Professional development programs help teachers improve their classroom practices, enabling them to offer more effective education to students. Educational institutions organize various training and workshop sessions to ensure that teachers continuously develop themselves. These programs are seen as the key to success in education by increasing teachers' motivation.",
                    SourceId = 5, // NTV
                    CategoryId = 4, // Eğitim
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                    {
                        NewsId = 21,
                        Heading = "Yeni Aşı Çalışmaları Umut Veriyor",
                        HeadingEN = "New Vaccine Studies Offer Hope",
                        PhotoPath = "img/sa1.jpg",
                        Description = "Son dönemde yapılan yeni aşı çalışmaları, sağlık dünyasında büyük umutlar vaat ediyor. Araştırmacılar, aşıların etkinliğini artırmak ve yan etkilerini azaltmak için yoğun bir şekilde çalışıyor. Yeni aşılar, özellikle salgın hastalıklarla mücadelede önemli bir rol oynayacak. Uzmanlar, bu aşıların insanlık için büyük bir adım olduğunu belirtiyor. Klinik denemeler olumlu sonuçlar verirken, aşının geniş çapta kullanımı için hazırlıklar sürüyor. Sağlık otoriteleri, aşıların güvenli ve etkili olduğunu vurgulayarak halkı aşı olmaya teşvik ediyor.",
                        DescriptionEN = "Recent vaccine studies promise great hope in the health world. Researchers are working intensively to increase the effectiveness of vaccines and reduce their side effects. New vaccines will play an important role, especially in the fight against epidemic diseases. Experts state that these vaccines are a major step for humanity. While clinical trials are showing positive results, preparations for the widespread use of the vaccine are ongoing. Health authorities emphasize that the vaccines are safe and effective, encouraging the public to get vaccinated.",
                        SourceId = 1, // Anadolu Ajansı
                        CategoryId = 5, // Sağlık
                        CreatedTime = DateTime.UtcNow,
                        Deleted = false
                    },
                new News
                {
                    NewsId = 22,
                    Heading = "Sağlıklı Yaşam İçin Yeni Öneriler",
                    HeadingEN = "New Recommendations for a Healthy Life",
                    PhotoPath = "img/sa2.jpg",
                    Description = "Sağlık uzmanları, sağlıklı bir yaşam sürdürmek için yeni önerilerde bulundu. Dengeli beslenme, düzenli egzersiz ve yeterli uyku, sağlıklı yaşamın temel taşları olarak vurgulanıyor. Ayrıca, stresten uzak durmak ve zihinsel sağlığa önem vermek de önemli. Uzmanlar, sağlıklı yaşamın sadece fiziksel değil, aynı zamanda zihinsel ve duygusal sağlığı da kapsadığını belirtiyor. Bu öneriler, bireylerin yaşam kalitesini artırmak ve hastalıklardan korunmak için hayati önem taşıyor.",
                    DescriptionEN = "Health experts have made new recommendations for maintaining a healthy life. Balanced nutrition, regular exercise, and adequate sleep are emphasized as the cornerstones of a healthy life. Additionally, avoiding stress and paying attention to mental health are also important. Experts state that a healthy life encompasses not only physical health but also mental and emotional well-being. These recommendations are vital for improving individuals' quality of life and protecting them from diseases.",
                    SourceId = 2, // BBC Türkçe
                    CategoryId = 5, // Sağlık
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 23,
                    Heading = "Pandemi Sonrası Psikolojik Destek",
                    HeadingEN = "Psychological Support After the Pandemic",
                    PhotoPath = "img/sa3.jpg",
                    Description = "Pandemi sonrası dönemde, psikolojik destek ihtiyacı artış gösterdi. Uzmanlar, pandeminin insanların zihinsel sağlığı üzerinde derin etkiler bıraktığını belirtiyor. Bu dönemde, psikolojik destek almak ve duygusal sağlığa önem vermek büyük önem taşıyor. Psikologlar, bireylerin bu zorlu süreçten daha güçlü çıkmaları için çeşitli terapi yöntemleri öneriyor. Toplum olarak, birbirimize destek olmanın ve dayanışmanın önemi vurgulanıyor.",
                    DescriptionEN = "In the post-pandemic period, the need for psychological support has increased. Experts state that the pandemic has had profound effects on people's mental health. During this period, it is crucial to seek psychological support and pay attention to emotional well-being. Psychologists recommend various therapy methods to help individuals emerge stronger from this challenging period. As a society, the importance of supporting and standing by each other is emphasized.",
                    SourceId = 3, // Hürriyet
                    CategoryId = 5, // Sağlık
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 24,
                    Heading = "Beslenme ve Diyet Uzmanlarından Yeni Tavsiyeler",
                    HeadingEN = "New Recommendations from Nutrition and Diet Experts",
                    PhotoPath = "img/sa4.jpg",
                    Description = "Beslenme ve diyet uzmanları, sağlıklı bir yaşam için yeni tavsiyelerde bulundu. Doğal ve işlenmemiş gıdaların tüketilmesi, günlük su alımının artırılması ve düzenli öğünler sağlıklı bir diyetin temelini oluşturuyor. Ayrıca, aşırı şeker ve tuz tüketiminden kaçınmak, kalp sağlığı için büyük önem taşıyor. Uzmanlar, bireylerin sağlıklı beslenme alışkanlıkları kazanarak yaşam kalitelerini artırabileceklerini belirtiyor. Bu tavsiyeler, dengeli ve sağlıklı bir diyet için yol gösterici nitelikte.",
                    DescriptionEN = "Nutrition and diet experts have made new recommendations for a healthy life. Consuming natural and unprocessed foods, increasing daily water intake, and having regular meals form the basis of a healthy diet. Additionally, avoiding excessive sugar and salt consumption is crucial for heart health. Experts state that individuals can improve their quality of life by adopting healthy eating habits. These recommendations serve as a guide for a balanced and healthy diet.",
                    SourceId = 4, // CNN Türk
                    CategoryId = 5, // Sağlık
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                },
                new News
                {
                    NewsId = 25,
                    Heading = "Düzenli Egzersizin Faydaları",
                    HeadingEN = "Benefits of Regular Exercise",
                    PhotoPath = "img/sa5.jpg",
                    Description = "Düzenli egzersiz yapmanın sağlık üzerinde sayısız faydası bulunuyor. Uzmanlar, haftada en az beş gün 30 dakika egzersiz yapmanın, kalp sağlığını korumak ve genel sağlık durumunu iyileştirmek için önemli olduğunu belirtiyor. Egzersiz, sadece fiziksel sağlık için değil, aynı zamanda zihinsel ve duygusal sağlık için de büyük önem taşıyor. Düzenli egzersiz yapmak, stres seviyelerini azaltmaya ve uyku kalitesini artırmaya yardımcı oluyor. Bu nedenle, günlük yaşamda egzersize yer vermek büyük önem taşıyor.",
                    DescriptionEN = "Regular exercise has numerous benefits for health. Experts state that exercising for at least 30 minutes five days a week is important for maintaining heart health and improving overall health. Exercise is not only important for physical health but also for mental and emotional well-being. Regular exercise helps reduce stress levels and improve sleep quality. Therefore, incorporating exercise into daily life is of great importance.",
                    SourceId = 5, // NTV
                    CategoryId = 5, // Sağlık
                    CreatedTime = DateTime.UtcNow,
                    Deleted = false
                }
            );






        }
    }
}
