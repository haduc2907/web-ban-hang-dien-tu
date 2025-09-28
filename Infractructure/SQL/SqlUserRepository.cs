using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Admin_side;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infractructure.SQL
{
    public class SqlUserRepository : IUserControllerRepository
    {
        private readonly string? connStr;
        public SqlUserRepository(IConfiguration configuration)
        {
            connStr = configuration.GetConnectionString("DefaultConnection");
        }
        public void Add(Users user)
        {
            using (var con = new SqlConnection(connStr))
            {
                con.Open();
                var query = "INSERT INTO Users (UserName, Password, Email, FullName, PhoneNumber, Address, Role, CreatedDate) " +
                            "VALUES (@UserName, @Password, @Email, @FullName, @PhoneNumber, @Address, @Role, @CreatedDate)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@FullName",
                        string.IsNullOrEmpty(user.FullName) ? DBNull.Value : user.FullName);
                    cmd.Parameters.AddWithValue("@PhoneNumber",
                        string.IsNullOrEmpty(user.PhoneNumber) ? DBNull.Value : user.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Address",
                        string.IsNullOrEmpty(user.Address) ? DBNull.Value : user.Address);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int userId)
        {
            using (var con = new SqlConnection(connStr))
            {
                con.Open();
                var query = "DELETE FROM Users WHERE Id = @UserId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Users> GetAll()
        {
            var users = new List<Users>();
            using (var con = new SqlConnection(connStr))
            {
                con.Open();
                var query = "SELECT * FROM Users";
                using (var cmd = new SqlCommand(query, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new Users
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserName = reader["UserName"].ToString() ?? string.Empty,
                                Password = reader["Password"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                                FullName = reader["FullName"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                Address = reader["Address"].ToString(),
                                Role = (ESRoleUser)Enum.Parse(typeof(ESRoleUser), reader["Role"].ToString() ?? "Admin"),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }

        public Users? GetById(int userId)
        {
            var user = null as Users;
            using (var con = new SqlConnection(connStr))
            {
                con.Open();
                var query = "SELECT * FROM Users WHERE Id = @UserId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new Users
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserName = reader["UserName"].ToString() ?? string.Empty,
                                Password = reader["Password"].ToString() ?? string.Empty,
                                Email = reader["Email"].ToString() ?? string.Empty,
                                FullName = reader["FullName"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                Address = reader["Address"].ToString(),
                                Role = (ESRoleUser)Enum.Parse(typeof(ESRoleUser), reader["Role"].ToString() ?? "Admin"),
                                CreatedDate = Convert.ToDateTime(reader["CreatedDate"])
                            };
                        }
                    }
                }
            }
            return user;
        }

        public void Update(Users user)
        {
            using (var con = new SqlConnection(connStr))
            {
                con.Open();

                var query = @"UPDATE Users 
                      SET UserName = @UserName,
                          Password = @Password,
                          Email = @Email,
                          FullName = @FullName,
                          PhoneNumber = @PhoneNumber,
                          Address = @Address,
                          Role = @Role,
                          CreatedDate = @CreatedDate
                      WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", user.Id);
                    cmd.Parameters.AddWithValue("@UserName", user.UserName);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@FullName", (object?)user.FullName ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PhoneNumber", (object?)user.PhoneNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Address", (object?)user.Address ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
