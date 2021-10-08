using System;
using System.Collections.Generic;
using System.Linq;

namespace word_of_the_day.Models
{
    public class DbInit
    {
        public static void Init(WordOfTheDayContext context)
        {
            context.Database.EnsureCreated();

            if(context.Words.Any() && context.Users.Any())
            {
                return;
            }

            var happy = new Word
            {
                WordId = 1,
                WordText = "Happy",
                Definition = "To be happy"
            };

            var words = new List<Word>
            {
                happy,

                new Word
                {
                    WordId = 2, 
                    WordText = "Sad",
                    Definition = "I am sad"
                }
            };

            foreach(Word w in words)
            {
                context.Words.Add(w);
            }

            var users = new List<User>()
            {
                new User
                {
                    UserId = Guid.Parse("844d024c-a958-11eb-bcbc-0242ac130004"),
                    Username = "nicktest", 
                    WordOfTheDayId = 1,
                    LastUpdated = DateTime.Now
                }
            };

            foreach(User u in users)
            {
                context.Users.Add(u);
            }

            var prevWords = new List<PreviouslyUsedWord>()
            {
                new PreviouslyUsedWord
                {
                    Id = 1,
                    WordId = 1,
                    UserId = Guid.Parse("844d024c-a958-11eb-bcbc-0242ac130004")
                }
            };

            foreach(PreviouslyUsedWord pW in prevWords)
            {
                context.PreviouslyUsedWords.Add(pW);
            }

            context.SaveChanges();
        }
    }
}