using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public class DiariaRepository : IDiariaRepository
    {
        private readonly DapperContext _context;

        public DiariaRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Diaria>> GetAllAsync()
        {
            var query = "SELECT * FROM Diaria ORDER BY DataDiaria DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Diaria>(query);
        }

        public async Task<Diaria?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Diaria WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Diaria>(query, new { Id = id });
        }

        public async Task<IEnumerable<Diaria>> GetByColaboradorDetalheIdAsync(int idColaboradorDetalhe)
        {
            var query = "SELECT * FROM Diaria WHERE IdColaboradorDetalhe = @IdColaboradorDetalhe ORDER BY DataDiaria DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Diaria>(query, new { IdColaboradorDetalhe = idColaboradorDetalhe });
        }

        public async Task<IEnumerable<Diaria>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var query = "SELECT * FROM Diaria WHERE DataDiaria BETWEEN @StartDate AND @EndDate ORDER BY DataDiaria DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Diaria>(query, new { StartDate = startDate, EndDate = endDate });
        }

        public async Task<int> CreateAsync(Diaria diaria)
        {
            var query = @"
                INSERT INTO Diaria (IdColaboradorDetalhe, DataDiaria, UserCadastro)
                VALUES (@IdColaboradorDetalhe, @DataDiaria, @UserCadastro);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, diaria);
        }

        public async Task<bool> UpdateAsync(Diaria diaria)
        {
            var query = @"
                UPDATE Diaria 
                SET IdColaboradorDetalhe = @IdColaboradorDetalhe, 
                    DataDiaria = @DataDiaria, 
                    UserCadastro = @UserCadastro
                WHERE Id = @Id";
            
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, diaria);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Diaria WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

