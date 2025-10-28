using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentManagementBusinessObject;

namespace OnlineEduTaskDAO
{
    public class UserAccountDAO
    {
        private static UserAccountDAO instance = null;
        private static readonly object instanceLock = new object();

        private UserAccountDAO() { }

        public static UserAccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserAccountDAO();
                    }
                    return instance;
                }
            }
        }

        public List<UserAccount> GetAllUsers()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.UserAccounts.ToList();
            }
        }

        public UserAccount GetUserById(int userAccountId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.UserAccounts
                    .FirstOrDefault(u => u.UserAccountId == userAccountId);
            }
        }

        public UserAccount GetUserByUsername(string username)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.UserAccounts
                    .FirstOrDefault(u => u.Username == username);
            }
        }

        public UserAccount GetUserByStudentId(int studentId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.UserAccounts
                    .FirstOrDefault(u => u.StudentId == studentId);
            }
        }

        public UserAccount Login(string username, string password)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.UserAccounts
                    .FirstOrDefault(u => u.Username == username && u.Password == password);
            }
        }

        public bool ValidateCredentials(string username, string password)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.UserAccounts
                    .Any(u => u.Username == username && u.Password == password);
            }
        }

        public List<UserAccount> GetUsersByRole(string role)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.UserAccounts
                    .Where(u => u.Role == role)
                    .ToList();
            }
        }

        public void AddUser(UserAccount userAccount)
        {
            using (var context = new StudentManagementDbContext())
            {
                context.UserAccounts.Add(userAccount);
                context.SaveChanges();
            }
        }

        public void UpdateUser(UserAccount userAccount)
        {
            using (var context = new StudentManagementDbContext())
            {
                context.Entry(userAccount).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteUser(int userAccountId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var user = context.UserAccounts.Find(userAccountId);
                if (user != null)
                {
                    context.UserAccounts.Remove(user);
                    context.SaveChanges();
                }
            }
        }

        public void ChangePassword(int userAccountId, string newPassword)
        {
            using (var context = new StudentManagementDbContext())
            {
                var user = context.UserAccounts.Find(userAccountId);
                if (user != null)
                {
                    user.Password = newPassword;
                    context.SaveChanges();
                }
            }
        }

        public bool UserExists(int userAccountId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.UserAccounts.Any(u => u.UserAccountId == userAccountId);
            }
        }

        public bool UsernameExists(string username)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.UserAccounts.Any(u => u.Username == username);
            }
        }

        public bool IsAdmin(int userAccountId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var user = context.UserAccounts.Find(userAccountId);
                return user != null && user.Role == "Admin";
            }
        }

        public bool IsStudent(int userAccountId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var user = context.UserAccounts.Find(userAccountId);
                return user != null && user.Role == "Student";
            }
        }
    }
}
