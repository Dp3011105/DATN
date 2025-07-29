using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class SanPhamSizeService : ISanPhamSizeService
    {
        private readonly ISanPhamSizeRepository _repository;

        public SanPhamSizeService(ISanPhamSizeRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<SanPhamSize>> GetAllAsync() => _repository.GetAllAsync();

        public Task<SanPhamSize?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(SanPhamSize entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(SanPhamSize entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}