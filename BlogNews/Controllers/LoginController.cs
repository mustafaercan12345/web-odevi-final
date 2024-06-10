using BlogNews.Models.Database;
using BlogNews.Models.Database.EntityModel;
using BlogNews.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlogNews.Controllers
{
    public class LoginController:Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly DataContext _context;

        public LoginController(ILogger<LoginController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                Countries = _context.Countries
                    .Select(c => new SelectListItem
                    {
                        Value = c.CountryId.ToString(),
                        Text = c.CountryNameMulti
                    }).ToList(),

                Cities = new List<SelectListItem>()
            };

            return View(model);
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(u => u.Email == model.LoginEmail);

                if (user != null && VerifyPasswordHash(model.LoginPassword, user.PasswordHash, user.PasswordSalt))
                {
                    // Login successful, sign in the user
                    var claims = new List<Claim>
                                            {
                                                new Claim(ClaimTypes.Name, user.Username),
                                                new Claim(ClaimTypes.Email, user.Email)
                                            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Redirect to home or dashboard
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ModelState.Remove("Countries");
            ModelState.Remove("Cities");
            if (ModelState.IsValid)
            {
                bool mailCheck = _context.Users.Any(a => a.Email == model.Email && !a.Deleted);
                if(mailCheck)
                    return RedirectToAction("Register", "Login",model);
                
                CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var user = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    CountryId = model.CountryId,
                    CityId = model.CityId,
                    Language = "EN",
                    NotificationEnabled = true,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    ProfilePicturePath="img/unknownpp.jpg"
                };

                _context.Add(user);
                await _context.SaveChangesAsync();

                // Login successful, sign in the user
                var claims = new List<Claim>
                                            {
                                                new Claim(ClaimTypes.Name, user.Username),
                                                new Claim(ClaimTypes.Email, user.Email)
                                            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                // Redirect to home or dashboard
                return RedirectToAction("UserInterestSettings", "Home");
            }

            model.Countries = _context.Countries
                .Select(c => new SelectListItem
                {
                    Value = c.CountryId.ToString(),
                    Text = c.CountryName
                }).ToList();

            model.Cities = _context.Cities
                .Where(c => c.CountryId == model.CountryId)
                .Select(c => new SelectListItem
                {
                    Value = c.CityId.ToString(),
                    Text = c.CityName
                }).ToList();

            return View(model);
        }

        [HttpGet]
        public IActionResult GetCountries()
        {
            var countries = _context.Countries
                .Select(c => new { c.CountryId, c.CountryNameMulti })
                .ToList();

            return Json(countries);
        }

        [HttpGet]
        public IActionResult GetCities(int countryId)
        {
            var cities = _context.Cities
                .Where(c => c.CountryId == countryId)
                .Select(c => new { c.CityId, c.CityNameMulti })
                .ToList();

            return Json(cities);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }


        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }
    }
}
