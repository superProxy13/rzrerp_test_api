using UserManagement.API.Models;
using UserManagement.Domain;
using Dapper;
using System.Data;
using System.Text;
using System.Security.Cryptography;

namespace UserManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;
        public UserRepository(AppDBContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsers(int CompanyId)
        {
            var query = "SELECT * FROM APP_USERS WHERE [CompanyId] = @CompanyId";
            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<UserModel>(query, new { CompanyId = CompanyId});
                return users;
            }
        }

        public async Task<IEnumerable<UserModel>> GetAllNonAdminUsers(int CompanyId)
        {
            var query = "SELECT * FROM APP_USERS WHERE [CompanyId] = @CompanyId AND [RoleId] = 2";
            using (var connection = _context.CreateConnection())
            {
                var users = await connection.QueryAsync<UserModel>(query, new { CompanyId = CompanyId });
                return users.ToList();
            }
        }

        public async Task<bool> IsUserExists(string Username)
        {
            var query = "SELECT * FROM APP_USERS WHERE [Username] = @Username";
            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QuerySingleAsync<UserModel>(query, new { Username = Username });
                if(result != null)
                {
                    return true;
                }
                
            }
            return false;
        }

        public async Task<UserModel> CreateNewUser(NewUserModel newUserModel)
        {
            var query = "INSERT INTO APP_USERS(Username,Password,RoleId,CompanyId,DateUpdated) VALUES(" +
                "@Username, @Password, @RoleId, @CompanyId, @DateUpdated) " +
                "SELECT CAST(SCOPE_IDENTITY() as bigint)";

            var dateNow = DateTime.Now;

            var pwdhash = ComputeSha256Hash(newUserModel.Password.Trim());

            var parameters = new DynamicParameters();
            parameters.Add("Username", newUserModel.Username, DbType.String);
            parameters.Add("Password", pwdhash, DbType.String);
            parameters.Add("RoleId", newUserModel.RoleId, DbType.Int16);
            parameters.Add("CompanyId", newUserModel.CompanyID, DbType.Int16);
            parameters.Add("DateUpdated", dateNow, DbType.DateTime);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parameters);
                var createdUser = new UserModel
                {
                    Id = id,
                    RoleId = newUserModel.RoleId,
                    CompanyID = newUserModel.CompanyID,
                    DateUpdated = dateNow,
                    Username = string.Empty,
                    Password = string.Empty
                };
                return createdUser;
            }
        }

        public async Task<int> UpdateUser(UserModel userModel)
        {
            var query = "UPDATE APP_USERS SET RoleId = @RoleId, CompanyId = @CompanyId " +
                "WHERE Id = @Id";

            var dateNow = DateTime.Now;

            var parameters = new DynamicParameters();
            parameters.Add("RoleId", userModel.RoleId, DbType.Int16);
            parameters.Add("CompanyId", userModel.CompanyID, DbType.Int16);
            parameters.Add("DateUpdated", dateNow, DbType.DateTime);
            parameters.Add("Id", userModel.Id, DbType.Int64);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, parameters);
                return result;
            }
        }

        public async Task<int> DeleteUser(long UserId)
        {
            var query = "DELETE FROM APP_USERS WHERE Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteAsync(query, new { Id = UserId });
                return result;
            }
        }

        public async Task<UserModel> ValidateUserLogin(LoginModel login)
        {
            var query = "SELECT Id,Password,RoleId,CompanyId,DateUpdated FROM APP_USERS " +
                "WHERE Username = @Username";

            UserModel userModel = new UserModel() { Username = string.Empty, Password = string.Empty};

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QuerySingleAsync<UserModel>(query, new { Username = login.Username });
                
                if(result != null)
                {
                    var tmpPwd = ComputeSha256Hash(login.Password);
                    if(tmpPwd == result.Password)
                    {
                        userModel.Id = result.Id;
                        userModel.RoleId = result.RoleId;
                        userModel.CompanyID = result.CompanyID;
                        userModel.Password = "******";
                        userModel.DateUpdated = result.DateUpdated;
                    }

                }
            }

            return userModel;
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }


}
