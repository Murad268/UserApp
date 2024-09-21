using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.db;
using UserApp.models;
using UserApp.helpers;

namespace UserApp.helpers
{
    internal class LoginHelper
    {
        Users usersDb;
        List<User> users;

        public LoginHelper()
        {
            usersDb = new Users();
            users = usersDb.users;
        }

        public List<User> login(string email, string password)
        {
            return users.Where(user => (user.Email == email) && (PasswordHelper.VerifyPassword(user.Password, password))).ToList();
        }
    }

}
