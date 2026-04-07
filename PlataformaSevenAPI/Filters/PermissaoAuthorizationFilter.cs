using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PlataformaSeven.API.Repositories;

namespace PlataformaSeven.API.Filters
{
    /// <summary>
    /// Filtro de autorizacao global.
    /// - Controllers livres: sem verificacao para qualquer perfil.
    /// - Admin (Tipo "A"): acesso total.
    /// - Demais perfis: verifica ApenasLeitura na tabela UsuarioPermissao pelo SubMenu da URL.
    ///   Se ApenasLeitura = true, apenas metodos GET sao permitidos.
    /// </summary>
    public class PermissaoAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IUsuarioPermissaoRepository _permissaoRepository;

        public PermissaoAuthorizationFilter(IUsuarioPermissaoRepository permissaoRepository)
        {
            _permissaoRepository = permissaoRepository;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
                return;

            var controllerName = context.RouteData.Values["controller"]?.ToString();
            if (string.IsNullOrEmpty(controllerName))
                return;

            var controllersLivres = new[] { "Autenticacao", "UsuarioPermissao", "Menu" };
            if (controllersLivres.Any(c => string.Equals(c, controllerName, StringComparison.OrdinalIgnoreCase)))
                return;

            var tipo = user.FindFirst("Tipo")?.Value;
            if (string.Equals(tipo, "A", StringComparison.OrdinalIgnoreCase))
                return;

            var userIdClaim = user.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            {
                context.Result = new ObjectResult(new { message = "Acesso negado. Usuario nao identificado." })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
                return;
            }

            // Obtem o path da requisicao para localizar o SubMenu correspondente
            var requestPath = context.HttpContext.Request.Path.Value ?? string.Empty;
            var permissao = await _permissaoRepository.GetByUsuarioAndSubMenuUrlAsync(userId, requestPath);

            // Se nao achou permissao pela URL exata, permite (a restricao e apenas no ApenasLeitura)
            if (permissao == null)
                return;

            // Se ApenasLeitura, bloqueia qualquer metodo que nao seja GET
            if (permissao.ApenasLeitura)
            {
                var httpMethod = context.HttpContext.Request.Method;
                if (!string.Equals(httpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    context.Result = new ObjectResult(new { message = "Acesso negado. Voce possui apenas permissao de leitura nesta pagina." })
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                }
            }
        }
    }
}
