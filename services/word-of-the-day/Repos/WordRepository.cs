using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using word_of_the_day.Interfaces;
using word_of_the_day.Models;
using System.Linq;
using System;

namespace word_of_the_day.Extensions
{
    public class WordRepository : IWordRepository
    {
        private readonly WordOfTheDayContext _context;
        private static readonly Random random = new Random();

        public WordRepository(WordOfTheDayContext context)
        {
            _context = context;
        }

        public async Task<List<Word>> GetListOfWordsAsync()
        {
            return await _context.Words.ToListAsync();
        }

        public async Task<List<Word>> GetListAvailableWordsAsync(User user)
        {
            var query = from word in _context.Set<Word>()
                        from p in _context.Set<PreviouslyUsedWord>().Where(
                            p => word.WordId == p.WordId && 
                            p.UserId == user.Username).DefaultIfEmpty()
                        where p == null
                        select word;

            return await query.ToListAsync();
        }

        public async Task<Word> GetExistingWordOfTheDayAsync(User user)
        {
            return await _context.Words.FirstOrDefaultAsync(x => x.WordId == user.WordOfTheDayId);
        }

        public Word GetNewWordOfTheDay(List<Word> words, int wordsCount)
        {
            int index = random.Next(0, wordsCount - 1);
            return words[index];
        }
    }
}
