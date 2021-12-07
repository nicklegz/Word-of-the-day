using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using word_of_the_day.Interfaces;

namespace word_of_the_day.Extensions
{
    public class WordExtension : IWordRepository
    {
        private readonly IWordRepository _wordRepo;
        public WordExtension(IWordRepository wordRepo)
        {
            _wordRepo = wordRepo;
        }

        public async Task<List<Word>> GetListOfWordsAsync()
        {
            return await _wordRepo.GetListOfWordsAsync();
        }

        public async Task<List<Word>> GetListAvailableWordsAsync(User user)
        {
            return await _wordRepo.GetListAvailableWordsAsync(user);
        }

        public async Task<Word> GetExistingWordOfTheDayAsync(User user)
        {
            return await _wordRepo.GetExistingWordOfTheDayAsync(user);
        }

        public Word GetNewWordOfTheDay(List<Word> words, int wordsCount)
        {
            return _wordRepo.GetNewWordOfTheDay(words, wordsCount);
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
    }
}