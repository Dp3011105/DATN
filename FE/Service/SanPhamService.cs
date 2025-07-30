using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class SanPhamService : ISanPhamService
    {
        private readonly ISanPhamService _repository;

        public SanPhamService(ISanPhamService repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<SanPham>> GetAllAsync() => _repository.GetAllAsync();

        public Task<SanPham?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(SanPham entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(SanPham entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}