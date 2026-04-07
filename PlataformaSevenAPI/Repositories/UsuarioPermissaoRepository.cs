using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public class UsuarioPermissaoRepository : IUsuarioPermissaoRepository
    {
        private readonly DapperContext _context;

        public UsuarioPermissaoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UsuarioPermissaoDetalhe>> GetAllAsync()
        {
            var query = @"
                SELECT
                    up.Id,
                    up.IdUsuario,
                    up.CodigoSubMenu,
                    s.Descricao  AS SubMenuDescricao,
                    s.Icone      AS SubMenuIcone,
                    s.Url        AS SubMenuUrl,
                    s.Ordem      AS SubMenuOrdem,
                    m.Codigo     AS CodigoMenu,
                    m.Descricao  AS MenuDescricao,
                    m.Icone      AS MenuIcone,
                    m.Ordem      AS MenuOrdem,
                    up.ApenasLeitura,
                    up.Ativo,
                    up.DataCadastro
                FROM UsuarioPermissao up
                INNER JOIN SubMenu s ON s.Codigo = up.CodigoSubMenu
                INNER JOIN Menu    m ON m.Codigo = s.CodigoMenu
                ORDER BY up.IdUsuario, m.Ordem, s.Ordem";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<UsuarioPermissaoDetalhe>(query);
        }

        public async Task<UsuarioPermissao?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM UsuarioPermissao WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UsuarioPermissao>(query, new { Id = id });
        }

        public async Task<IEnumerable<UsuarioPermissaoDetalhe>> GetByUsuarioIdAsync(int idUsuario)
        {
            var query = @"
                SELECT
                    up.Id,
                    up.IdUsuario,
                    up.CodigoSubMenu,
                    s.Descricao  AS SubMenuDescricao,
                    s.Icone      AS SubMenuIcone,
                    s.Url        AS SubMenuUrl,
                    s.Ordem      AS SubMenuOrdem,
                    m.Codigo     AS CodigoMenu,
                    m.Descricao  AS MenuDescricao,
                    m.Icone      AS MenuIcone,
                    m.Ordem      AS MenuOrdem,
                    up.ApenasLeitura,
                    up.Ativo,
                    up.DataCadastro
                FROM UsuarioPermissao up
                INNER JOIN SubMenu s ON s.Codigo = up.CodigoSubMenu
                INNER JOIN Menu    m ON m.Codigo = s.CodigoMenu
                WHERE up.IdUsuario = @IdUsuario AND up.Ativo = 1
                ORDER BY m.Ordem, s.Ordem";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<UsuarioPermissaoDetalhe>(query, new { IdUsuario = idUsuario });
        }

        public async Task<UsuarioPermissao?> GetByUsuarioAndSubMenuAsync(int idUsuario, int codigoSubMenu)
        {
            var query = @"
                SELECT *
                FROM UsuarioPermissao
                WHERE IdUsuario = @IdUsuario AND CodigoSubMenu = @CodigoSubMenu AND Ativo = 1";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UsuarioPermissao>(query, new { IdUsuario = idUsuario, CodigoSubMenu = codigoSubMenu });
        }

        public async Task<UsuarioPermissao?> GetByUsuarioAndSubMenuUrlAsync(int idUsuario, string subMenuUrl)
        {
            var query = @"
                SELECT up.*
                FROM UsuarioPermissao up
                INNER JOIN SubMenu s ON s.Codigo = up.CodigoSubMenu
                WHERE up.IdUsuario = @IdUsuario
                  AND up.Ativo = 1
                  AND s.Url = @Url";

            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UsuarioPermissao>(query, new { IdUsuario = idUsuario, Url = subMenuUrl });
        }

        public async Task<int> CreateAsync(UsuarioPermissao permissao)
        {
            var query = @"
                INSERT INTO UsuarioPermissao (IdUsuario, CodigoSubMenu, ApenasLeitura, Ativo, DataCadastro)
                VALUES (@IdUsuario, @CodigoSubMenu, @ApenasLeitura, @Ativo, @DataCadastro);
                SELECT CAST(SCOPE_IDENTITY() AS int)";

            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, permissao);
        }

        /// <summary>
        /// Remove todas as permissoes existentes do usuario e insere as novas em uma unica transacao.
        /// </summary>
        public async Task<bool> SalvarPermissoesLoteAsync(int idUsuario, List<UsuarioPermissaoItem> permissoes)
        {
            using var connection = _context.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                await connection.ExecuteAsync(
                    "DELETE FROM UsuarioPermissao WHERE IdUsuario = @IdUsuario",
                    new { IdUsuario = idUsuario },
                    transaction);

                if (permissoes.Count > 0)
                {
                    var rows = permissoes.Select(p => new
                    {
                        IdUsuario     = idUsuario,
                        p.CodigoSubMenu,
                        p.ApenasLeitura,
                        Ativo         = true,
                        DataCadastro  = DateTime.Now
                    });

                    await connection.ExecuteAsync(@"
                        INSERT INTO UsuarioPermissao (IdUsuario, CodigoSubMenu, ApenasLeitura, Ativo, DataCadastro)
                        VALUES (@IdUsuario, @CodigoSubMenu, @ApenasLeitura, @Ativo, @DataCadastro)",
                        rows,
                        transaction);
                }

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM UsuarioPermissao WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new { Id = id });
            return rows > 0;
        }

        public async Task<bool> DeleteByUsuarioIdAsync(int idUsuario)
        {
            var query = "DELETE FROM UsuarioPermissao WHERE IdUsuario = @IdUsuario";
            using var connection = _context.CreateConnection();
            var rows = await connection.ExecuteAsync(query, new { IdUsuario = idUsuario });
            return rows > 0;
        }
    }
}
