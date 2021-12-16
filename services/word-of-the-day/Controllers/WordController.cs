using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using word_of_the_day.Interfaces;

namespace word_of_the_day.Controllers
{
    [ApiController]
    [Route("api/")]
    public class WordController : ControllerBase
    {
        private static readonly Random random = new Random();
        private readonly IUserRepository _userRepo;
        private readonly IWordRepository _wordRepo;

        public WordController(IUserRepository userRepo, IWordRepository wordRepo)
        {
            _userRepo = userRepo;
            _wordRepo = wordRepo;
        }

        [HttpGet("[controller]")]
        // [Authorize] 
        // [RequiredScope("read:word")]
        public async Task<ActionResult<List<Word>>> GetListWords()
        {
            var words = await _wordRepo.GetListOfWordsAsync();

            if(words == null){
                return NotFound();
            }

            return Ok(words);
        }

        [HttpGet("[controller]/word-of-the-day/{username}")]
        // [Authorize] 
        // [RequiredScope("read:word")]
        public async Task<ActionResult<Word>> GetWordOfTheDay(string username)
        {
            User user = await _userRepo.GetUserAsync(username);

            if(user == null)
                return NotFound($"User {username} does not exist.");

            if(_wordRepo.IsNewWordRequired(user) == false)
                return await _wordRepo.GetExistingWordOfTheDayAsync(user);

            var availableWords = await _wordRepo.GetListAvailableWordsAsync(user);

            int availableWordsCount = availableWords.Count();
            if(availableWordsCount < 1)
                return NoContent();

            Word newWord = _wordRepo.GetNewWordOfTheDay(availableWords, availableWordsCount);

            //update User with new Word of the day
            user.LastUpdated = DateTime.Now;
            user.WordOfTheDayId = newWord.WordId;
            await _userRepo.UpdateUserAsync(user);
            await _wordRepo.AddPreviouslyUsedWordAsync(username, newWord.WordId);

            return Ok(newWord);
        }        

        [HttpGet("[controller]/previously-used-words/{username}")]
        public async Task<List<Word>> GetPreviouslyUsedWords(string username)
        {
            return await _wordRepo.GetPreviouslyUsedWordsAsync(username);
        }

        [HttpGet("[controller]/liked-words/{username}")]
        public async Task<List<Word>> GetLikedWords(string username)
        {
            return await _wordRepo.GetLikedWordsAsync(username);
        }

        [HttpGet("[controller]/liked-words/{username}/{wordId}")]
        public async Task<bool> GetIsLikedWordOfTheDay(string username, int wordId)
        {
            return await _wordRepo.GetIsLikedWordOfTheDayAsync(username, wordId);
        }

        [HttpPost("[controller]/liked-words/{username}")]
        public async Task<IActionResult> AddLikedWord(string username, [FromBody] int wordId)
        {
            await _wordRepo.AddLikedWordAsync(username, wordId);
            return Ok();
        }

        [HttpDelete("[controller]/liked-words/{username}/{wordId}")]
        public async Task<IActionResult> DeleteLikedWord(string username, int wordId)
        {
            await _wordRepo.DeleteLikedWordAsync(username, wordId);
            return Ok();
        }
    }
}