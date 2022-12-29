using System;
using System.Collections.Generic;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;

namespace Sat.Recruitment.Api.Services
{
    public class ValidationService:IValidationService
    {
        public string ValidateErrors(string name, string email, string address, string phone)
        {
            string errors = string.Empty;

            if (string.IsNullOrEmpty(name))
                errors = "The name is required";
            if (string.IsNullOrEmpty(email))
                errors += " The email is required";
            if (string.IsNullOrEmpty(address))
                errors += " The address is required";
            if (string.IsNullOrEmpty(phone))
                errors += " The phone is required";

            return errors;
        }

        public bool IsDuplicatedUser(User userToValidate, List<User> users)
        {
            foreach (var user in users)
            {
                if (userToValidate.Email == user.Email || userToValidate.Phone == user.Phone)
                {
                    return true;
                }

                if (userToValidate.Name == user.Name && userToValidate.Address == user.Address)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
