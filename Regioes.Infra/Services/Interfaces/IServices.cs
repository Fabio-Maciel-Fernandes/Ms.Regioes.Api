namespace Regioes.Infra.Services.Interfaces
{
    public interface IServices<T>
    {
        public Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        public Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);
        public Task<bool> ExistAsync(int id, CancellationToken cancellationToken);
        public void Create(T model);
        public void Update(T model);
        public void Delete(int id);
    }
}
