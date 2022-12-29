using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Sat.Recruitment.Api.Interfaces;
using Sat.Recruitment.Api.Models;

namespace Sat.Recruitment.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IValidationService _validationService;

        public UserService(IValidationService validationService)
        {
            _validationService = validationService;
        }

        public Result CreateUser(User user)
        {
            user.Money = CalculateMoney(user.Money, user.UserType);
            user.Email = NormalizeEmail(user.Email);
            var users = GetUsersFromFile();

            if (_validationService.IsDuplicatedUser(user, users))
            {
                Debug.WriteLine("The user is duplicated");
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "The user is duplicated"
                };
            }

            Debug.WriteLine("User Created");

            return new Result()
            {
                IsSuccess = true,
                Errors = "User Created"
            };
        }

        private string NormalizeEmail(string userEmail)
        {
            var aux = userEmail.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);
            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);
            return string.Join("@", aux[0], aux[1]);
        }

        private List<User> GetUsersFromFile()
        {
            var users = new List<User>();
            var reader = ReadUsersFromFile();
            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                var userFile = new User
                {
                    Name = line.Split(',')[0],
                    Email = line.Split(',')[1],
                    Phone = line.Split(',')[2],
                    Address = line.Split(',')[3],
                    UserType = line.Split(',')[4],
                    Money = decimal.Parse((ReadOnlySpan<char>)line.Split(',')[5]),
                };

                users.Add(userFile);
            }

            reader.Close();

            return users;
        }

        private decimal CalculateMoney(decimal money, string userType)
        {
            if (userType == "Normal")
            {
                if (money > 100)
                {
                    var percentage = Convert.ToDecimal(0.12);
                    //If new user is normal and has more than USD100
                    var gif = money * percentage;
                    money += gif;
                }

                if (money > 10)
                {
                    var percentage = Convert.ToDecimal(0.8);
                    var gif = money * percentage;
                    money += gif;
                }
            }

            if (userType == "SuperUser")
            {
                if (money > 100)
                {
                    var percentage = Convert.ToDecimal(0.20);
                    var gif = money * percentage;
                    money += gif;
                }
            }

            if (userType == "Premium")
            {
                if (money > 100)
                {
                    var gif = money * 2;
                    money += gif;
                }
            }

            return money;
        }

        private StreamReader ReadUsersFromFile()
        {
            var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";

            FileStream fileStream = new FileStream(path, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);
            return reader;
        }
    }
}
