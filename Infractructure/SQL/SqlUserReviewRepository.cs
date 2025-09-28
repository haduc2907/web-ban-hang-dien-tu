using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using UseCase.User_side;

namespace Infractructure.SQL
{
    public class SqlUserReviewRepository : IUserReviewControllerRepository
    {
        private readonly string? strCnn;

        public SqlUserReviewRepository(IConfiguration configuration)
        {
            strCnn = configuration.GetConnectionString("DefaultConnection");
        }

        // CREATE
        public void Add(Reviews review)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = @"INSERT INTO Reviews (ProductId, UserId, UserName, Rating, Comment, CreatedDate)
                              VALUES (@ProductId, @UserId, @UserName, @Rating, @Comment, @CreatedDate)";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ProductId", review.ProductId);
                    cmd.Parameters.AddWithValue("@UserId", review.UserId);
                    cmd.Parameters.AddWithValue("@UserName", review.UserName);
                    cmd.Parameters.AddWithValue("@Rating", review.Rating);
                    cmd.Parameters.AddWithValue("@Comment", (object?)review.Comment ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedDate", review.CreatedDate);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // READ ALL
        public List<Reviews> GetAll()
        {
            var list = new List<Reviews>();

            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "SELECT * FROM Reviews";

                using (var cmd = new SqlCommand(query, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Reviews
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            UserName = reader.GetString(reader.GetOrdinal("UserName")),
                            Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                            Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? null : reader.GetString(reader.GetOrdinal("Comment")),
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                        });
                    }
                }
            }

            return list;
        }

        // READ BY ID
        public Reviews? GetById(int id)
        {
            var review = null as Reviews;

            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "SELECT * FROM Reviews WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            review = new Reviews
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                UserName = reader.GetString(reader.GetOrdinal("UserName")),
                                Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                                Comment = reader.IsDBNull(reader.GetOrdinal("Comment")) ? null : reader.GetString(reader.GetOrdinal("Comment")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                            };
                        }
                    }
                }
            }

            return review;
        }

        // UPDATE
        public void Update(Reviews review)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = @"UPDATE Reviews
                              SET ProductId = @ProductId,
                                  UserId = @UserId,
                                  UserName = @UserName,
                                  Rating = @Rating,
                                  Comment = @Comment,
                                  CreatedDate = @CreatedDate
                              WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", review.Id);
                    cmd.Parameters.AddWithValue("@ProductId", review.ProductId);
                    cmd.Parameters.AddWithValue("@UserId", review.UserId);
                    cmd.Parameters.AddWithValue("@UserName", review.UserName);
                    cmd.Parameters.AddWithValue("@Rating", review.Rating);
                    cmd.Parameters.AddWithValue("@Comment", (object?)review.Comment ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedDate", review.CreatedDate);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE
        public void Delete(int id)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "DELETE FROM Reviews WHERE Id = @Id";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
