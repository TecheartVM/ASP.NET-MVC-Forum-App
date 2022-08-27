using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_test.Data;
using MVC_test.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace MVC_test.Controllers
{
    public class AccountController : Controller
    {
        private readonly ForumDbContext _db;

        public AccountController(ForumDbContext dbContext)
        {
            _db = dbContext;
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            string? username = User?.Identity?.Name;
            if (username == null)
                return RedirectToAction("Login");

            User? user = await _db.Users.FirstOrDefaultAsync(u => u.Name == username);
            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpGet]
        public IActionResult Register()
        {
            IIdentity? id = User?.Identity;
            if (id != null && id.IsAuthenticated)
                return RedirectToAction("Profile");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationData data)
        {
            if (ModelState.IsValid)
            {
                User? user = await _db.Users.FirstOrDefaultAsync(x => x.Email == data.Email || x.Name == data.Username);
                if (user == null)
                {
                    user = new User { Email = data.Email, Name = data.Username, Password = data.Password };
                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();
                    await Authenticate(user.Name);
                    return RedirectToAction("Profile", "Account");
                }
                else
                {
                    if(user.Email == data.Email)
                        ModelState.AddModelError("", "Email is already in use");
                    if(user.Name == data.Username)
                        ModelState.AddModelError("", "Username is already in use");
                }
            }
            return View(data);
        }

        [HttpGet]
        public IActionResult Login()
        {
            IIdentity? id = User?.Identity;
            if (id != null && id.IsAuthenticated)
                return RedirectToAction("Profile");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginData data)
        {
            if (ModelState.IsValid)
            {
                User? user = await _db.Users.FirstOrDefaultAsync(x => x.Email == data.Email && x.Password == data.Password);
                if (user != null)
                {
                    await Authenticate(user.Name);
                    return RedirectToAction("Index", "Home");
                }
                else ModelState.AddModelError("", "Email and(or) password was incorrect");
            }
            return View(data);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        private async Task Authenticate(string userName)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            ClaimsIdentity ci = new ClaimsIdentity(claims, "ApplicationCookie");
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci));
        }
    }
}
