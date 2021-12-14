using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using word_of_the_day.Models;

namespace word_of_the_day.Data
{
    public class DbInit
    {
        private static readonly Random random = new Random();

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

            int count = 0;
            int index;
            while(count < 5000)
            {
                index = random.Next(0, words.Count - 1);
                context.Words.Add(words[index]);
                count++;
            }

            // var users = new List<User>()
            // {
            //     new User
            //     {
            //         Id = Guid.Parse("844d024c-a958-11eb-bcbc-0242ac130002"),
            //         Username = "test01",
            //         WordOfTheDayId = 1,
            //         LastUpdated = DateTime.Now
            //     },

            //     new User
            //     {
            //         Id = Guid.Parse("844d024c-a958-11eb-bcbc-0242ac130001"),
            //         Username = "test02",
            //         WordOfTheDayId = 1,
            //         LastUpdated = DateTime.Now
            //     }
            // };

            // foreach(User u in users)
            // {
            //     context.Users.Add(u);
            // }

            // var prevWords = new List<PreviouslyUsedWord>()
            // {
            //     new PreviouslyUsedWord
            //     {
            //         Id = 1,
            //         WordId = 1,
            //         Username = "test01"
            //     }
            // };

            // foreach(PreviouslyUsedWord pW in prevWords)
            // {
            //     context.PreviouslyUsedWords.Add(pW);
            // }

            context.SaveChanges();
        }
    }
}