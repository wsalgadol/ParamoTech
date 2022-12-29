using System.Collections.Generic;
using System.Security.Policy;
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
    public class ValidationServiceTest
    {

        [Fact]
        public Task ValidateErrors_Ok()
        {
            var validationService = new ValidationService();
            var result = validationService.ValidateErrors("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215");
            
            Assert.True(string.IsNullOrEmpty(result));
            return Task.CompletedTask;
        }

        [Theory]
        [InlineData("", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "The name is required")]
        [InlineData("Mike", "", "Av. Juan G", "+349 1122354215", " The email is required")]
        [InlineData("Mike", "mike@gmail.com", "", "+349 1122354215", " The address is required")]
        [InlineData("Mike", "mike@gmail.com", "Av. Juan G", "", " The phone is required")]
        public Task ValidateErrors_Fail(string name, string email, string address, string phone, string expectedMessage)
        {
            var validationService = new ValidationService();
            var result = validationService.ValidateErrors(name, email,address,phone);

            Assert.False(string.IsNullOrEmpty(result));
            Assert.Equal(expectedMessage,result);
            return Task.CompletedTask;
        }

        [Theory]
        [InlineData("Mikel", "mikel@gmail.com", "Av. Juan G", "+349 11223542151", "Normal", "120", false)]
        [InlineData("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "120", true)]
        [InlineData("Mike", "mike@gmail.com", "Av. Juan G", "+349 11223542151", "Normal", "120", true)]
        [InlineData("Mikel", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "120", true)]
        [InlineData("Mike", "mikel@gmail.com", "Av. Juan G", "+349 11223542151", "Normal", "120", true)]
        public Task IsDuplicatedUser(string name, string email, string address, string phone, string userType, string money, bool expectedResult)
        {
            var validationService = new ValidationService();
            var user = new User()
            {
                Name = name,
                Email = email,
                Address = address,
                Phone = phone,
                UserType = userType,
                Money = decimal.Parse(money)
            };
            var users = new List<User>
            {
                new User
                {
                    Name = "Mike",
                    Money = 120,
                    Address = "Av. Juan G",
                    Email = "mike@gmail.com",
                    Phone = "+349 1122354215",
                    UserType = "Normal"
                }
            };
            var result = validationService.IsDuplicatedUser(user, users);

            Assert.Equal(expectedResult,result);
            return Task.CompletedTask;
        }


    }
}
