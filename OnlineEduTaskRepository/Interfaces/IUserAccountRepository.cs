using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;

namespace OnlineEduTaskRepository.Interfaces
{
    public interface IUserAccountRepository
    {
        List<UserAccount> GetAllUsers();
        UserAccount GetUserById(int userAccountId);
        UserAccount GetUserByUsername(string username);
        UserAccount GetUserByStudentId(int studentId);
        UserAccount Login(string username, string password);
        bool ValidateCredentials(string username, string password);
        List<UserAccount> GetUsersByRole(string role);
        void AddUser(UserAccount userAccount);
        void UpdateUser(UserAccount userAccount);
        void DeleteUser(int userAccountId);
        void ChangePassword(int userAccountId, string newPassword);
        bool UserExists(int userAccountId);
        bool UsernameExists(string username);
        bool IsAdmin(int userAccountId);
        bool IsStudent(int userAccountId);
    }
}
