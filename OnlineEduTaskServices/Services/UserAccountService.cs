using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Interfaces;

namespace OnlineEduTaskServices.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _userAccountRepository;

        public UserAccountService(IUserAccountRepository userAccountRepository)
        {
            _userAccountRepository = userAccountRepository ?? throw new ArgumentNullException(nameof(userAccountRepository));
        }

        public List<UserAccount> GetAllUsers()
        {
            return _userAccountRepository.GetAllUsers();
        }

        public UserAccount GetUserById(int userAccountId)
        {
            if (userAccountId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            return _userAccountRepository.GetUserById(userAccountId);
        }

        public UserAccount GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required");

            return _userAccountRepository.GetUserByUsername(username);
        }

        public UserAccount GetUserByStudentId(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("Student ID must be greater than 0");

            return _userAccountRepository.GetUserByStudentId(studentId);
        }

        public UserAccount Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            var user = _userAccountRepository.Login(username, password);
            if (user == null)
                throw new InvalidOperationException("Invalid username or password");

            return user;
        }

        public bool ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required");

            return _userAccountRepository.ValidateCredentials(username, password);
        }

        public List<UserAccount> GetUsersByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role is required");

            return _userAccountRepository.GetUsersByRole(role);
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

            _userAccountRepository.AddUser(userAccount);
        }

        public void UpdateUser(UserAccount userAccount)
        {
            if (userAccount == null)
                throw new ArgumentNullException(nameof(userAccount));

            if (userAccount.UserAccountId <= 0)
                throw new ArgumentException("Valid user ID is required");

            _userAccountRepository.UpdateUser(userAccount);
        }

        public void DeleteUser(int userAccountId)
        {
            if (userAccountId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            _userAccountRepository.DeleteUser(userAccountId);
        }

        public void ChangePassword(int userAccountId, string newPassword)
        {
            if (userAccountId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password is required");

            _userAccountRepository.ChangePassword(userAccountId, newPassword);
        }

        public bool UserExists(int userAccountId)
        {
            if (userAccountId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            return _userAccountRepository.UserExists(userAccountId);
        }

        public bool UsernameExists(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required");

            return _userAccountRepository.UsernameExists(username);
        }

        public bool IsAdmin(int userAccountId)
        {
            if (userAccountId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            return _userAccountRepository.IsAdmin(userAccountId);
        }

        public bool IsStudent(int userAccountId)
        {
            if (userAccountId <= 0)
                throw new ArgumentException("User ID must be greater than 0");

            return _userAccountRepository.IsStudent(userAccountId);
        }

        public int GetTotalUserCount()
        {
            return GetAllUsers().Count;
        }

        public int GetUserCountByRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role is required");

            return GetUsersByRole(role).Count;
        }
    }
}
