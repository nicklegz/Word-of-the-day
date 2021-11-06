using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using word_of_the_day.Interfaces;
using word_of_the_day.Models;

namespace word_of_the_day.Extensions
{
    public class UserExtension : IUserExtension
    {
        private readonly WordOfTheDayContext _context;

        public UserExtension(WordOfTheDayContext context)
        {
            _context = context;
        }
        public string GetUserId(ClaimsPrincipal user)
        {
            var userId = ((ClaimsIdentity)user.Identity).Claims.Select(x => x.Value).FirstOrDefault();
            return userId;
        } 

        public async Task<User> GetUserAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task AddUserAsync(string userId, int newWordId)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                LastUpdated = DateTime.Now,
                WordOfTheDayId = newWordId
            };

            await _context.Users.AddAsync(user);
            _context.SaveChanges();
        }
    }
}
