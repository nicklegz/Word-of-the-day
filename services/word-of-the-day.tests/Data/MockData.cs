using System;
using System.Collections.Generic;

namespace word_of_the_day.tests.Data
{
    public static class MockData
    {
        public static List<User> GetTestUsers()
        {
            var users = new List<User>();
            users.Add(new User{
                Id = Guid.NewGuid(),
                Username = "nicktest",
                LastUpdated = DateTime.Now,
                WordOfTheDayId = 1

            });
            users.Add(new User{
                Id = Guid.NewGuid(),
                Username = "jack",
                LastUpdated = DateTime.Now,
                WordOfTheDayId = 1

            });
            return users;
        }
    }
}