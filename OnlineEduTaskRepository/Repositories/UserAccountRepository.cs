using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;
using OnlineEduTaskDAO;
using OnlineEduTaskRepository.Interfaces;

namespace OnlineEduTaskRepository.Repositories
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly UserAccountDAO _userAccountDAO;

        public UserAccountRepository()
        {
            _userAccountDAO = UserAccountDAO.Instance;
        }

        public List<UserAccount> GetAllUsers()
        {
            return _userAccountDAO.GetAllUsers();
        }

        public UserAccount GetUserById(int userAccountId)
        {
            return _userAccountDAO.GetUserById(userAccountId);
        }

        public UserAccount GetUserByUsername(string username)
        {
            return _userAccountDAO.GetUserByUsername(username);
        }

        public UserAccount GetUserByStudentId(int studentId)
        {
            return _userAccountDAO.GetUserByStudentId(studentId);
        }

        public UserAccount Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            return _userAccountDAO.Login(username, password);
        }

        public bool ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            return _userAccountDAO.ValidateCredentials(username, password);
        }

        public List<UserAccount> GetUsersByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role is required");

            return _userAccountDAO.GetUsersByRole(role);
        }

        public void AddUser(UserAccount userAccount)
        {
            if (userAccount == null)
                throw new ArgumentNullException(nameof(userAccount));

            if (string.IsNullOrWhiteSpace(userAccount.Username))
                throw new ArgumentException("Username is required");

            if (string.IsNullOrWhiteSpace(userAccount.Password))
                throw new ArgumentException("Password is required");

            if (string.IsNullOrWhiteSpace(userAccount.Role))
                throw new ArgumentException("Role is required");

            if (UsernameExists(userAccount.Username))
                throw new InvalidOperationException($"User with username {userAccount.Username} already exists");

            _userAccountDAO.AddUser(userAccount);
        }

        public void UpdateUser(UserAccount userAccount)
        {
            if (userAccount == null)
                throw new ArgumentNullException(nameof(userAccount));

            if (!UserExists(userAccount.UserAccountId))
                throw new InvalidOperationException($"User with ID {userAccount.UserAccountId} does not exist");

            _userAccountDAO.UpdateUser(userAccount);
        }

        public void DeleteUser(int userAccountId)
        {
            if (!UserExists(userAccountId))
                throw new InvalidOperationException($"User with ID {userAccountId} does not exist");

            _userAccountDAO.DeleteUser(userAccountId);
        }

        public void ChangePassword(int userAccountId, string newPassword)
        {
            if (!UserExists(userAccountId))
                throw new InvalidOperationException($"User with ID {userAccountId} does not exist");

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password is required");

            _userAccountDAO.ChangePassword(userAccountId, newPassword);
        }

        public bool UserExists(int userAccountId)
        {
            return _userAccountDAO.UserExists(userAccountId);
        }

        public bool UsernameExists(string username)
        {
            return _userAccountDAO.UsernameExists(username);
        }

        public bool IsAdmin(int userAccountId)
        {
            return _userAccountDAO.IsAdmin(userAccountId);
        }

        public bool IsStudent(int userAccountId)
        {
            return _userAccountDAO.IsStudent(userAccountId);
        }
    }
}
