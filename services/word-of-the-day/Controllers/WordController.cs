using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly Uri _apiEndpoint;
        private readonly IUserExtension _userExtension;
        private readonly IWordExtension _wordExtension;
        private readonly int wordTimeInterval = 24;

        public WordController(
            IConfiguration configuration,
            IUserExtension userExtension,
            IWordExtension wordExtension)
        {
            // if(configuration["WordApiEndpoint"] == null)
            //     throw new ArgumentNullException("The Word Api Endpoint is missing from the configuration");
            
            // _apiEndpoint = new Uri(configuration["WordApiEndpoint"], UriKind.Absolute);
            _userExtension = userExtension;
            _wordExtension = wordExtension;
        }

        [HttpGet("[controller]")]
        // [Authorize] 
        // [RequiredScope("read:word")]
        public async Task<ActionResult<List<Word>>> GetListWords()
        {
            var words = await _wordExtension.GetListOfWordsAsync();

            if(words == null){
                return NotFound();
            }

            return Ok(words);
        }

        [HttpGet("[controller]/word-of-the-day/{userName}")]
        // [Authorize] 
        // [RequiredScope("read:word")]
        public async Task<ActionResult<Word>> GetWordOfTheDay(string username)
        {
            User user = await GetUserAsync(username);

            if(user == null)
                return NotFound($"User {username} does not exist.");

            if(IsNewWordRequired(user))
                return await GetExistingWordAsync(user);

            var availableWords = await GetListAvailableWords(user);

            int availableWordsCount = availableWords.Count();
            if(availableWordsCount < 1)
                return NoContent();

            Word newWord = GetNewWordOfTheDay(availableWords, availableWordsCount);

            return Ok(newWord);
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await _userExtension.GetUserAsync(username);
        }

        public async Task<Word> GetExistingWordAsync(User user)
        {
            return await _wordExtension.GetExistingWordOfTheDayAsync(user);
        }

        public Boolean IsNewWordRequired(User user)
        {
            TimeSpan diff = DateTime.Now - user.LastUpdated;
            if(diff.Hours < 24)
            {
                return true;
            }

            return false;
        }

        public async Task<List<Word>> GetListAvailableWords(User user)
        {
            return await _wordExtension.GetListAvailableWordsAsync(user);
        }

        public Word GetNewWordOfTheDay(List<Word> availableWords, int availableWordsCount)
        {
            return _wordExtension.GetNewWordOfTheDay(availableWords, availableWordsCount);
        }
    }
}