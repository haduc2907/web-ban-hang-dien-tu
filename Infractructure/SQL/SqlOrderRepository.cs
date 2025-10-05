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
    public class SqlOrderRepository : IOrderControllerRepository
    {
        private readonly string? strCnn;

        public SqlOrderRepository(IConfiguration configuration)
        {
            strCnn = configuration.GetConnectionString("DefaultConnection");
        }
        public void Add(Orders order)
        {
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = @"INSERT INTO Orders (UserId, UserName, OrderDate, TotalAmount)  OUTPUT INSERTED.Id VALUES (@UserId, @UserName, @OrderDate, @TotalAmount)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", order.UserId);
                    cmd.Parameters.AddWithValue("@UserName", order.UserName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                    cmd.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    var newId = (int)cmd.ExecuteScalar();
                    order.Id = newId;
                }
            }
        }

        public List<Orders> GetAllOrdersByUserId(int userId)
        {
            List<Orders> orders = new List<Orders>();
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "SELECT * FROM Orders WHERE UserId = @UserId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Orders order = new Orders
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                UserName = reader["UserName"].ToString() ?? "",
                                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"])
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }

        public IEnumerable<Orders> GetAllOrders()
        {
            List<Orders> orders = new List<Orders>();
            using (var con = new SqlConnection(strCnn))
            {
                con.Open();
                var query = "SELECT * FROM Orders";
                using (var cmd = new SqlCommand(query, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Orders order = new Orders
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                UserId = Convert.ToInt32(reader["UserId"]),
                                UserName = reader["UserName"].ToString() ?? "",
                                OrderDate = Convert.ToDateTime(reader["OrderDate"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"])
                            };
                            orders.Add(order);
                        }
                    }
                }
            }
            return orders;
        }


    }
}
