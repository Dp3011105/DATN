using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class SanPhamLuongDaService : ISanPhamLuongDaService
    {
        private readonly ISanPhamLuongDaRepository _repository;

        public SanPhamLuongDaService(ISanPhamLuongDaRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<SanPhamLuongDa>> GetAllAsync() => _repository.GetAllAsync();

        public Task<SanPhamLuongDa?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(SanPhamLuongDa entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(SanPhamLuongDa entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}