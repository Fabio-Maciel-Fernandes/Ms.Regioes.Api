using Regioes.Core.Models;
using Regioes.Infra.Repositories.Interfaces;
using Regioes.Infra.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Contatos.Infra.Services
{
    public class RegiaoServices : IServices<Regiao>
    {
        private readonly IRepository<Regiao> _repository;
        private readonly ICacheService _cacheServices;
        private readonly string CHAVE_CACHE_REGIAO = "CHAVE_CACHE_REGIAO";

        public RegiaoServices(IRepository<Regiao> repository, ICacheService cacheServices)
        {
            _repository = repository;
            _cacheServices = cacheServices;
        }

        public void Create(Regiao model)
        {
            if (model.Ok())
            {
                _repository.CreateAsync(model);
            }
            else
            {
                throw new ValidationException(model.ErrrsString());
            }
            
        }

        public void Delete(int id)
        {
            _repository.DeleteAsync(id);
        }

        public async Task<bool> ExistAsync(int id, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(id, cancellationToken) != null;
        }

        public async Task<IEnumerable<Regiao>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _cacheServices.GetOrCreateAsync(CHAVE_CACHE_REGIAO,
                 () => _repository.GetAllAsync(cancellationToken));
        }

        public async Task<Regiao> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _cacheServices.GetOrCreateAsync(CHAVE_CACHE_REGIAO + id,
                 () => _repository.GetByIdAsync(id, cancellationToken));
        }

        public void Update(Regiao model)
        {
            if (model.Ok())
            {
                _repository.UpdateAsync(model);
            }
            else
            {
                throw new ValidationException(model.ErrrsString());
            }

        }
    }
}