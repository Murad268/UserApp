using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.models;
using UserApp.enums;
using UserApp.db;

namespace UserApp.db
{
    internal class Users
    {
        public Users()
        {
            
        }

        public List<User> users = new List<User> {
            new User() { Id = 1, Age = 26, Name = "Murad", Surname = "Agamedov", Email = "agamedov94@mail.ru", Password = "5994471abb01112afcc18159f6cc74b4f511b99806da59b3caf5a9c173cacfc5", UserRole = UserRole.Admin }
        };

        public List<User> All()
        {
            return users;
        }


        public string GetUserRole(UserRole userRole)
        {
            return userRole.ToString();  
        }
    }
}
