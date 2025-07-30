using BE.models;
using Repository.IRepository;
using Service.IService;

namespace Service
{
    public class KhachHangService : IKhachHangService
    {
        private readonly IKhachHangRepository _repository;

        public KhachHangService(IKhachHangRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<KhachHang>> GetAllAsync() => _repository.GetAllAsync();

        public Task<KhachHang?> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task AddAsync(KhachHang entity) => _repository.AddAsync(entity);

        public Task UpdateAsync(KhachHang entity) => _repository.UpdateAsync(entity);

        public Task DeleteAsync(int id) => _repository.DeleteAsync(id);
    }
}