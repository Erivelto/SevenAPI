using Dapper;
using PlataformaSeven.API.Data;
using PlataformaSeven.API.Models;

namespace PlataformaSeven.API.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DapperContext _context;

        public UsuarioRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            var query = @"SELECT Id, [User], [Password], DataCadastro, DataAtualizacao, UserCadatro, Tipo 
                          FROM Usuario ORDER BY [User]";
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Usuario>(query);
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            var query = @"SELECT Id, [User], [Password], DataCadastro, DataAtualizacao, UserCadatro, Tipo 
                          FROM Usuario WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Usuario>(query, new { Id = id });
        }

        public async Task<Usuario?> GetByUsernameAsync(string username)
        {
            var query = @"SELECT Id, [User], [Password], DataCadastro, DataAtualizacao, UserCadatro, Tipo 
                          FROM Usuario WHERE [User] = @Username";
            using var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Usuario>(query, new { Username = username });
        }

        public async Task<int> CreateAsync(Usuario usuario)
        {
            var query = @"
                INSERT INTO Usuario ([User], [Password], DataCadastro, DataAtualizacao, UserCadatro, Tipo)
                VALUES (@User, @Password, @DataCadastro, @DataAtualizacao, @UserCadatro, @Tipo);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            
            using var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query, usuario);
        }

        public async Task<bool> UpdateAsync(Usuario usuario)
        {
            var query = @"
                UPDATE Usuario 
                SET [User] = @User, 
                    [Password] = @Password, 
                    DataAtualizacao = @DataAtualizacao, 
                    Tipo = @Tipo
                WHERE Id = @Id";
            
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, usuario);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM Usuario WHERE Id = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}

