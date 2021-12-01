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
    }
}