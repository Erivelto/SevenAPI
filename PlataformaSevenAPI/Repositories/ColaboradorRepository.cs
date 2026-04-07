using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public class ColaboradorRepository : IColaboradorRepository
    {
        private readonly DapperContext _context;

        public ColaboradorRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Colaborador>> GetAllAsync()
        {
            var query = "SELECT * FROM Colaborador WHERE Excluido = 0 ORDER BY Nome";
            
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Colaborador>(query);
        }

        public async Task<Colaborador?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM Colaborador WHERE Id = @Id AND Excluido = 0";
            
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Colaborador>(query, new { Id = id });
        }

        public async Task<int> CreateAsync(Colaborador colaborador)
        {
            var query = @"
                INSERT INTO Colaborador 
                (Nome, Pix, Referencia, Endereco, Numero, Complemento, Bairro, Cidade, UF, CEP, 
                 DataCadastro, DataAlteracao, UserCad, UserAlt, Excluido)
                VALUES 
                (@Nome, @Pix, @Referencia, @Endereco, @Numero, @Complemento, @Bairro, @Cidade, @UF, @CEP, 
                 @DataCadastro, @DataAlteracao, @UserCad, @UserAlt, @Excluido);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, colaborador);
        }

        public async Task<bool> UpdateAsync(Colaborador colaborador)
        {
            var query = @"
                UPDATE Colaborador 
                SET Nome = @Nome, 
                    Pix = @Pix, 
                    Referencia = @Referencia, 
                    Endereco = @Endereco, 
                    Numero = @Numero, 
                    Complemento = @Complemento, 
                    Bairro = @Bairro, 
                    Cidade = @Cidade, 
                    UF = @UF, 
                    CEP = @CEP, 
                    DataAlteracao = @DataAlteracao, 
                    UserAlt = @UserAlt
                WHERE Id = @Id";
            
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, colaborador);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "UPDATE Colaborador SET Excluido = 1, DataAlteracao = GETDATE() WHERE Id = @Id";
            
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

