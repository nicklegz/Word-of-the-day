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

        private IEnumerable<Word> GetWords()
        {
            List<Word> words = new List<Word>();
            words.Add(new Word {Id = 1, WordText = "Happy", Definition = "This is a definition"});
            words.Add(new Word {Id = 2, WordText = "Sad", Definition = "This is a definition"});
            words.Add(new Word {Id = 3, WordText = "Hungry", Definition = "This is a definition"});

            return words;
        }

        /*
        [HttpGet("/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReadFileDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReadFileDTO>> GetFile(Guid id)
        {
            var file =  await _extensions.GetFile(id);

            if(file == null)
            {
                return NotFound();
            }

            return Ok(file);
        }

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