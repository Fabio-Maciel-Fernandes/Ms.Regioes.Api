namespace Regioes.Infra.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        public Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task CreateAsync(T model);
        public Task UpdateAsync(T model);
        public Task DeleteAsync(int id);
    }
}
