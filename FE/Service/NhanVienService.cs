using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class NhanVienService : INhanVienService
    {
        private readonly INhanVienRepository _repository;

        public NhanVienService(INhanVienRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<NhanVien>> GetAllAsync() => _repository.GetAllAsync();

        public Task<NhanVien?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(NhanVien entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(NhanVien entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}