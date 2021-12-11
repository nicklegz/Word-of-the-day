using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using word_of_the_day.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace word_of_the_day.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AuthController : Controller
    {
        private static readonly Random random = new Random();
        private readonly IUserRepository _userRepo;
        private readonly IWordRepository _wordRepo;

        public AuthController(IUserRepository userRepo, IWordRepository wordRepo)
        {
            _userRepo = userRepo;
            _wordRepo = wordRepo;
        }

        // [Route("[controller]/login")]
        // public async Task<ActionResult> Login(string username)
        // {
        //     User user = await _userRepo.GetUserAsync(username);
        //     if(user == null)
        //     {
        //         return NotFound();
        //     }

        //     return Ok();
        // }

        // [Authorize]
        // [Route("[controller]/logout")]
        // public async Task Logout()
        // {
        // }

        [HttpGet]
        [Route("[controller]/user/{username}")]
        public async Task<ActionResult> GetUser(string userName)
        {
            bool createUser = false;
    
            User user =  await _userRepo.GetUserAsync(userName);
            if(user == null)
            {
                createUser = true;
            }

            else
            {
                createUser = false;
            }
                
            return Json(new { createUser });
        }

        [HttpPost]
        [Route("[controller]/user/{username}")]
        public async Task CreateUser(string userName)
        {
            List<Word> availableWords = await _wordRepo.GetListOfWordsAsync();
            int wordCount = availableWords.Count();
            int newWordId = _wordRepo.GetNewWordOfTheDay(availableWords, wordCount).WordId;
            await _userRepo.AddUserAsync(userName, newWordId);
        }
    }
}