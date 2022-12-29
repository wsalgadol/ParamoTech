using Sat.Recruitment.Api.Models;

namespace Sat.Recruitment.Api.Interfaces
{
    public interface IUserService
    {
        Result CreateUser(User user);
    }
}
