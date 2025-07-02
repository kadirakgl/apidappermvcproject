using Dapper;
using apidappermvcproje.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apidappermvcproje.Data
{
    public class OrderRepository
    {
        private readonly DapperContext _context;
        public OrderRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            var query = @"
                SELECT o.*, c.FirstName, c.LastName, c.Email, p.Name as ProductName, p.Price as ProductPrice
                FROM Orders o
                LEFT JOIN Customers c ON o.CustomerId = c.Id
                LEFT JOIN Products p ON o.ProductId = p.Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Order, Customer, Product, Order>(query, 
                    (order, customer, product) =>
                    {
                        order.Customer = customer;
                        order.Product = product;
                        return order;
                    }, splitOn: "FirstName,ProductName");
            }
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            var query = @"
                SELECT o.*, c.FirstName, c.LastName, c.Email, p.Name as ProductName, p.Price as ProductPrice
                FROM Orders o
                LEFT JOIN Customers c ON o.CustomerId = c.Id
                LEFT JOIN Products p ON o.ProductId = p.Id
                WHERE o.Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                var orders = await connection.QueryAsync<Order, Customer, Product, Order>(query, 
                    (order, customer, product) =>
                    {
                        order.Customer = customer;
                        order.Product = product;
                        return order;
                    }, new { Id = id }, splitOn: "FirstName,ProductName");
                return orders.FirstOrDefault();
            }
        }

        public async Task<int> CreateAsync(Order order)
        {
            var query = "INSERT INTO Orders (CustomerId, ProductId, Quantity, TotalPrice, OrderDate, Status) VALUES (@CustomerId, @ProductId, @Quantity, @TotalPrice, @OrderDate, @Status); SELECT last_insert_rowid();";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(query, order);
            }
        }

        public async Task<int> UpdateAsync(Order order)
        {
            var query = "UPDATE Orders SET CustomerId = @CustomerId, ProductId = @ProductId, Quantity = @Quantity, TotalPrice = @TotalPrice, Status = @Status WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, order);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM Orders WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
} 