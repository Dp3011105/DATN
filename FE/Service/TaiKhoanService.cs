using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class TaiKhoanService : ITaiKhoanService
    {
        private readonly ITaiKhoanRepository _repository;

        public TaiKhoanService(ITaiKhoanRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<TaiKhoan>> GetAllAsync() => _repository.GetAllAsync();

        public Task<TaiKhoan?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(TaiKhoan entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(TaiKhoan entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}