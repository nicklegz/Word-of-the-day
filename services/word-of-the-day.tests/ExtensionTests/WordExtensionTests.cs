using System.Threading.Tasks;
using Moq;
using word_of_the_day.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;
using word_of_the_day.Controllers;
using Microsoft.Extensions.Configuration;
using Xunit;
using word_of_the_day.Extensions;
using Newtonsoft.Json;

namespace word_of_the_day.tests.ExtensionTests
{
    public class WordExtensionTests
    {
        private DateTime dtNow = DateTime.Now;
        private User _mockUser;
        private Word _mockWord;
        private PreviouslyUsedWord _mockPreWords;
        private readonly Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();

        public WordExtensionTests()
        {
            _mockUser = new User{
                Id = Guid.Parse("d8af31a4-f108-425f-a5f0-28d0e566548b"),
                Username = "nicktest",
                LastUpdated = dtNow.AddHours(-6),
                WordOfTheDayId = 1
            };

            _mockWord = new Word{
                WordId = 2,
                Text = "sad",
                Definition = "Im sad",
                Type = "noun"
            };

            _mockPreWords = new PreviouslyUsedWord{
                WordId = 1
            };
        }

        [Fact]
        public async Task GetListOfWordsAsync_ShouldReturnListOfWords()
        {
            //Arrange
            var testWords = GetTestWords();
            Mock<IWordRepository> wordRepoMock = new Mock<IWordRepository>();
            wordRepoMock.Setup(wordExt => wordExt.GetListOfWordsAsync()).ReturnsAsync(GetTestWords());
            
            var wordExtension = new WordExtension(wordRepoMock.Object);

            //Act
            var result = await wordExtension.GetListOfWordsAsync();
            var jsonResult = JsonConvert.SerializeObject(result);

            //Assert
            Assert.Equal(JsonConvert.SerializeObject(testWords), jsonResult);
        }

        
        private List<User> GetTestUsers()
        {
            var users = new List<User>();
            users.Add(new User{
                Id = Guid.Parse("d8af31a4-f108-425f-a5f0-28d0e566548b"),
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

        private List<Word> GetTestWords()
        {
            List<Word> words = new List<Word>();
            words.Add(new Word{
                WordId = 1,
                Text = "happy",
                Definition = "Im happy",
                Type = "noun"
            });
            words.Add(new Word{
                WordId = 2,
                Text = "sad",
                Definition = "Im sad",
                Type = "noun"
            });

            return words;
        }

        private List<PreviouslyUsedWord> GetPreviouslyUsedWords()
        {
            return new List<PreviouslyUsedWord>()
            {
                new PreviouslyUsedWord
                {
                    Id = 1,
                    WordId = 1,
                    UserId = "17927udiud2981"
                }
            };
        }

        private List<Word> GetAvailableWords()
        {
            List<Word> words = new List<Word>();
            words.Add(new Word{
                WordId = 2,
                Text = "sad",
                Definition = "Im sad",
                Type = "noun"
            });

            return words;
        }
        
    }
}