using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

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

            var words = new List<Word>();

            var filePath = "./Data/word.json";
            var wordJson = File.ReadAllText(filePath);
            words = JsonSerializer.Deserialize<List<Word>>(wordJson);

            for(int i = 0; i < 7000; i++)
            {
                context.Words.Add(words[i]);
            }

            var users = new List<User>()
            {
                new User
                {
                    Id = Guid.Parse("844d024c-a958-11eb-bcbc-0242ac130002"),
                    Username = "17927udiud2981",
                    WordOfTheDayId = 1,
                    LastUpdated = DateTime.Now
                },

                new User
                {
                    Id = Guid.Parse("844d024c-a958-11eb-bcbc-0242ac130001"),
                    Username = "2797ehkwh293",
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
                    UserId = "17927udiud2981"
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