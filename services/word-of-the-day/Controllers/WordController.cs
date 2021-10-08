using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using word_of_the_day.Models;

namespace word_of_the_day.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WordController : ControllerBase
    {
        private static readonly Random random = new Random();
        private readonly WordOfTheDayContext _context;

        public WordController(WordOfTheDayContext context)
        {
            _context = context;
        }

        //GET /words
        [HttpGet]
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

        [HttpGet("/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Word))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Word>> GetNewWord(Guid userId)
        {
            User user =  await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);

            if(user == null)
            {
                return NotFound();
            }

            var prevWords = await _context.PreviouslyUsedWords.Where(x => x.UserId == userId).ToListAsync();

            var query = from word in _context.Set<Word>()
                        from p in _context.Set<PreviouslyUsedWord>().Where(p => word.WordId == p.WordId).DefaultIfEmpty()
                        where p == null
                        select word;

            var result = await query.ToListAsync();


            //var availableWords = await _context.Words.FromSqlRaw("select \"Word\".\"WordId\",\"Word\".\"Definition\",\"Word\".\"WordText\" from \"Word\" left join \"PreviouslyUsedWord\" on \"Word\".\"WordId\" = \"PreviouslyUsedWord\".\"WordId\" where \"PreviouslyUsedWord\".\"WordId\" is null;").ToListAsync();
            // if(availableWords.Count() < 1)
            // {
            //     availableWords = await _context.Words.ToListAsync();
            // }    
            
            // int index = random.Next(0, availableWords.Count());
            // Word newWord = availableWords[index];

            // if(newWord == null)
            // {
            //     return NoContent();
            // }

            // return Ok(newWord);
            return Ok();
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