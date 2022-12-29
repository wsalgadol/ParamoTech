using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;

namespace Sat.Recruitment.Api.Controllers
{


    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly IUserService _userService;

        public UsersController(IValidationService validationService, IUserService userService)
        {
            _validationService = validationService;
            _userService = userService;
        }

        [HttpPost]
        [Route("/create-user")]
        public async Task<Result> CreateUser(string name, string email, string address, string phone, string userType, string money)
        {
            var errors = _validationService.ValidateErrors(name, email, address, phone);

            if (!string.IsNullOrEmpty(errors))
            {
                return new Result()
                {
                    IsSuccess = false,
                    Errors = errors
                };
            }

            return _userService.CreateUser(
                new User {
                Name = name,
                Email = email,
                Address = address,
                Phone = phone,
                UserType = userType,
                Money = decimal.Parse(money)
            });
        }

        
        
    }
   
}
