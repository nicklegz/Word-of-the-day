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

        public async Task<List<Word>> GetPreviouslyUsedWordsAsync(string username)
        {
            return await _wordRepo.GetPreviouslyUsedWordsAsync(username);
        }

        public async Task AddPreviouslyUsedWordAsync(string username, int wordId)
        {
            await _wordRepo.AddPreviouslyUsedWordAsync(username, wordId);
        }

        public async Task<List<Word>> GetLikedWordsAsync(string username)
        {
            return await _wordRepo.GetLikedWordsAsync(username);
        }

        public async Task<bool> GetIsLikedWordOfTheDayAsync(string username, int wordId)
        {
            return await _wordRepo.GetIsLikedWordOfTheDayAsync(username, wordId);
        }

        public async Task AddLikedWordAsync(string username, int wordId)
        {
            await _wordRepo.AddLikedWordAsync(username, wordId);
        }

        public async Task DeleteLikedWordAsync(string username, int wordId)
        {
            await _wordRepo.DeleteLikedWordAsync(username, wordId);
        }
    }
}