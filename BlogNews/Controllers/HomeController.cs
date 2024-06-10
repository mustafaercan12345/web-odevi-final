using BlogNews.Models;
using BlogNews.Models.Database;
using BlogNews.Models.Database.EntityModel;
using BlogNews.Models.ViewModels;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using static BlogNews.Models.ViewModels.HomeViewModel;

namespace BlogNews.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, DataContext context, IWebHostEnvironment environment)
        {
            _logger = logger;
            _context = context;
            _environment = environment;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();


            // Kullanıcının en çok like attığı 3 kategoriyi bul
            var userId = GetUserId();
            var cityName = "Ankara";
            var cityNameUser = _context.Users.Where(a => a.UserId == userId && !a.Deleted).Select(b => b.City.CityNameMulti).FirstOrDefault();
            if (cityNameUser != null)
                cityName = cityNameUser.ToString();

            var topCategoriesUserInterest = _context.UserInterests.Where(a => a.UserId == userId && !a.Deleted).Select(a => a.CategoryId).ToList();

            var topCategoriesLiked = _context.Rates
                .Where(r => r.UserId == userId && r.RateDef.Value == "Like" && !r.Deleted)
                .GroupBy(r => r.News.CategoryId)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .ToList();

            List<HomeViewModel.NewsForYouViewModel> newsForYouList;
            List<HomeViewModel.NewsSliderHomeViewModel> newsSliderList;

            if (topCategoriesUserInterest == null || !topCategoriesUserInterest.Any())
            {
                newsSliderList = _context.News.Include(a => a.Source).OrderByDescending(n => n.CreatedTime).Take(5).Select(n => new NewsSliderHomeViewModel
                {
                    NewsId = n.NewsId,
                    Heading = n.HeadingMulti,
                    PhotoPath = n.PhotoPath,
                    SourceName = n.Source.SourceNameMulti
                }).ToList(); // Son 5 haberi al
            }
            else
            {
                // En son eklenen 6 haberi al
                newsSliderList = _context.News.Include(a => a.Source).Where(n => topCategoriesUserInterest.Contains(n.CategoryId) && !n.Deleted).OrderByDescending(n => n.CreatedTime).Take(5).Select(n => new NewsSliderHomeViewModel
                {
                    NewsId = n.NewsId,
                    Heading = n.HeadingMulti,
                    PhotoPath = n.PhotoPath,
                    SourceName = n.Source.SourceNameMulti
                }).ToList(); // Son 5 haberi al
            }

            var existingSliderNewsIds = newsSliderList.Select(n => n.NewsId).ToList();

            if (topCategoriesLiked == null || !topCategoriesLiked.Any())
            {
                // Eğer topCategories boşsa, tüm haberler içerisindeki son 6 haberi al
                newsForYouList = _context.News
                    .Where(n => !n.Deleted && !existingSliderNewsIds.Contains(n.NewsId))
                    .OrderByDescending(n => n.CreatedTime)
                    .Take(6)
                    .Select(n => new HomeViewModel.NewsForYouViewModel
                    {
                        NewsId = n.NewsId,
                        Heading = n.HeadingMulti,
                        PhotoPath = n.PhotoPath,
                        SourceName = n.Source.SourceNameMulti,
                        LikeNumber = n.Rates.Count(r => r.RateDef.Value == "Like" && !r.Deleted),
                        DislikeNumber = n.Rates.Count(r => r.RateDef.Value == "Dislike" && !r.Deleted)
                    })
                    .ToList();
            }
            else
            {
                // En son eklenen 6 haberi al beğenilen haberlere göre.
                newsForYouList = _context.News
                    .Where(n => topCategoriesLiked.Contains(n.CategoryId) && !n.Deleted && !existingSliderNewsIds.Contains(n.NewsId))
                    .OrderByDescending(n => n.CreatedTime)
                    .Take(6)
                    .Select(n => new HomeViewModel.NewsForYouViewModel
                    {
                        NewsId = n.NewsId,
                        Heading = n.HeadingMulti,
                        PhotoPath = n.PhotoPath,
                        SourceName = n.Source.SourceNameMulti,
                        LikeNumber = n.Rates.Count(r => r.RateDef.Value == "Like" && !r.Deleted),
                        DislikeNumber = n.Rates.Count(r => r.RateDef.Value == "Dislike" && !r.Deleted)
                    })
                    .ToList();

                if (newsForYouList.Count < 6)
                {
                    var existingNewsIds = newsForYouList.Select(n => n.NewsId).ToList();
                    var newsForYouListAdded = _context.News
                        .Where(n => !n.Deleted && !existingNewsIds.Contains(n.NewsId) && !existingSliderNewsIds.Contains(n.NewsId))
                        .OrderByDescending(n => n.CreatedTime)
                        .Take(6 - newsForYouList.Count)
                        .Select(n => new HomeViewModel.NewsForYouViewModel
                        {
                            NewsId = n.NewsId,
                            Heading = n.HeadingMulti,
                            PhotoPath = n.PhotoPath,
                            SourceName = n.Source.SourceNameMulti,
                            LikeNumber = n.Rates.Count(r => r.RateDef.Value == "Like" && !r.Deleted),
                            DislikeNumber = n.Rates.Count(r => r.RateDef.Value == "Dislike" && !r.Deleted)
                        })
                        .ToList();
                    newsForYouList.AddRange(newsForYouListAdded);
                }
            }


            var viewModel = new HomeViewModel
            {
                Categories = categories.Select(c => new HomeViewModel.CategoryViewModel
                {
                    Id = c.CategoryId,
                    Name = c.CategoryNameMulti
                }).ToList(),
                NewsSliderHomeList = newsSliderList,
                NewsForYouHomeList = newsForYouList.Select(n => new HomeViewModel.NewsForYouViewModel
                {
                    NewsId = n.NewsId,
                    Heading = n.Heading,
                    PhotoPath = n.PhotoPath,
                    SourceName = n.SourceName,
                    LikeNumber = n.LikeNumber,
                    DislikeNumber = n.DislikeNumber

                }).ToList(),
                CityName = cityName


            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetLatestNews()
        {
            var userId = GetUserId();
            bool notificationIsEnabled = _context.Users.Where(a => a.UserId == userId).Select(a => a.NotificationEnabled).FirstOrDefault();
            if (!notificationIsEnabled)
                return Json("");

            var latestNews = await _context.News
                   .Include(n => n.Source)
                   .OrderByDescending(n => n.CreatedTime)
                   .Take(5)
                   .ToListAsync();

            var latestNewsDto = latestNews.Select(n => new
            {
                NewsId = n.NewsId,
                Heading = n.HeadingMulti,
                Description = n.DescriptionMulti,
                SourceName = n.Source.SourceNameMulti,
                CreatedTimeAgo = GetTimeAgo(n.CreatedTime)
            });

            return Json(latestNewsDto);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<IActionResult> Settings()
        {
            var userId = GetUserId(); // Kullanıcı ID'sini al
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }


            var model = new ProfileSettingsViewModel
            {
                UserId = user.UserId,
                Language = user.Language,
                ProfilePicturePath = user.ProfilePicturePath,
                NotificationEnabled = user.NotificationEnabled,
                Languages = new List<SelectListItem>
            {
                new SelectListItem { Value = "en", Text = "English", Selected = user.Language == "en" },
                new SelectListItem { Value = "tr", Text = "Türkçe", Selected = user.Language == "tr" }
            },
                NotificationOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "true", Text = "Yes", Selected = user.NotificationEnabled },
                new SelectListItem { Value = "false", Text = "No", Selected = !user.NotificationEnabled }
            },
                Countries = _context.Countries
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(),
                    Text = c.CountryName
                }).ToList(),
                Cities = _context.Cities.Where(a => a.CountryId == user.CountryId)
                .Select(c => new SelectListItem
                {
                    Value = c.CityId.ToString(),
                    Text = c.CityNameMulti
                }).ToList(),
                CityId = user.CityId,
                CountryId = user.CountryId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Settings(ProfileSettingsViewModel model)
        {
            var userId = GetUserId();
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            model.UserId = (int)userId;
            ModelState.Remove("Languages");
            ModelState.Remove("NotificationOptions");
            ModelState.Remove("ProfilePicture");
            ModelState.Remove("Countries");
            ModelState.Remove("Cities");
            ModelState.Remove("ProfilePicturePath");
            if (!ModelState.IsValid)
            {
                model.Languages = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "en", Text = "English", Selected = model.Language == "en" },
                        new SelectListItem { Value = "tr", Text = "Türkçe", Selected = model.Language == "tr" }
                    };
                model.NotificationOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Value = "true", Text = "Yes", Selected = model.NotificationEnabled },
                        new SelectListItem { Value = "false", Text = "No", Selected = !model.NotificationEnabled }
                    };
                model.Cities = _context.Cities.Where(a => a.CountryId == user.CountryId)
                .Select(c => new SelectListItem
                {
                    Value = c.CityId.ToString(),
                    Text = c.CityNameMulti
                }).ToList();
                model.CityId = user.CityId;
                model.CountryId = user.CountryId;
                model.Countries = _context.Countries
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(),
                    Text = c.CountryName
                }).ToList();
                model.ProfilePicturePath = user.ProfilePicturePath;
                return View(model);
            }



            var acceptedContentTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/tiff" };
            var acceptedFileExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif" };

            if (model.ProfilePicture != null && acceptedContentTypes.Contains(model.ProfilePicture.ContentType.ToLower()))
            {
                var fileExtension = Path.GetExtension(model.ProfilePicture.FileName).ToLower();

                if (acceptedFileExtensions.Contains(fileExtension))
                {
                    var fileName = $"{Guid.NewGuid()}{fileExtension}";
                    var filePath = Path.Combine(_environment.WebRootPath, "img", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfilePicture.CopyToAsync(fileStream);
                    }

                    user.ProfilePicturePath = Path.Combine("img", fileName);
                }
                else
                {
                    // Hata mesajı: Geçersiz dosya uzantısı
                    ModelState.AddModelError("ProfilePicture", "Geçersiz dosya uzantısı. Sadece .jpg, .jpeg, .png, .gif, .bmp, .tiff dosyaları kabul edilir.");
                }
            }
            if (!ModelState.IsValid)
                return RedirectToAction("Settings");

            user.Language = model.Language;
            user.NotificationEnabled = model.NotificationEnabled;
            user.UpdatedAt = DateTime.UtcNow;
            user.CountryId = model.CountryId;
            user.CityId = model.CityId;

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(user.Language)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Settings");
        }

        [HttpGet]
        public IActionResult GetUserProfilePicturePath()
        {
            var userId = GetUserId(); // Kullanıcı ID'sini al
            var user = _context.Users.Find(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Json(new { profilePicturePath = user.ProfilePicturePath });
        }
        [HttpGet]
        public async Task<IActionResult> UserInterestSettings()
        {
            var userId = GetUserId(); // Kullanıcı ID'sini al

            var categories = await _context.Categories
                .Where(c => !c.Deleted)
                .Select(c => new CategoryUserInterestViewModel
                {
                    CategoryId = c.CategoryId,
                    CategoryName = c.CategoryNameEN
                }).ToListAsync();

            var selectedCategories = await _context.UserInterests
                .Where(ui => ui.UserId == userId && !ui.Deleted)
                .Select(ui => ui.CategoryId)
                .ToListAsync();

            foreach (var category in categories)
            {
                category.IsSelected = selectedCategories.Contains(category.CategoryId);
            }

            var model = new UserInterestSettingsViewModel
            {
                Categories = categories
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserInterestSettings(UserInterestSettingsViewModel model)
        {
            var userId = GetUserId(); // Kullanıcı ID'sini al

            // Mevcut kullanıcı ilgi alanlarını al
            var existingInterests = await _context.UserInterests
                .Where(ui => ui.UserId == userId && !ui.Deleted)
                .ToListAsync();

            // Mevcut kullanıcı ilgi alanlarını sil (soft delete)
            if (existingInterests != null)
            {
                foreach (var interest in existingInterests)
                {
                    interest.Deleted = true;
                }
                await _context.SaveChangesAsync();
            }

            // Yeni ilgi alanlarını ekle
            foreach (var categoryId in model.SelectedCategoryIds)
            {
                var userInterest = new UserInterest
                {
                    UserId = (int)userId,
                    CategoryId = categoryId,
                    Deleted = false
                };

                _context.UserInterests.Add(userInterest);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("UserInterestSettings");
        }

        [HttpGet]
        public IActionResult GetCurrentCulture()
        {
            return Json(new { currentCulture = CultureInfo.CurrentCulture.Name });
        }

        [NonAction]
        private int? GetUserId()
        {
            // Kullanıcı ID'sini al
            var username = User.Identity.Name;
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            return user?.UserId;
        }
        [NonAction]

        private static string GetTimeAgo(DateTime createdTime)
        {
            var timeSpan = DateTime.UtcNow - createdTime;
            if (timeSpan.TotalMinutes < 1) return "just now";
            if (timeSpan.TotalHours < 1) return $"{timeSpan.Minutes}m ago";
            if (timeSpan.TotalDays < 1) return $"{timeSpan.Hours}h ago";
            return $"{timeSpan.Days}d ago";
        }
    }
}