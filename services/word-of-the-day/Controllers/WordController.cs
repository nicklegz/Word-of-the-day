using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    [EnableCors]
    [Route("api/")]
    public class WordController : ControllerBase
    {
        private static readonly Random random = new Random();
        private readonly WordOfTheDayContext _context;
        private readonly HttpClient _httpClient;
        private readonly Uri _apiEndpoint;

        public WordController(WordOfTheDayContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;

            if (configuration["WordApiEndpoint"] == null)
                throw new ArgumentNullException("The Word Api Endpoint is missing from the configuration");

            _apiEndpoint = new Uri(configuration["WordApiEndpoint"], UriKind.Absolute);
            _context = context;
        }

        [HttpGet("[controller]/getuser")]
        public async Task Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("Auth0", "access_token");

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(_apiEndpoint, "/api/word"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            await response.Content.CopyToAsync(HttpContext.Response.Body);
        }

        [HttpGet("[controller]")]
        [Authorize("read:word")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Word>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Word>>> GetListWords()
        {
            var words = await _context.Words.ToListAsync();
            if(words == null)
            {
                return NotFound();
            }

            return Ok(words);
        }

        [HttpGet("[controller]/{username}")]
        [Authorize("read:word")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Word))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Word>> GetNewWord(string username)
        {
            User user =  await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

            if(user == null)
                return NotFound();

            TimeSpan diff = DateTime.Now - user.LastUpdated;
            if(diff.Hours < 23)
                return _context.Words.FirstOrDefault(x => x.WordId == user.WordOfTheDayId);

            var query = from word in _context.Set<Word>()
                        from p in _context.Set<PreviouslyUsedWord>().Where(
                            p => word.WordId == p.WordId && 
                            p.UserId == user.UserId).DefaultIfEmpty()
                        where p == null
                        select word;

            var availableWords = await query.ToListAsync();

            int index = random.Next(0, availableWords.Count());
            Word newWord = availableWords[index];

            if(newWord == null)
                return NoContent();

            return Ok(newWord);
        }

        /*

        // POST /user
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ReadFileDTO>> CreateFile(CreateFileDTO fileDTO)
        {
            File file = new()
            {
                UserId = fileDTO.UserId,
                Name = fileDTO.Name,
                Path = ""
            };

            var newFile = await _extensions.CreateFile(file);

            return CreatedAtAction(nameof(GetFile), new { id = file.Id }, newFile);
        }*/
    }
}