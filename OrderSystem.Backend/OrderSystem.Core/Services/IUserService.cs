using OrderSystem.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Core.Services
{
    public interface IUserService
    {
        public UserEntity? GetUserById(int id);
        public UserEntity? GetUserByUsername(string username);
        public Result CreateUser(UserEntity user);
        public UserEntity[] GetUsers();
    }
}