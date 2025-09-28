using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase;

namespace Infractructure.SQL
{
    public class SqlAuthRepository : IAuthRepository
    {
        private readonly string? strCnn;
        public SqlAuthRepository(IConfiguration configuration)
        {
            strCnn = configuration.GetConnectionString("DefaultConnection");
        }
        public Users? Login(string username, string password)
        {
            var user = null as Users;
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "SELECT Id, Role, UserName, Password FROM Users WHERE UserName=@username AND Password=@password";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Nếu có dữ liệu => login thành công
                        {
                            return new Users
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Password = reader.GetString(reader.GetOrdinal("Password")),
                                Role = (ESRoleUser)reader.GetInt32(reader.GetOrdinal("Role"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void Register(Users user)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "INSERT INTO Users (UserName, Password, Email, Role) VALUES (@Username, @Password, @Email, @Role)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Role", (int)user.Role);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
