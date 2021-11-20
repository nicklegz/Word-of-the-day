using System.Security.Claims;
using System.Threading.Tasks;

namespace word_of_the_day.Interfaces
{
    public interface IUserExtension
    {
        Task<User> GetUserAsync(string userId);
        Task AddUserAsync(string userId, int newWordId);
    }
}