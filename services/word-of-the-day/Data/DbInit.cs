using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using word_of_the_day.Models;

namespace word_of_the_day.Models
{
    public class DbInit
    {
        public static void Init(WordOfTheDayContext context)
        {
            context.Database.EnsureCreated();

            if(context.Words.Any())
            {
                return;
            }

            Word happy = new Word()
            {
                Id = 1,
                WordText = "Happy",
                Definition = "To be happy"
            };


            var users = new User[]
            {
                new User
                { 
                    UserId = Guid.Parse("844d024c-a958-11eb-bcbc-0242ac130004"),
                    Username = "nicktest", 
                    PreviouslyUsedWords = new List<int>(){1,2,3},
                    WordOfTheDay = happy,
                    LastUpdated = DateTime.Now
                }
            };

            foreach(User u in users)
            {
                context.Users.Add(u);
            }

            var words = new Word[]
            {
                happy,

                new Word()
                {
                    Id = 2, 
                    WordText = "Sad",
                    Definition = "I am sad"
                }
            };

            foreach(Word w in words)
            {
                context.Words.Add(w);
            }

            context.SaveChanges();
        }
    }
}