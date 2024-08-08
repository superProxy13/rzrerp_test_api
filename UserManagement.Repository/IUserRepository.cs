using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.API.Models;
using UserManagement.Domain;

namespace UserManagement.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetAllUsers(int CompanyId);
        Task<IEnumerable<UserModel>> GetAllNonAdminUsers(int CompanyId);
        Task<bool> IsUserExists(string Username);
        Task<UserModel> CreateNewUser(NewUserModel userModel);
        Task<int> UpdateUser(UpdateUserModel User);
        Task<int> DeleteUser(long UserId);
        Task<UserModel> ValidateUserLogin(LoginModel login);
    }
}
