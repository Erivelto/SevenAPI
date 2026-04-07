using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public class SupervisorRepository : ISupervisorRepository
    {
        private readonly DapperContext _context;

        public SupervisorRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supervisor>> GetAllAsync()
        {
            var query = "SELECT * FROM Supervisor ORDER BY Nome";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Supervisor>(query);
        }

        public async Task<Supervisor?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Supervisor WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Supervisor>(query, new { Id = id });
        }

        public async Task<int> CreateAsync(Supervisor supervisor)
        {
            var query = "INSERT INTO Supervisor (Nome) VALUES (@Nome); SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, supervisor);
        }

        public async Task<bool> UpdateAsync(Supervisor supervisor)
        {
            var query = "UPDATE Supervisor SET Nome = @Nome WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, supervisor);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Supervisor WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

