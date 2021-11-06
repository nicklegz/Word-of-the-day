using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using word_of_the_day.Interfaces;

namespace word_of_the_day.Controllers
{
    [ApiController]
    [Route("api/")]
    public class WordController : ControllerBase
    {
        private static readonly Random random = new Random();
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Uri _apiEndpoint;
        private readonly IUserExtension _userExtension;
        private readonly IWordExtension _wordExtension;

        public WordController(
            IHttpClientFactory httpClientFactory, 
            IConfiguration configuration,
            IUserExtension userExtension,
            IWordExtension wordExtension)
        {
            if(configuration["WordApiEndpoint"] == null)
                throw new ArgumentNullException("The Word Api Endpoint is missing from the configuration");
            
            _httpClientFactory = httpClientFactory;  
            _apiEndpoint = new Uri(configuration["WordApiEndpoint"], UriKind.Absolute);
            _userExtension = userExtension;
            _wordExtension = wordExtension;
        }

        [HttpGet("[controller]")]
        [Authorize]    
        [RequiredScope("read:word")]
        public async Task<ActionResult<List<Word>>> GetListWords()
        {
            var words = await _wordExtension.GetListOfWordsAsync();

            if(words == null){
                return NotFound();
            }

            return Ok(words);
        }

        [HttpGet("[controller]/word-of-the-day")]
        [Authorize] 
        [RequiredScope("read:word")]
        public async Task<ActionResult<Word>> GetWordOfTheDay()
        {
            var userId = _userExtension.GetUserId(this.User);
            User user =  await _userExtension.GetUserAsync(userId);

            if(user == null)
                return NotFound();

            TimeSpan diff = DateTime.Now - user.LastUpdated;
            if(diff.Hours < 24)
                return await _wordExtension.GetExistingWordOfTheDayAsync(user);

            var availableWords = await _wordExtension.GetListAvailableWordsAsync(user);

            int availableWordsCount = availableWords.Count();
            if(availableWordsCount < 1)
                return NoContent();

            Word newWord = _wordExtension.GetNewWordOfTheDay(availableWords, availableWordsCount);

            return Ok(newWord);
        }
    }
}