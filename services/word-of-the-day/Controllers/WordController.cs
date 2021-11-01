using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using word_of_the_day.Models;

namespace word_of_the_day.Controllers
{
    [ApiController]
    [Route("api/")]
    public class WordController : ControllerBase
    {
        private static readonly Random random = new Random();
        private readonly WordOfTheDayContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Uri _apiEndpoint;

        public WordController(WordOfTheDayContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;  

            if(configuration["WordApiEndpoint"] == null)
                throw new ArgumentNullException("The Word Api Endpoint is missing from the configuration");

            _apiEndpoint = new Uri(configuration["WordApiEndpoint"], UriKind.Absolute);
            _context = context;
        }

        [HttpGet("[controller]")]
        [Authorize]    
        [RequiredScope("read:word")]
        public async Task<ActionResult<IEnumerable<Word>>> GetListWords()
        {
            var words = await _context.Words.ToListAsync();
            if(words == null)
            {
                return NotFound();
            }

            return Ok(words);
        }

        [HttpGet("[controller]/word-of-the-day")]
        [Authorize] 
        [RequiredScope("read:word")]
        public async Task<ActionResult<Word>> GetWordOfTheDay()
        {
            var userId = UserExtension.GetUserId(this.User);
            User user =  await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            if(user == null)
                return NotFound();

            TimeSpan diff = DateTime.Now - user.LastUpdated;
            if(diff.Hours < 23){
                var wordOfTheDay = _context.Words.FirstOrDefault(x => x.WordId == user.WordOfTheDayId);
                return wordOfTheDay;
            }

            var query = from word in _context.Set<Word>()
                        from p in _context.Set<PreviouslyUsedWord>().Where(
                            p => word.WordId == p.WordId && 
                            p.UserId == user.UserId).DefaultIfEmpty()
                        where p == null
                        select word;

            var availableWords = await query.ToListAsync();

            int availableWordsCount = availableWords.Count();
            if(availableWordsCount < 1)
                return NoContent();

            int index = random.Next(0, availableWordsCount - 1);
            Word newWord = availableWords[index];

            return Ok(newWord);
        }

        // [HttpGet]
        // public async Task Get()
        // {
        //     var accessToken = await HttpContext.GetTokenAsync("Auth0", "access_token");

        //     var httpClient = _httpClientFactory.CreateClient();

        //     var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_apiEndpoint, "api/word"));
        //     request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //     var response = await httpClient.SendAsync(request);

        //     response.EnsureSuccessStatusCode();

        //     await response.Content.CopyToAsync(HttpContext.Response.Body);

        // }
    }
}