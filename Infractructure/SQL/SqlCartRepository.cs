using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.User_side;

namespace Infractructure.SQL
{
    public class SqlCartRepository : ICartControllerRepository
    {
        private readonly string? strCnn;

        public SqlCartRepository(IConfiguration configuration)
        {
            strCnn = configuration.GetConnectionString("DefaultConnection");
        }
        public void AddToCart(Products product, int? userID)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "INSERT INTO CartItems (UserId, ProductId, Name, ImageUrl, Price, Quantity) " +
                            "VALUES (@UserId, @ProductId, @Name, @ImageUrl, @Price, @Quantity)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userID);
                    cmd.Parameters.AddWithValue("@ProductId", product.Id);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@ImageUrl", product.ImageUrl);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Quantity", 1);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Clear(int userId)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "DELETE FROM CartItems WHERE UserId = @UserId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id, int userId)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "DELETE FROM CartItems WHERE UserId = @UserId AND ProductId = @ProductId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<CartItems> GetAll(int? userId)
        {
            var carts = new List<CartItems>();
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "SELECT * FROM CartItems WHERE UserId = @UserId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            carts.Add(new CartItems
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity"))
                            });
                        }
                    }
                }
            }
            return carts;
        }

        public CartItems? GetById(int productId, int userId)
        {
            var cartItems = null as CartItems;
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "SELECT * FROM CartItems WHERE UserId = @UserId AND ProductId = @ProductId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cartItems = new CartItems()
                            {
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };

                        }
                    }
                }
            }
            return cartItems;
        }

        public void UpdateQuantity(int id, int quantity, int userId)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "UPDATE CartItems SET Quantity = Quantity + @Quantity " +
                            "WHERE UserId = @UserId AND ProductId = @ProductId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@ProductId", id);
                    cmd.Parameters.AddWithValue("@Quantity", quantity);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
