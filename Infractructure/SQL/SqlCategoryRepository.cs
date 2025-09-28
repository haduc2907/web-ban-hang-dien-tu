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
    public class SqlCategoryRepository : IAdminCategoryControllerRepository
    {
        private readonly string? conStr;
        public SqlCategoryRepository(IConfiguration configuration)
        {
            conStr = configuration.GetConnectionString("DefaultConnection");
        }
        public void Add(Categories category)
        {
            using (var con = new SqlConnection(conStr))
            {
                con.Open();
                var query = "INSERT INTO Categories (Name) Values (@Name)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Name", category.Name);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int categoryId)
        {
            using (var con = new SqlConnection(conStr))
            {
                con.Open();
                var query = "DELETE FROM Categories Where Id = @Id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", categoryId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Categories> GetAll()
        {
            var categories = new List<Categories>();
            using (var con = new SqlConnection(conStr))
            {
                con.Open();
                var query = "SELECT * FROM Categories";
                using (var cmd = new SqlCommand(query, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var category = new Categories()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            categories.Add(category);
                        }
                    }
                }
            }
            return categories;
        }

        public Categories? GetById(int categoryId)
        {
            var category = null as Categories;
            using (var con = new SqlConnection(conStr))
            {
                con.Open();
                var query = "SELECT * FROM Categories WHERE Id = @Id";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", categoryId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            category = new Categories()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                    }
                }
            }
            return category;
        }

        public void Update(Categories category)
        {
            using (var con = new SqlConnection(conStr))
            {
                con.Open();

                var query = @"UPDATE Categories 
                      SET Name = @Name
                      WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", category.Id);
                    cmd.Parameters.AddWithValue("@Name", category.Name);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
