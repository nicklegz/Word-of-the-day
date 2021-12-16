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
                            p.Username == user.Username).DefaultIfEmpty()
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

        public Boolean IsNewWordRequired(User user)
        {
            TimeSpan diff = DateTime.Now - user.LastUpdated;
            if(diff.TotalDays > 1)
            {
                return true;
            }

            return false;
        }

        public async Task<List<Word>> GetPreviouslyUsedWordsAsync(string username)
        {
            var query = from word in _context.Set<Word>()
                        from p in _context.Set<PreviouslyUsedWord>().Where(
                            p => word.WordId == p.WordId && 
                            p.Username == username).DefaultIfEmpty()
                        where p != null
                        select word;

            return await query.ToListAsync();
        }

        public async Task AddPreviouslyUsedWordAsync(string username, int wordId)
        {
            await _context.PreviouslyUsedWords.AddAsync(new PreviouslyUsedWord{
                Username = username,
                WordId = wordId
            });

            await _context.SaveChangesAsync();
        }

        public async Task<List<Word>> GetLikedWordsAsync(string username)
        {
            var query = from word in _context.Set<Word>()
                        from p in _context.Set<LikedWord>().Where(
                            p => word.WordId == p.WordId && 
                            p.Username == username).DefaultIfEmpty()
                        where p != null
                        select word;

            return await query.ToListAsync();
        }

        public async Task<bool> GetIsLikedWordOfTheDayAsync(string username, int wordId)
        {
            var isLiked = await _context.LikedWords.FirstOrDefaultAsync(word => word.Username == username && word.WordId == wordId);
            if(isLiked != null){
                return true;
            }

            return false;
        }

        public async Task AddLikedWordAsync(string username, int wordId)
        {
            await _context.LikedWords.AddAsync(new LikedWord{
                Username = username,
                WordId = wordId
            });

            await _context.SaveChangesAsync();
        }

        public async Task DeleteLikedWordAsync(string username, int wordId)
        {
            var likedWord = await _context.LikedWords.FirstOrDefaultAsync(word => word.WordId == wordId && word.Username == username);
            if(likedWord == null)
            {
                return; 
            }

            _context.Remove(likedWord);
            await _context.SaveChangesAsync();
        }
    }
}
