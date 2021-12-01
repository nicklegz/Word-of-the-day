using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using word_of_the_day.Interfaces;
using word_of_the_day.Models;

namespace word_of_the_day.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly WordOfTheDayContext _context;

        public UserRepository(WordOfTheDayContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task AddUserAsync(string userId, int newWordId)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = userId,
                LastUpdated = DateTime.Now,
                WordOfTheDayId = newWordId
            };

            await _context.Users.AddAsync(user);
            _context.SaveChanges();
        }
    }
}
