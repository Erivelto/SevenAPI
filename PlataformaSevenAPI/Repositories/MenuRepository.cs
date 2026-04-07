using Dapper;
using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public interface IMenuRepository
    {
        Task<IEnumerable<Models.Menu>> GetAllMenusAsync();
        Task<IEnumerable<SubMenu>> GetAllSubMenusAsync();
        Task<IEnumerable<SubMenu>> GetSubMenusByMenuAsync(int codigoMenu);
        Task<IEnumerable<MenuComSubMenus>> GetMenuAdminAsync();
        Task<IEnumerable<MenuComSubMenus>> GetMenuPorPermissaoAsync(int idUsuario);
    }

    public class MenuRepository : IMenuRepository
    {
        private readonly DapperContext _context;

        public MenuRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Models.Menu>> GetAllMenusAsync()
        {
            var query = "SELECT * FROM Menu WHERE Ativo = 1 ORDER BY Ordem";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Models.Menu>(query);
        }

        public async Task<IEnumerable<SubMenu>> GetAllSubMenusAsync()
        {
            var query = "SELECT * FROM SubMenu WHERE Ativo = 1 ORDER BY CodigoMenu, Ordem";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<SubMenu>(query);
        }

        public async Task<IEnumerable<SubMenu>> GetSubMenusByMenuAsync(int codigoMenu)
        {
            var query = "SELECT * FROM SubMenu WHERE CodigoMenu = @CodigoMenu AND Ativo = 1 ORDER BY Ordem";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<SubMenu>(query, new { CodigoMenu = codigoMenu });
        }

        /// <summary>
        /// Retorna todos os menus e submenus ativos para o perfil Admin (sem restricao de permissao).
        /// SubMenus com PerfilAcesso preenchido sao incluidos normalmente.
        /// </summary>
        public async Task<IEnumerable<MenuComSubMenus>> GetMenuAdminAsync()
        {
            var query = @"
                SELECT
                    m.Codigo     AS Codigo,
                    m.Descricao  AS Descricao,
                    m.Icone      AS Icone,
                    m.Url        AS Url,
                    m.Ordem      AS Ordem,
                    s.Codigo     AS Codigo,
                    s.CodigoMenu AS CodigoMenu,
                    s.Descricao  AS Descricao,
                    s.Icone      AS Icone,
                    s.Url        AS Url,
                    s.Ordem      AS Ordem,
                    s.Ativo      AS Ativo,
                    s.PerfilAcesso AS PerfilAcesso,
                    s.DataCriacao  AS DataCriacao
                FROM Menu m
                LEFT JOIN SubMenu s ON s.CodigoMenu = m.Codigo AND s.Ativo = 1
                WHERE m.Ativo = 1
                ORDER BY m.Ordem, s.Ordem";

            return await ExecutarQueryMenuAsync(query, null);
        }

        /// <summary>
        /// Retorna apenas os menus e submenus que o usuario tem permissao de acesso
        /// registrada na tabela UsuarioPermissao.
        /// </summary>
        public async Task<IEnumerable<MenuComSubMenus>> GetMenuPorPermissaoAsync(int idUsuario)
        {
            var query = @"
                SELECT
                    m.Codigo     AS Codigo,
                    m.Descricao  AS Descricao,
                    m.Icone      AS Icone,
                    m.Url        AS Url,
                    m.Ordem      AS Ordem,
                    s.Codigo     AS Codigo,
                    s.CodigoMenu AS CodigoMenu,
                    s.Descricao  AS Descricao,
                    s.Icone      AS Icone,
                    s.Url        AS Url,
                    s.Ordem      AS Ordem,
                    s.Ativo      AS Ativo,
                    s.PerfilAcesso AS PerfilAcesso,
                    s.DataCriacao  AS DataCriacao
                FROM Menu m
                INNER JOIN SubMenu s
                    ON s.CodigoMenu = m.Codigo AND s.Ativo = 1
                INNER JOIN UsuarioPermissao up
                    ON up.CodigoSubMenu = s.Codigo
                   AND up.IdUsuario = @IdUsuario
                   AND up.Ativo = 1
                WHERE m.Ativo = 1
                ORDER BY m.Ordem, s.Ordem";

            return await ExecutarQueryMenuAsync(query, new { IdUsuario = idUsuario });
        }

        private async Task<IEnumerable<MenuComSubMenus>> ExecutarQueryMenuAsync(string query, object? param)
        {
            using var connection = _context.CreateConnection();
            var menuDict = new Dictionary<int, MenuComSubMenus>();

            await connection.QueryAsync<MenuComSubMenus, SubMenu, MenuComSubMenus>(
                query,
                (menu, subMenu) =>
                {
                    if (!menuDict.TryGetValue(menu.Codigo, out var entry))
                    {
                        entry = menu;
                        menuDict[menu.Codigo] = entry;
                    }

                    if (subMenu?.Codigo > 0)
                        entry.SubMenus.Add(subMenu);

                    return entry;
                },
                param,
                splitOn: "Codigo"
            );

            return menuDict.Values;
        }
    }
}
