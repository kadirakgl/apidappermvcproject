using Dapper;
using apidappermvcproje.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apidappermvcproje.Data
{
    public class CustomerRepository
    {
        private readonly DapperContext _context;
        public CustomerRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var query = "SELECT * FROM Customers";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Customer>(query);
            }
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Customers WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<Customer>(query, new { Id = id });
            }
        }

        public async Task<int> CreateAsync(Customer customer)
        {
            var query = "INSERT INTO Customers (FirstName, LastName, Email, Phone, Address) VALUES (@FirstName, @LastName, @Email, @Phone, @Address); SELECT last_insert_rowid();";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteScalarAsync<int>(query, customer);
            }
        }

        public async Task<int> UpdateAsync(Customer customer)
        {
            var query = "UPDATE Customers SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Phone = @Phone, Address = @Address WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, customer);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var query = "DELETE FROM Customers WHERE Id = @Id";
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
} 