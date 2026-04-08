using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public class AprovacaoRepository : IAprovacaoRepository
    {
        private readonly DapperContext _context;

        public AprovacaoRepository(DapperContext context)
        {
            _context = context;
        }

        // ==================== Config ====================

        public async Task<IEnumerable<AprovacaoConfig>> GetAllConfigsAsync()
        {
            var query = "SELECT * FROM AprovacaoConfig ORDER BY Entidade";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<AprovacaoConfig>(query);
        }

        public async Task<IEnumerable<AprovacaoConfig>> GetConfigsByEntidadeAsync(string entidade)
        {
            var query = "SELECT * FROM AprovacaoConfig WHERE Entidade = @Entidade AND Ativo = 1";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<AprovacaoConfig>(query, new { Entidade = entidade });
        }

        public async Task<AprovacaoConfig?> GetConfigByIdAsync(int id)
        {
            var query = "SELECT * FROM AprovacaoConfig WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AprovacaoConfig>(query, new { Id = id });
        }

        public async Task<int> CreateConfigAsync(AprovacaoConfig config)
        {
            var query = @"
                INSERT INTO AprovacaoConfig (Entidade, IdUsuarioAprovador, Ativo, DataCadastro)
                VALUES (@Entidade, @IdUsuarioAprovador, @Ativo, @DataCadastro);
                SELECT CAST(SCOPE_IDENTITY() AS int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, config);
        }

        public async Task<bool> DeleteConfigAsync(int id)
        {
            var query = "UPDATE AprovacaoConfig SET Ativo = 0 WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new { Id = id });
            return rows > 0;
        }

        public async Task<IEnumerable<AprovacaoConfig>> GetAprovadoresAtivosAsync(string entidade)
        {
            var query = "SELECT * FROM AprovacaoConfig WHERE Entidade = @Entidade AND Ativo = 1";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<AprovacaoConfig>(query, new { Entidade = entidade });
        }

        // ==================== Stage ====================

        public async Task<int> CreateStageAsync(AprovacaoStage stage)
        {
            var query = @"
                INSERT INTO AprovacaoStage
                    (Entidade, PayloadJson, IdUsuarioSolicitante, IdUsuarioAprovador, Status, DataSolicitacao)
                VALUES
                    (@Entidade, @PayloadJson, @IdUsuarioSolicitante, @IdUsuarioAprovador, @Status, @DataSolicitacao);
                SELECT CAST(SCOPE_IDENTITY() AS int)";
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, stage);
        }

        public async Task<AprovacaoStage?> GetStageByIdAsync(int id)
        {
            var query = "SELECT * FROM AprovacaoStage WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AprovacaoStage>(query, new { Id = id });
        }

        public async Task<IEnumerable<AprovacaoStageDetalhe>> GetPendentesPorAprovadorAsync(int idUsuarioAprovador)
        {
            var query = @"
                SELECT
                    s.Id, s.Entidade, s.PayloadJson,
                    s.IdUsuarioSolicitante, us.[User] AS NomeSolicitante,
                    s.IdUsuarioAprovador,  ua.[User] AS NomeAprovador,
                    s.Status, s.DataSolicitacao, s.DataAprovacao,
                    s.Observacao, s.IdRegistroGerado
                FROM AprovacaoStage s
                INNER JOIN Usuario us ON us.Id = s.IdUsuarioSolicitante
                INNER JOIN Usuario ua ON ua.Id = s.IdUsuarioAprovador
                WHERE s.IdUsuarioAprovador = @IdUsuarioAprovador
                  AND s.Status = 'Pendente'
                ORDER BY s.DataSolicitacao";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<AprovacaoStageDetalhe>(query, new { IdUsuarioAprovador = idUsuarioAprovador });
        }

        public async Task<IEnumerable<AprovacaoStageDetalhe>> GetPorSolicitanteAsync(int idUsuarioSolicitante)
        {
            var query = @"
                SELECT
                    s.Id, s.Entidade, s.PayloadJson,
                    s.IdUsuarioSolicitante, us.[User] AS NomeSolicitante,
                    s.IdUsuarioAprovador,  ua.[User] AS NomeAprovador,
                    s.Status, s.DataSolicitacao, s.DataAprovacao,
                    s.Observacao, s.IdRegistroGerado
                FROM AprovacaoStage s
                INNER JOIN Usuario us ON us.Id = s.IdUsuarioSolicitante
                INNER JOIN Usuario ua ON ua.Id = s.IdUsuarioAprovador
                WHERE s.IdUsuarioSolicitante = @IdUsuarioSolicitante
                ORDER BY s.DataSolicitacao DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<AprovacaoStageDetalhe>(query, new { IdUsuarioSolicitante = idUsuarioSolicitante });
        }

        public async Task<IEnumerable<AprovacaoStageDetalhe>> GetAllStagesAsync(string? entidade, string? status)
        {
            var query = @"
                SELECT
                    s.Id, s.Entidade, s.PayloadJson,
                    s.IdUsuarioSolicitante, us.[User] AS NomeSolicitante,
                    s.IdUsuarioAprovador,  ua.[User] AS NomeAprovador,
                    s.Status, s.DataSolicitacao, s.DataAprovacao,
                    s.Observacao, s.IdRegistroGerado
                FROM AprovacaoStage s
                INNER JOIN Usuario us ON us.Id = s.IdUsuarioSolicitante
                INNER JOIN Usuario ua ON ua.Id = s.IdUsuarioAprovador
                WHERE (@Entidade IS NULL OR s.Entidade = @Entidade)
                  AND (@Status  IS NULL OR s.Status   = @Status)
                ORDER BY s.DataSolicitacao DESC";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<AprovacaoStageDetalhe>(query, new { Entidade = entidade, Status = status });
        }

        public async Task<bool> AprovarAsync(int id, int idRegistroGerado, string? observacao)
        {
            var query = @"
                UPDATE AprovacaoStage
                SET Status           = 'Aprovado',
                    DataAprovacao    = @DataAprovacao,
                    Observacao       = @Observacao,
                    IdRegistroGerado = @IdRegistroGerado
                WHERE Id = @Id AND Status = 'Pendente'";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new
            {
                Id               = id,
                DataAprovacao    = DateTime.Now,
                Observacao       = observacao,
                IdRegistroGerado = idRegistroGerado
            });
            return rows > 0;
        }

        public async Task<bool> RejeitarAsync(int id, string observacao)
        {
            var query = @"
                UPDATE AprovacaoStage
                SET Status        = 'Rejeitado',
                    DataAprovacao = @DataAprovacao,
                    Observacao    = @Observacao
                WHERE Id = @Id AND Status = 'Pendente'";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new
            {
                Id            = id,
                DataAprovacao = DateTime.Now,
                Observacao    = observacao
            });
            return rows > 0;
        }
    }
}
