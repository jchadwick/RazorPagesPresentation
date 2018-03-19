using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Globalization;

namespace TopsyTurvyCakes.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        [Required]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public IActionResult OnPost()
        {
            var isValidUser =
                   EmailAddress == "admin@topsyturvycakedesign.com"
                && Password == "topsecret!";

            if(!isValidUser) {
                ModelState.AddModelError("", "Invalid username or password!");
            }

            if(!ModelState.IsValid)
            {
                return Page();
            }

            var username = EmailAddress.Substring(0, EmailAddress.IndexOf('@'));
            var name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(username);
            var scheme = CookieAuthenticationDefaults.AuthenticationScheme;

            var user = new ClaimsPrincipal(
                new ClaimsIdentity(
                        new [] { new Claim(ClaimTypes.Name, name) },
                        scheme
                    )
                );

            return SignIn(user, scheme);
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToPage("/Index");
        }
    }
}