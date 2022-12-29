using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Api.Services;
using Xunit;

namespace Sat.Recruitment.Test.Services
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserServiceTest
    {
        private readonly Mock<IValidationService> _mockValidationService;

        public UserServiceTest()
        {
            _mockValidationService = new Mock<IValidationService>();

            _mockValidationService
                .Setup(x => x.IsDuplicatedUser(It.IsAny<User>(), It.IsAny<List<User>>())).Returns(false);
        }

        [Theory]
        [InlineData("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124")]
        public Task CreateUser_Ok(string name,string email, string  address, string phone, string userType, string money)
        {
            var userService = new UserService(_mockValidationService.Object);

            var user = new User()
            {
                Name = name,
                Email = email,
                Address = address,
                Phone = phone,
                UserType = userType,
                Money = decimal.Parse(money)
            };
            var result = userService.CreateUser(user);
            
            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
            return Task.CompletedTask;
        }

        [Theory]
        [InlineData("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124")]
        public Task CreateUser_Fail(string name, string email, string address, string phone, string userType, string money)
        {
            _mockValidationService
                .Setup(x => x.IsDuplicatedUser(It.IsAny<User>(), It.IsAny<List<User>>())).Returns(true);

            var userService = new UserService(_mockValidationService.Object);

            var user = new User()
            {
                Name = name,
                Email = email,
                Address = address,
                Phone = phone,
                UserType = userType,
                Money = decimal.Parse(money)
            };
            var result = userService.CreateUser(user);

            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Errors);
            return Task.CompletedTask;
        }


    }
}
