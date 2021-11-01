using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using word_of_the_day.Models;
using System;

namespace word_of_the_day.Controllers
{
    [ApiController]
    [EnableCors]
    [Route("api/")]
    public class AuthController : Controller
    {
        private readonly WordOfTheDayContext _context;
        private static readonly Random random = new Random();


        public AuthController(WordOfTheDayContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("[controller]/login")]
        public ActionResult Login(string returnUrl = "http://localhost:4200")
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
                
                User user =  await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
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
            var userId = UserExtension.GetUserId(this.User);
            if(userId != "")
            {
                var availableWords = await _context.Words.ToListAsync();
                var wordCount = availableWords.Count();
                var index = random.Next(0, wordCount - 1);

                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    LastUpdated = DateTime.Now,
                    WordOfTheDayId = availableWords[index].WordId
                };

                var newUser = await _context.Users.AddAsync(user);
                _context.SaveChanges();
            }

            else{
                throw new Exception("User does not exist");  
            }
        }
    }
}