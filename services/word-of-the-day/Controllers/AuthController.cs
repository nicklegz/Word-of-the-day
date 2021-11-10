using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using word_of_the_day.Interfaces;
using System.Collections.Generic;

namespace word_of_the_day.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("api/")]
    public class AuthController : Controller
    {
        private static readonly Random random = new Random();
        private readonly IUserExtension _userExtension;
        private readonly IWordExtension _wordExtension;

        public AuthController(IUserExtension userExtension, IWordExtension wordExtension)
        {
            _userExtension = userExtension;
            _wordExtension = wordExtension;
        }

        [HttpGet]
        [Route("[controller]/login")]
        public ActionResult Login(string returnUrl = "https://worddujour.herokuapp.com")
        {
            return new ChallengeResult("Auth0", new AuthenticationProperties() { RedirectUri = returnUrl});
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
        public async Task<ActionResult> GetUser()
        {
            bool createUser = false;

            if (User.Identity.IsAuthenticated)
            {
                var claims = ((ClaimsIdentity)this.User.Identity).Claims.Select(c =>
                            new { type = c.Type, value = c.Value })
                            .ToArray();
                var userId = claims.Select(x => x.value).FirstOrDefault();
                
                User user =  await _userExtension.GetUserAsync(userId);
                if(user == null){
                    createUser = true;
                }
                
                return Json(new { isAuthenticated = true, claims = claims, createUser });
            }

            return Json(new { isAuthenticated = false });
        }

        [HttpPost]
        [Route("[controller]/user")]
        public async Task CreateUser()
        {
            var userId = _userExtension.GetUserId(this.User);
            
            if(userId != "")
            {
                List<Word> availableWords = await _wordExtension.GetListOfWordsAsync();
                int wordCount = availableWords.Count();
                int newWordId = _wordExtension.GetNewWordOfTheDay(availableWords, wordCount).WordId;
                await _userExtension.AddUserAsync(userId, newWordId);
            }

            else
            {
                throw new Exception("User does not exist");  
            }
        }
    }
}