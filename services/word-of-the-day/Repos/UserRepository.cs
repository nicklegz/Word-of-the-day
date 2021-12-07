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

        public async Task AddUserAsync(string username, int newWordId)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Username = username,
                LastUpdated = DateTime.Now,
                WordOfTheDayId = newWordId
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);
            if(userEntity == null)
                return;
            
            userEntity.WordOfTheDayId = user.WordOfTheDayId;
            userEntity.LastUpdated = user.LastUpdated;
            await _context.SaveChangesAsync();
        }
    }
}
