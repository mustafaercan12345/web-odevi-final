# BlogNews Application

## Overview
BlogNews is a news aggregation platform where users can search for news, manage their interests, and interact with news content. The application supports English and Turkish languages and provides a responsive user interface.

### Key Features:
- **Multi-language Support**: The application supports both English and Turkish languages, allowing users to switch between languages easily from the settings page.
- **Responsive Design**: Utilizes Bootstrap to ensure that the application is fully responsive and provides a seamless user experience across various devices, including desktops, tablets, and mobile phones.
- **User Authentication**: Secure login and registration system using email and password. Users can register with an email, password, and location details (country and city). Passwords are securely hashed and stored.
- **Personalized News Feed**: Users can manage their interest areas which affect the type of news displayed on their home page and under each news article in the "More for You" section.
- **Interactive News Content**: Logged-in users can like or dislike news articles, share articles on social media, and view detailed news content including headings and images.
- **Notification Settings**: Users can turn notifications on or off for top news updates.
- **Admin and User Management**: Provides robust management capabilities for both users and content, ensuring the platform remains up-to-date and relevant.

### Technical Implementation:
- **ASP.NET Core MVC**: The backend is built using ASP.NET Core MVC, ensuring a clean separation of concerns and a scalable architecture.
- **Entity Framework Core**: Used for data access, managing database operations, and migrations.
- **Microsoft SQL Server**: The database system used to store user data, news articles, and settings.
- **Razor Pages**: Used for the frontend, providing dynamic content rendering and interactive features.
- **Custom APIs**: Developed to handle various functionalities within the application, adhering to the separation of concerns principle.

### Data Models:
- **Category**: Represents news categories, supporting multiple languages.
- **City**: Represents city data, used for user registration and location-based content.
- **Country**: Represents country data, used for user registration and location-based content.
- **News**: Represents news articles, including headings, descriptions, images, and source information.
- **Rate**: Manages user interactions with news articles, such as likes and dislikes.
- **Source**: Represents the source of news articles.
- **User**: Manages user information, authentication details, and settings.
- **UserInterest**: Manages the categories of interest for each user, influencing the personalized news feed.

### ViewModels:
- **CategoryUserInterestViewModel**: Manages the user's interest categories.
- **HomeViewModel**: Represents data for the home page, including categories, news sliders, and personalized news.
- **LoginViewModel**: Handles login form data.
- **NewsSearchViewModel**: Manages search results for news articles.
- **NewsViewModel**: Represents detailed news content.
- **ProfileSettingsViewModel**: Manages user profile settings including language and notification preferences.
- **RegisterViewModel**: Handles registration form data.
- **UserInterestSettingsViewModel**: Manages user interest settings.

### Controllers:
- **HomeController**: Manages home page content, user settings, and notifications.
- **LoginController**: Handles user authentication, including login and registration.
- **NewsController**: Manages news content, including searching, liking/disliking, and displaying detailed news articles.

### Personalized News
Based on user interests, personalized news suggestions are displayed.
### Search Capability by News Contents
Users can search for news by content.
- **Implementation**: `NewsController.cs` - `Search` method
```csharp
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
        .ToListAsync();

    return View(new NewsSearchListViewModel
    {
        SearchedText = searchText,
        NewsSearchList = newsList,
        Categories = await _context.Categories.Select(c => new NewsSearchListViewModel.CategorySearchViewModel
        {
            Id = c.CategoryId,
            Name = c.CategoryNameMulti
        }).ToListAsync()
    });
}
```

### Interest Management
Users can manage their interest areas, which will affect the news displayed on the home page and under each news content.
- **Implementation**: `HomeController.cs` - `UserInterestSettings` method
```csharp
[HttpGet]
public async Task<IActionResult> UserInterestSettings()
{
    var userId = GetUserId();
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
    var userId = GetUserId();

    var existingInterests = await _context.UserInterests
        .Where(ui => ui.UserId == userId && !ui.Deleted)
        .ToListAsync();

    foreach (var interest in existingInterests)
    {
        interest.Deleted = true;
    }
    await _context.SaveChangesAsync();

    foreach (var categoryId in model.SelectedCategoryIds)
    {
        var userInterest = new UserInterest
        {
            UserId = userId.Value,
            CategoryId = categoryId,
            Deleted = false
        };

        _context.UserInterests.Add(userInterest);
    }

    await _context.SaveChangesAsync();

    return RedirectToAction("UserInterestSettings");
}

```

### User Interactions
Like/Dislike
Users can like or dislike news content.
- **Implementation**: `NewsController.cs` - `Like` and `Dislike` method
```csharp
[HttpPost]
public IActionResult Like(int newsId)
{
    var userId = GetUserId();
    if (userId == null)
    {
        return Unauthorized();
    }

    var rate = new Rate
    {
        NewsId = newsId,
        UserId = userId.Value,
        RateDefId = _context.RateDefs.First(rd => rd.Value == "Like").RateDefId,
        Deleted = false
    };

    _context.Rates.Add(rate);
    _context.SaveChanges();

    return Ok();
}

[HttpPost]
public IActionResult Dislike(int newsId)
{
    var userId = GetUserId();
    if (userId == null)
    {
        return Unauthorized();
    }

    var rate = new Rate
    {
        NewsId = newsId,
        UserId = userId.Value,
        RateDefId = _context.RateDefs.First(rd => rd.Value == "Dislike").RateDefId,
        Deleted = false
    };

    _context.Rates.Add(rate);
    _context.SaveChanges();

    return Ok();
}

```

### Notification and Language Settings 
Users can turn on/off notifications for top news.
- **Implementation**: `HomeController.cs` - `Settings` method
```csharp
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

    user.NotificationEnabled = model.NotificationEnabled;
    user.Language = model.Language;
    Response.Cookies.Append(
        CookieRequestCultureProvider.DefaultCookieName,
        CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(user.Language)),
        new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
    );
    _context.Users.Update(user);
    await _context.SaveChangesAsync();

    return RedirectToAction("Settings");
}


```

### Login with Email/Password
Users can search for news by content.
- **Implementation**: `LoginController.cs` - `Login` method
```csharp
[HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
{
    if (ModelState.IsValid)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.LoginEmail);
        if (user != null && VerifyPasswordHash(model.LoginPassword, user.PasswordHash, user.PasswordSalt))
        {
            // Authentication logic here
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
    }
    return View(model);
}


```

### News Details
Clicking on a news link displays the news heading and body image.
- **Implementation**: `NewsController.cs` - `Index` method
```csharp
[Route("News/{newsId:int}")]
public async Task<IActionResult> Index(int newsId)
{
    var news = await _context.News
        .Include(n => n.Source)
        .Include(n => n.Category)
        .FirstOrDefaultAsync(n => n.NewsId == newsId);

    if (news == null)
    {
        return NotFound();
    }

    var model = new NewsViewModel
    {
        NewsId = news.NewsId,
        Heading = news.HeadingMulti,
        PhotoPath = news.PhotoPath,
        Description = news.DescriptionMulti,
        SourceName = news.Source.SourceNameMulti,
        CategoryName = news.Category.CategoryNameMulti
    };

    return View(model);
}


```
