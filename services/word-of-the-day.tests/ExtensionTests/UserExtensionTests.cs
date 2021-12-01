using System.Threading.Tasks;
using Moq;
using word_of_the_day.Interfaces;
using System;
using System.Linq;
using word_of_the_day.Controllers;
using Microsoft.Extensions.Configuration;
using Xunit;
using word_of_the_day.tests.Data;
using word_of_the_day.Extensions;

namespace word_of_the_day.tests.ExtensionTests
{
    public class UserExtensionTests
    {
        private DateTime dtNow = DateTime.Now;
        private readonly Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
        private readonly Mock<IWordRepository> wordRepoMock = new Mock<IWordRepository>();


        [Fact]
        public async Task GetUserAsync_ValidUsername_ShouldReturnMockUser()
        {
            //Arrange
            string username =  "nicktest";
            
            var userRepoMock = new Mock<IUserRepository>();

            userRepoMock.Setup(userExt => userExt.GetUserAsync(username))
            .ReturnsAsync(
                MockData.GetTestUsers().FirstOrDefault(
                    u => u.Username == username
            ));
            
            var userExtension = new UserExtension(userRepoMock.Object);

            //Act
            var result =  await userExtension.GetUserAsync(username);

            //Assert
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async Task GetUserAsync_InvalidUsername_ShouldReturnNull()
        {
            //Arrange
            string username = "fail";

            var userRepoMock = new Mock<IUserRepository>();

            userRepoMock.Setup(userExt => userExt.GetUserAsync(username))
            .ReturnsAsync(
                MockData.GetTestUsers().FirstOrDefault(
                    u => u.Username == username
            ));
                
            var userExtension = new UserExtension(userRepoMock.Object);

            //Act
            var result =  await userExtension.GetUserAsync(username);

            //Assert
            Assert.Null(result);
        }
        
    }
}