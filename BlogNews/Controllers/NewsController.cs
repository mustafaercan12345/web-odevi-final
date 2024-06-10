using BlogNews.Models.Database;
using BlogNews.Models.Database.EntityModel;
using BlogNews.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static BlogNews.Models.ViewModels.NewsSearchListViewModel;

namespace BlogNews.Controllers
{
    public class NewsController : Controller
    {

        private readonly ILogger<NewsController> _logger;
        private readonly DataContext _context;

        public NewsController(ILogger<NewsController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("News/{newsId:int}")]
        public async Task<IActionResult> Index(int newsId)
        {
            var news = await _context.News
                .Include(n => n.Source)
                .Include(n => n.Category)
                .Where(n => n.NewsId == newsId && !n.Deleted)
                .FirstOrDefaultAsync();

            if (news == null)
            {
                return NotFound();
            }
            var likeNumber = 0;
            var dislikeNumber = 0;

            if (User.Identity.IsAuthenticated)
            {
                likeNumber = await _context.Rates
                .Where(r => r.NewsId == newsId && r.RateDef.Value == "Like" && !r.Deleted)
                  .CountAsync();

                dislikeNumber = await _context.Rates
                    .Where(r => r.NewsId == newsId && r.RateDef.Value == "Dislike" && !r.Deleted)
                    .CountAsync();
            }

            // Kullanıcının user interestlerini çek.
            var userId = GetUserId();
            var topCategoriesUserInterest = _context.UserInterests.Where(a => a.UserId == userId && !a.Deleted).Select(a => a.CategoryId).ToList();
            var topCategories = await _context.Rates
                .Where(r => r.UserId == userId && r.RateDef.Value == "Like" && !r.Deleted)
                .GroupBy(r => r.News.CategoryId)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .Select(g => g.Key)
                .ToListAsync();

            List<NewsForYouViewModel> newsForYouList;

            if (topCategoriesUserInterest == null || !topCategoriesUserInterest.Any())
            {
                // Eğer topCategories boşsa, tüm haberler içerisindeki son 6 haberi al
                newsForYouList = await _context.News
                    .Where(n => !n.Deleted && n.NewsId != newsId)
                    .OrderByDescending(n => n.CreatedTime)
                    .Take(6)
                    .Select(n => new NewsForYouViewModel
                    {
                        NewsId = n.NewsId,
                        Heading = n.HeadingMulti,
                        PhotoPath = n.PhotoPath,
                        SourceName = n.Source.SourceName,
                        LikeNumber = n.Rates.Count(r => r.RateDef.Value == "Like" && !r.Deleted),
                        DislikeNumber = n.Rates.Count(r => r.RateDef.Value == "Dislike" && !r.Deleted)
                    })
                    .ToListAsync();
            }
            else
            {
                // En son eklenen 6 haberi al
                newsForYouList = await _context.News
                    .Where(n => topCategoriesUserInterest.Contains(n.CategoryId) && !n.Deleted && n.NewsId != newsId)
                    .OrderByDescending(n => n.CreatedTime)
                    .Take(6)
                    .Select(n => new NewsForYouViewModel
                    {
                        NewsId = n.NewsId,
                        Heading = n.HeadingMulti,
                        PhotoPath = n.PhotoPath,
                        SourceName = n.Source.SourceName,
                        LikeNumber = n.Rates.Count(r => r.RateDef.Value == "Like" && !r.Deleted),
                        DislikeNumber = n.Rates.Count(r => r.RateDef.Value == "Dislike" && !r.Deleted)
                    })
                    .ToListAsync();
            }


            var model = new NewsViewModel
            {
                NewsId = news.NewsId,
                Heading = news.HeadingMulti,
                PhotoPath = news.PhotoPath,
                Description = news.DescriptionMulti,
                SourceName = news.Source.SourceNameMulti,
                SourceId = news.SourceId,
                CategoryName = news.Category.CategoryNameMulti,
                CategoryId = news.CategoryId,
                CreatedTimeAgo = GetTimeAgo(news.CreatedTime),
                LikeNumber = likeNumber,
                DislikeNumber = dislikeNumber,
                NewsForYouList = newsForYouList
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Like(int newsId)
        {
            var userId = GetUserId(); // Kullanıcı ID'sini al
            if (userId == null)
            {
                return Unauthorized();
            }
            // RateDef tablosundan "Like" olan değeri al
            var likeRateDef = _context.RateDefs.FirstOrDefault(rd => rd.Value == "Like");
            if (likeRateDef == null)
            {
                return BadRequest("Like definition not found.");
            }

            // Var olan beğeni veya beğenmeme kaydını kontrol et
            var existingRate = _context.Rates
                .FirstOrDefault(r => r.NewsId == newsId && r.UserId == userId.Value && !r.Deleted);

            if (existingRate != null)
            {
                existingRate.Deleted = true;
                _context.Rates.Update(existingRate);
                _context.SaveChanges();
                if (existingRate.RateDefId == likeRateDef.RateDefId)
                    return Ok();
            }

            var rate = new Rate
            {
                NewsId = newsId,
                UserId = userId.Value,
                RateDefId = likeRateDef.RateDefId,
                Deleted = false
            };

            _context.Rates.Add(rate);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public IActionResult Dislike(int newsId)
        {
            var userId = GetUserId(); // Kullanıcı ID'sini al
            if (userId == null)
            {
                return Unauthorized();
            }
            // RateDef tablosundan "Dislike" olan değeri al
            var dislikeRateDef = _context.RateDefs.FirstOrDefault(rd => rd.Value == "Dislike");
            if (dislikeRateDef == null)
            {
                return BadRequest("Dislike definition not found.");
            }

            // Var olan beğeni veya beğenmeme kaydını kontrol et
            var existingRate = _context.Rates
                .FirstOrDefault(r => r.NewsId == newsId && r.UserId == userId.Value && !r.Deleted);

            if (existingRate != null)
            {
                existingRate.Deleted = true;
                _context.Rates.Update(existingRate);
                _context.SaveChanges();

                if (existingRate.RateDefId == dislikeRateDef.RateDefId)
                    return Ok();
            }



            var rate = new Rate
            {
                NewsId = newsId,
                UserId = userId.Value,
                RateDefId = dislikeRateDef.RateDefId,
                Deleted = false
            };

            _context.Rates.Add(rate);
            _context.SaveChanges();

            return Ok();
        }
        [HttpGet]
        [Route("News/GetRates/{newsId:int}")]
        public async Task<IActionResult> GetRates(int newsId)
        {
            var likeNumber = await _context.Rates
            .Where(r => r.NewsId == newsId && r.RateDef.Value == "Like" && !r.Deleted)
            .CountAsync();

            var dislikeNumber = await _context.Rates
                .Where(r => r.NewsId == newsId && r.RateDef.Value == "Dislike" && !r.Deleted)
                .CountAsync();

            return Json(new { Like = likeNumber, Dislike = dislikeNumber });
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchText, int? categoryId)
        {
            var newsQuery = _context.News
                .Include(n => n.Source)
                .OrderByDescending(n => n.CreatedTime)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchText))
            {
                newsQuery = newsQuery.Where(n => n.Heading.Contains(searchText) || n.HeadingEN.Contains(searchText) || n.Description.Contains(searchText) || n.DescriptionEN.Contains(searchText));
            }

            if (categoryId.HasValue)
            {
                newsQuery = newsQuery.Where(n => n.CategoryId == categoryId.Value);
            }

            var newsList = await newsQuery
                .Select(n => new NewsSearchViewModel
                {
                    NewsId = n.NewsId,
                    CategoryId = n.CategoryId,
                    Heading = n.HeadingMulti,
                    CreatedTimeAgo = GetTimeAgo(n.CreatedTime),
                    PhotoPath = n.PhotoPath,
                    SourceName = n.Source.SourceNameMulti,
                    NotRelatedToCategory = false
                })
                .Take(6)
                .ToListAsync();

            if (newsList.Count < 6)
            {
                if (categoryId.HasValue)
                {
                    var additionalNewsCategory = await _context.News
                    .Include(n => n.Source)
                    .Where(n => !newsList.Select(x => x.NewsId).Contains(n.NewsId) && n.CategoryId == (int)categoryId)
                    .OrderByDescending(n => n.CreatedTime)
                    .Select(n => new NewsSearchViewModel
                    {
                        NewsId = n.NewsId,
                        CategoryId = n.CategoryId,
                        Heading = n.HeadingMulti,
                        CreatedTimeAgo = GetTimeAgo(n.CreatedTime),
                        PhotoPath = n.PhotoPath,
                        SourceName = n.Source.SourceNameMulti,
                        NotRelatedToCategory = false
                    })
                    .Take(6 - newsList.Count)
                    .ToListAsync();

                    newsList.AddRange(additionalNewsCategory);
                }

            }
            if (newsList.Count < 6)
            {
                var additionalNews = await _context.News
               .Include(n => n.Source)
               .Where(n => !newsList.Select(x => x.NewsId).Contains(n.NewsId))
               .OrderByDescending(n => n.CreatedTime)
               .Select(n => new NewsSearchViewModel
               {
                   NewsId = n.NewsId,
                   CategoryId = n.CategoryId,
                   Heading = n.HeadingMulti,
                   CreatedTimeAgo = GetTimeAgo(n.CreatedTime),
                   PhotoPath = n.PhotoPath,
                   SourceName = n.Source.SourceNameMulti,
                   NotRelatedToCategory = true 
               })
             .Take(6 - newsList.Count)
             .ToListAsync();
                newsList.AddRange(additionalNews);
            }

            var categories = await _context.Categories
            .Select(c => new CategorySearchViewModel
            {
                Id = c.CategoryId,
                Name = c.CategoryNameMulti
            })
            .ToListAsync();
            if (categoryId != null)
                newsList = newsList.OrderByDescending(a => a.CategoryId == (int)categoryId).ToList();
            var model = new NewsSearchListViewModel
            {
                SearchedText = searchText,
                NewsSearchList = newsList,
                Categories = categories,
                SearchedCategoryId = categoryId == null ? 0 : (int)categoryId
            };

            return View(model);
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
        private static string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{timeSpan.TotalMinutes:F0} minutes ago";
            if (timeSpan.TotalHours < 24)
                return $"{timeSpan.TotalHours:F0} hours ago";
            if (timeSpan.TotalDays < 7)
                return $"{timeSpan.TotalDays:F0} days ago";
            if (timeSpan.TotalDays < 30)
                return $"{timeSpan.TotalDays / 7:F0} weeks ago";
            if (timeSpan.TotalDays < 365)
                return $"{timeSpan.TotalDays / 30:F0} months ago";

            return $"{timeSpan.TotalDays / 365:F0} years ago";
        }
    }
}
