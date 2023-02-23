using OrderSystem.Data;
using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext dbContext;

        public UserService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Result CreateUser(UserEntity user)
        {
            try
            {
                if (dbContext.Users.Any(u => u.Username == user.Username))
                    return Result.Fail($"User '{user.Username}' already exists");

                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                return Result.Ok();
            }
            catch(Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public UserEntity? GetUserById(int id)
        {
            return dbContext.Users.FirstOrDefault(user => user.Id == id);
        }

        public UserEntity? GetUserByUsername(string username)
        {
            return dbContext.Users.FirstOrDefault(user => user.Username == username);
        }

        public UserEntity[] GetUsers()
        {
            return dbContext.Users.ToArray();
        }
    }
}