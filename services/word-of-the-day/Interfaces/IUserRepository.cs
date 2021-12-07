using System;
using System.Threading.Tasks;

namespace word_of_the_day.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserAsync(string userId);
        Task AddUserAsync(string userId, int newWordId);
        Task UpdateUserAsync(User user);
    }
}