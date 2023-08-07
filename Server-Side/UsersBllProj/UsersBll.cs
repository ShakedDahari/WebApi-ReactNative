using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersDalProj;

namespace UsersBllProj
{
    public static class UsersBll
    {
        public static User[] GetUsers() 
        {
            User[] users = null;
            users = UsersDB.GetAllUsers();
            return users;
        }

        public static User GetUser(int id) 
        { 
            User user = UsersDB.GetUser(id);
            return user;
        }

        public static User InsertUser(User user)
        {
            User newUser = UsersDB.InsertUser(user);
            return newUser;
        }

        public static User CheckEmail(string email)
        {
            User user = UsersDB.CheckEmail(email);
            return user;
        }

        public static User Login(string email, string password)
        {
            User user = null;
            user = UsersDB.Login(email, password);
            return user;
        }

        public static bool DeleteUser(int id)
        {
            return UsersDB.DeleteUser(id);
        }
    }
}
