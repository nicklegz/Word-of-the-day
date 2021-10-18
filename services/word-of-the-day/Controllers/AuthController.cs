using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace word_of_the_day.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("api/")]
    public class AuthController : Controller
    {
        [HttpGet]
        [Route("[controller]/login")]
        public ActionResult Login(string returnUrl = "http://localhost:4200/home")
        {
            return new ChallengeResult("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        [HttpGet]
        [Authorize]
        [Route("[controller]/logout")]
        public ActionResult Logout()
        {
            return new SignOutResult("Auth0", new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            });
        }

        [HttpGet]
        [Route("[controller]/user")]
        public ActionResult GetUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claims = ((ClaimsIdentity)this.User.Identity).Claims.Select(c =>
                            new { type = c.Type, value = c.Value })
                            .ToArray();

            return Json(new { isAuthenticated = true, claims = claims });
            }

            return Json(new { isAuthenticated = false });
        }
    }
}