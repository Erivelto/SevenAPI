using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public class PostoRepository : IPostoRepository
    {
        private readonly DapperContext _context;

        public PostoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Posto>> GetAllAsync()
        {
            var query = "SELECT * FROM Posto ORDER BY Nome";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Posto>(query);
        }

        public async Task<Posto?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Posto WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Posto>(query, new { Id = id });
        }

        public async Task<int> CreateAsync(Posto posto)
        {
            var query = "INSERT INTO Posto (Nome) VALUES (@Nome); SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, posto);
        }

        public async Task<bool> UpdateAsync(Posto posto)
        {
            var query = "UPDATE Posto SET Nome = @Nome WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, posto);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Posto WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

