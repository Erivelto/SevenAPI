using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public class FuncaoRepository : IFuncaoRepository
    {
        private readonly DapperContext _context;

        public FuncaoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Funcao>> GetAllAsync()
        {
            var query = "SELECT * FROM Funcao ORDER BY Nome";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Funcao>(query);
        }

        public async Task<Funcao?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Funcao WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Funcao>(query, new { Id = id });
        }

        public async Task<int> CreateAsync(Funcao funcao)
        {
            var query = "INSERT INTO Funcao (Nome) VALUES (@Nome); SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, funcao);
        }

        public async Task<bool> UpdateAsync(Funcao funcao)
        {
            var query = "UPDATE Funcao SET Nome = @Nome WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, funcao);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Funcao WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

