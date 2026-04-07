using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public class DiariaDescontoRepository : IDiariaDescontoRepository
    {
        private readonly DapperContext _context;

        public DiariaDescontoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DiariaDesconto>> GetAllAsync()
        {
            var query = "SELECT * FROM DiariaDesconto ORDER BY Data DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DiariaDesconto>(query);
        }

        public async Task<DiariaDesconto?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM DiariaDesconto WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<DiariaDesconto>(query, new { Id = id });
        }

        public async Task<IEnumerable<DiariaDesconto>> GetByColaboradorIdAsync(int idColaborador)
        {
            var query = "SELECT * FROM DiariaDesconto WHERE IdColaborador = @IdColaborador ORDER BY Data DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DiariaDesconto>(query, new { IdColaborador = idColaborador });
        }

        public async Task<IEnumerable<DiariaDesconto>> GetByPeriodoAsync(int idColaborador, DateTime dataInicio, DateTime dataFim)
        {
            var query = @"SELECT * FROM DiariaDesconto 
                          WHERE IdColaborador = @IdColaborador
                            AND Data >= @DataInicio 
                            AND Data <= @DataFim
                          ORDER BY Data DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DiariaDesconto>(query, new { IdColaborador = idColaborador, DataInicio = dataInicio, DataFim = dataFim });
        }

        public async Task<int> CreateAsync(DiariaDesconto diariaDesconto)
        {
            var query = @"
                INSERT INTO DiariaDesconto (IdColaborador, Data, UserCadastro, Valor)
                VALUES (@IdColaborador, @Data, @UserCadastro, @Valor);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, diariaDesconto);
        }

        public async Task<bool> UpdateAsync(DiariaDesconto diariaDesconto)
        {
            var query = @"
                UPDATE DiariaDesconto 
                SET IdColaborador = @IdColaborador,
                    Data          = @Data,
                    UserCadastro  = @UserCadastro,
                    Valor         = @Valor
                WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, diariaDesconto);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM DiariaDesconto WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}
