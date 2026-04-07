using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public class DiariaDisponivelRepository : IDiariaDisponivelRepository
    {
        private readonly DapperContext _context;

        public DiariaDisponivelRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DiariaDisponivel>> GetAllAsync()
        {
            var query = "SELECT * FROM DiariaDisponivel WHERE Excluido = 0";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DiariaDisponivel>(query);
        }

        public async Task<DiariaDisponivel?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM DiariaDisponivel WHERE Id = @Id AND Excluido = 0";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<DiariaDisponivel>(query, new { Id = id });
        }

        public async Task<IEnumerable<DiariaDisponivel>> GetByFuncaoIdAsync(int idFuncao)
        {
            var query = "SELECT * FROM DiariaDisponivel WHERE IdFuncao = @IdFuncao AND Excluido = 0";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DiariaDisponivel>(query, new { IdFuncao = idFuncao });
        }

        public async Task<IEnumerable<DiariaDisponivel>> GetByPostoIdAsync(int idPosto)
        {
            var query = "SELECT * FROM DiariaDisponivel WHERE IdPosto = @IdPosto AND Excluido = 0";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DiariaDisponivel>(query, new { IdPosto = idPosto });
        }

        public async Task<IEnumerable<DiariaDisponivel>> GetBySupervisorIdAsync(int idSupervisor)
        {
            var query = "SELECT * FROM DiariaDisponivel WHERE IdSupervisor = @IdSupervisor AND Excluido = 0";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DiariaDisponivel>(query, new { IdSupervisor = idSupervisor });
        }

        public async Task<IEnumerable<DiariaDisponivel>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            var query = @"SELECT * FROM DiariaDisponivel 
                          WHERE DataReferenciaInical >= @DataInicio 
                            AND DataReferenciaFinal  <= @DataFim 
                            AND Excluido = 0
                          ORDER BY DataReferenciaInical";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DiariaDisponivel>(query, new { DataInicio = dataInicio, DataFim = dataFim });
        }

        public async Task<IEnumerable<DiariaDisponivelResponse>> GetListaDisponivelAsync()
        {
            var query = @"SELECT
                              dd.[Id],
                              dd.[QuantidadeDiaria],
                              dd.[DataReferenciaInical],
                              dd.[DataReferenciaFinal],
                              f.Nome  AS Funcao,
                              s.Nome  AS Supervisor,
                              p.Nome  AS Posto
                          FROM [dbo].[DiariaDisponivel] AS dd
                          INNER JOIN Funcao     AS f ON dd.IdFuncao     = f.Id
                          INNER JOIN Supervisor AS s ON dd.IdSupervisor = s.Id
                          INNER JOIN Posto      AS p ON dd.IdPosto      = p.Id
                          WHERE dd.Excluido = 0
                            AND MONTH(dd.DataReferenciaInical) = MONTH(GETDATE())
                            AND YEAR(dd.DataReferenciaInical)  = YEAR(GETDATE())
                          ORDER BY dd.Id DESC";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DiariaDisponivelResponse>(query);
        }

        public async Task<int> CreateAsync(DiariaDisponivel diariaDisponivel)
        {
            var query = @"
                INSERT INTO DiariaDisponivel 
                    (QuantidadeDiaria, DataReferenciaInical, DataReferenciaFinal, IdFuncao, IdSupervisor, IdPosto, UsuarioCadAlt, DataCadastro, DataAlteracao, Excluido)
                VALUES 
                    (@QuantidadeDiaria, @DataReferenciaInical, @DataReferenciaFinal, @IdFuncao, @IdSupervisor, @IdPosto, @UsuarioCadAlt, @DataCadastro, @DataAlteracao, @Excluido);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, diariaDisponivel);
        }

        public async Task<bool> UpdateAsync(DiariaDisponivel diariaDisponivel)
        {
            var query = @"
                UPDATE DiariaDisponivel 
                SET QuantidadeDiaria      = @QuantidadeDiaria,
                    DataReferenciaInical  = @DataReferenciaInical,
                    DataReferenciaFinal   = @DataReferenciaFinal,
                    IdFuncao              = @IdFuncao,
                    IdSupervisor          = @IdSupervisor,
                    IdPosto               = @IdPosto,
                    UsuarioCadAlt         = @UsuarioCadAlt,
                    DataAlteracao         = @DataAlteracao
                WHERE Id = @Id AND Excluido = 0";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, diariaDisponivel);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "UPDATE DiariaDisponivel SET Excluido = 1, DataAlteracao = GETDATE() WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}
