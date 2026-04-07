using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;
using System.Collections.Generic;

namespace PlataformaSeven.API.Repositories
{
    public class ColaboradorDetalheRepository : IColaboradorDetalheRepository
    {
        private readonly DapperContext _context;

        public ColaboradorDetalheRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ColaboradorDetalhe>> GetAllAsync()
        {
            var query = "SELECT * FROM ColaboradorDetalhe";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ColaboradorDetalhe>(query);
        }

        public async Task<ColaboradorDetalhe?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM ColaboradorDetalhe WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ColaboradorDetalhe>(query, new { Id = id });
        }

        public async Task<IEnumerable<ColaboradorDetalhe>> GetByColaboradorIdAsync(int idColaborador)
        {
            var query = "SELECT * FROM ColaboradorDetalhe WHERE IdColaborador = @IdColaborador";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ColaboradorDetalhe>(query, new { IdColaborador = idColaborador });
        }

		public async Task<IEnumerable<SelectItens>> GetSelectColaboradorDetalhe(int idColaborador)
		{

            var query = @"  select cd.Id ,Upper (f.Nome +' - '+ s.Nome +' - '+ p.Nome +' - '+ CAST(cd.ValorDiaria as varchar(20))) as Descricao from ColaboradorDetalhe as cd inner join Funcao as f on cd.IdFuncao = f.Id inner join Supervisor as s on cd.IdSupervisor = s.Id inner join Posto as p on cd.IdPosto = p.Id 
                             where cd.IdColaborador = @IdColaborador";

			using var connection = _context.CreateConnection();
			return await connection.QueryAsync<SelectItens>(query, new { IdColaborador = idColaborador });
		}
		public async Task<int> CreateAsync(ColaboradorDetalhe detalhe)
        {
            var query = @"
                INSERT INTO ColaboradorDetalhe (IdColaborador, ValorDiaria, IdFuncao, IdSupervisor, IdPosto, Periodo)
                VALUES (@IdColaborador, @ValorDiaria, @IdFuncao, @IdSupervisor, @IdPosto, @Periodo);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, detalhe);
        }

        public async Task<bool> UpdateAsync(ColaboradorDetalhe detalhe)
        {
            var query = @"
                UPDATE ColaboradorDetalhe 
                SET IdColaborador = @IdColaborador, 
                    ValorDiaria = @ValorDiaria, 
                    IdFuncao = @IdFuncao, 
                    IdSupervisor = @IdSupervisor, 
                    IdPosto = @IdPosto,
                    Periodo = @Periodo
                WHERE Id = @Id";
            
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, detalhe);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM ColaboradorDetalhe WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }

    }
}

