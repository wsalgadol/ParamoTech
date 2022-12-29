using System.Threading.Tasks;
using Moq;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;
using Xunit;

namespace Sat.Recruitment.Test.Controllers
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserControllerTest
    {
        private readonly Mock<IValidationService> _mockValidationService;
        private readonly Mock<IUserService> _mockUserService;

        public UserControllerTest()
        {
            _mockValidationService = new Mock<IValidationService>();
            _mockUserService = new Mock<IUserService>();

            _mockValidationService
                .Setup(x => x.ValidateErrors(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<string>())).Returns(string.Empty);

            _mockUserService.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(new Result()
            {
                IsSuccess = true,
                Errors = "User Created"
            });
        }

        [Fact]
        public async Task CreateUser()
        {
            var userController = new UsersController(_mockValidationService.Object, _mockUserService.Object);

            var result = await userController.CreateUser("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124");


            Assert.True(result.IsSuccess);
            Assert.Equal("User Created", result.Errors);
        }

        [Fact]
        public void CreateUser_Fail()
        {
            _mockUserService.Setup(x => x.CreateUser(It.IsAny<User>())).Returns(new Result()
            {
                IsSuccess = false,
                Errors = "The user is duplicated"
            });

            var userController = new UsersController(_mockValidationService.Object, _mockUserService.Object);

            var result = userController.CreateUser("Agustina", "Agustina@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124").Result;

            Assert.False(result.IsSuccess);
            Assert.Equal("The user is duplicated", result.Errors);
        }
    }
}
