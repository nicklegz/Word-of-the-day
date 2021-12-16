using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace word_of_the_day.Interfaces
{
    public interface IWordRepository
    {
        Task<List<Word>> GetListOfWordsAsync();
        Task<List<Word>> GetListAvailableWordsAsync(User user);
        Task<Word> GetExistingWordOfTheDayAsync(User user);
        Word GetNewWordOfTheDay(List<Word> words, int wordCount);
        Task<List<Word>> GetPreviouslyUsedWordsAsync(string username);
        Task AddPreviouslyUsedWordAsync(string username, int wordId);
        Task<List<Word>> GetLikedWordsAsync(string username);
        Task<bool> GetIsLikedWordOfTheDayAsync(string username, int wordId);
        Task AddLikedWordAsync(string username, int wordId);
        Task DeleteLikedWordAsync(string username, int wordId);
        Boolean IsNewWordRequired(User user);
    }
}