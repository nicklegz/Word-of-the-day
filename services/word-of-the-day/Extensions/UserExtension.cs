using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using word_of_the_day.Interfaces;
using word_of_the_day.Models;

namespace word_of_the_day.Extensions
{
    public class UserExtension : IUserRepository
    {
        private readonly IUserRepository _userRepo;

        public UserExtension(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<User> GetUserAsync(string username)
        {
            return await _userRepo.GetUserAsync(username);
        }

        public async Task AddUserAsync(string userId, int newWordId)
        {
            await _userRepo.AddUserAsync(userId, newWordId);
        }

    }
}
