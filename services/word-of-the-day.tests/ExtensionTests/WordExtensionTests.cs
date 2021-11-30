using System.Threading.Tasks;
using Moq;
using word_of_the_day.Interfaces;
using System.Collections.Generic;
using System;
using System.Linq;
using word_of_the_day.Controllers;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace word_of_the_day.tests.ExtensionTests
{
    public class WordExtensionTests
    {
        private DateTime dtNow = DateTime.Now;
        private readonly Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();

        // [Fact]
        public async Task GetUserAsync_ShouldReturnUser()
        {
            //Arrange
            //add mock user
            User mockUser = new User{
                Id = Guid.Parse("d8af31a4-f108-425f-a5f0-28d0e566548b"),
                Username = "nicktest",
                LastUpdated = dtNow.AddHours(-6),
                WordOfTheDayId = 1
            };

            Word mockWord = new Word{
                WordId = 2,
                Text = "sad",
                Definition = "Im sad",
                Type = "noun"
            };

            PreviouslyUsedWord mockPreWords = new PreviouslyUsedWord{
                WordId = 1
            };

            var mockUserExtension = new Mock<IUserExtension>();
            var mockWordExtension = new Mock<IWordExtension>();
            mockUserExtension.Setup(userExt => userExt.GetUserAsync(mockUser.Username))
            .ReturnsAsync(
                GetTestUsers().FirstOrDefault(
                    u => u.Username == mockUser.Username
            ));

            var availableWords = GetAvailableWords();

            mockWordExtension.Setup(
                ext => ext.GetListAvailableWordsAsync(mockUser))
                .ReturnsAsync(availableWords);
            
            mockWordExtension.Setup(
                ext => ext.GetExistingWordOfTheDayAsync(mockUser))
                .ReturnsAsync(
                    GetTestWords().FirstOrDefault(
                        word => word.WordId == mockUser.WordOfTheDayId
                    ));
            
            
            mockWordExtension.Setup(
                ext => ext.GetNewWordOfTheDay(availableWords, availableWords.Count));
                
            var controller = new WordController(
                mockConfig.Object, 
                mockUserExtension.Object,
                mockWordExtension.Object);

            //Act
            var result =  await controller.GetWordOfTheDay(mockUser.Username);

            //Assert
            Assert.Equal(mockWord, result);
            
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