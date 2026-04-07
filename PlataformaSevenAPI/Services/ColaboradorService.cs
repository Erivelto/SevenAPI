using PlataformaSeven.API.Models;
using PlataformaSeven.API.Repositories;

namespace PlataformaSeven.API.Services
{
    public class ColaboradorService : IColaboradorService
    {
        private readonly IColaboradorRepository _repository;

        public ColaboradorService(IColaboradorRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Colaborador>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Colaborador?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(Colaborador colaborador)
        {
            colaborador.DataCadastro = DateTime.Now;
            colaborador.DataAlteracao = DateTime.Now;
            return await _repository.CreateAsync(colaborador);
        }

        public async Task<bool> UpdateAsync(Colaborador colaborador)
        {
            colaborador.DataAlteracao = DateTime.Now;
            return await _repository.UpdateAsync(colaborador);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}

