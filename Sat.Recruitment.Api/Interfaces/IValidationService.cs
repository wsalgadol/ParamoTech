using System.Collections.Generic;
using Sat.Recruitment.Api.Models;

namespace Sat.Recruitment.Api.Interfaces
{
    public interface IValidationService
    {
        string ValidateErrors(string name, string email, string address, string phone);
        bool IsDuplicatedUser(User user, List<User> users);
    }
}
