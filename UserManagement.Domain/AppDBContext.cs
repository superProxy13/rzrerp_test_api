using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace UserManagement.Domain
{
    public class AppDBContext
    {
        private readonly IConfiguration _configurarion;
        private string _connectionString = string.Empty;
        public AppDBContext(IConfiguration configuration)
        {
            _configurarion = configuration;
            _connectionString = _configurarion.GetConnectionString("DefaultConnection");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    }
}
