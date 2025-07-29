using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class SanPhamToppingService : ISanPhamToppingService
    {
        private readonly ISanPhamToppingRepository _repository;

        public SanPhamToppingService(ISanPhamToppingRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<SanPhamTopping>> GetAllAsync() => _repository.GetAllAsync();

        public Task<SanPhamTopping?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(SanPhamTopping entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(SanPhamTopping entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}