using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Admin_side;
using UseCase.User_side;

namespace Infractructure.SQL
{
    public class SqlProductRepository : IAdminProductControllerRepository, IUserProductControllerRepository
    {
        private readonly string? strCnn;

        public SqlProductRepository(IConfiguration configuration)
        {
            strCnn = configuration.GetConnectionString("DefaultConnection");
        }
        public void Add(Products product)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "INSERT INTO Products (Name, Description, Price, ImageUrl, Quantity, Status, CreatedDate, UpdatedDate, Brand, CategoryId) " +
                                      "VALUES (@Name, @Description, @Price, @ImageUrl, @Quantity, @Status, @CreatedDate, @UpdatedDate, @Brand, @CategoryId)";
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Description",
    string.IsNullOrEmpty(product.Description) ? DBNull.Value : product.Description);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@ImageUrl", product.ImageUrl);
                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                    cmd.Parameters.AddWithValue("@Status", product.Status);
                    cmd.Parameters.AddWithValue("@CreatedDate", product.CreatedDate);
                    cmd.Parameters.AddWithValue("@UpdatedDate", product.UpdatedDate);
                    cmd.Parameters.AddWithValue("@Brand", product.Brand);
                    cmd.Parameters.AddWithValue("@CategoryId", (object?)product.CategoryId ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int Id)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "DELETE FROM Products WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Products> Filters(IEnumerable<Products> source, ProductFilterOptions options)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    var query = new StringBuilder("SELECT * FROM Products WHERE 1=1");
                    if (!string.IsNullOrEmpty(options.Keyword))
                    {
                        query.Append(" AND Name LIKE @Keyword");
                        cmd.Parameters.AddWithValue("@Keyword", $"%{options.Keyword}%");
                    }
                    if (!string.IsNullOrEmpty(options.Brand))
                    {
                        query.Append(" AND Brand LIKE @Brand");
                        cmd.Parameters.AddWithValue("@Brand", $"%{options.Brand}%");
                    }
                    if (options.MinPrice.HasValue)
                    {
                        query.Append(" AND Price >= @MinPrice");
                        cmd.Parameters.AddWithValue("@MinPrice", options.MinPrice.Value);
                    }
                    if (options.MaxPrice.HasValue)
                    {
                        query.Append(" AND Price <= @MaxPrice");
                        cmd.Parameters.AddWithValue("@MaxPrice", options.MaxPrice.Value);
                    }
                    if (options.Status.HasValue)
                    {
                        query.Append(" AND Status = @Status");
                        cmd.Parameters.AddWithValue("@Status", options.Status.Value);
                    }
                    if (options.CategoryId.HasValue)
                    {
                        query.Append(" AND CategoryId = @CategoryId");
                        cmd.Parameters.AddWithValue("@CategoryId", options.CategoryId.Value);
                    }
                    cmd.CommandText = query.ToString();
                    using (var reader = cmd.ExecuteReader())
                    {
                        var products = new List<Products>();
                        while (reader.Read())
                        {
                            var product = new Products
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                Status = (EStatusProduct)reader.GetInt32(reader.GetOrdinal("Status")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                            };
                            products.Add(product);
                        }
                        return products;
                    }
                }
            }
        }



        public IEnumerable<Products> GetAll()
        {
            var products = new List<Products>();
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Products", con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Products
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                Status = (EStatusProduct)reader.GetInt32(reader.GetOrdinal("Status")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        public IEnumerable<Products> GetByCategoryId(int categoryId)
        {
            var products = new List<Products>();
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Products WHERE CategoryId = @CategoryId", con))
                {
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Products
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                Status = (EStatusProduct)reader.GetInt32(reader.GetOrdinal("Status")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                            };
                            products.Add(product);
                        }
                    }
                }
            }
            return products;
        }

        public Products? GetById(int Id)
        {
            var product = null as Products;
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                using (var cmd = new SqlCommand("SELECT * FROM Products WHERE Id = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Products
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                Status = (EStatusProduct)reader.GetInt32(reader.GetOrdinal("Status")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                UpdatedDate = reader.GetDateTime(reader.GetOrdinal("UpdatedDate")),
                                Brand = reader.GetString(reader.GetOrdinal("Brand")),
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId"))
                            };
                        }
                    }
                }
            }
            return product;
        }

        public void Update(Products product)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "UPDATE Products SET Name = @Name, Description = @Description, Price = @Price, ImageUrl = @ImageUrl, " +
                                      "Quantity = @Quantity, Status = @Status, UpdatedDate = @UpdatedDate, Brand = @Brand, CategoryId = @CategoryId " +
                                      "WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Description",
        string.IsNullOrEmpty(product.Description) ? DBNull.Value : product.Description);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@ImageUrl", product.ImageUrl);
                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                    cmd.Parameters.AddWithValue("@Status", product.Status);
                    cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Brand", product.Brand);
                    cmd.Parameters.AddWithValue("@CategoryId",(object?)product.CategoryId ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
