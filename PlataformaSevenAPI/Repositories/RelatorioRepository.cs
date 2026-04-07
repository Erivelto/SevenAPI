using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public class RelatorioRepository : IRelatorioRepository
    {
        private readonly DapperContext _context;

        public RelatorioRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ListaDiariaColaboradorResponse>> ListaDiariaColaboradorAsync(DateTime inicial, DateTime final, int idColaboradorDetalhe)
        {
            var dataInc = inicial.Date;
            var dataFim = final.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var query = @"SELECT
                              D.DataDiaria,
                              P.Nome as NomePosto
                          FROM [dbo].[Diaria] as D
                          INNER JOIN ColaboradorDetalhe AS CD ON D.IdColaboradorDetalhe = CD.Id
                          INNER JOIN Posto AS P ON CD.IdPosto = P.Id
                          WHERE D.IdColaboradorDetalhe = @IdColaboradorDetalhe
                            AND D.DataDiaria BETWEEN @DataInc AND @DataFim";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ListaDiariaColaboradorResponse>(query, new
            {
                IdColaboradorDetalhe = idColaboradorDetalhe,
                DataInc = dataInc,
                DataFim = dataFim
            });
        }

        public async Task<IEnumerable<ListaDiariaRelatorio>> ListaDiariaRelatoriosAsync(DateTime inicial, DateTime final, int? colaborador, int? posto)
        {
            var dataInc = inicial.Date;
            var dataFim = final.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var query = @"SELECT 
                              d.IdColaboradorDetalhe,
                              COUNT(d.IdColaboradorDetalhe) AS Quantidade,
                              [Colaborador] = UPPER(c.Nome),
                              [Periodo] = STRING_AGG(CONVERT(varchar, DAY(d.DataDiaria)), '|') 
                                  WITHIN GROUP (ORDER BY d.DataDiaria),
                              UPPER(f.Nome) AS Funcao,
                              UPPER(s.Nome) AS Supervisor,
                              UPPER(p.Nome) AS Posto
                          FROM Diaria AS d
                          INNER JOIN ColaboradorDetalhe AS cd ON d.IdColaboradorDetalhe = cd.Id
                          INNER JOIN Colaborador AS c ON cd.IdColaborador = c.Id
                          INNER JOIN Funcao AS f ON cd.IdFuncao = f.Id
                          INNER JOIN Supervisor AS s ON cd.IdSupervisor = s.Id
                          INNER JOIN Posto AS p ON cd.IdPosto = p.Id
                          WHERE d.DataDiaria BETWEEN @DataInc AND @DataFim
                            AND c.Excluido = 0"
                            + (colaborador.HasValue ? " AND c.Id = @Colaborador" : "")
                            + (posto.HasValue ? " AND p.Id = @Posto" : "") +
                          @" GROUP BY 
                              d.IdColaboradorDetalhe,
                              c.Nome,
                              f.Nome,
                              s.Nome,
                              p.Nome
                          ORDER BY [Colaborador] ASC";

            var parameters = new DynamicParameters();
            parameters.Add("DataInc", dataInc);
            parameters.Add("DataFim", dataFim);
            if (colaborador.HasValue) parameters.Add("Colaborador", colaborador.Value);
            if (posto.HasValue) parameters.Add("Posto", posto.Value);

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ListaDiariaRelatorio>(query, parameters);
        }

        public async Task<IEnumerable<DiariaConsolidado>> RelatorioConsolidadoAsync(DateTime inicial, DateTime final)
        {
            var dataInc = inicial.Date;
            var dataFim = final.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            var query = @"SELECT 
                              COUNT(D.Id) AS QuantidadeDiaria,
                              C.Id as IdColaborador,
                              CD.ValorDiaria * COUNT(D.Id) AS ValorTotal,
                              C.[Nome],
                              C.[Pix],
                              STRING_AGG(CONVERT(varchar, CD.ValorDiaria), ', ') AS ValorDiaria,
                              (SELECT ISNULL(SUM(DD.Valor), 0) FROM DiariaDesconto AS DD 
                               WHERE C.Id = DD.IdColaborador 
                                 AND DD.Data BETWEEN @DataInc AND @DataFim) AS Adiantamento
                          INTO #tmp
                          FROM Diaria AS D
                          INNER JOIN ColaboradorDetalhe AS CD ON D.IdColaboradorDetalhe = CD.Id
                          INNER JOIN Colaborador AS C ON CD.IdColaborador = C.Id
                          WHERE D.DataDiaria BETWEEN @DataInc AND @DataFim
                          GROUP BY C.Id, C.[Nome], C.[Pix], CD.ValorDiaria;

                          SELECT
                              IdColaborador,
                              [Nome],
                              [Pix],
                              SUM(ValorTotal) - ISNULL(Adiantamento, 0) AS ValorTotal,
                              STRING_AGG(CONVERT(varchar, ValorDiaria), ', ') AS ValorDiaria,
                              SUM(QuantidadeDiaria) AS QuantidadeDiaria,
                              ISNULL(Adiantamento, 0) * (-1) AS Adiantamento
                          FROM #tmp
                          GROUP BY IdColaborador, [Nome], [Pix], Adiantamento";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<DiariaConsolidado>(query, new { DataInc = dataInc, DataFim = dataFim });
        }
    }
}
