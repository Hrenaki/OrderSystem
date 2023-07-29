using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Data.Entities
{
    public enum UserRole
    {
        Public,
        Admin
    }

    public class UserEntity : Entity
    {
        [MaxLength(20)]
        public string Username { get; set; }

        [MaxLength(256)]
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
    }
}