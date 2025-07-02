using Dapper;
using apidappermvcproje.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apidappermvcproje.Data
{
    public class ProductRepository
    {
        private readonly DapperContext _context;
        public ProductRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var query = @"
                SELECT p.*, c.Name as CategoryName, c.Description as CategoryDescription
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryId = c.Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Product, Category, Product>(query, 
                    (product, category) =>
                    {
                        product.Category = category;
                        return product;
                    }, splitOn: "CategoryName");
            }
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var query = @"
                SELECT p.*, c.Name as CategoryName, c.Description as CategoryDescription
                FROM Products p
                LEFT JOIN Categories c ON p.CategoryId = c.Id
                WHERE p.Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                var products = await connection.QueryAsync<Product, Category, Product>(query, 
                    (product, category) =>
                    {
                        product.Category = category;
                        return product;
                    }, new { Id = id }, splitOn: "CategoryName");
                return products.FirstOrDefault();
            }
        }

        public async Task<int> CreateAsync(Product product)
        {
            var query = "INSERT INTO Products (Name, Price, CategoryId, Description) VALUES (@Name, @Price, @CategoryId, @Description); SELECT last_insert_rowid();";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(query, product);
            }
        }

        public async Task<int> UpdateAsync(Product product)
        {
            var query = "UPDATE Products SET Name = @Name, Price = @Price, CategoryId = @CategoryId, Description = @Description WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, product);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM Products WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
} 