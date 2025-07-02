using Dapper;
using apidappermvcproje.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apidappermvcproje.Data
{
    public class CategoryRepository
    {
        private readonly DapperContext _context;
        public CategoryRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            var query = "SELECT * FROM Categories";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Category>(query);
            }
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Categories WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Category>(query, new { Id = id });
            }
        }

        public async Task<int> CreateAsync(Category category)
        {
            var query = "INSERT INTO Categories (Name, Description) VALUES (@Name, @Description); SELECT last_insert_rowid();";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(query, category);
            }
        }

        public async Task<int> UpdateAsync(Category category)
        {
            var query = "UPDATE Categories SET Name = @Name, Description = @Description WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, category);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM Categories WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
} 