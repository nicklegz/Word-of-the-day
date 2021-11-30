using System.Threading.Tasks;
using Moq;
using word_of_the_day.Interfaces;
using System;
using System.Linq;
using word_of_the_day.Controllers;
using Microsoft.Extensions.Configuration;
using Xunit;
using word_of_the_day.tests.Data;

namespace word_of_the_day.tests.ExtensionTests
{
    public class UserExtensionTests
    {
        private DateTime dtNow = DateTime.Now;
        private readonly Mock<IConfiguration> mockConfig = new Mock<IConfiguration>();
        private readonly Mock<IWordExtension> wordExtensionMock = new Mock<IWordExtension>();

        [Fact]
        public async Task GetUserAsync_ValidUsername_ShouldReturnMockUser()
        {
            //Arrange
            string username =  "nicktest";
              
            var mockUserExtension = new Mock<IUserExtension>();

            mockUserExtension.Setup(userExt => userExt.GetUserAsync(username))
            .ReturnsAsync(
                MockData.GetTestUsers().FirstOrDefault(
                    u => u.Username == username
            ));
                
            var controller = new WordController(
                mockConfig.Object, 
                mockUserExtension.Object,
                wordExtensionMock.Object);

            //Act
            var result =  await controller.GetUserAsync(username);

            //Assert
            Assert.Equal(username, result.Username);
        }

        [Fact]
        public async Task GetUserAsync_InvalidUsername_ShouldReturnNull()
        {
            //Arrange
            string username = "fail";

            var mockUserExtension = new Mock<IUserExtension>();

            mockUserExtension.Setup(userExt => userExt.GetUserAsync(username))
            .ReturnsAsync(
                MockData.GetTestUsers().FirstOrDefault(
                    u => u.Username == username
            ));
                
            var controller = new WordController(
                mockConfig.Object, 
                mockUserExtension.Object,
                wordExtensionMock.Object);

            //Act
            var result =  await controller.GetUserAsync(username);

            //Assert
            Assert.Null(result);
        }
        
    }
}