using System.Security.Claims;
using AuthorizationServer.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Controllers;

public class AccountController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        // Serves login form we just created
        // query parameter used to redirect user after successful login
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        ViewData["ReturnUrl"] = model.ReturnUrl;

        // ModelState is validated i.e. username and password required.
        // Any combination of credentials is accepted here. Use this chance to check credentials against database
        if (ModelState.IsValid)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username)
            };

            // create claims identity; adding one claim - name of user.
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // SignInAsync - an extension method that call the AuthenticationService which
            // class the CookieAuthenticationHandler because that's scheme that was specified
            // when creating claims identity.
            // Performs the sign in
            await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));

            // check that specified url is a local url to prevent open redirect attacks before redirecting user.
            if (Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home" );
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        // calls AuthenticationService to log out user. AuthenticationService calls the
        // authentication middleware, in this case, the cookie authentication middleware to 
        // sign out the user
        await HttpContext.SignOutAsync();
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }
}
