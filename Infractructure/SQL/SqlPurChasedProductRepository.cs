using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Admin_side;

namespace Infractructure.SQL
{
    public class SqlPurChasedProductRepository : IPurchasedProductControllerRepository
    {
        private readonly string? strCnn;

        public SqlPurChasedProductRepository(IConfiguration configuration)
        {
            strCnn = configuration.GetConnectionString("DefaultConnection");
        }
        public void Add(PurchasedProducts product)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = @"INSERT INTO PurchasedProducts 
                        (UserId, UserName, ProductId, Name, ImageUrl, Price, Quantity, PurchasedDate, Status)
                      VALUES 
                        (@UserId, @UserName, @ProductId, @Name, @ImageUrl, @Price, @Quantity, @PurchasedDate, @Status)";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", product.UserId);
                    cmd.Parameters.AddWithValue("@UserName", product.UserName);
                    cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@ImageUrl", product.ImageUrl);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                    cmd.Parameters.AddWithValue("@PurchasedDate", product.PurchasedDate);
                    cmd.Parameters.AddWithValue("@Status", (int)product.Status);

                    cmd.ExecuteNonQuery();
                }

            }
        }

        public void Delete(int id)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "DELETE FROM PurchasedProducts WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<PurchasedProducts> GetAll()
        {
            var list = new List<PurchasedProducts>();

            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "SELECT * FROM PurchasedProducts";

                using (var cmd = new SqlCommand(query, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PurchasedProducts
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            PurchasedDate = reader.GetDateTime(reader.GetOrdinal("PurchasedDate")),
                            Status = (EOrderStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                        });
                    }
                }
            }

            return list;
        }

        public PurchasedProducts? GetById(int id)
        {
            var product = null as PurchasedProducts;
            using (var con = new SqlConnection(strCnn))
            {
                
                con.Open();
                var query = "SELECT * FROM PurchasedProducts WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new PurchasedProducts
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                PurchasedDate = reader.GetDateTime(reader.GetOrdinal("PurchasedDate")),
                                Status = (EOrderStatus)reader.GetInt32(reader.GetOrdinal("Status"))
                            };
                        }
                    }
                }
            }

            return product;
        }

        public void Update(PurchasedProducts product)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = @"UPDATE PurchasedProducts
                              SET UserId = @UserId,
                                  UserName = @UserName,
                                  ProductId = @ProductId,
                                  Name = @Name,
                                  ImageUrl = @ImageUrl,
                                  Price = @Price,
                                  Quantity = @Quantity,
                                  PurchasedDate = @PurchasedDate,
                                  Status = @Status
                              WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@UserId", product.UserId);
                    cmd.Parameters.AddWithValue("@UserName", product.UserName);
                    cmd.Parameters.AddWithValue("@ProductId", product.ProductId);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@ImageUrl", product.ImageUrl);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                    cmd.Parameters.AddWithValue("@PurchasedDate", product.PurchasedDate);
                    cmd.Parameters.AddWithValue("@Status", (int)product.Status);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
